using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core
{
    internal sealed class AsyncBenchmarkInvoker : BenchmarkInvoker
    {
        public AsyncBenchmarkInvoker(string name, int iteration, Func<object, object[], object> invoker) : base(name, iteration, invoker)
        {
        }

        protected internal override void Invoke(object instance)
        {
            Task.Run(async () =>
            {
                await CodeTimer.TimeAsync(_name, _iteration, () => _invoker(instance, Args) as Task);
            }).GetAwaiter().GetResult();
        }
    }
}