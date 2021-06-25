
using Microsoft.AspNetCore.Http;

namespace OperationContext
{
    /// <summary>
    /// Logical  result type
    /// <para> Warning: dont uses cast to any numbers <see cref="int"/> ... 
    /// only cheked by <see langword="enum"/></para>
    /// </summary>
    public enum OperationResultTypes:uint
    {
        /// <summary>
        /// As <see cref="StatusCodes.Status200OK"/>
        /// </summary>
        Success,
        /// <summary>
        /// As <see cref="StatusCodes.Status202Accepted"/>
        /// </summary>
        Exist,
        /// <summary>
        /// As <see cref="StatusCodes.Status204NoContent"/>
        /// </summary>
        NotExist,
        /// <summary>
        /// As <see cref="StatusCodes.Status400BadRequest"/>
        /// </summary>
        Failed,
        /// <summary>
        /// As <see cref="StatusCodes.Status403Forbidden"/>
        /// </summary>
        Forbidden,
        /// <summary>
        /// As <see cref="StatusCodes.Status500InternalServerError"/>
        /// </summary>
        Exception,
        /// <summary>
        /// Useful in third party api
        /// As <see cref="StatusCodes.Status401Unauthorized"/>
        /// </summary>
        Unauthorized
    }
}
