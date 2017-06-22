using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core
{
    internal sealed class AsyncConcurrentBenchmarkInvoker : ConcurrentBenchmarkInvoker
    {
        public AsyncConcurrentBenchmarkInvoker(string name, int iteration, int iterationOfConcurrent, Func<object, object[], object> invoker) : base(name, iteration, iterationOfConcurrent, invoker)
        {
        }

        protected internal override void Invoke(object instance)
        {
            CodeTimer.TimeConcurrent(_name, _iteration, _iterationOfConcurrent, () => (_invoker(instance, Args) as Task));
        }
    }
}
