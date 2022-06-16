using Meteors;
using Meteors.OperationResult;
using Microsoft.AspNetCore.Mvc;
using OperationResult.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OperationResult.Tests
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
            } else

            if (type == Statuses.Exception)
            {
                Assert.True(operation.HasException);
                Assert.Equal(Seed.ToFullException(operation.Exception), result.Value?.ToString());
            }
            else
            if (type != Statuses.Success && type != Statuses.Exception)
            {
                Assert.Equal(type.ToString(), result.Value?.ToString());
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
                Assert.Equal(type.ToString(), result.Value?.ToString());
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
                Assert.Equal(type.ToString(), bodyResult.Message.ToString());
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
                Assert.Equal(type.ToString(), bodyResult.Message.ToString());
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
        public void IntoOnceReturnObj(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = operation.Into(o => new FooInto() { User = o.Data, 
                StatusCode = o.StatusCode??0 } );

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

    }
}
