using System;
using System.Text;

namespace ExtensionMethods.Helper.Exception
{
    /// <summary>
    /// basics extesions
    /// </summary>
    internal static class ExceptionExtension
    {
        /// <summary>
        /// return full message of <see cref="Exception"/>  inner  and depth
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        internal static string ToFullException(this System.Exception exception)
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
