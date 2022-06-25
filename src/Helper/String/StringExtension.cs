namespace Meteors.OperationResult.ExtensionMethods
{
    /// <summary>
    /// Basics extensions
    /// </summary>
    internal static class StringExtension
    {
        /// <summary>
        /// Indicates whether the specified string is null or an empty string ("").
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool IsNullOrEmpty(this string? value)
            => System.String.IsNullOrEmpty(value);
    }
}
