namespace BenchmarkApp
{
    internal class Program
    {
        private static void Main()
        {
            var viewComponentBenchmarks = new ViewComponentBenchmarks();

            viewComponentBenchmarks.RunTests().Wait();
        }
    }
}
