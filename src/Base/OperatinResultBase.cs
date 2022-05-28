using System;

namespace OperationContext.Base
{
    /// <summary>
    /// Main prop not changed or effect on return
    /// abstract of for <see cref="OperationContext._Operation"/>
    /// </summary>
    public class OperationResultBase : IEquatable<OperationResultBase>//, IDisposable
    {
        /// <summary>
        /// Any validation text or result-message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Result type.
        /// </summary>
        public OperationResultTypes OperationResultType { get; set; }

        /// <summary>
        ///  Represents errors that occur during CONTEXT execution.
        ///  protected of for <see cref="OperationContext._Operation"/>
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// custom return StatusCode-HTTP used with web-requests.
        /// <para>Not effect with native C# code lib as un-host-web projects or responses.</para>
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(OperationResultBase other)
        {
            return Message == other.Message && OperationResultType == other.OperationResultType &&
                (Exception == other.Exception || Exception.Message == other.Exception.Message) && StatusCode == other.StatusCode;
        }

        ///// <summary>
        ///// Flag enter disposed
        ///// </summary>
        //bool disposed = false;

        ///// <summary>
        ///// Destructor
        ///// </summary>
        //~OperationResultBase()
        //{
        //    this.Dispose(false);
        //}


        ///// <summary>
        ///// Default finalize instead of type Result <code>T = null</code> .
        ///// </summary>
        //public void Dispose()
        //{
        //    this.Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        ///// <summary>
        ///// The virtual dispose method that allows
        ///// classes inherited from this one to dispose their resources.
        ///// </summary>
        ///// <param name="disposing"></param>
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposed)
        //    {
        //        //if (disposing) {//}
        //        Exception = null;
        //        Message = null;
        //    }

        //    disposed = true;
        //}

    }
}
