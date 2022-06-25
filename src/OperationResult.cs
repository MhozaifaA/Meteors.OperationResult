using Meteors.OperationResult;
using Meteors.OperationResult.ExtensionMethods;
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
    public class OperationResult<T> : OperationResultBase, IResult<T>, IEquatable<OperationResult<T>>//, IDisposable
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
        public bool IsSuccess => Status is Statuses.Success;

        /// <summary>
        /// Check <see cref="Statuses.Exception"/>.
        /// </summary>
        public bool HasException => this.Status is Statuses.Exception;


        /// <summary>
        /// Return deep inner exceptions messages.
        /// </summary>
        public string? FullExceptionMessage => Exception?.ToFullException();


        /// <summary>
        /// Check <see cref="OperationResultBase.StatusCode"/> if init with value > 0.
        /// <para>Custom return StatusCode-http used with web-requests.
        /// Not effect with native C# code lib as un-host-web projects or responses.</para>
        /// <remark>
        /// Always will be <see langword="true"/> after call <see cref="OpertaionResultExtesnsion.ToJsonResult{T}(OperationResult{T})"/>
        /// </remark>
        /// </summary>
        [JsonIgnore]
        public bool HasCustomStatusCode => StatusCode > 0;


        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// </summary>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetSuccess()
        {
            Status = Statuses.Success;
            return this;
        }

        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// </summary>
        /// <param name="result"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetSuccess(T result)
        {
            Data = result;
            Status = Statuses.Success;
            return this;
        }


        /// <summary>
        /// Helper
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Success"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetSuccess(string message)
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
        public OperationResult<T> SetSuccess(T result, string? message)
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
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetFailed(string? message, Statuses type = Statuses.Failed)
        {
            if (type is not Statuses.Failed && type is not Statuses.Forbidden && type is not Statuses.Unauthorized)
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
        public OperationResult<T> SetFailed(Statuses type = Statuses.Failed)
        {
            if (type is not Statuses.Failed && type is not Statuses.Forbidden && type is not Statuses.Unauthorized)
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResult<T>)} take {type} should use with {Statuses.Failed}, {Statuses.Forbidden} or {Statuses.Unauthorized} .");

            Status = type;
            return this;
        }


        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetException(Exception exception, string message)
        {
            Exception = exception;
            Status = Statuses.Exception;
            Message = message;
            return this;
        }

        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetException(Exception? exception)
        {
            Exception = exception;
            Status = Statuses.Exception;
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
        public OperationResult<T> SetContent(Statuses type, string message)
        {
            if (type is not Statuses.Exist && type is not Statuses.NotExist)
                throw new ArgumentException($"Directly  return {nameof(OperationResult<T>)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            Message = message;
            Status = type;
            return this;
        }

        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exist"/> or <seealso cref="Statuses.NotExist"/>  </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Content .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetContent(Statuses type)
        {
            if (type is not Statuses.Exist && type is not Statuses.NotExist)
                throw new ArgumentException($"Directly  return {nameof(OperationResult<T>)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            Status = type;
            return this;
        }


        /// <summary>
        /// Directly return implicit take assign <see cref="Statuses"/> and allow to return as <see cref="OperationResult{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        public static implicit operator OperationResult<T>(Statuses type)
        {
            if (type is not Statuses.Exist && type is not Statuses.NotExist)
                throw new ArgumentException($"Directly return {nameof(OperationResult<T>)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            return new OperationResult<T>() { Status = type };
        }


        /// <summary>
        /// Directly return implicit take assign <see cref="Statuses"/> and <see cref="string" langword=" Message"/> as tuple , Allow to return as <see cref="OperationResult{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type_message"></param>
        public static implicit operator OperationResult<T>((Statuses type, string message) type_message)
        {
            if (type_message.type is not Statuses.Exist && type_message.type is not Statuses.NotExist)
                throw new ArgumentException($"Directly return {nameof(OperationResult<T>)} take {type_message.type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            return new OperationResult<T>() { Status = type_message.type, Message = type_message.message };
        }


        /// <summary>
        /// Directly return implicit take assign <see cref="Statuses"/> and <see cref="string" langword=" Message"/> as tuple , Allow to return as <see cref="OperationResult{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type_message"></param>
        public static implicit operator OperationResult<T>((string message, Statuses type) type_message)
        {
            if (type_message.type is not Statuses.Failed && type_message.type is not Statuses.Forbidden && type_message.type is not Statuses.Unauthorized)
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
