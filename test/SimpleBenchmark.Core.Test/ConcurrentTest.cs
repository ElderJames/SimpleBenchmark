using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleBenchmark.Core.Test
{
    [TestClass]
    public class ConcurrentTest
    {
        [TestMethod]
        public void Run()
        {
            BenchmarkRunner.Initialize();
            BenchmarkRunner.Run<ConcurrentBenchmark>();
        }
    }

    public class ConcurrentBenchmark
    {
        [Concurrent(4)]
        [Benchmark(Iteration = 100000)]
        public void Concurrent()
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
