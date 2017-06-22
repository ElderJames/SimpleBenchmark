﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleBenchmark.Core.Test
{
    [TestClass]
    public class Md5VsSha256Test
    {
        [TestMethod]
        public void Run()
        {
            BenchmarkRunner.Initialize();
            BenchmarkRunner.Run<Md5VsSha256>();
        }
    }

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

        [Benchmark(Iteration = 100000)]
        public byte[] Sha256() => sha256.ComputeHash(data);

        [Benchmark(Iteration = 100000)]
        public byte[] Md5() => md5.ComputeHash(data);

    }
}