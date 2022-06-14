using Meteors;
using OperationResult.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OperationResult.Tests
{

    public class BasicTest
    {
        public static IEnumerable<FooUser> Users => new List<FooUser>() {
            new ("Admin","P@$$W0rd"),
            new ("user","1234"),
            new ("guest",""),
        };
        public static IEnumerable<object[]> FactData =>
        new List<object[]>
        {
            new object[] { (FooUser)null , OperationResultTypes.Exception},
            new object[] { new FooUser("other","1234"), OperationResultTypes.NotExist},
            new object[] { new FooUser("Admin", null), OperationResultTypes.Exist}, // null password mean containt if exist
            new object[] { new FooUser("guest",string.Empty), OperationResultTypes.Unauthorized},
            new object[] { new FooUser("guest","1234"), OperationResultTypes.Forbidden},//con't guest to have password
            new object[] { new FooUser("Admin","password"), OperationResultTypes.Failed},
            new object[] { new FooUser("Admin","P@$$W0rd"), OperationResultTypes.Success},
        };

       

        [Theory]
        [MemberData(nameof(FactData))]
        public OperationResult<FooUser> AsRegular(FooUser user, OperationResultTypes resultTypes)
        {
            OperationResult<FooUser> operation = new OperationResult<FooUser>();

            try
            {
                if (user is null)
                    throw new System.NullReferenceException();

                var one = Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if(one is null)
                {
                    operation.OperationResultType = OperationResultTypes.NotExist;
                    operation.Message = "message";
                }
                else if(user.Password is null)
                {
                    operation.OperationResultType = OperationResultTypes.Exist;
                    operation.Message = $"{one.UserName} found";
                } else if(one.UserName == "guest" && user.Password == string.Empty)
                {
                    operation.OperationResultType = OperationResultTypes.Unauthorized;
                    operation.Message = $"{one.UserName} is guest";
                }
                else if (one.UserName == "guest" && user.Password.Length > 0 )
                {
                    operation.OperationResultType = OperationResultTypes.Forbidden;
                    operation.Message = $"{one.UserName} is guest con't have password";
                }
                else if (one.Password != user.Password )
                {
                    operation.OperationResultType = OperationResultTypes.Failed;
                    operation.Message = $"{one.UserName} faild to access";
                }
                else if (one.Password == user.Password)
                {
                    operation.OperationResultType = OperationResultTypes.Success;
                    operation.Message = $"Success to access";
                    operation.Data = one; //set result
                }else
                     Assert.NotNull(null); //not match any facts
            }
            catch (System.Exception e)
            {
                operation.Exception = e;
                operation.OperationResultType = OperationResultTypes.Exception;
            }

            Assert.Equal(resultTypes, operation.OperationResultType); 
            return operation;
        }




        [Theory]
        [MemberData(nameof(FactData))]
        public OperationResult<FooUser> AsMethod(FooUser user, OperationResultTypes resultTypes)
        {
            OperationResult<FooUser> operation = new OperationResult<FooUser>();

            try
            {
                if (user is null)
                    throw new System.NullReferenceException();

                var one = Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if (one is null)
                {
                    operation.SetContent(OperationResultTypes.NotExist,"message");
                }
                else if (user.Password is null)
                {
                    operation.SetContent(OperationResultTypes.Exist, $"{one.UserName} found");
                }
                else if (one.UserName == "guest" && user.Password == string.Empty)
                {
                    operation.SetFailed($"{one.UserName} is guest" , OperationResultTypes.Unauthorized);
                }
                else if (one.UserName == "guest" && user.Password.Length > 0)
                {
                    operation.SetFailed($"{one.UserName} is guest con't have password", OperationResultTypes.Forbidden);
                }
                else if (one.Password != user.Password)
                {
                    operation.SetFailed($"{one.UserName} faild to access");
                }
                else if (one.Password == user.Password)
                {
                    operation.SetSuccess(one, $"Success to access");
                }
                else
                    Assert.NotNull(null); //not match any facts
            }
            catch (System.Exception e)
            {
                operation.SetException(e);
            }

            Assert.Equal(resultTypes, operation.OperationResultType);
            return operation;
        }




        [Theory]
        [MemberData(nameof(FactData))]
        public OperationResult<FooUser> AsStatic(FooUser user, OperationResultTypes resultTypes)
        {
            OperationResult<FooUser> operation = null;

            try
            {
                if (user is null)
                    throw new System.NullReferenceException();

                var one = Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if (one is null)
                {
                    operation = _Operation.SetContent<FooUser>(OperationResultTypes.NotExist, "message");
                }
                else if (user.Password is null)
                {
                    operation = _Operation.SetContent<FooUser>(OperationResultTypes.Exist, $"{one.UserName} found");
                }
                else if (one.UserName == "guest" && user.Password == string.Empty)
                {
                    operation = _Operation.SetFailed<FooUser>($"{one.UserName} is guest", OperationResultTypes.Unauthorized);
                }
                else if (one.UserName == "guest" && user.Password.Length > 0)
                {
                    operation = _Operation.SetFailed<FooUser>($"{one.UserName} is guest con't have password", OperationResultTypes.Forbidden);
                }
                else if (one.Password != user.Password)
                {
                    operation = _Operation.SetFailed<FooUser>($"{one.UserName} faild to access");
                }
                else if (one.Password == user.Password)
                {
                    operation = _Operation.SetSuccess<FooUser>(one, $"Success to access");
                }
                else
                    Assert.NotNull(null); //not match any facts
            }
            catch (System.Exception e)
            {
                operation = _Operation.SetException<FooUser>(e);
            }

            Assert.Equal(resultTypes, operation.OperationResultType);
            return operation;
        }




        [Theory]
        [MemberData(nameof(FactData))]
        public OperationResult<FooUser> AsImplicit(FooUser user, OperationResultTypes resultTypes)
        {
            OperationResult<FooUser> operation = null;

            try
            {
                if (user is null)
                    throw new System.NullReferenceException();

                var one = Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if (one is null)
                {
                    operation = (OperationResultTypes.NotExist, "message");
                }
                else if (user.Password is null)
                {
                    operation = (OperationResultTypes.Exist, $"{one.UserName} found");
                }
                else if (one.UserName == "guest" && user.Password == string.Empty)
                {
                    operation = ($"{one.UserName} is guest", OperationResultTypes.Unauthorized);
                }
                else if (one.UserName == "guest" && user.Password.Length > 0)
                {
                    operation = ($"{one.UserName} is guest con't have password", OperationResultTypes.Forbidden);
                }
                else if (one.Password != user.Password)
                {
                    operation =($"{one.UserName} faild to access", OperationResultTypes.Failed);
                }
                else if (one.Password == user.Password)
                {
                    operation = (one , $"Success to access");
                }
                else
                    Assert.NotNull(null); //not match any facts
            }
            catch (System.Exception e)
            {
                operation = e; //not supporeted yet
            }

            Assert.Equal(resultTypes, operation.OperationResultType);
            return operation;
        }

    }
}