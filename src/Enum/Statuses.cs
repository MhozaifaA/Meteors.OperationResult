using Microsoft.AspNetCore.Http;

namespace Meteors.OperationResult
{
    /// <summary>
    /// Logical  result type
    /// <para> Warning: don't uses cast to any numbers <see cref="int"/> ... 
    /// only checked by <see langword="enum"/></para>
    /// </summary>
    public enum Statuses : uint
    {
        /// <summary>
        /// As <see cref="StatusCodes.Status200OK"/>
        /// </summary>
        Success = 200,
        /// <summary>
        /// As <see cref="StatusCodes.Status202Accepted"/>
        /// </summary>
        Exist = 202,
        /// <summary>
        /// As <see cref="StatusCodes.Status404NotFound"/>
        /// </summary>
        NotExist = 404,
        /// <summary>
        /// As <see cref="StatusCodes.Status400BadRequest"/>
        /// </summary>
        Failed = 400,
        /// <summary>
        /// As <see cref="StatusCodes.Status403Forbidden"/>
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// As <see cref="StatusCodes.Status500InternalServerError"/>
        /// </summary>
        Exception = 500,
        /// <summary>
        /// Useful in third party API
        /// As <see cref="StatusCodes.Status401Unauthorized"/>
        /// </summary>
        Unauthorized = 401,
    }
}
