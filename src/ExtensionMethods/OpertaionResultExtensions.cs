/* MIT License

Copyright (c) 2022 Huzaifa Aseel

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

using Meteors.OperationContext;
using Meteors.OperationContext.ExtensionMethods;
using System.Threading.Tasks;

namespace Meteors
{

    /// <summary>
    /// Helper extensions of <see cref="OperationResult{T}"/>.
    /// </summary>
    public static class OpertaionResultExtensions
    {

        /// <summary>
        /// Encapsulation object to <see cref="OperationResult{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> ToOperationResult<T>(this T @object)
         => new OperationResult<T>().SetSuccess(@object);


        /// <summary>
        /// Helper to append messge, this will affect on base,
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="Message">start with capital as base</param>
        /// <param name="space">join with space " " or ""</param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult Append(this OperationResult operation, string Message, bool space = true)
        {
            //can be null
            if (operation.Message.IsNullOrEmpty())
                operation.Message = Message.ToString();
            else
                operation.Message = string.Join(space ? " " : string.Empty, operation.Message, Message);
            return operation;
        }


        /// <summary>
        /// Helper to append messge, this will affect on base,
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="Messages">start with capital as base</param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult Append(this OperationResult operation, params string[] Messages)
        {
                //can be null
                if (operation.Message.IsNullOrEmpty())
                operation.Message = string.Join(" ", Messages);
                else
                operation.Message = string.Join(" ", operation.Message, Messages);
                return operation;
        }


        /// <summary>
        /// Helper to pass messge, this will affect on base
        /// <para>Effect in <code>base.Status</code> .</para>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="Status"> start with capital as base </param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult Append(this OperationResult operation, Statuses Status)
        {
            operation.Status = Status;
            return operation;
        }


        /// <summary>
        /// Helper to append messge, this will affect on base,
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="Message">start with capital as base</param>
        /// <param name="space">join with space " " or ""</param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult<T> Append<T>(this OperationResult<T> operation, string Message, bool space = true)
        {
            return (OperationResult<T>)Append((OperationResult)operation,Message, space);
        }


        /// <summary>
        /// Helper to append messge, this will affect on base,
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="Messages">start with capital as base</param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult<T> Append<T>(this OperationResult<T> operation, params string[] Messages)
        {
            return (OperationResult<T>)Append((OperationResult)operation, Messages);
        }


        /// <summary>
        /// Helper to pass messge, this will affect on base
        /// <para>Effect in <code>base.Status</code> .</para>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="Status"> start with capital as base </param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult<T> Append<T>(this OperationResult<T> operation, Statuses Status)
        {
            return (OperationResult<T>)Append((OperationResult)operation, Status);
        }



        /// <summary>
        /// Set custom <see cref="OperationResult.StatusCode"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static OperationResult WithStatusCode(this OperationResult result, int statusCode)
        {
            result.StatusCode = statusCode;
            return result;
        }

        /// <summary>
        /// Set custom <see cref="OperationResult.StatusCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static OperationResult<T> WithStatusCode<T>(this OperationResult<T> result, int statusCode)
        {
            return (OperationResult<T>)WithStatusCode((OperationResult)result, statusCode);
        }

        /// <summary>
        /// Set custom <see cref="OperationResult.StatusCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static async Task<OperationResult<T>> WithStatusCodeAsync<T>(this Task<OperationResult<T>> result, int statusCode)
        {
            var _result = await result;
            return (OperationResult<T>)WithStatusCode((OperationResult)_result, statusCode);
        }

    }
}
