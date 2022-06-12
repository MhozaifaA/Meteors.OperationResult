namespace Meteors.OperationResult
{
    internal interface IResult<T>
    {
        /// <summary>
        /// Main object result.
        /// </summary>
        public T Data { get; set; }
    }

    internal interface IDynamicResult
    {
        /// <summary>
        /// Main object result.
        /// </summary>
        public dynamic Data { get; set; }
    }
}
