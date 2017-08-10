using System;
using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NodeServicesBenchmark;

namespace BenchmarkApp
{
    public class ViewComponentBenchmarks :IDisposable
    {
        private const string Request = "/Home/WithViewComponent";
        public const int TestRunCount = 1;

        private readonly TestServer _server;
        private readonly HttpClient _client;

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
            _client.BaseAddress = new Uri("http://localhost:5000");
        }

        [Benchmark]
        public async Task RunTests()
        {
            for (var i = 0; i < TestRunCount; i++)
			{
				Console.WriteLine($"Test run {i} starting");
				await RunTest();
                Console.WriteLine($"Test run {i} completed");
            }
        }

        public async Task RunTest()
        {
            var response = await _client.GetAsync(Request);
            await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }
    }
}
