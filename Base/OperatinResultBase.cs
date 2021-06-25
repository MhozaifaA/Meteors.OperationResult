using System;

namespace OperationContext.Base
{
    /// <summary>
    /// Main prop not changed or effect on return
    /// </summary>
    public abstract class OperatinResultBase
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
        /// </summary>
        public Exception Exception { get; protected set;}

    }
}
