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

    }
}
