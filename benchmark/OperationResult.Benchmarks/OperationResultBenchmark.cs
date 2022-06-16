using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc;
using Meteors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meteors.OperationResult;

namespace OperationResult.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, targetCount: 1)]
    public class OperationResultBenchmark
    {
        public class OperationNewBase
        {
            public int status { get; set; }

            /// <summary>
            /// Flag enter disposed
            /// </summary>
            bool disposed = false;

            /// <summary>
            /// Destructor
            /// </summary>
            ~OperationNewBase()
            {
                this.Dispose(false);
            }




            /// <summary>
            /// The virtual dispose method that allows
            /// classes inherited from this one to dispose their resources.
            /// </summary>
            /// <param name="disposing"></param>
            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    //if (disposing) {//}
                    status = 0;
                }

                disposed = true;
            }

        }
        public class OperationNew<T>: OperationNewBase
        {
            public T Result { get; set; }



        }
        private string ExecuteFun()
        {
            return "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"; ;
        }

        private async Task<string> ExecuteFunAsync()
        {
            await Task.Delay(0);
            return "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        }

        private OperationNew<string> ExecuteFunNew()
        {
            OperationNew<string> op = new();
            op.Result = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            op.status = 200;
            return op;
        }

        private async Task<OperationNew<string>> ExecuteFunNewAsync()
        {
            await Task.Delay(0);
            OperationNew<string> op = new();
            op.Result = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

            op.status = 200;
            return op;

        }

        private OperationResult<int> ExecuteFunImplicity() {
            return 0.ToOperationResult();                            
        }
       
        private async Task<OperationResult<int>> ExecuteFunImplicityAsync()
        {
            await Task.Delay(0);
            return 0.ToOperationResult();
        }

        private OperationResult<int> ExecuteFunEx()
        {
            return _Operation.SetSuccess(0);
        }

        private async Task<OperationResult<int>> ExecuteFunExAsync()
        {
            await Task.Delay(0);
            return _Operation.SetSuccess(0);
        }


        private OperationResult<int> ExecuteFunOp()
        {
            OperationResult<int> op = new();
            op.Data = 0;
            op.Status = Statuses.Success;
            return op;
        }

        private async Task<OperationResult<int>> ExecuteFunOpAsync()
        {
            await Task.Delay(0);
            OperationResult<int> op = new();
            op.Data = 0;
            op.Status = Statuses.Success;
            return op;
        }
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

        [Benchmark]
        public void StatusToPerString()
        {
            foreach (var status in StatusList)
            {
                var name = status.ToPerString();
            }
        }


        //[Benchmark]
        //public void ExecuteFunTo()
        //{
        //    new JsonResult(ExecuteFun()) { StatusCode = 200 };
        //}



        //[Benchmark]
        //public async Task ExecuteFunToAsync()
        //{
        //    new JsonResult(await ExecuteFunAsync()) { StatusCode = 200 };
        //}




        //[Benchmark]
        //public void ExecuteFunNewTo()
        //{
        //    new JsonResult(ExecuteFunNew()) { StatusCode = 200 };
        //}



        //[Benchmark]
        //public async Task ExecuteFunToNewAsync()
        //{
        //    new JsonResult(await ExecuteFunNewAsync()) { StatusCode = 200 };
        //}


        //[Benchmark]
        //public void ImplicityToJsonResult()
        //{
        //    ExecuteFunImplicity().ToJsonResult();
        //}

      

        //[Benchmark]
        //public async Task ImplicityToJsonResultAsync()
        //{
        //    await ExecuteFunImplicityAsync().ToJsonResultAsync();
        //}


        //[Benchmark]
        //public void ExToJsonResult()
        //{
        //    ExecuteFunEx().ToJsonResult();
        //}

        //[Benchmark]
        //public async Task ExToJsonResultAsync()
        //{
        //    await ExecuteFunExAsync().ToJsonResultAsync();
        //}

        //[Benchmark]
        //public void OpToJsonResult()
        //{
        //    ExecuteFunOp().ToJsonResult();
        //}

        //[Benchmark]
        //public async Task OpToJsonResultAsync()
        //{
        //    await ExecuteFunOpAsync().ToJsonResultAsync();
        //}
    }
}
