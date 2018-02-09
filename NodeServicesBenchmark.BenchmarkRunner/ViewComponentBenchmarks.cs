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
        public async Task No_Template() => await _server.HttpGet("no-template");

        [Benchmark]
        public async Task With_Template() => await _server.HttpGet("with-template");

        [Benchmark]
        public async Task With_Cached_Template() => await _server.HttpGet("with-cached-template");
    }
}
