using Meteors;
using Meteors.OperationContext;
using Microsoft.AspNetCore.Mvc;
using OperationContext.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OperationContext.Tests
{
    public class ExtensionTest
    {
        public static IEnumerable<object[]> FactData =>
        new List<object[]>
        {
            new object[] {  Statuses.Exception},
            new object[] { Statuses.NotExist},
            new object[] {Statuses.Exist},
            new object[] {  Statuses.Unauthorized},
            new object[] { Statuses.Forbidden},
            new object[] { Statuses.Failed},
            new object[] {  Statuses.Success},
            //Unknown not in case
        };


        [Fact]
        public void ToOperationResult()
        {
            var data = new FooUser();
            var operation = data.ToOperationResult();


            OperationResult<FooUser> operation1 = new();
            operation1.Data = data;
            operation1.Status = Statuses.Success;

            Assert.True(operation.Equals(operation1));
        }


        [Theory]
        [MemberData(nameof(FactData))]
        public void ToJsonResult(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = operation.ToJsonResult();

            Assert.Equal((int)type, result.StatusCode);

            if (type == Statuses.Success)
            {
                var data = result.Value as FooUser;
                Assert.NotNull(data);
            }
            else

            if (type == Statuses.Exception)
            {
                Assert.True(operation.HasException);
                Assert.Equal(Seed.ToFullException(operation.Exception), result.Value?.ToString());
            }
            else
            if (type != Statuses.Success && type != Statuses.Exception)
            {
                Assert.Equal(operation.Message, result.Value?.ToString());
            }

        }


        [Theory]
        [MemberData(nameof(FactData))]
        public async Task ToJsonResultAsync(Statuses type)
        {
            var operationAsync = Task.FromResult(Seed.Create<FooUser>(type));
            var operation = await operationAsync;
            var result = await operationAsync.ToJsonResultAsync();

            Assert.Equal((int)type, result.StatusCode);

            if (type == Statuses.Success)
            {
                var data = result.Value as FooUser;
                Assert.NotNull(data);
            }
            else

            if (type == Statuses.Exception)
            {
                Assert.True(operation.HasException);
                Assert.Equal(Seed.ToFullException(operation.Exception), result.Value?.ToString());
            }
            else
            if (type != Statuses.Success && type != Statuses.Exception)
            {
                Assert.Equal(operation.Message, result.Value?.ToString());
            }

        }



        [Theory]
        [MemberData(nameof(FactData))]
        public void ToJsonResult_IsBody(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = operation.ToJsonResult(true);

            Assert.Equal((int)type, result.StatusCode);

            OperationResult<FooUser> bodyResult = result.Value as OperationResult<FooUser>;
            Assert.NotNull(bodyResult);

            if (type == Statuses.Success)
            {
                var data = bodyResult.Data;
                Assert.NotNull(data);
            }
            else

            if (type == Statuses.Exception)
            {
                Assert.True(operation.HasException);
                Assert.Equal(Seed.ToFullException(operation.Exception), bodyResult?.FullExceptionMessage);
            }
            else
            if (type != Statuses.Success && type != Statuses.Exception)
            {
                Assert.Equal(operation.Message, bodyResult.Message.ToString());
            }

        }


        [Theory]
        [MemberData(nameof(FactData))]
        public async Task ToJsonResultAsync_IsBody(Statuses type)
        {
            var operationAsync = Task.FromResult(Seed.Create<FooUser>(type));
            var operation = await operationAsync;
            var result = await operationAsync.ToJsonResultAsync(true);

            Assert.Equal((int)type, result.StatusCode);

            OperationResult<FooUser> bodyResult = result.Value as OperationResult<FooUser>;
            Assert.NotNull(bodyResult);

            if (type == Statuses.Success)
            {
                var data = bodyResult.Data;
                Assert.NotNull(data);
            }
            else

            if (type == Statuses.Exception)
            {
                Assert.True(operation.HasException);
                Assert.Equal(Seed.ToFullException(operation.Exception), bodyResult?.FullExceptionMessage);
            }
            else
            if (type != Statuses.Success && type != Statuses.Exception)
            {
                Assert.Equal(operation.Message, bodyResult.Message.ToString());
            }

        }





        [Theory]
        [MemberData(nameof(FactData))]
        public void WithStatusCode(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = operation.WithStatusCode(507);

            Assert.Equal(507, result.StatusCode);
        }


        [Theory]
        [MemberData(nameof(FactData))]
        public async Task WithStatusCodeAsync(Statuses type)
        {
            var operation = Task.FromResult(Seed.Create<FooUser>(type));
            var result = await operation.WithStatusCodeAsync(507);

            Assert.Equal(507, result.StatusCode);
        }



        [Theory]
        [MemberData(nameof(FactData))]
        public void CollectOnce(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = operation.Collect();

            Assert.Equal(operation, result);
        }

        [Theory]
        [MemberData(nameof(FactData))]
        public async Task CollectOnceAsync(Statuses type)
        {
            var operation = Task.FromResult(Seed.Create<FooUser>(type));
            var result = operation.CollectAsync();

            Assert.Equal(await operation, await result);
        }

        [Theory]
        [MemberData(nameof(FactData))]
        public void IntoOnce(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = operation.Into(o => o);

            //this global Priority
            if (type == Statuses.Success)
                Assert.Equal(operation, result.Data);
            else if (type != Statuses.Exist && type != Statuses.NotExist)
            {
                Assert.Equal(operation.Data, result.Data?.Data);
                Assert.Equal(operation.Message, result.Message);
                Assert.Equal(operation.Status, result.Status);
                Assert.Equal(operation.Exception, result.Exception);
                Assert.Equal(operation.StatusCode, result.StatusCode);
            }
            else
            {
                Assert.Equal(operation.Data, result.Data?.Data);
                Assert.Equal(operation.Message, result.Message);
                Assert.Equal(Statuses.Success, result.Status);
                Assert.Equal(operation.Exception, result.Exception);
                Assert.Equal(operation.StatusCode, result.StatusCode);
            }

        }


        [Theory]
        [MemberData(nameof(FactData))]
        public async Task IntoOnceAsync(Statuses type)
        {
            var operationAsync = Task.FromResult(Seed.Create<FooUser>(type));
            var resultAsync = operationAsync.IntoAsync(o => o);

            var operation = await operationAsync;
            var result = await resultAsync;

            //this global Priority
            if (type == Statuses.Success)
                Assert.Equal(operation, result.Data);
            else if (type != Statuses.Exist && type != Statuses.NotExist)
            {
                Assert.Equal(operation.Data, result.Data?.Data);
                Assert.Equal(operation.Message, result.Message);
                Assert.Equal(operation.Status, result.Status);
                Assert.Equal(operation.Exception, result.Exception);
                Assert.Equal(operation.StatusCode, result.StatusCode);
            }
            else
            {
                Assert.Equal(operation.Data, result.Data?.Data);
                Assert.Equal(operation.Message, result.Message);
                Assert.Equal(Statuses.Success, result.Status);
                Assert.Equal(operation.Exception, result.Exception);
                Assert.Equal(operation.StatusCode, result.StatusCode);
            }

        }




        [Theory]
        [MemberData(nameof(FactData))]
        public void IntoOnceReturnObj(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = operation.Into(o => new FooInto()
            {
                User = o.Data,
                StatusCode = o.StatusCode ?? 0
            });

            //this global Priority
            if (type == Statuses.Success)
            {
                Assert.Equal(Statuses.Success, result.Status);

                Assert.Equal(operation.Data, result.Data.User);
            }
            else if (type != Statuses.Exist && type != Statuses.NotExist)
            {
                Assert.Equal(type, result.Status);

                Assert.Equal(operation.Data, result.Data?.User);

                Assert.Equal(operation.Message, result.Message);

                Assert.Equal(operation.Status, result.Status);
                Assert.Equal(operation.Exception, result.Exception);
                Assert.Equal(operation.StatusCode, result.StatusCode);
            }
            else
            {

                Assert.Equal(Statuses.Success, result.Status);

                Assert.Equal(operation.Data, result.Data?.User);
                Assert.Equal(operation.Message, result.Message);
                Assert.Equal(operation.Exception, result.Exception);
                Assert.Equal(operation.StatusCode, result.StatusCode);
            }

        }

        [Theory]
        [MemberData(nameof(FactData))]
        public async void IntoOnceReturnObjAsync(Statuses type)
        {
            var operationAsync = Task.FromResult(Seed.Create<FooUser>(type));
            var resultAsync = operationAsync.IntoAsync(o => new FooInto()
            {
                User = o.Data,
                StatusCode = o.StatusCode ?? 0
            });

            var operation = await operationAsync;
            var result = await resultAsync;


            //this global Priority
            if (type == Statuses.Success)
            {
                Assert.Equal(Statuses.Success, result.Status);

                Assert.Equal(operation.Data, result.Data.User);
            }
            else if (type != Statuses.Exist && type != Statuses.NotExist)
            {
                Assert.Equal(type, result.Status);

                Assert.Equal(operation.Data, result.Data?.User);

                Assert.Equal(operation.Message, result.Message);

                Assert.Equal(operation.Status, result.Status);
                Assert.Equal(operation.Exception, result.Exception);
                Assert.Equal(operation.StatusCode, result.StatusCode);
            }
            else
            {

                Assert.Equal(Statuses.Success, result.Status);

                Assert.Equal(operation.Data, result.Data?.User);
                Assert.Equal(operation.Message, result.Message);
                Assert.Equal(operation.Exception, result.Exception);
                Assert.Equal(operation.StatusCode, result.StatusCode);
            }

        }




        [Theory]
        [MemberData(nameof(FactData))]
        public void Collect(Statuses type)
        {
            var operation1 = Seed.Create<FooUser>(type);
            var operation2 = Seed.Create<FooUser>(type);
            var operation3 = Seed.Create<FooUser>(type);
            var result = operation1.Collect(operation2, operation3);

            Assert.Equal((operation1, operation2, operation3), result);
        }

        [Theory]
        [MemberData(nameof(FactData))]
        public async Task CollectAsync(Statuses type)
        {
            var operation1 = Task.FromResult(Seed.Create<FooUser>(type));
            var operation2 = Task.FromResult(Seed.Create<FooUser>(type));
            var operation3 = Task.FromResult(Seed.Create<FooUser>(type));
            var result = operation1.CollectAsync(operation2, operation3);


            Assert.Equal((await operation1, await operation2, await operation3), await result);
        }



        [Theory]
        [MemberData(nameof(FactData))]
        public void Into(Statuses type)
        {
            var type1 = Seed.RandomStatus();
            var type2 = Seed.RandomStatus();
            var operation1 = Seed.Create<FooUser>(type);
            var operation2 = Seed.Create<FooUser>(type1); //random type
            var operation3 = Seed.Create<FooUser>(type2);
            var result = (operation1, operation2, operation3).
                            Into((r1, r2, r3) => new FooInto
                            {
                                StatusCode = r1.StatusCode ?? 0,
                                OtherUsers = new List<FooUser>() { r2.Data, r3.Data }.ToList(),
                                User = r1.Data
                            });
            List<Statuses> types = new() { type, type1, type2 };

            //order requierd
            if (result.HasException)
            {
                var priorityException = types.Any(o => o == Statuses.Exception);
                Assert.True(priorityException, userMessage: $" {type} - {type1} - {type2} ");
                Assert.Equal(Statuses.Exception, result.Status);
            }
            else
            if (!result.IsSuccess && result.Status != Statuses.Exist && result.Status != Statuses.NotExist)
            {
                var priorityFailed = types.Any(o => o == Statuses.Failed || o == Statuses.Forbidden || o == Statuses.Unauthorized);
                Assert.True(priorityFailed, userMessage: $" {type} - {type1} - {type2} ");

                var maxFailded  = types.Where(result => result == Statuses.Failed || result == Statuses.Forbidden ||
                        result == Statuses.Unauthorized).Max(result => (Statuses?)result);

                Assert.Equal(maxFailded, result.Status);
            }
            else //success
            {
                //unknow ! 
                
                var prioritySuccess = types.All(o => o == Statuses.Success || o == Statuses.Exist || o == Statuses.NotExist);
                Assert.True(prioritySuccess,userMessage: $" {type} - {type1} - {type2} " );
                Assert.Equal(Statuses.Success, result.Status);
            }

        }

        [Theory]
        [MemberData(nameof(FactData))]
        public async Task IntoAsync(Statuses type)
        {
            var type1 = Seed.RandomStatus();
            var type2 = Seed.RandomStatus();
            var operation1 = Task.FromResult(Seed.Create<FooUser>(type));
            var operation2 = Task.FromResult(Seed.Create<FooUser>(type1)); //random type
            var operation3 = Task.FromResult(Seed.Create<FooUser>(type2));
            await Task.WhenAll(operation1, operation2, operation3);
            var result = await  Task.FromResult((await operation1, await operation2, await operation3)).
                            IntoAsync((r1, r2, r3) => new FooInto
                            {
                                StatusCode = r1.StatusCode ?? 0,
                                OtherUsers = new List<FooUser>() { r2.Data, r3.Data }.ToList(),
                                User = r1.Data
                            });
            List<Statuses> types = new() { type, type1, type2 };

            //order requierd
            if (result.HasException)
            {
                var priorityException = types.Any(o => o == Statuses.Exception);
                Assert.True(priorityException, userMessage: $" {type} - {type1} - {type2} ");
                Assert.Equal(Statuses.Exception, result.Status);
            }
            else
            if (!result.IsSuccess && result.Status != Statuses.Exist && result.Status != Statuses.NotExist)
            {
                var priorityFailed = types.Any(o => o == Statuses.Failed || o == Statuses.Forbidden || o == Statuses.Unauthorized);
                Assert.True(priorityFailed, userMessage: $" {type} - {type1} - {type2} ");

                var maxFailded = types.Where(result => result == Statuses.Failed || result == Statuses.Forbidden ||
                       result == Statuses.Unauthorized).Max(result => (Statuses?)result);

                Assert.Equal(maxFailded, result.Status);
            }
            else //success
            {
                //unknow ! 

                var prioritySuccess = types.All(o => o == Statuses.Success || o == Statuses.Exist || o == Statuses.NotExist);
                Assert.True(prioritySuccess, userMessage: $" {type} - {type1} - {type2} ");
                Assert.Equal(Statuses.Success, result.Status);
            }

        }
    }
}
