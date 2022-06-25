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

using Microsoft.AspNetCore.Http;

namespace Meteors.OperationResult
{
    /// <summary>
    /// Logical  result type
    /// <para> Warning: don't uses cast to any numbers <see cref="int"/> ... 
    /// only checked by <see langword="enum"/></para>
    /// </summary>
    public enum Statuses : uint
    {
        /// <summary>
        /// Unknown status take zero(default) value when not assert  
        /// </summary>
        UnKnown,
        /// <summary>
        /// As <see cref="StatusCodes.Status200OK"/>
        /// </summary>
        Success = 200,
        /// <summary>
        /// As <see cref="StatusCodes.Status202Accepted"/>
        /// </summary>
        Exist = 202,
        /// <summary>
        /// As <see cref="StatusCodes.Status404NotFound"/>
        /// </summary>
        NotExist = 404,
        /// <summary>
        /// As <see cref="StatusCodes.Status400BadRequest"/>
        /// </summary>
        Failed = 400,
        /// <summary>
        /// As <see cref="StatusCodes.Status403Forbidden"/>
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// As <see cref="StatusCodes.Status500InternalServerError"/>
        /// </summary>
        Exception = 500,
        /// <summary>
        /// Useful in third party API
        /// As <see cref="StatusCodes.Status401Unauthorized"/>
        /// </summary>
        Unauthorized = 401,
    }


    /// <summary>
    /// Extension methods for <see cref="Statuses"/> only support some extra feature to <see cref="System.Enum"/> <see cref="Statuses"/>
    /// </summary>
    public static class StatusesExtensions
    {
        /// <summary>
        /// Like <see cref="object.ToString()"/> but no Allocated with enum and the most efficient way to performance
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToPerString(this Statuses status)
        {
            return status switch
            {
                Statuses.UnKnown => "UnKnown",
                Statuses.Success => "Success",
                Statuses.Exist => "Exist",
                Statuses.NotExist => "NotExist",
                Statuses.Failed => "Failed",
                Statuses.Forbidden => "Forbidden",
                Statuses.Exception => "Exception",
                Statuses.Unauthorized => "Unauthorized",
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}