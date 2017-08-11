using System;
using BenchmarkDotNet.Running;

namespace BenchmarkApp
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ViewComponentBenchmarks>();

            Console.Write("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
