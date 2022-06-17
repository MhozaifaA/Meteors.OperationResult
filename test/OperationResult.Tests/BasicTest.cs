using Meteors;
using Meteors.OperationResult;
using OperationResult.Tests.Mocks;
using System;
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
            new object[] { (FooUser)null , Statuses.Exception},
            new object[] { new FooUser("other","1234"), Statuses.NotExist},
            new object[] { new FooUser("Admin", null), Statuses.Exist}, // null password mean containt if exist
            new object[] { new FooUser("guest",string.Empty), Statuses.Unauthorized},
            new object[] { new FooUser("guest","1234"), Statuses.Forbidden},//con't guest to have password
            new object[] { new FooUser("Admin","password"), Statuses.Failed},
            new object[] { new FooUser("Admin","P@$$W0rd"), Statuses.Success},
        };



        [Fact]
        public void ToPerString()
        {
            foreach (var status in System.Enum.GetValues<Statuses>())
            {
                Assert.Equal(status.ToString(), status.ToPerString());
            }
        }


        [Fact]
        public OperationResult<FooUser> UnknownStatus()
        {
            OperationResult<FooUser> operation = new OperationResult<FooUser>();

            //act as success
            operation.Data = new FooUser();
            operation.Message = "Success";

            Assert.Equal(Statuses.UnKnown, operation.Status);

            return operation;
        }


        [Fact]
        public void TupleList()
        {
            var type1 = Seed.RandomStatus();
            var type2 = Seed.RandomStatus();
            var type3 = Seed.RandomStatus();

            var operation1 = Seed.Create<FooUser>(type1);
            var operation2 = Seed.Create<FooUser>(type1); //random type
            var operation3 = Seed.Create<FooUser>(type3);

            Tuple<OperationResult<FooUser>, OperationResult<FooUser>, OperationResult<FooUser>> results = new (
                operation1,
                operation2,
                operation3);

            System.Runtime.CompilerServices.ITuple Iresults = results;

            List<OperationResultBase> listResult = new();
            for (int i = 0; i < Iresults.Length; i++)
            {
                listResult.Add((OperationResultBase)Iresults[i]);
            }
            //List<OperationResultBase> listResult = Enumerable.Repeat(0, Iresults.Length)
            //    .Select(index => Iresults[index]).Cast<OperationResultBase>().ToList();


            Assert.Equal(operation1.Status, listResult[0].Status);
            Assert.Equal(operation2.Status, listResult[1].Status);
            Assert.Equal(operation3.Status, listResult[2].Status);

            //act as success
        }



        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void CastToBase(Statuses type)
        {
            OperationResult<FooUser> operation = Seed.Create<FooUser>(type);
            var operationBase = (OperationResultBase) operation;

            Assert.Equal(operation.Status ,operationBase.Status);
            Assert.Equal(operation.StatusCode, operationBase.StatusCode);
            Assert.Equal(operation.Message ,operationBase.Message);
            Assert.Equal(operation.Exception ,operationBase.Exception);
        }

        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void IsSuccess(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);

            if (type == Statuses.Success)
                Assert.True(operation.IsSuccess);
             else
                Assert.False(operation.IsSuccess);
        }


        [Theory]
        [MemberData(nameof(ExtensionTest.FactData), MemberType = typeof(ExtensionTest))]
        public void HasException(Statuses type)
        {
            var operation = Seed.Create<FooUser>(type);

            if (type == Statuses.Exception)
                Assert.True(operation.HasException);
            else
                Assert.False(operation.HasException);
        }


        [Theory]
        [MemberData(nameof(FactData))]
        public OperationResult<FooUser> AsRegular(FooUser user, Statuses resultTypes)
        {
            OperationResult<FooUser> operation = new OperationResult<FooUser>();

            try
            {
                if (user is null)
                    throw new System.NullReferenceException();

                var one = Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if (one is null)
                {
                    operation.Status = Statuses.NotExist;
                    operation.Message = "message";
                }
                else if (user.Password is null)
                {
                    operation.Status = Statuses.Exist;
                    operation.Message = $"{one.UserName} found";
                }
                else if (one.UserName == "guest" && user.Password == string.Empty)
                {
                    operation.Status = Statuses.Unauthorized;
                    operation.Message = $"{one.UserName} is guest";
                }
                else if (one.UserName == "guest" && user.Password.Length > 0)
                {
                    operation.Status = Statuses.Forbidden;
                    operation.Message = $"{one.UserName} is guest con't have password";
                }
                else if (one.Password != user.Password)
                {
                    operation.Status = Statuses.Failed;
                    operation.Message = $"{one.UserName} faild to access";
                }
                else if (one.Password == user.Password)
                {
                    operation.Status = Statuses.Success;
                    operation.Message = $"Success to access";
                    operation.Data = one; //set result
                }
                else
                    Assert.NotNull(null); //not match any facts
            }
            catch (System.Exception e)
            {
                operation.Exception = e;
                operation.Status = Statuses.Exception;
            }

            Assert.Equal(resultTypes, operation.Status);
            return operation;
        }




        [Theory]
        [MemberData(nameof(FactData))]
        public OperationResult<FooUser> AsMethod(FooUser user, Statuses resultTypes)
        {
            OperationResult<FooUser> operation = new OperationResult<FooUser>();

            try
            {
                if (user is null)
                    throw new System.NullReferenceException();

                var one = Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if (one is null)
                {
                    operation.SetContent(Statuses.NotExist, "message");
                }
                else if (user.Password is null)
                {
                    operation.SetContent(Statuses.Exist, $"{one.UserName} found");
                }
                else if (one.UserName == "guest" && user.Password == string.Empty)
                {
                    operation.SetFailed($"{one.UserName} is guest", Statuses.Unauthorized);
                }
                else if (one.UserName == "guest" && user.Password.Length > 0)
                {
                    operation.SetFailed($"{one.UserName} is guest con't have password", Statuses.Forbidden);
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

            Assert.Equal(resultTypes, operation.Status);
            return operation;
        }




        [Theory]
        [MemberData(nameof(FactData))]
        public OperationResult<FooUser> AsStatic(FooUser user, Statuses resultTypes)
        {
            OperationResult<FooUser> operation = null;

            try
            {
                if (user is null)
                    throw new System.NullReferenceException();

                var one = Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if (one is null)
                {
                    operation = _Operation.SetContent<FooUser>(Statuses.NotExist, "message");
                }
                else if (user.Password is null)
                {
                    operation = _Operation.SetContent<FooUser>(Statuses.Exist, $"{one.UserName} found");
                }
                else if (one.UserName == "guest" && user.Password == string.Empty)
                {
                    operation = _Operation.SetFailed<FooUser>($"{one.UserName} is guest", Statuses.Unauthorized);
                }
                else if (one.UserName == "guest" && user.Password.Length > 0)
                {
                    operation = _Operation.SetFailed<FooUser>($"{one.UserName} is guest con't have password", Statuses.Forbidden);
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

            Assert.Equal(resultTypes, operation.Status);
            return operation;
        }




        [Theory]
        [MemberData(nameof(FactData))]
        public OperationResult<FooUser> AsImplicit(FooUser user, Statuses resultTypes)
        {
            OperationResult<FooUser> operation = null;

            try
            {
                if (user is null)
                    throw new System.NullReferenceException();

                var one = Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if (one is null)
                {
                    operation = (Statuses.NotExist, "message");
                }
                else if (user.Password is null)
                {
                    operation = (Statuses.Exist, $"{one.UserName} found");
                }
                else if (one.UserName == "guest" && user.Password == string.Empty)
                {
                    operation = ($"{one.UserName} is guest", Statuses.Unauthorized);
                }
                else if (one.UserName == "guest" && user.Password.Length > 0)
                {
                    operation = ($"{one.UserName} is guest con't have password", Statuses.Forbidden);
                }
                else if (one.Password != user.Password)
                {
                    operation = ($"{one.UserName} faild to access", Statuses.Failed);
                }
                else if (one.Password == user.Password)
                {
                    operation = (one, $"Success to access");
                }
                else
                    Assert.NotNull(null); //not match any facts
            }
            catch (System.Exception e)
            {
                operation = e; //not supporeted yet
            }

            Assert.Equal(resultTypes, operation.Status);
            return operation;
        }

    }
}