using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meteors;


namespace OperationContext
{
    /// <summary>
    /// Global props using to controll in some features.
    /// </summary>
    public class OperationResultOptions
    {
        /// <summary>
        /// Activate return from <see cref="OperationJsonResultExtensions.ToJsonResult{T}(OperationResult{T})"/>.
        /// </summary>
        internal static bool? _IsBody;

        /// <summary>
        /// Activate return from <see cref="OperationJsonResultExtensions.ToJsonResult{T}(OperationResult{T})"/>.
        /// <para>Values: </para>
        /// <list type="bullet">
        /// <item><see langword="null/default"/>: off global</item>
        /// <item><see langword="true"/>: all True global</item>
        /// <item><see langword="false"/>: off False global</item>
        /// </list>
        /// </summary>
        /// <param name="isbody"></param>
        public static void IsBody(bool? isbody)
        {
            _IsBody = isbody;
        }

        /// <summary>
        /// Re-Fill body to select new way of return body, <para></para>  work only with <see cref="_IsBody" langword="True"/> 
        /// </summary>

        internal static Func<OperationResult<dynamic?>, object>? _IntoBody;

        /// <summary>
        /// Re-Fill body to select new way of return body, <para></para>  work only with <see cref="_IsBody" langword="True"/> 
        /// </summary>
        /// <param name="body">First <see cref="object"/> dynamic data type of <see cref="OperationResult{T}.Data"/>, Secound <see cref="object"/> new object to fill by user</param>
        public static void IntoBody(Func<OperationResult<dynamic?>, object>? body)
        {
            _IntoBody = body;
        }


        ///// <summary>
        ///// Enable <see cref="_SerializerSettings"/>.
        ///// </summary>
        //internal static bool _UseSerializerSettings;

        ///// <summary>
        ///// Enable <see cref="_SerializerSettings"/>.
        ///// </summary>
        ///// <param name="use"></param>
        //public static void UseSerializerSettings(bool use)
        //{
        //    _UseSerializerSettings = use;
        //}



        /// <summary>
        /// The serializer settings to be used by the formatter.
        /// </summary>
        internal static object? _SerializerSettings;

        /// <summary>
        /// The serializer settings to be used by the formatter.
        /// <para> When using System.Text.Json, this should be an instance of System.Text.Json.JsonSerializerOptions.</para>
        /// <para>When using Newtonsoft.Json, this should be an instance of JsonSerializerSettings.</para>
        /// </summary>
        /// <param name="settings"></param>
        public static void SerializerSettings(object? settings)
        {
            _SerializerSettings = settings;
        }
    }
}
