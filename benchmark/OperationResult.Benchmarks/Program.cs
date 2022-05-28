using BenchmarkDotNet.Running;
using OperationResult.Benchmarks;

var summary = BenchmarkRunner.Run<OperationResultBenchmark>();