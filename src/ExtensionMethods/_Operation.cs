using Meteors.OperationResult;
using System;

namespace Meteors
{
    /// <summary>
    /// Quick return from <see cref="OperationResultBase"/> and <see cref="IResult{T}"/>
    /// <para>Used when un-enable Handler.</para>
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles
    public sealed class _Operation
#pragma warning restore IDE1006 // Naming Styles
    {
        /// <summary>
        /// Normal init used for end return
        /// </summary>
        /// <returns><see cref="OperationResultBase"/></returns>
        public static OperationResultBase Operation()
        {
            return new OperationResultBase();
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
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public static OperationResultBase SetSuccess()
        {
            return new OperationResultBase() { Status = Statuses.Success };
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
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public static OperationResultBase SetSuccess(string message)
        {
            return new OperationResultBase() { Message=message,Status=Statuses.Success };
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
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public static OperationResultBase SetFailed(string message, Statuses type = Statuses.Failed)
        {
            if (type is not Statuses.Failed && type is not Statuses.Forbidden && type is not Statuses.Unauthorized)
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResultBase)} take {type} should use with {Statuses.Failed}, {Statuses.Forbidden} or {Statuses.Unauthorized} .");

            return new OperationResultBase() { Message = message, Status = type };
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
            return new OperationResult<T>().SetFailed(message,type);
        }


        /// <summary>
        /// Helper  
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Failed"/></para>
        /// <para>Effect in <code>base.OperationResultType</code> default value <see cref=" Statuses.Failed"/> , <see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> </para>
        /// <para>Exception :  <see langword="throw"/> <see cref="ArgumentException"/> if type not kind of Failed .</para>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="type"></param>
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public static OperationResultBase SetFailed(Statuses type = Statuses.Failed)
        {
            if (type is not Statuses.Failed && type is not Statuses.Forbidden && type is not Statuses.Unauthorized)
                throw new ArgumentException($"{nameof(SetFailed)} in {nameof(OperationResultBase)} take {type} should use with {Statuses.Failed}, {Statuses.Forbidden} or {Statuses.Unauthorized} .");

            return new OperationResultBase() { Status = type };
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
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public static OperationResultBase SetException(Exception exception)
        {
            return new OperationResultBase() { Exception = exception, Status = Statuses.Exception };
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
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public static OperationResultBase SetException(Exception exception,string message)
        {
            return new OperationResultBase() { Exception = exception, Status = Statuses.Exception, Message = message };
        }


        /// <summary>
        /// Helper to pass exception result 
        /// <para>Effect in <code>base.OperationResultType</code> to <seealso cref="Statuses.Exception"/> .</para>
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> SetException<T>(Exception exception,string message)
        {
            return new OperationResult<T>().SetException(exception,message);
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
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public static OperationResultBase SetContent(Statuses type, string message)
        {
            if (type is not Statuses.Exist && type is not Statuses.NotExist)
                throw new ArgumentException($"Directly return {nameof(OperationResultBase)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            return new OperationResultBase() { Status = type, Message = message };
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
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public static OperationResultBase SetContent(Statuses type)
        {
            if (type is not Statuses.Exist && type is not Statuses.NotExist)
                throw new ArgumentException($"Directly return {nameof(OperationResultBase)} take {type} should use with {Statuses.Exist} or {Statuses.NotExist} .");

            return new OperationResultBase() { Status = type };
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


    }
}
