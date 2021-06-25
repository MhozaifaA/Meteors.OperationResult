

namespace OperationContext.Base
{
    internal interface IResult<T>
    {
        /// <summary>
        /// Main object result.
        /// </summary>
        public T Result { get; set; }
    }

    internal interface IDynamicResult
    {
        /// <summary>
        /// Main object result.
        /// </summary>
        public dynamic Result { get; set; }
    }
}
