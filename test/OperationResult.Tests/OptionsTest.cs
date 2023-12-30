using Meteors;
using Meteors.OperationContext;
using OperationContext.Tests.Mocks;
using Xunit;

namespace OperationContext.Tests
{
    public class OptionsTest
    {
        const string Skip = "contain static value";

        [Fact(Skip = Skip)]
        public void IsBody()
        {
            var operation =Seed.Create<FooUser>(Statuses.Success);

            var result1 =  operation.ToJsonResult();
            FooUser bodyResult1 = result1.Value as FooUser;
            Assert.NotNull(bodyResult1);


            var result2 = operation.ToJsonResult(true);
            OperationResult<FooUser> bodyResult2 = result2.Value as OperationResult<FooUser>;
            Assert.NotNull(bodyResult2);


            OperationResultOptions.IsBody(true);
            var result3 = operation.ToJsonResult();
            OperationResult<FooUser> bodyResult3 = result3.Value as OperationResult<FooUser>;
            Assert.NotNull(bodyResult3);


            OperationResultOptions.IsBody(false);
            var result4 = operation.ToJsonResult();
            FooUser bodyResult4 = result4.Value as FooUser;
            Assert.NotNull(bodyResult4);


            OperationResultOptions.IsBody(true);
            var result5 = operation.ToJsonResult(true);
            OperationResult<FooUser> bodyResult5 = result3.Value as OperationResult<FooUser>;
            Assert.NotNull(bodyResult5);

            OperationResultOptions.IsBody(false);
            var result6 = operation.ToJsonResult(true);
            OperationResult<FooUser> bodyResult6 = result6.Value as OperationResult<FooUser>;
            Assert.NotNull(bodyResult6);



            OperationResultOptions.IsBody(true);
            var result7 = operation.ToJsonResult(false);
            FooUser bodyResult7 = result7.Value as FooUser;
            Assert.NotNull(bodyResult7);

            OperationResultOptions.IsBody(false);
            var result8 = operation.ToJsonResult(false);
            FooUser bodyResult8 = result8.Value as FooUser;
            Assert.NotNull(bodyResult8);

        }

        [Fact(Skip = Skip)]
        public void IntoBody()
        {
            var operation = Seed.Create<FooUser>(Statuses.Success);
            operation.Data.Password = "P@$$w0rd";
            OperationResultOptions.IsBody(true); //can
            OperationResultOptions.IntoBody(op => new FooIntoBody
            {
             Message = op.Message + " "+ op.Status.ToString(),
             User = op.Data,
             StatusCode = op.StatusCode??500,
             PasswordLength = ((FooUser)op.Data).Password.Length
            });

            var result = operation.ToJsonResult(); // operation.ToJsonResult(true);
            FooIntoBody bodyResult = result.Value as FooIntoBody;
            Assert.NotNull(bodyResult);

        }
    }
}
