﻿/* MIT License

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
using System;

namespace Meteors
{
    /// <summary>
    /// Quick return from <see cref="OperationResult"/> and <see cref="IResult{T}"/>
    /// <para>Used when un-enable Handler.</para>
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles
    public sealed class _Operation
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        /// Normal init used for end return
        /// </summary>
        /// <returns><see cref="OperationResult"/></returns>
        public static OperationResult Operation()
        {
            return new OperationResult();
        }


        /// <summary>
        /// Normal init used for end return
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see cref="OperationResult{T}"/></returns>
        public static OperationResult<T> Operation<T>()
        {
            return new OperationResult<T>();
        }


        /// <summary>
        /// Helper
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// </summary>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult SetSuccess()
        {
            return new OperationResult() { Status = Statuses.Success };
        }

        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetSuccess<T>()
        {
            return new OperationResult<T>().SetSuccess();
        }


        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetSuccess<T>(T result)
        {
            return new OperationResult<T>().SetSuccess(result);
        }


        /// <summary>
        /// Helper
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult SetSuccess(string message)
        {
            return new OperationResult() { Message = message, Status = Statuses.Success };
        }

        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetSuccess<T>(string message)
        {
            return new OperationResult<T>().SetSuccess(message);
        }


        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetSuccess<T>(T result, string message)
        {
            return new OperationResult<T>().SetSuccess(result, message);
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Failed"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// <para>Effect in <code>base.OperationResultType</code> default value <see cref=" Statuses.Failed"/> , <see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Failed .</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult SetFailed(string message, Statuses type = Statuses.Failed)
        {
            if (type is not Statuses.Failed && type is not Statuses.Forbidden && type is not Statuses.Unauthorized)
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResult)} take {type} should use with {Statuses.Failed}, {Statuses.Forbidden} or {Statuses.Unauthorized} .");

            return new OperationResult() { Message = message, Status = type };
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Failed"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// <para>Effect in <code>base.OperationResultType</code> default value <see cref=" Statuses.Failed"/> , <see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Failed .</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetFailed<T>(string message, Statuses type = Statuses.Failed)
        {
            return new OperationResult<T>().SetFailed(message, type);
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Failed"/></para>
        /// <para>Effect in <code>base.OperationResultType</code> default value <see cref=" Statuses.Failed"/> , <see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Failed .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult SetFailed(Statuses type = Statuses.Failed)
        {
            if (type is not Statuses.Failed && type is not Statuses.Forbidden && type is not Statuses.Unauthorized)
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResult)} take {type} should use with {Statuses.Failed}, {Statuses.Forbidden} or {Statuses.Unauthorized} .");

            return new OperationResult() { Status = type };
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Failed"/></para>
        /// <para>Effect in <code>base.OperationResultType</code> default value <see cref=" Statuses.Failed"/> , <see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Failed .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetFailed<T>(Statuses type = Statuses.Failed)
        {
            return new OperationResult<T>().SetFailed(type);
        }




        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult SetException(Exception exception)
        {
            return new OperationResult() { Exception = exception, Status = Statuses.Exception };
        }


        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <returns><see cref="OperationResult{T}"/>  </returns>
        public static OperationResult<T> SetException<T>(Exception exception)
        {
            return new OperationResult<T>().SetException(exception);
        }



        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult SetException(Exception exception, string message)
        {
            return new OperationResult() { Exception = exception, Status = Statuses.Exception, Message = message };
        }


        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetException<T>(Exception exception, string message)
        {
            return new OperationResult<T>().SetException(exception, message);
        }



        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exist"/> or <seealso cref="Statuses.NotExist"/>  </para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Content .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult SetContent(Statuses type, string message)
        {
            if (type is not Statuses.Exist && type is not Statuses.NotExist)
                throw new ArgumentException($"Directly return {nameof(OperationResult)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            return new OperationResult() { Status = type, Message = message };
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exist"/> or <seealso cref="Statuses.NotExist"/>  </para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Content .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetContent<T>(Statuses type, string message)
        {
            return new OperationResult<T>().SetContent(type, message);
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exist"/> or <seealso cref="Statuses.NotExist"/>  </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Content .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult SetContent(Statuses type)
        {
            if (type is not Statuses.Exist && type is not Statuses.NotExist)
                throw new ArgumentException($"Directly return {nameof(OperationResult)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            return new OperationResult() { Status = type };
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exist"/> or <seealso cref="Statuses.NotExist"/>  </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Content .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetContent<T>(Statuses type)
        {
            return new OperationResult<T>().SetContent(type);
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in all props</para>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns> <see cref="OperationResult"/> </returns>
        public static OperationResult Set(Statuses type = default, string? message = default, Exception? exception = default)
        {
            return new OperationResult() { Status = type,Message = message , Exception = exception };
        }

        /// <summary>
        /// Helper  
        /// <para>Effect in all props  </para>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> Set<T>(Statuses type = default, T? data = default, string? message = default, Exception? exception = default)
        {
            return new OperationResult<T>().Set(type, data,message,exception);
        }



    }
}
