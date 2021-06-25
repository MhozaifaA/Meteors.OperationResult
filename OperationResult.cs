using ExtensionMethods.Helper.Exception;
using OperationContext.Base;
using System;



namespace OperationContext
{
    /// <summary>
    /// Capsulation result.
    /// <para>Depends on context repository/http/mvc/response/request .</para>
    /// </summary>
    /// <typeparam name="T"> Type of class </typeparam>
    public class OperationResult<T> : OperatinResultBase, IResult<T> ,IDisposable
    {

        /// <summary>
        /// Main object result.
        /// </summary>
        public T Result { get; set; }
        
        /// <summary>
        /// Check <see cref="OperationResultTypes.Success"/>.
        /// </summary>
        public bool IsSuccess => OperationResultType == OperationResultTypes.Success;

        /// <summary>
        /// Return deep inner exeptions messages.
        /// </summary>
        public string FullExceptionMessage => Exception?.ToFullException();

        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="OperationResultTypes.Success"/></para>
        /// </summary>
        /// <param name="result"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetSuccess(T result)
        {
            Result = result;
            OperationResultType = OperationResultTypes.Success;
            return this;
        }


        /// <summary>
        /// Helper
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="OperationResultTypes.Success"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetSuccess(string message)
        {
            Message = message;
            OperationResultType = OperationResultTypes.Success;
            return this;
        }

        /// <summary>
        /// Helper to pass success result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="OperationResultTypes.Success"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetSuccess(T result,string message)
        {
            Message = message;
            Result = result;
            OperationResultType = OperationResultTypes.Success;
            return this;
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="OperationResultTypes.Failed"/></para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// <para>Effect in <code>base.OperationResultType</code> default value <see cref=" OperationResultTypes.Failed"/> , <see cref="OperationResultTypes.Forbidden"/> and <see cref="OperationResultTypes.Unauthorized"/> </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Failed .</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetFailed(string message, OperationResultTypes type = OperationResultTypes.Failed)
        {
            if (type != OperationResultTypes.Failed && type != OperationResultTypes.Forbidden && type != OperationResultTypes.Unauthorized)
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResult<T>)} take {type} should use with {OperationResultTypes.Failed}, {OperationResultTypes.Forbidden} or {OperationResultTypes.Unauthorized} .");
           
            Message = message;
            OperationResultType = type;
            return this;
        }


        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="OperationResultTypes.Exception"/> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public OperationResult<T> SetException(Exception exception)
        {
            Exception = exception;
            OperationResultType = OperationResultTypes.Exception;
            return this;
        }


        /// <summary>
        /// Duirrectory return implicit take assign <see cref="OperationResultTypes"/> and allow to return as <see cref="OperationResult{T}"/>
        /// </summary>
        /// <param name="type"></param>
        public static implicit operator OperationResult<T>(OperationResultTypes type)
        {
            if (type != OperationResultTypes.Exist && type != OperationResultTypes.NotExist)
                throw new ArgumentException($"Duirrectory return {nameof(OperationResult<T>)} take {type} should use with {OperationResultTypes.Exist} or {OperationResultTypes.NotExist} .");

            return new OperationResult<T>() { OperationResultType = type };
        }

        /// <summary>
        /// Duirrectory return implicit take assign <see cref="OperationResultTypes"/> and <see cref="string" langword="Message"/> as tuple , Allow to return as <see cref="OperationResult{T}"/>
        /// </summary>
        /// <param name="type_message"></param>
        public static implicit operator OperationResult<T>((OperationResultTypes type,string message) type_message)
        {
            if (type_message.type != OperationResultTypes.Exist && type_message.type != OperationResultTypes.NotExist)
                throw new ArgumentException($"Duirrectory return {nameof(OperationResult<T>)} take {type_message.type} should use with {OperationResultTypes.Exist} or {OperationResultTypes.NotExist} .");

            return new OperationResult<T>() { OperationResultType = type_message.type ,Message = type_message.message };
        }

        /// <summary>
        /// Default finalize insted of type Result <code>T = null</code> .
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
