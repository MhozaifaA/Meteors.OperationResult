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
using System;
using System.Text.Json.Serialization;

namespace Meteors
{
    /// <summary>
    /// Encapsulation result.
    /// <para>Depends on context repository/http/mvc/response/request .</para>
    /// </summary>
    /// <typeparam name="T"> Type of class </typeparam>
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class OperationResult<T> : OperationResult, IResult<T>, IEquatable<OperationResult<T>>//, IDisposable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {

        /// <summary>
        /// Main object result.
        /// </summary>
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public T? Data { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

        /// <summary>
        /// Check <see cref="Statuses.Success"/>.
        /// </summary>
        public bool IsSuccess => Status.Is(Statuses.Success);

        /// <summary>
        /// Check <see cref="Statuses.Exception"/>.
        /// </summary>
        public bool HasException => this.Status.Is(Statuses.Exception);


        /// <summary>
        /// Return deep inner exceptions messages.
        /// </summary>
        public string? FullExceptionMessage => Exception?.ToFullException();


        /// <summary>
        /// Check <see cref="OperationResult.StatusCode"/> if init with value > 0.
        /// <para>Custom return StatusCode-http used with web-requests.
        /// Not effect with native C# code lib as un-host-web projects or responses.</para>
        /// <remark>
        /// Always will be <see langword="true"/> after call <see cref="OperationJsonResultExtensions.ToJsonResult{T}(OperationResult{T})"/>
        /// </remark>
        /// </summary>
        [JsonIgnore]
        public bool HasCustomStatusCode => StatusCode > 0;


       
        /// <summary>
        /// Helper
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetSuccess(string? message = null)
        {
            Message = message;
            Status = Statuses.Success;
            return this;
        }

        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetSuccess(T result, string? message = null)
        {
            Message = message;
            Data = result;
            Status = Statuses.Success;
            return this;
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Failed"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// <para>Effect in <code>base.OperationResultType</code> default value <see cref=" Statuses.Failed"/> , <see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Failed .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetFailed(string? message = null)
        {   
            return SetFailed(message, Statuses.Failed);
        }

        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Failed"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// <para>Effect in <code>base.OperationResultType</code> default value <see cref=" Statuses.Failed"/> , <see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Failed .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetFailed(string? message,Statuses type)
        {
            if (type.IsNot(Statuses.Failed) && type.IsNot(Statuses.Forbidden) && type.IsNot(Statuses.Unauthorized))
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResult<T>)} take {type} should use with {Statuses.Failed}, {Statuses.Forbidden} or {Statuses.Unauthorized} .");

            Message = message;
            Status = type;
            return this;
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
        public OperationResult<T> SetFailed(Statuses type)
        {
            return SetFailed(null, type);
        }


        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetException(Exception exception, string? message = null)
        {
            Exception = exception;
            Status = Statuses.Exception;
            Message = message;
            return this;
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
        public OperationResult<T> SetContent(Statuses type, string? message = null)
        {
            if (type.IsNot(Statuses.Exist) && type.IsNot(Statuses.NotExist))
                throw new ArgumentException($"Directly  return {nameof(OperationResult<T>)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            Message = message;
            Status = type;
            return this;
        }



        /// <summary>
        /// Helper to pass props without filter or throw exception if satus not correctly.
        /// <para>method can push to handle props before <see langword="Set-"/>Methods </para>
        /// </summary>
        /// <param name="status"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> Set(Statuses status = default, T? data = default, string? message = default, Exception? exception = default)
        {
            Status = status;
            Data = data;
            Message = message;
            Exception = exception;
            return this;
        }


        /// <summary>
        /// Directly return implicit take assign <see cref="Statuses"/> and allow to return as <see cref="OperationResult{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        public static implicit operator OperationResult<T>(Statuses type)
        {
            if (type.IsNot(Statuses.Exist) && type.IsNot(Statuses.NotExist))
                throw new ArgumentException($"Directly return {nameof(OperationResult<T>)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            return new OperationResult<T>() { Status = type };
        }


        /// <summary>
        /// Directly return implicit take assign <see cref="Statuses"/> and <see cref="string" langword=" Message"/> as tuple , Allow to return as <see cref="OperationResult{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type_message"></param>
        public static implicit operator OperationResult<T>((Statuses type, string? message) type_message)
        {
            if (type_message.type.IsNot(Statuses.Exist) && type_message.type.IsNot(Statuses.NotExist))
                throw new ArgumentException($"Directly return {nameof(OperationResult<T>)} take {type_message.type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            return new OperationResult<T>() { Status = type_message.type, Message = type_message.message };
        }


        /// <summary>
        /// Directly return implicit take assign <see cref="Statuses"/> and <see cref="string" langword=" Message"/> as tuple , Allow to return as <see cref="OperationResult{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type_message"></param>
        public static implicit operator OperationResult<T>((string? message, Statuses type) type_message)
        {
            if (type_message.type.IsNot(Statuses.Failed) && type_message.type.IsNot(Statuses.Forbidden) && type_message.type.IsNot(Statuses.Unauthorized))
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResult<T>)} take {type_message.type} should use with {Statuses.Failed}, {Statuses.Forbidden} or {Statuses.Unauthorized} .");

            return new OperationResult<T>() { Status = type_message.type, Message = type_message.message };
        }


        ///// <summary>
        ///// Directly return implicit take assign <see cref="Data"/> and allow to return as <see cref="OperationResult{T}"/>
        ///// </summary>
        ///// <param name="result"></param>
        //public static implicit operator OperationResult<T>(T result)
        //{
        //    return new OperationResult<T>().SetSuccess(result);
        //}

        ///// <summary>
        /////  Directly return explicit take assign <see cref="Data"/> and allow to return as <see cref="OperationResult{T}"/>
        ///// </summary>
        ///// <param name="result"></param>
        //public static explicit operator OperationResult<T>(T result)
        //{
        //    return new OperationResult<T>().SetSuccess(result);
        //}

        /// <summary>
        /// Convert not static to dynamic? data
        /// </summary>
        /// <returns></returns>
        public OperationResult<dynamic?> ToOperationResultDynamic()
        {
            return new OperationResult<dynamic?>() {
                Data = this.Data,
                Exception = this.Exception,
                Message = this.Message,
                Status = this.Status,
                StatusCode = this.StatusCode
            };
        }


        /// <summary>
        /// Directly return implicit take assign <see cref="Data"/> and <see cref="string" langword=" Message"/> as tuple, and allow to return as <see cref="OperationResult{T}"/>
        /// <para> with int {T} type will face Ambiguous </para>
        /// </summary>
        /// <param name="result_message"></param>
        public static implicit operator OperationResult<T>((T result, string message) result_message)
        {
            return new OperationResult<T>().SetSuccess(result_message.result, result_message.message);
        }


        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static implicit operator OperationResult<T>(Exception exception)
        {
            return new OperationResult<T>().SetException(exception);
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(OperationResult<T>? other)
        {
            return base.Equals(other) && ((Data is null && other.Data is null) || Data!.Equals(other.Data));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as OperationResult<T>);
        }


        ///// <summary>
        ///// Flag enter disposed
        ///// </summary>
        //bool disposed = false;

        ///// <summary>
        ///// Destructor
        ///// </summary>
        //~OperationResult()
        //{
        //    this.Dispose(false);
        //}


        ///// <summary>
        ///// Default finalize instead of type Result <code>T = null</code> .
        ///// </summary>
        //public new void Dispose()
        //{
        //    this.Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        ///// <summary>
        ///// The virtual dispose method that allows
        ///// classes inherited from this one to dispose their resources.
        ///// </summary>
        ///// <param name="disposing"></param>
        //protected new virtual void Dispose(bool disposing)
        //{
        //    if (!disposed)
        //    {
        //        if (disposing) { Dispose(); }
        //        Data = default;
        //    }

        //    disposed = true;
        //}

    }
}
