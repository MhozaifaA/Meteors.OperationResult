using BenchmarkDotNet.Running;
using OperationContext.Benchmarks;

var summary = BenchmarkRunner.Run<OperationResultBenchmark>();