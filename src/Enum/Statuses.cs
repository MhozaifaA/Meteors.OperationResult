/* MIT License

Copyright (c) 2022 Huzaifa Aseel

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Meteors.OperationContext
{

    /// <summary>
    /// Logical  result type over <see cref="_Statuses"/>
    /// </summary>
    [JsonConverter(typeof(StatusesJsonConverter))]
    public record Statuses(_Statuses value = _Statuses.UnKnown) : IComparable<Statuses>
    {
        /// <summary>
        /// <see cref="_Statuses.UnKnown"/> = 0
        /// </summary>
        public static readonly Statuses UnKnown = _Statuses.UnKnown;
        /// <summary>
        /// <see cref="_Statuses.UnKnown"/> = 200
        /// </summary>
        public static readonly Statuses Success = _Statuses.Success;
        /// <summary>
        /// <see cref="_Statuses.Exist"/> = 202
        /// </summary>
        public static readonly Statuses Exist = _Statuses.Exist;
        /// <summary>
        /// <see cref="_Statuses.NotExist"/> = 404
        /// </summary>
        public static readonly Statuses NotExist = _Statuses.NotExist;
        /// <summary>
        /// <see cref="_Statuses.Failed"/> = 400
        /// </summary>
        public static readonly Statuses Failed = _Statuses.Failed;
        /// <summary>
        /// <see cref="_Statuses.Forbidden"/> = 403
        /// </summary>
        public static readonly Statuses Forbidden = _Statuses.Forbidden;
        /// <summary>
        /// <see cref="_Statuses.Exception"/> = 500
        /// </summary>
        public static readonly Statuses Exception = _Statuses.Exception;
        /// <summary>
        /// <see cref="_Statuses.Unauthorized"/> = 401
        /// </summary>
        public static readonly Statuses Unauthorized = _Statuses.Unauthorized;


        public static Statuses[] GetValues() => [UnKnown, Success, Exist, NotExist, Failed, Forbidden, Exception, Unauthorized];

        public bool Is(Statuses right) => this.Equals(right);
        public bool IsNot(Statuses right) => !this.Equals(right);



        public static implicit operator Statuses(_Statuses source) => new Statuses(source);


        public static explicit operator int(Statuses source) => (int)source.value;


        //public static implicit operator _Statuses(Statuses source) => source.value;

        public override string ToString() => this.ToPerString();


        //     Value – Meaning
        //     Less than zero –x is less than y.
        //     Zero –x equals y.
        //     Greater than zero –x is greater than y.
        int IComparable<Statuses>.CompareTo(Statuses? other)
        {
            if (this is null && other is null) return 0;
            if (this is null) return -1;
            if (other is null) return 1;

            int _x = (int)this;
            int _y = (int)other;

            return _x.CompareTo(_y);
        }
    }

    /// <summary>
    /// Logical  result type
    /// <para> Warning: don't uses cast to any numbers <see cref="int"/> ... 
    /// only checked by <see langword="enum"/></para>
    /// </summary>
    public enum _Statuses// : uint
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
    /// Extension methods for <see cref="Statuses"/> only support some extra feature to <see cref="System.Enum"/> <see cref="Statuses"/>
    /// </summary>
    file static class StatusesExtensions
    {
        /// <summary>
        /// Like <see cref="object.ToString()"/> but no Allocated with enum and the most efficient way to performance
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string ToPerString(this _Statuses status)
        {
            return status switch
            {
                _Statuses.UnKnown => "UnKnown",//nameof
                _Statuses.Success => "Success",
                _Statuses.Exist => "Exist",
                _Statuses.NotExist => "NotExist",
                _Statuses.Failed => "Failed",
                _Statuses.Forbidden => "Forbidden",
                _Statuses.Exception => "Exception",
                _Statuses.Unauthorized => "Unauthorized",
                _ => throw new System.NotImplementedException(),
            };
        }
        internal static string ToPerString(this Statuses status)
        {
            return status switch
            {
                { value: _Statuses.UnKnown } => "UnKnown",//nameof
                { value: _Statuses.Success } => "Success",
                { value: _Statuses.Exist } => "Exist",
                { value: _Statuses.NotExist } => "NotExist",
                { value: _Statuses.Failed } => "Failed",
                { value: _Statuses.Forbidden } => "Forbidden",
                { value: _Statuses.Exception } => "Exception",
                { value: _Statuses.Unauthorized } => "Unauthorized",
                _ => throw new System.NotImplementedException(),
            };
        }

        internal static Statuses ToPerEnumStatuses(this string? status)
        {
            return status.ToUpper() switch
            {
                 null or "UNKNOWN"  => _Statuses.UnKnown,
                "SUCCESS" => _Statuses.Success, 
                "EXIST" => _Statuses.Exist,
                "NOTEXIST" => _Statuses.NotExist,
                "FAILED" => _Statuses.Failed,
                "FORBIDDEN" => _Statuses.Forbidden,
                "EXCEPTION" => _Statuses.Exception,
                "UNAUTHORIZED" => _Statuses.Unauthorized,
                _ => throw new System.NotImplementedException(),
            };
        }
    }


    public class StatusesJsonConverter : JsonConverter<Statuses>
    {
        public override Statuses? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.GetString().ToPerEnumStatuses();
        

        public override void Write(Utf8JsonWriter writer, Statuses value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }

}
