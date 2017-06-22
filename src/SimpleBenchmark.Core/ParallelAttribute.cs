using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core
{
    public sealed class ParallelAttribute : Attribute
    {
        public int MaxDegreeOfParallelism { get; }

        public ParallelAttribute() : this(4)
        {
        }

        public ParallelAttribute(int maxDegreeOfParallelism)
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism;
        }
    }
}
