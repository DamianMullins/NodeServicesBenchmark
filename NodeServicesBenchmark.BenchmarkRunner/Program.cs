using System;

namespace NodeServicesBenchmark.BenchmarkRunner
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<LoopBenchmarks>();
            BenchmarkDotNet.Running.BenchmarkRunner.Run<RandomLoopBenchmarks>();

            Console.Write("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
