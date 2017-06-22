using System;

namespace SimpleBenchmark.Core
{
    internal abstract class BenchmarkInvoker
    {
        protected static readonly object[] Args = new object[0];

        protected readonly Func<object, object[], object> _invoker;
        protected readonly string _name;
        protected readonly int _iteration;

        internal BenchmarkInvoker(string name, int iteration, Func<object, object[], object> invoker)
        {
            _name = name;
            _iteration = iteration;
            _invoker = invoker;
        }

        protected internal abstract void Invoke(object instance);
    }
}