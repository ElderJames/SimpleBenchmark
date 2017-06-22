using System;
using System.Linq;
using System.Reflection;

namespace SimpleBenchmark.Core
{
    public class BenchmarkRunner
    {
        public static void Initialize()
        {
            CodeTimer.Initialize();
        }

        public static void Run<T>(params object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            var benchmarkInstance = Activator.CreateInstance(typeof(T), args);
            var benchmarkInvokers = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.IsDefined(typeof(BenchmarkAttribute), true)).Select(x => BenchmarkInvokerFactory.Create(x)).ToArray();
            if (benchmarkInvokers.Length == 0)
            {
                Console.WriteLine("Not found benchmark !");
            }
            foreach (var invoker in benchmarkInvokers)
            {
                invoker.Invoke(benchmarkInstance);
            }
        }
    }
}