using Meteors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationResult.Tests.Mocks
{
    public class Seed
    {
        public static OperationResult<T> Create<T>(OperationResultTypes type)
        {
            switch (type)
            {
                case OperationResultTypes.Success:
                    return _Operation.SetSuccess<T>(Activator.CreateInstance<T>());
                    break;
                case OperationResultTypes.Exist:
                    return _Operation.SetContent<T>(type, type.ToString());
                    break;
                case OperationResultTypes.NotExist:
                    return _Operation.SetContent<T>(type, type.ToString());
                    break;
                case OperationResultTypes.Failed:
                    return _Operation.SetFailed<T>(type.ToString());
                    break;
                case OperationResultTypes.Forbidden:
                    return _Operation.SetFailed<T>(type.ToString(), type);
                    break;
                case OperationResultTypes.Exception:
                    return _Operation.SetException<T>(new Exception(type.ToString()));
                    break;
                case OperationResultTypes.Unauthorized:
                    return _Operation.SetFailed<T>(type.ToString(), type);
                    break;
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
