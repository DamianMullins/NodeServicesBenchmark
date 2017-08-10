namespace BenchmarkApp
{
    internal class Program
    {
        private static void Main()
        {
            using (var test = new ViewComponentBenchmarks
            {
                TestTitle = "With View Component",
                TestUrl = "/Home/WithViewComponent"
            })
            {
                test.RunTests().Wait();
            }

            using (var test = new ViewComponentBenchmarks
            {
                TestTitle = "No View Component",
                TestUrl = "/Home/NoViewComponent"
            })
            {
                test.RunTests().Wait();
            }
        }
    }
}
