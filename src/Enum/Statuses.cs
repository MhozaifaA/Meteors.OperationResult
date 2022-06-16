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
        /// Unknown status take zero(default) value when not assert  
        /// </summary>
        UnKnown,
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


    /// <summary>
    /// Extension methods for <see cref="Statuses"/> only support some extra feature to <see cref="enum"/> <see cref="Statuses"/>
    /// </summary>
    public static class StatusesExtensions
    {
        /// <summary>
        /// Like <see cref="object.ToString()"/> but no Allocated with enum and the most efficient way to performance
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToPerString(this Statuses status)
        {
            switch (status)
            {
                case Statuses.UnKnown:
                    return "UnKnown";

                case Statuses.Success:
                    return "Success";

                case Statuses.Exist:
                    return "Exist";

                case Statuses.NotExist:
                    return "NotExist";

                case Statuses.Failed:
                    return "Failed";

                case Statuses.Forbidden:
                    return "Forbidden";

                case Statuses.Exception:
                    return "Exception";

                case Statuses.Unauthorized:
                    return "Unauthorized";

                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}