using System;
using System.Collections.Generic;

namespace BenchmarkApp
{
    internal class Program
    {
        private static readonly IDictionary<string, double> Results = new Dictionary<string, double>();
        private const int TestRunCount = 10000;

        private static void Main()
        {
            RunTest(title: "No Template Service", url: "/no-template", testRuns: TestRunCount);
            RunTest(title: "With Template Service", url: "/with-template", testRuns: TestRunCount);
            RunTest(title: "With Cached Template Service", url: "/with-cached-template", testRuns: TestRunCount);

            DisplayResults();
        }

        private static void RunTest(string title, string url, int testRuns)
        {
            using (var test = new ViewComponentBenchmarks(url, testRuns))
            {
                var time = test.RunTests().Result;
                Results.Add(title, time);
            }
        }

        private static void DisplayResults()
        {
            foreach (var result in Results)
            {
                Console.WriteLine($"{result.Key} ran {TestRunCount} times with an average time of {result.Value:N2} milliseconds.");
            }
        }
    }
}
