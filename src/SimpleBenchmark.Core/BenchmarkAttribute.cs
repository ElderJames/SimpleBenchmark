using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BenchmarkAttribute : Attribute
    {
        public string Name { get; set; }
        public int Iteration { get; set; }

        public BenchmarkAttribute()
            : this(null, BenchmarkIteration.Default)
        {
        }

        public BenchmarkAttribute(string name)
          : this(name, BenchmarkIteration.Default)
        {
        }

        public BenchmarkAttribute(string name, int iteration)
        {
            Name = name;
            Iteration = iteration;
        }
    }
}