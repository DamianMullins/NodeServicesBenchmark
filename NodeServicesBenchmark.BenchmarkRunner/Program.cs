using System;

namespace NodeServicesBenchmark.BenchmarkRunner
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<ViewComponentBenchmarks>();

            Console.Write("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
