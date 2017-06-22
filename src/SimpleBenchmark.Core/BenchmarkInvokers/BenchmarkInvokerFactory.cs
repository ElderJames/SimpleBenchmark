using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AspectCore.Core.Internal;

namespace SimpleBenchmark.Core
{
    internal static class BenchmarkInvokerFactory
    {
        internal static BenchmarkInvoker Create(MethodInfo method)
        {
            var benchmarkAttribute = method.GetCustomAttribute<BenchmarkAttribute>();
            var name = string.IsNullOrEmpty(benchmarkAttribute.Name) ? method.Name : benchmarkAttribute.Name;
            var iteration = benchmarkAttribute.Iteration <= 0 ? BenchmarkIteration.Default : benchmarkAttribute.Iteration;
            var invoker = new MethodReflector(method).CreateMethodInvoker();
            if (method.IsDefined(typeof(ParallelAttribute)) && method.ReturnType == typeof(void))
            {
                var parallel = method.GetCustomAttribute<ParallelAttribute>();
                return new ConcurrentBenchmarkInvoker(name, iteration, parallel.MaxDegreeOfParallelism, invoker);
            }
            if (method.IsDefined(typeof(ConcurrentAttribute)))
            {
                var concurrent = method.GetCustomAttribute<ConcurrentAttribute>();
                if (typeof(Task).IsAssignableFrom(method.ReturnType))
                {
                    return new AsyncConcurrentBenchmarkInvoker(name, iteration, concurrent.IterationOfConcurrent, invoker);
                }
                return new ConcurrentBenchmarkInvoker(name, iteration, concurrent.IterationOfConcurrent, invoker);
            }
            if (typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                return new AsyncBenchmarkInvoker(name, iteration, invoker);
            }
            return new VoidBenchmarkInvoker(name, iteration, invoker);
        }
    }
}
