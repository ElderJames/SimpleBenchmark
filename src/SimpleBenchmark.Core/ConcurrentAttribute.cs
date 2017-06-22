using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core
{
    public sealed class ConcurrentAttribute : Attribute
    {
        public int IterationOfConcurrent { get; }

        public ConcurrentAttribute() : this(100)
        {
        }

        public ConcurrentAttribute(int iterationOfConcurrent)
        {
            IterationOfConcurrent = iterationOfConcurrent;
        }
    }
}
