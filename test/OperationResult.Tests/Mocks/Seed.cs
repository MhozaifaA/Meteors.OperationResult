using Meteors;
using Meteors.OperationContext;
using System;
using System.Text;

namespace OperationContext.Tests.Mocks
{
    public class Seed
    {
        internal static OperationResult<T> Create<T>(Statuses type)
        {
            switch (type.value)
            {
                case _Statuses.Success:
                    return _Operation.SetSuccess<T>(Activator.CreateInstance<T>(), nameof(OperationResult.Message) + type.ToString());
                case _Statuses.Exist:
                    return _Operation.SetContent<T>(type, nameof(OperationResult.Message) + type.ToString());
                case _Statuses.NotExist:
                    return _Operation.SetContent<T>(type, nameof(OperationResult.Message) + type.ToString());
                case _Statuses.Failed:
                    return _Operation.SetFailed<T>(nameof(OperationResult.Message) + type.ToString());
                case _Statuses.Forbidden:
                    return _Operation.SetFailed<T>(nameof(OperationResult.Message) + type.ToString(), type);
                case _Statuses.Exception:
                    return _Operation.SetException<T>(new Exception(nameof(OperationResult.Message) + type.ToString()));
                case _Statuses.Unauthorized:
                    return _Operation.SetFailed<T>(nameof(OperationResult.Message) + type.ToString(), type);
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

        private static Statuses[] StatusList = new[] {
            //Statuses.UnKnown,
            Statuses.Unauthorized,
            Statuses.Success,
            Statuses.Exist,
            Statuses.NotExist,
            Statuses.Failed,
            Statuses.Forbidden,
            Statuses.Exception,
        };
        internal static Statuses RandomStatus()
        {
           return StatusList[Random.Shared.Next(1, StatusList.Length) - 1];
        }

    }
}
