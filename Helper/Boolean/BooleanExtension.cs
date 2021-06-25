using System;
using System.Collections.Generic;
using System.Text;

namespace OperationResult.Helper.Boolean
{
    /// <summary>
    /// basics extesions
    /// </summary>
    internal static class BooleanExtension
    {
        /// <summary>
        /// Nested iF statment return side by <see langword="Boolean"/> <see cref="value"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="trueSide"></param>
        /// <param name="falseSide"></param>
        /// <returns></returns>
        internal static string NestedIF(this bool value, string trueSide, string falseSide)
         => value ? trueSide : falseSide;
    }
}
