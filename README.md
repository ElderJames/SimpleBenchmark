## SimpleBenchmark
SimpleBenchmark是一个简单的 C# 程序性能测试库，基于老赵[CodeTimer](http://blog.zhaojie.me/2009/03/codetimer.html)的性能统计和分析，支持框架版本.NETFramework 4.5及以上。SimpleBenchmark的API参考[BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)（BenchmarkDotNet是一个功能强大的性能测试库，但BenchmarkDotNet仅支持.NETFramework 4.6和.NETCoreApp 1.1及以上，不支持.NETFramework 4.5┬_┬）。
## 开始使用
1. 使用nuget安装[SimpleBenchmark](http://www.nuget.org/packages/SimpleBenchmark.Core/)
```
PM> Install-Package SimpleBenchmark.Core
```
2. 创建`benchmark`类
```
namespace MyBenchmarks
{
    public class Md5VsSha256
    {
        private const int N = 10000;
        private readonly byte[] data;

        private readonly SHA256 sha256 = SHA256.Create();
        private readonly MD5 md5 = MD5.Create();

        public Md5VsSha256()
        {
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        public byte[] Sha256() => sha256.ComputeHash(data);

        [Benchmark]
        public byte[] Md5() => md5.ComputeHash(data);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Initialize();

            BenchmarkRunner.Run<Md5VsSha256>();

            Console.ReadKey();
        }
    }
}
```

