using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace NodeServicesBenchmark.BenchmarkRunner
{
    [CsvMeasurementsExporter]
    [RPlotExporter]
    public class ViewComponentBenchmarks
    {
        private readonly TestHttpServer _server;

        public ViewComponentBenchmarks()
        {
            _server = new TestHttpServer();
        }

        [Benchmark(Baseline = true)]
        public async Task No_Template() => await _server.HttpGet("benchmarks/RazorTemplate");

        [Benchmark]
        public async Task With_Template() => await _server.HttpGet("benchmarks/NodeServices");

        [Benchmark]
        public async Task With_Cached_Template() => await _server.HttpGet("benchmarks/CachedNodeServices");
    }
}
