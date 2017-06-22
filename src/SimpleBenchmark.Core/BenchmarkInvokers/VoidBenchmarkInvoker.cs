using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core
{
    internal class VoidBenchmarkInvoker : BenchmarkInvoker
    {
        internal VoidBenchmarkInvoker(string name, int iteration, Func<object, object[], object> invoker) : base(name, iteration, invoker)
        {
        }

        protected internal override void Invoke(object instance)
        {
            CodeTimer.Time(_name, _iteration, () =>
            {
                _invoker(instance, Args);
            });
        }
    }
}
