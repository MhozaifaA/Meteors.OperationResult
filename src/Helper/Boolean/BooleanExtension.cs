using System;

namespace ExtensionMethods.Helper.Boolean
{
    /// <summary>
    /// Basics extensions
    /// </summary>
    internal static class BooleanExtension
    {
     
        /// <summary>
        /// Nested iF statement return side by <see langword="Boolean"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="trueSide"></param>
        /// <param name="falseSide"></param>
        /// <returns></returns>
        internal static T NestedIF<T>(this bool value, Func<T> trueSide, Func<T> falseSide)
        => value ? trueSide() : falseSide();

    }
}
