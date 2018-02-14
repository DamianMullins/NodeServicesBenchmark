using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace NodeServicesBenchmark.BenchmarkRunner
{
    public class RandomLoopBenchmarks
    {
        private readonly TestHttpServer _server;

        public RandomLoopBenchmarks()
        {
            _server = new TestHttpServer();
        }

        [Benchmark(Baseline = true)]
        public async Task RazorTemplate() => await _server.HttpGet("random-loop-benchmarks/RazorTemplate");

        [Benchmark]
        public async Task NodeServices() => await _server.HttpGet("random-loop-benchmarks/NodeServices");

        [Benchmark]
        public async Task CachedNodeServices() => await _server.HttpGet("random-loop-benchmarks/CachedNodeServices");
    }
}
