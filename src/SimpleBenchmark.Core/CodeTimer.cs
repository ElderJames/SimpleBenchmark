using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleBenchmark.Core
{
    internal static class CodeTimer
    {
        private static readonly Action IdleTarget = () => { };

        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            CodeTimer.Time("", 1, (Action)(() => { }));
        }

        public static void Time(string name, int iteration, Action action)
        {
            if (string.IsNullOrEmpty(name))
                return;
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] numArray = new int[GC.MaxGeneration + 1];
            for (int generation = 0; generation <= GC.MaxGeneration; ++generation)
                numArray[generation] = GC.CollectionCount(generation);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ulong cycleCount = CodeTimer.GetCycleCount();
            for (int index = 0; index < iteration; ++index)
                action();
            ulong num1 = CodeTimer.GetCycleCount() - cycleCount;
            stopwatch.Stop();
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine("\tTime Elapsed:\t" + stopwatch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tCPU Cycles:\t" + num1.ToString("N0"));
            for (int generation = 0; generation <= GC.MaxGeneration; ++generation)
            {
                int num2 = GC.CollectionCount(generation) - numArray[generation];
                Console.WriteLine("\tGen " + (object)generation + ": \t\t" + (object)num2);
            }
            Console.WriteLine();
        }

        private static ulong GetCycleCount()
        {
            ulong cycleTime = 0;
            CodeTimer.QueryThreadCycleTime(CodeTimer.GetCurrentThread(), ref cycleTime);
            return cycleTime;
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();
    }
}
