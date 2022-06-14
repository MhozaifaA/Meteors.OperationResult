using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult.Tests.Mocks
{
    public class FooUser
    {
        public FooUser(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public FooUser()
        {

        }

        public string UserName { get; set; }
        public string Password { get; set; }

      
        public bool Check(string userName, string password)
        {
            return UserName.Equals(userName) && Password.Equals(password);
        }
    }
}
