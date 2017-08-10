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

        private readonly HttpClient _client;
        private readonly TestServer _server;
        private readonly IList<long> _results;
        private readonly string _testUrl;
        private readonly int _testRuns;

        private static string ContentPath
        {
            get
            {
                var path = PlatformServices.Default.Application.ApplicationBasePath;
                var contentPath = Path.GetFullPath(Path.Combine(path, $@"..\..\..\..\{nameof(NodeServicesBenchmark)}"));
                return contentPath;
            }
        }

        public ViewComponentBenchmarks(string testUrl, int testRuns)
        {
            _testUrl = testUrl;
            _testRuns = testRuns;

            var builder = new WebHostBuilder()
                .UseContentRoot(ContentPath)
                .UseStartup<Startup>();

            _server = new TestServer(builder);

            _client = _server.CreateClient();
            _client.BaseAddress = new UriBuilder { Port = 5001 }.Uri;

            _results = new List<long>();
        }

        public async Task<double> RunTests()
        {
            for (var i = 0; i < _testRuns; i++)
            {
                await RunTest();
            }

            // Remove the first result from the list as this include the time taken to spin up the test server
            var averageTime = _results.Skip(1).Average();
            return averageTime;
        }

        
        public async Task RunTest()
        {
            var watch = Stopwatch.StartNew();

            var response = await _client.GetAsync(_testUrl);
            await response.Content.ReadAsStringAsync();

            watch.Stop();
            var elapsedTime = watch.ElapsedMilliseconds;
            _results.Add(elapsedTime);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }
    }
}
