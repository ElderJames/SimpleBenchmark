using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleBenchmark.Core.Test
{
    [TestClass]
    public class AsyncTest
    {
        [TestMethod]
        public void Run()
        {
            BenchmarkRunner.Initialize();
            
            BenchmarkRunner.Run<DelayBenchmark>();
        }
    }

    public class DelayBenchmark
    {
        [Benchmark(Iteration = 5000000)]
        public Task Async()
        {
            return Task.FromResult(new DelayBenchmark());
        }

        [Benchmark(Iteration = 5000000)]
        public void Wait()
        {
            Task.FromResult(new DelayBenchmark()).Wait();
        }
    }
}