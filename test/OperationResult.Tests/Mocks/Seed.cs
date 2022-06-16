using Meteors;
using Meteors.OperationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult.Tests.Mocks
{
    public class Seed
    {
        public static OperationResult<T> Create<T>(Statuses type)
        {
            switch (type)
            {
                case Statuses.Success:
                    return _Operation.SetSuccess<T>(Activator.CreateInstance<T>());
                case Statuses.Exist:
                    return _Operation.SetContent<T>(type, type.ToString());
                case Statuses.NotExist:
                    return _Operation.SetContent<T>(type, type.ToString());
                case Statuses.Failed:
                    return _Operation.SetFailed<T>(type.ToString());
                case Statuses.Forbidden:
                    return _Operation.SetFailed<T>(type.ToString(), type);
                case Statuses.Exception:
                    return _Operation.SetException<T>(new Exception(type.ToString()));
                case Statuses.Unauthorized:
                    return _Operation.SetFailed<T>(type.ToString(), type);
                default:
                    return new NotImplementedException();
            }
        }

        internal static string ToFullException(Exception exception)
        {
            StringBuilder FullMessage = new StringBuilder();
            return Recursive(exception);
            //local function
            string Recursive(System.Exception deep)
            {
                FullMessage.Append(Environment.NewLine + deep.ToString() + Environment.NewLine + deep.Message);
                if (deep.InnerException is null) return FullMessage.ToString();
                return Recursive(deep.InnerException);
            }
        }

    }
}
