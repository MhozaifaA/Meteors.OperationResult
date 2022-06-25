using System;
using System.Text;

namespace Meteors.OperationResult.ExtensionMethods
{
    /// <summary>
    /// basics Extensions
    /// </summary>
    internal static class ExceptionExtension
    {
        /// <summary>
        /// return full message of <see cref="Exception"/>  inner  and depth
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        internal static string? ToFullException(this System.Exception? exception)
        {
            if (exception is null) return null;
            StringBuilder FullMessage = new();
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
