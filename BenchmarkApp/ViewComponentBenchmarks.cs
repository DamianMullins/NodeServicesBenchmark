using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using NodeServicesBenchmark;

namespace BenchmarkApp
{
    public class ViewComponentBenchmarks : IDisposable
    {
        public const int TestRunCount = 50;

        private readonly HttpClient _client;
        private readonly TestServer _server;
        private readonly IList<long> _results;

        public string TestUrl { get; set; }
        public string TestTitle { get; set; }

        private static string ContentPath
        {
            get
            {
                var path = PlatformServices.Default.Application.ApplicationBasePath;
                var contentPath = Path.GetFullPath(Path.Combine(path, $@"..\..\..\..\{nameof(NodeServicesBenchmark)}"));
                return contentPath;
            }
        }

        public ViewComponentBenchmarks()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(ContentPath)
                .UseStartup<Startup>();

            _server = new TestServer(builder);

            _client = _server.CreateClient();
            _client.BaseAddress = new UriBuilder { Port = 5001 }.Uri;

            _results = new List<long>();
        }

        public async Task RunTests()
        {
            Console.WriteLine($"Running {TestTitle} tests");

            for (var i = 0; i < TestRunCount; i++)
            {
                var watch = Stopwatch.StartNew();
                
				await RunTest();

                watch.Stop();

                var elapsedTime = watch.ElapsedMilliseconds;

                _results.Add(elapsedTime);

                Console.WriteLine($"Test {i+1} completed in {elapsedTime} milliseconds");
            }

            // Remove the first result from the list as this include the time taken to spin up the test server
            var averageTime = _results.Skip(1).Average();

            Console.WriteLine($"Average time taken was {averageTime} milliseconds");
            Console.WriteLine("");
            Console.WriteLine("---------------");
            Console.WriteLine("");
        }

        
        public async Task RunTest()
        {
            var response = await _client.GetAsync(TestUrl);
            await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }
    }
}
