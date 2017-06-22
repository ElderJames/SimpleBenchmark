using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleBenchmark.Core.Test
{
    [TestClass]
    public class ParallelTest
    {
        [TestMethod]
        public void Run()
        {
            BenchmarkRunner.Initialize();
            BenchmarkRunner.Run<ParallelBenchmark>();
        }
    }

    public class ParallelBenchmark
    {
        [Parallel]
        [Benchmark(Iteration = 100000)]
        public void Parallel()
        {
            int sum = 0;
            for (int i = 0; i < 5000; i++)
            {
                sum += i;
            }
        }

        [Benchmark(Iteration = 100000)]
        public void Serial()
        {
            int sum = 0;
            for (int i = 0; i < 5000; i++)
            {
                sum += i;
            }
        }
    }
}
