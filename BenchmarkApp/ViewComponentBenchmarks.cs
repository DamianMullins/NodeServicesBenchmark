using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using NodeServicesBenchmark;

namespace BenchmarkApp
{
    public class ViewComponentBenchmarks : IDisposable
    {
        // Code to get stuff working borrowed from https://github.com/aspnet/Mvc/blob/e1c1682065724bbf8e245d40cb32b300c07505bb/src/Microsoft.AspNetCore.Mvc.Testing/MvcWebApplicationBuilder.cs

        private readonly HttpClient _client;
        private readonly TestServer _server;

        public List<Assembly> ApplicationAssemblies { get; set; } = new List<Assembly>();

        public ViewComponentBenchmarks()
        {
            var depsFileName = $"{typeof(Startup).GetTypeInfo().Assembly.GetName().Name}.deps.json";
            var depsFile = new FileInfo(Path.Combine(AppContext.BaseDirectory, depsFileName));

            var requiredPath = depsFile.FullName;

            // HACK Ensure that the BenchmarkDotNet generated folder contains the .deps.json files
            while (!depsFile.Exists)
            {
                depsFile = new FileInfo(Path.Combine(depsFile.DirectoryName, "..", depsFileName));
            }

            if (depsFile.FullName != requiredPath)
            {
                File.Copy(depsFile.FullName, requiredPath);
            }
            // ENDHACK

            ApplicationAssemblies.AddRange(DefaultAssemblyPartDiscoveryProvider
                .DiscoverAssemblyParts(typeof(Startup).GetTypeInfo().Assembly.GetName().Name)
                .Select(s => ((AssemblyPart)s).Assembly)
                .ToList());

            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseSetting(WebHostDefaults.ApplicationKey, typeof(Startup).GetTypeInfo().Assembly.GetName().Name)
                .UseContentRoot(GetProjectPath<Startup>())
                .ConfigureServices(InitializeServices);

            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }

        [Benchmark(Baseline = true)]
        public async Task No_Template() => await HttpGet("no-template");

        [Benchmark]
        public async Task With_Template() => await HttpGet("with-template");
        
        [Benchmark]
        public async Task With_Cached_Template() => await HttpGet("with-cached-template");

        private async Task HttpGet(string uri) => await _client.GetStringAsync(uri);

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {
            var manager = new ApplicationPartManager();

            foreach (var assembly in ApplicationAssemblies)
            {
                manager.ApplicationParts.Add(new AssemblyPart(assembly));
            }

            services.AddSingleton(manager);
        }

        private static string GetProjectPath<TStartup>()
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            return GetProjectPath(".", startupAssembly);
        }

        private static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            string projectName = startupAssembly.GetName().Name;
            string solutionName = "NodeServicesBenchmark.sln";
            string applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            // Find the directory containing the solution file
            DirectoryInfo directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                string fileName = Path.Combine(directoryInfo.FullName, solutionName);

                FileInfo solutionFileInfo = new FileInfo(fileName);

                if (solutionFileInfo.Exists)
                {
                    string contentRoot = Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName);
                    return Path.GetFullPath(contentRoot);
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new FileNotFoundException($"Solution file could not be located using application root '{applicationBasePath}'.", solutionName);
        }
    }
}
