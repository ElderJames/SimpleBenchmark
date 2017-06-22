using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core.BenchmarkInvokers
{
    internal sealed class ParallelBenchmarkInvoker: BenchmarkInvoker
    {
        private readonly int _maxDegreeOfParallelism;
        public ParallelBenchmarkInvoker(string name, int iteration, int maxDegreeOfParallelism, Func<object, object[], object> invoker) : base(name, iteration, invoker)
        {
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        protected internal override void Invoke(object instance)
        {
            CodeTimer.TimeParallel(_name, _iteration, _maxDegreeOfParallelism, () => _invoker(instance, Args));
        }
    }
}
