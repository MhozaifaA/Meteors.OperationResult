using Meteors.OperationResult.ExtensionMethods;
using System;
using System.Text.Json.Serialization;

namespace Meteors.OperationResult
{
    /// <summary>
    /// Main prop not changed or effect on return
    /// abstract of for <see cref="_Operation"/>
    /// </summary>
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class OperationResultBase : IEquatable<OperationResultBase>//, IDisposable
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Any validation text or result-message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Result type/status.
        /// </summary>
        public Statuses Status { get; set; }

        /// <summary>
        ///  Represents errors that occur during CONTEXT execution.
        ///  protected of for <see cref="_Operation"/>
        /// </summary>
        [JsonIgnore]
        public Exception? Exception { get; set; }

        /// <summary>
        /// custom return StatusCode-HTTP used with web-requests.
        /// <para>Not effect with native C# code lib as un-host-web projects or responses.</para>
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Helper to append messge, this will affect on base,
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="Message">start with capital as base</param>
        /// <param name="space">join with space " " or ""</param>
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public OperationResultBase Append(string Message,bool space = true)
        {
            //can be null
            if (this.Message.IsNullOrEmpty())
                this.Message = Message.ToString();
            else
                this.Message = string.Join(space ?" ":string.Empty , this.Message, Message);
            return this;
        }


        /// <summary>
        /// Helper to append messge, this will affect on base,
        /// <para>Effect in <code>base.Message</code> .</para>
        /// </summary>
        /// <param name="Message">start with capital as base</param
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public OperationResultBase Append(params string[] Message)
        {
            //can be null
            if (this.Message.IsNullOrEmpty())
                this.Message = string.Join(" ", Message);
            else
                this.Message = string.Join(" ", this.Message, Message);
            return this;
        }

        ///// <summary>
        ///// Helper to append messge, this will affect on base,
        ///// <para>Effect in <code>base.Message</code> .</para>
        ///// </summary>
        ///// <param name="Message">start with capital as base</param>
        ///// <returns> <see cref="OperationResultBase"/> </returns>
        //public OperationResultBase Append( Readonly<char> Message)
        //{
        //    //can be null
        //    if (this.Message.IsNullOrEmpty())
        //        this.Message = Message.ToString();
        //    else
        //        this.Message = string.Join(" ", this.Message, Message.ToString());

        //    return this;
        //}

        /// <summary>
        /// Helper to pass messge, this will affect on base
        /// <para>Effect in <code>base.Status</code> .</para>
        /// </summary>
        /// <param name="Status"> start with capital as base </param>
        /// <returns> <see cref="OperationResultBase"/> </returns>
        public OperationResultBase Append(Statuses Status)
        {
            this.Status = Status;
            return this;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(OperationResultBase? other)
        {
            if (other is null)
                return false;

            return Message == other!.Message && Status == other!.Status &&
                (Exception == other!.Exception || Exception?.Message == other!.Exception?.Message)
                && StatusCode == other!.StatusCode;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        public override bool Equals(object obj)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        {
            return Equals(obj as OperationResultBase);
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
