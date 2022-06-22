using Meteors;
using Meteors.OperationResult;
using OperationResult.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace OperationResult.Tests
{
    public class OperationTest
    {

        private readonly ITestOutputHelper output;
        public OperationTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void OperationToJsonResult(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = Seed.Create<FooUser>(type).ToJsonResult();

            var jsontxt = string.Empty;
            if(operation.HasException)
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.FullExceptionMessage);
            }
            else
            if(operation.IsSuccess)
            {
                jsontxt =  System.Text.Json.JsonSerializer.Serialize(operation.Data);
            }else
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.Message);
            }

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result.Value);

            Assert.Equal(jsontxt, jsonResult);
        }

        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public async Task OperationToJsonResultAsync(Statuses type)
        {
            var operation =await Task.FromResult(Seed.Create<FooUser>(type));
            var result =await Task.FromResult(Seed.Create<FooUser>(type)).ToJsonResultAsync();

            var jsontxt = string.Empty;
            if (operation.HasException)
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.FullExceptionMessage);
            }
            else
            if (operation.IsSuccess)
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.Data);
            }
            else
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.Message);
            }

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result.Value);

            Assert.Equal(jsontxt, jsonResult);
        }



        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void OperationToJsonResultBody(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            operation.StatusCode = (int)type;
            var result = Seed.Create<FooUser>(type).ToJsonResult(true);

            var jsontxt = string.Empty;
            jsontxt = System.Text.Json.JsonSerializer.Serialize(operation);

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result.Value);

            Assert.Equal(jsontxt, jsonResult);
        }

        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public async Task OperationToJsonResultBodyAsync(Statuses type)
        {
            var operation =await Task.FromResult(Seed.Create<FooUser>(type));
            operation.StatusCode = (int)type;
            var result =await Task.FromResult(Seed.Create<FooUser>(type)).ToJsonResultAsync(true);

            var jsontxt = string.Empty;
            jsontxt = System.Text.Json.JsonSerializer.Serialize(operation);

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result.Value);

            Assert.Equal(jsontxt, jsonResult);
        }


        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void OperationWithStatusCodeToJsonResult(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = Seed.Create<FooUser>(type).WithStatusCode(467).ToJsonResult();

            operation.StatusCode = 467;
            var jsontxt = string.Empty;
            if (operation.HasException)
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.FullExceptionMessage);
            }
            else
            if (operation.IsSuccess)
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.Data);
            }
            else
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.Message);
            }

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result.Value);

            Assert.Equal(jsontxt, jsonResult);
        }

        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public async Task OperationWithStatusCodeToJsonResultAsync(Statuses type)
        {
            var operation =await Task.FromResult(Seed.Create<FooUser>(type));
            var result =await Task.FromResult(Seed.Create<FooUser>(type)).WithStatusCodeAsync(467).ToJsonResultAsync();

                operation.StatusCode = 467;
            var jsontxt = string.Empty;
            if (operation.HasException)
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.FullExceptionMessage);
            }
            else
            if (operation.IsSuccess)
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.Data);
            }
            else
            {
                jsontxt = System.Text.Json.JsonSerializer.Serialize(operation.Message);
            }

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result.Value);

            Assert.Equal(jsontxt, jsonResult);
        }


        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void OperationWithStatusCodeToJsonResultBody(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var result = Seed.Create<FooUser>(type).WithStatusCode(476).ToJsonResult(true);

            operation.StatusCode = 476;
            var jsontxt = string.Empty;
            jsontxt = System.Text.Json.JsonSerializer.Serialize(operation);

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result.Value);
            output.WriteLine(jsontxt);
            output.WriteLine(jsonResult);
            Assert.Equal(jsontxt, jsonResult);
        }


        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public async Task OperationWithStatusCodeToJsonResultBodyAsync(Statuses type)
        {
            var operation =await Task.FromResult(Seed.Create<FooUser>(type));
            var result = await Task.FromResult(Seed.Create<FooUser>(type)).WithStatusCodeAsync(476).ToJsonResultAsync(true);

            operation.StatusCode = 476;
            var jsontxt = string.Empty;
            jsontxt = System.Text.Json.JsonSerializer.Serialize(operation);

            var jsonResult = System.Text.Json.JsonSerializer.Serialize(result.Value);
            output.WriteLine(jsontxt);
            output.WriteLine(jsonResult);
            Assert.Equal(jsontxt, jsonResult);
        }



        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void OperationCollectOnceIntoOnceToJsonResult(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);
            var col = operation.Collect();

           //same concept in Extension-Test
            var result = col.Into(o => new FooInto()
            {
                User = o.Data,
                StatusCode = o.StatusCode ?? 0
            });


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
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public async Task OperationCollectOnceIntoOnceToJsonResultAsync(Statuses type)
        {
            var operationAsync = Task.FromResult(Seed.Create<FooUser>(type));
            var col = operationAsync.CollectAsync();

            //same concept in Extension-Test
            var resultAsync = operationAsync.IntoAsync(o => new FooInto()
            {
                User = o.Data,
                StatusCode = o.StatusCode ?? 0
            });

            var operation = await operationAsync;
            var result = await resultAsync;


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
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void OperationCollectIntoToJsonResult(Statuses type)
        {
            var type1 = Seed.RandomStatus();
            var type2 = Seed.RandomStatus();
            var operation1 = Seed.Create<FooUser>(type);
            var operation2 = Seed.Create<FooUser>(type1);
            var operation3 = Seed.Create<FooUser>(type2);
            var col = operation1.Collect(operation2, operation3);

     
            var result = col.
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



        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public async void OperationCollectIntoToJsonResultAsync(Statuses type)
        {
            var type1 = Seed.RandomStatus();
            var type2 = Seed.RandomStatus();

            var operation1 = Task.FromResult(Seed.Create<FooUser>(type));
            var operation2 = Task.FromResult(Seed.Create<FooUser>(type1));
            var operation3 = Task.FromResult(Seed.Create<FooUser>(type2));
            var col = operation1.CollectAsync(operation2, operation3);

           
            var result =await col.
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
