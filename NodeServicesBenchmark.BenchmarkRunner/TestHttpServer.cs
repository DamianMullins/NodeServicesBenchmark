using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using NodeServicesBenchmark.Website;

namespace NodeServicesBenchmark.BenchmarkRunner
{
    public sealed class TestHttpServer : IDisposable
    {
        private static readonly Assembly Assembly = typeof(Startup).Assembly;

        private readonly List<Assembly> _applicationAssemblies;
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public async Task HttpGet(string uri) => await _client.GetStringAsync(uri);

        public TestHttpServer()
        {
            _applicationAssemblies = new List<Assembly>();

            CopyProjectDependencies();

            _server = CreateTestServer();
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }

        private static void CopyProjectDependencies()
        {
            var depsFileName = $"{Assembly.GetName().Name}.deps.json";

            var benchmarkFileDirectory = Assembly.Location
                .Remove(Assembly.Location.LastIndexOf('\\') + 1);
            var benchmarkDepsFile = new FileInfo(Path.Combine(benchmarkFileDirectory, depsFileName));

            var depFileDirectory = Assembly.Location
                .Remove(Assembly.Location.LastIndexOf('\\') + 1)
                .Replace("NodeServicesBenchmark.BenchmarkRunner", "NodeServicesBenchmark.Website"); // Not nice but it works.
            var depsFile = new FileInfo(Path.Combine(depFileDirectory, depsFileName));

            while (!depsFile.Exists)
            {
                var file = new FileInfo(Path.Combine(depsFile.DirectoryName, "..", depsFileName));
                depsFile = file;
            }

            if (depsFile.FullName != benchmarkDepsFile.FullName)
            {
                File.Copy(depsFile.FullName, benchmarkDepsFile.FullName);
            }
        }

        private void AddApplicationAssemblies()
        {
            _applicationAssemblies.AddRange(DefaultAssemblyPartDiscoveryProvider
                .DiscoverAssemblyParts(Assembly.GetName().Name)
                .Select(ap => ((AssemblyPart)ap).Assembly)
                .ToList());
        }

        private void InitializeServices(IServiceCollection services)
        {
            AddApplicationAssemblies();

            var manager = new ApplicationPartManager();

            foreach (var assembly in _applicationAssemblies)
            {
                manager.ApplicationParts.Add(new AssemblyPart(assembly));
            }

            services.AddSingleton(manager);
        }

        private static string GetProjectPath()
        {
            var projectName = Assembly.GetName().Name;
            var solutionName = "NodeServicesBenchmark.sln";
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            // Find the directory containing the solution file
            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                var fileName = Path.Combine(directoryInfo.FullName, solutionName);
                var solutionFileInfo = new FileInfo(fileName);

                if (solutionFileInfo.Exists)
                {
                    var contentRoot = Path.Combine(directoryInfo.FullName, projectName);
                    return Path.GetFullPath(contentRoot);
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo?.Parent != null);

            throw new FileNotFoundException($"Solution file could not be located using application root '{applicationBasePath}'.", solutionName);
        }

        private TestServer CreateTestServer()
        {
            var builder = new WebHostBuilder()
               .UseStartup<Startup>()
               .UseSetting(WebHostDefaults.ApplicationKey, Assembly.GetName().Name)
               .UseContentRoot(GetProjectPath())
               .ConfigureServices(InitializeServices)
               .UseKestrel();

            return new TestServer(builder);
        }
    }
}
