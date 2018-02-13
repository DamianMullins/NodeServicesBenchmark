using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace NodeServicesBenchmark.BenchmarkRunner
{
    public class ViewComponentBenchmarks
    {
        private readonly TestHttpServer _server;

        public ViewComponentBenchmarks()
        {
            _server = new TestHttpServer();
        }

        [Benchmark(Baseline = true)]
        public async Task RazorTemplate() => await _server.HttpGet("benchmarks/RazorTemplate");

        [Benchmark]
        public async Task NodeServices() => await _server.HttpGet("benchmarks/NodeServices");

        [Benchmark]
        public async Task CachedNodeServices() => await _server.HttpGet("benchmarks/CachedNodeServices");
    }
}
