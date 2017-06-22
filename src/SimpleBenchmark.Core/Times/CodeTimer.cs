using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SimpleBenchmark.Core.Schedulers;

namespace SimpleBenchmark.Core
{
    internal static class CodeTimer
    {
        private static readonly Action IdleTarget = () => { };

        private static readonly Task FromResult = Task.FromResult(0);

        private static readonly Func<Task> IdleTaskFunc = () => FromResult;

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

        public static async Task TimeAsync(string name, int iteration, Func<Task> taskFunc)
        {
            if (string.IsNullOrEmpty(name))
                return;

            #region
            Stopwatch Idlewatch = new Stopwatch();
            Idlewatch.Start();
            for (int index = 0; index < iteration; ++index)
                await IdleTaskFunc();
            Idlewatch.Stop();
            #endregion

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
                await taskFunc();
            ulong num1 = CodeTimer.GetCycleCount() - cycleCount;
            stopwatch.Stop();
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine("\tTime Elapsed:\t" + (stopwatch.ElapsedMilliseconds - Idlewatch.ElapsedMilliseconds).ToString("N0") + "ms");
            Console.WriteLine("\tCPU Cycles:\t" + num1.ToString("N0"));
            for (int generation = 0; generation <= GC.MaxGeneration; ++generation)
            {
                int num2 = GC.CollectionCount(generation) - numArray[generation];
                Console.WriteLine("\tGen " + (object)generation + ": \t\t" + (object)num2);
            }
            Console.WriteLine();
        }

        public static void TimeParallel(string name, int iteration, int iterationOfConcurrent, Action action)
        {
            if (string.IsNullOrEmpty(name))
                return;

            Parallel.For(0, iteration, i => IdleTarget());

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

            Parallel.For(0, iteration, new ParallelOptions { MaxDegreeOfParallelism = iterationOfConcurrent }, i => action());

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

        public static void TimeConcurrent(string name, int iteration, int iterationOfConcurrent, Action action)
        {
            if (string.IsNullOrEmpty(name))
                return;
            var taskScheduler = new LimitedConcurrencyLevelTaskScheduler(iterationOfConcurrent);
            var taskFactory = new TaskFactory(taskScheduler);
            var tasks = new Task[iteration];
            for (var i = 0; i < iteration; i++)
            {
                tasks[i] = new Task(action);
            }
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
            for (var i = 0; i < iteration; i++)
            {
                tasks[i].Start(taskScheduler);
            }
            Task.WaitAll(tasks);
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

        public static void TimeConcurrent(string name, int iteration, int iterationOfConcurrent, Func<Task> action)
        {
            if (string.IsNullOrEmpty(name))
                return;
            var taskScheduler = new LimitedConcurrencyLevelTaskScheduler(iterationOfConcurrent);
            var tasks = new Task[iteration];
            for (var i = 0; i < iteration; i++)
            {
                tasks[i] = new Task<Task>(action);
            }
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
            for (var i = 0; i < iteration; i++)
            {
                tasks[i].Start(taskScheduler);
            }
            Task.WaitAll(tasks);
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
