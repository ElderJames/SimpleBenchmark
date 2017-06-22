using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core
{
    internal class ConcurrentBenchmarkInvoker : BenchmarkInvoker
    {
        protected readonly int _iterationOfConcurrent;
        public ConcurrentBenchmarkInvoker(string name, int iteration, int iterationOfConcurrent, Func<object, object[], object> invoker) : base(name, iteration, invoker)
        {
            _iterationOfConcurrent = iterationOfConcurrent;
        }

        protected internal override void Invoke(object instance)
        {
            CodeTimer.TimeConcurrent(_name, _iteration, _iterationOfConcurrent, () => _invoker(instance, Args));
        }
    }
}
