using BenchmarkDotNet.Attributes;
using Meteors;
using Meteors.OperationContext;
using System.Text.Json.Serialization;

namespace OperationContext.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, iterationCount: 1)]
    public class OperationResultBenchmark
    {
      
        private Statuses[] StatusList = new[] { 
            Statuses.UnKnown,
            Statuses.Unauthorized,
            Statuses.Success,
            Statuses.Exist,
            Statuses.NotExist,
            Statuses.Failed,
            Statuses.Forbidden,        
            Statuses.Exception,        
        };


        [Benchmark]
        public void StatusToString()
        {
            foreach (var status in StatusList)
            {
                var name = status.ToString();
            }
        }


        public class ResultContainer(string? message, _Statuses status, int? code = null, Exception? e = null);
        public class ResultContainer<T>(T? data, string? message, _Statuses status, int? code = null, Exception? e = null) :
            ResultContainer(message,status,code,e)
        {
            public bool IsSuccess => status == _Statuses.Success;
            public bool HasException => status == _Statuses.Exception;
            public string? FullExceptionMessage => e?.Message + e?.InnerException?.Message;

            [JsonIgnore]
            public bool HasCustomStatusCode => code > 0;
        }

        [Benchmark]
        public void WithNormalResult()
        {
            string result = "Hello World!";

            var _ret = new ResultContainer<string>(result,"M",_Statuses.Success);
        }

        [Benchmark]
        public void WithOperatoinResult()
        {
            string result = "Hello World!";

            var op = new OperationResult<string>()
            {
                Data = result,
                Message = "M",
                Status = Statuses.Success,
               // StatusCode = 200,
            };
           
        }

        [Benchmark]
        public void WithOperatoinResultExtension()
        {
            string result = "Hello World!";

            var op = _Operation.SetSuccess(result, "M");//.WithStatusCode(200);
        }


        [Benchmark]
        public void IntoSimple()
        {
            var operation1 = _Operation.SetSuccess(new object());
            var operation2 = _Operation.SetSuccess(new object());
            var operation3 = _Operation.SetSuccess(new object());

            var result = operation1.Collect(operation1, operation2).
                Into((r1, r2, r3) => new { r1, r2, r3 });
        }

        [Benchmark]
        public async Task IntoSimpleAsync()
        {
            var operation1 = Task.FromResult(_Operation.SetSuccess(new object()));
            var operation2 = Task.FromResult(_Operation.SetSuccess(new object()));
            var operation3 = Task.FromResult(_Operation.SetSuccess(new object()));

            var result = await operation1.CollectAsync(operation1, operation2).
                IntoAsync((r1, r2, r3) => new { r1, r2, r3 });
        }


        private async Task<OperationResult<object>> Job()
        {
            await Task.Delay(40);
            return new OperationResult<object>();
        }

        private OperationResult<object> JobSync()
        {
            Task.Delay(40).ConfigureAwait(false).GetAwaiter().GetResult();
            return new OperationResult<object>();
        }

        [Benchmark]
        public void Into()
        {
            var operation1 = JobSync();
            var operation2 = JobSync();
            var operation3 = JobSync();

            var result = operation1.Collect(operation1, operation2).
                Into((r1, r2, r3) => new { r1, r2, r3 });
        }


        [Benchmark]
        public async Task IntoAwait()
        {
            var operation1 = await Job();
            var operation2 = await Job();
            var operation3 = await Job();

            var result = operation1.Collect(operation1, operation2).
                Into((r1, r2, r3) => new { r1, r2, r3 });
        }

        [Benchmark]
        public async Task IntoAsync()
        {
            var operation1 = Job();
            var operation2 = Job();
            var operation3 = Job();

            var result = await operation1.CollectAsync(operation1, operation2).
                IntoAsync((r1, r2, r3) => new { r1, r2, r3 });
        }

    }
}
