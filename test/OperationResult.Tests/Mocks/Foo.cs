using System.Collections.Generic;

namespace OperationContext.Tests.Mocks
{
    public class FooUser
    {
        public FooUser(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public FooUser() {}

        public string UserName { get; set; }
        public string Password { get; set; }

        public bool EqulaInner(FooUser foo)
        {
            return foo.UserName.Equals(UserName) && foo.Password.Equals(Password);
        }
    }

    public class FooInto
    {
        public FooUser User { get; set; }
        public IEnumerable<FooUser> OtherUsers { get; set; }        
        public int StatusCode { get; set; }
    }

    public  class FooIntoBody
    {
        public FooUser User { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public int PasswordLength { get; set; }
    }
}
