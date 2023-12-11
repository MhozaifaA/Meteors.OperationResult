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

using Meteors.OperationContext;
using Meteors.OperationContext.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using OperationContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Meteors
{
    /// <summary>
    /// Helper extensions of support <see cref="JsonResult"/> and <see langword="Http"/>.
    /// </summary>
    public static class OperationJsonResultExtensions
    {

        /// <summary>
        /// Return <see cref="JsonResult"/> with real result completely .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns><see cref="JsonResult"/></returns>
        public static JsonResult ToJsonResult<T>(this OperationResult<T> result)
        {
            return result.ToJsonResult(default); //extra to refactoring
        }

        /// <summary>
        /// Return <see cref="JsonResult"/> with real result completely .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="isBody">boolean value if return json complete body of operation</param>
        /// <returns></returns>
        public static JsonResult ToJsonResult<T>(this OperationResult<T> result, bool? isBody = default)
        {
            return result.Status.value switch
            {
                _Statuses.Success => result.GetValidResult(jsonMessage: result.Message, hasResult: true, isBody),
                _Statuses.Exist => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                _Statuses.NotExist => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                _Statuses.Failed => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                _Statuses.Forbidden => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                _Statuses.Unauthorized => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                _Statuses.Exception => result.GetValidResult(jsonMessage: result.FullExceptionMessage, isBody: isBody),
                _ => throw new NotImplementedException($"Update source code to catch new {nameof(Statuses)} value: {result.Status}"),
            };
        }


        /// <summary>
        /// Return <see cref="JsonResult"/> with real result completely .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns><see cref="Task{JsonResult}"/></returns>
        public static Task<JsonResult> ToJsonResultAsync<T>(this Task<OperationResult<T>> result) => result.ToJsonResultAsync(default);


        /// <summary>
        /// Return <see cref="JsonResult"/> with real result completely .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="isBody">boolean value if return json complete body of operation</param>
        /// <returns><see cref="Task{JsonResult}"/></returns>
        public static async Task<JsonResult> ToJsonResultAsync<T>(this Task<OperationResult<T>> result, bool? isBody = default) => (await result).ToJsonResult(isBody);


        /// <summary>
        /// Return <see langword="real result"/> .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="jsonMessage">String of type</param>
        /// <param name="hasResult"> boolean value if return json of main type T </param>
        /// <param name="isBody"> boolean value if return json complete body of operation</param>
        /// <returns><see cref="JsonResult"/></returns>
        private static JsonResult GetValidResult<T>(this OperationResult<T> result, string? jsonMessage = null, bool hasResult = false, bool? isBody = default)
        {
            if (!result.HasCustomStatusCode && (result.StatusCode ?? 0) == 0)
                result.StatusCode = (int)result.Status;

            bool jsonMessageIsNullOrEmpty = jsonMessage.IsNullOrEmpty();

            if (isBody is true || (isBody is null && OperationResultOptions._IsBody is true))
            {
                if (jsonMessageIsNullOrEmpty)
                    result.Message = result.Status.ToString();

                if (OperationResultOptions._IntoBody is not null)
                    return ReturnJsonResult(OperationResultOptions.
                        _IntoBody!(result.ToOperationResultDynamic())
                        , result.StatusCode);

                return ReturnJsonResult(result, result.StatusCode);
            }

            if (hasResult)
                return ReturnJsonResult(result.Data, result.StatusCode);

            if (jsonMessageIsNullOrEmpty)
                return ReturnJsonResult(result.Status.ToString(), result.StatusCode);

            return ReturnJsonResult(jsonMessage, result.StatusCode);
        }


        /// <summary>
        /// Only to create <see cref="JsonResult"/> as one place changed.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="statuscode"></param>
        /// <returns></returns>
        private static JsonResult ReturnJsonResult(object? value, int? statuscode)
        {
            return new JsonResult(value)
            {
                StatusCode = statuscode,
                SerializerSettings = OperationResultOptions._SerializerSettings
            };
        }


        #region -   One   -

        #region -   Sync   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <param name="result1"></param>
        /// <returns><see cref="OperationResult{TResult1}"/></returns>
        public static OperationResult<TResult1> Collect<TResult1>(this OperationResult<TResult1> result1)
        => (result1);


        /// <summary>
        /// Sugar use to fill results after <see cref="Collect{TResult1}(OperationResult{TResult1})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/> .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result1"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static OperationResult<TResult> Into<TResult1, TResult>(this OperationResult<TResult1> result1,
            Func<OperationResult<TResult1>, TResult> receiver)
        => result1.InOnce(receiver(result1));


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result"></param>
        /// <returns><see cref="OperationResult{TResult}"/></returns>
        private static OperationResult<TResult> InOnce<TResult1, TResult>(this OperationResult<TResult1> result1, TResult result)
        => OnePriority(result1, result);

        #endregion

        #region -   Async   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <param name="result1"></param>
        /// <returns><see cref="OperationResult{TResult1}"/></returns>
        public static async Task<OperationResult<TResult1>> CollectAsync<TResult1>(this Task<OperationResult<TResult1>> result1)
        {
            //await Task.WhenAll(result1); //ca1842
            return (await result1);
        }


        /// <summary>
        /// Sugar use to fill results after <see cref="CollectAsync{TResult1}(Task{OperationResult{TResult1}})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/> .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result1"></param>
        /// <param name="receiver"></param>
        /// <returns><see cref="OperationResult{TResult}"/></returns>
        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult>(this Task<OperationResult<TResult1>> result1,
            Func<OperationResult<TResult1>, TResult> receiver)
        => await result1.InOnceAsync(receiver(await result1));


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result"></param>
        /// <returns><see cref="OperationResult{TResult}"/></returns>
        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult>(this Task<OperationResult<TResult1>> result1, TResult result)
        => OnePriority(await result1, result);

        #endregion

        #endregion

        #region -   Two   -

        #region -   Sync   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <returns></returns>
        public static (OperationResult<TResult1> result1, OperationResult<TResult2> result2) Collect<TResult1, TResult2>(this OperationResult<TResult1> result1, OperationResult<TResult2> result2)
        => (result1, result2);


        /// <summary>
        /// Sugar use to fill results after <see cref="Collect{TResult1, TResult2}(OperationResult{TResult1}, OperationResult{TResult2})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/> .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static OperationResult<TResult> Into<TResult1, TResult2, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2) results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, TResult> receiver)
        => results.InOnce(receiver(results.result1, results.result2));


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static OperationResult<TResult> InOnce<TResult1, TResult2, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2) results, TResult result)
        => OncePriority(results, result);

        #endregion

        #region -   Async   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <returns></returns>
        public static async Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2)> CollectAsync<TResult1, TResult2>(this Task<OperationResult<TResult1>> result1, Task<OperationResult<TResult2>> result2)
        {
            await Task.WhenAll(result1, result2);
            return (await result1, await result2);
        }


        /// <summary>
        /// Sugar use to fill results after <see cref="CollectAsync{TResult1, TResult2}(Task{OperationResult{TResult1}}, Task{OperationResult{TResult2}})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/> .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult2, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2)> results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, TResult> receiver)
        { var (result1, result2) = await results; return await results.InOnceAsync(receiver(result1, result2)); }


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult2, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2)> results, TResult result)
        => OncePriority(await results, result);

        #endregion

        #endregion

        #region -   Three   -

        #region -   Sync   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <returns></returns>
        public static (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3) Collect<TResult1, TResult2, TResult3>(this OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3)
        => (result1, result2, result3);

        /// <summary>
        /// Sugar use to fill results after <see cref="Collect{TResult1, TResult2,TResult3}(OperationResult{TResult1}, OperationResult{TResult2}, OperationResult{TResult3})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static OperationResult<TResult> Into<TResult1, TResult2, TResult3, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3) results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, TResult> receiver)
        => results.InOnce(receiver(results.result1, results.result2, results.result3));


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static OperationResult<TResult> InOnce<TResult1, TResult2, TResult3, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3) results, TResult result)
        => OncePriority(results, result);

        #endregion

        #region -   Async   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <returns></returns>
        public static async Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3)> CollectAsync<TResult1, TResult2, TResult3>(this Task<OperationResult<TResult1>> result1, Task<OperationResult<TResult2>> result2, Task<OperationResult<TResult3>> result3)
        {
            await Task.WhenAll(result1, result2, result3);
            return (await result1, await result2, await result3);
        }

        /// <summary>
        /// Sugar use to fill results after <see cref="CollectAsync{TResult1, TResult2,TResult3}(Task{OperationResult{TResult1}}, Task{OperationResult{TResult2}}, Task{OperationResult{TResult3}})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult2, TResult3, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3)> results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, TResult> receiver)
        { var (result1, result2, result3) = await results; return await results.InOnceAsync(receiver(result1, result2, result3)); }


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult2, TResult3, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3)> results, TResult result)
        => OncePriority(await results, result);

        #endregion

        #endregion

        #region -   Four   -

        #region -   Sync   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <param name="result4"></param>
        /// <returns></returns>
        public static (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4) Collect<TResult1, TResult2, TResult3, TResult4>(this OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4)
        => (result1, result2, result3, result4);

        /// <summary>
        /// Sugar use to fill results after <see cref="Collect{TResult1, TResult2, TResult3, TResult4}(OperationResult{TResult1}, OperationResult{TResult2}, OperationResult{TResult3}, OperationResult{TResult4})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static OperationResult<TResult> Into<TResult1, TResult2, TResult3, TResult4, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4) results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, OperationResult<TResult4>, TResult> receiver)
        => results.InOnce(receiver(results.result1, results.result2, results.result3, results.result4));


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static OperationResult<TResult> InOnce<TResult1, TResult2, TResult3, TResult4, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4) results, TResult result)
        => OncePriority(results, result);

        #endregion

        #region -   Async   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <param name="result4"></param>
        /// <returns></returns>
        public static async Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4)> CollectAsync<TResult1, TResult2, TResult3, TResult4>(this Task<OperationResult<TResult1>> result1, Task<OperationResult<TResult2>> result2, Task<OperationResult<TResult3>> result3, Task<OperationResult<TResult4>> result4)
        {
            await Task.WhenAll(result1, result2, result3, result4);
            return (await result1, await result2, await result3, await result4);
        }

        /// <summary>
        /// Sugar use to fill results after <see cref="CollectAsync{TResult1, TResult2, TResult3, TResult4}(Task{OperationResult{TResult1}}, Task{OperationResult{TResult2}}, Task{OperationResult{TResult3}}, Task{OperationResult{TResult4}})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult2, TResult3, TResult4, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4)> results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, OperationResult<TResult4>, TResult> receiver)
        { var (result1, result2, result3, result4) = await results; return await results.InOnceAsync(receiver(result1, result2, result3, result4)); }


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult2, TResult3, TResult4, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4)> results, TResult result)
        => OncePriority(await results, result);

        #endregion

        #endregion

        #region -   Five   -

        #region -   Sync   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <param name="result4"></param>
        /// <param name="result5"></param>
        /// <returns></returns>
        public static (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5) Collect<TResult1, TResult2, TResult3, TResult4, TResult5>(this OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5)
        => (result1, result2, result3, result4, result5);

        /// <summary>
        /// Sugar use to fill results after <see cref="Collect{TResult1, TResult2, TResult3, TResult4,TResult5}(OperationResult{TResult1}, OperationResult{TResult2}, OperationResult{TResult3}, OperationResult{TResult4}, OperationResult{TResult5})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static OperationResult<TResult> Into<TResult1, TResult2, TResult3, TResult4, TResult5, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5) results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, OperationResult<TResult4>, OperationResult<TResult5>, TResult> receiver)
        => results.InOnce(receiver(results.result1, results.result2, results.result3, results.result4, results.result5));


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static OperationResult<TResult> InOnce<TResult1, TResult2, TResult3, TResult4, TResult5, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5) results, TResult result)
        => OncePriority(results, result);

        #endregion

        #region -   Async   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <param name="result4"></param>
        /// <param name="result5"></param>
        /// <returns></returns>
        public static async Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5)> CollectAsync<TResult1, TResult2, TResult3, TResult4, TResult5>(this Task<OperationResult<TResult1>> result1, Task<OperationResult<TResult2>> result2, Task<OperationResult<TResult3>> result3, Task<OperationResult<TResult4>> result4, Task<OperationResult<TResult5>> result5)
        {
            await Task.WhenAll(result1, result2, result3, result4, result5);
            return (await result1, await result2, await result3, await result4, await result5);
        }

        /// <summary>
        /// Sugar use to fill results after <see cref="CollectAsync{TResult1, TResult2, TResult3, TResult4,TResult5}(Task{OperationResult{TResult1}}, Task{OperationResult{TResult2}}, Task{OperationResult{TResult3}}, Task{OperationResult{TResult4}}, Task{OperationResult{TResult5}})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult2, TResult3, TResult4, TResult5, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5)> results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, OperationResult<TResult4>, OperationResult<TResult5>, TResult> receiver)
        { var (result1, result2, result3, result4, result5) = await results; return await results.InOnceAsync(receiver(result1, result2, result3, result4, result5)); }


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult2, TResult3, TResult4, TResult5, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5)> results, TResult result)
        => OncePriority(await results, result);

        #endregion

        #endregion

        #region -   Six   -

        #region -   Sync   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <param name="result4"></param>
        /// <param name="result5"></param>
        /// <param name="result6"></param>
        /// <returns></returns>
        public static (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6) Collect<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6)
        => (result1, result2, result3, result4, result5, result6);

        /// <summary>
        /// Sugar use to fill results after <see cref="Collect{TResult1, TResult2, TResult3, TResult4,TResult5,TResult6}(OperationResult{TResult1}, OperationResult{TResult2}, OperationResult{TResult3}, OperationResult{TResult4}, OperationResult{TResult5}, OperationResult{TResult6})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static OperationResult<TResult> Into<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6) results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, OperationResult<TResult4>, OperationResult<TResult5>, OperationResult<TResult6>, TResult> receiver)
        => results.InOnce(receiver(results.result1, results.result2, results.result3, results.result4, results.result5, results.result6));


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static OperationResult<TResult> InOnce<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6) results, TResult result)
        => OncePriority(results, result);

        #endregion

        #region -   Async   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <param name="result4"></param>
        /// <param name="result5"></param>
        /// <param name="result6"></param>
        /// <returns></returns>
        public static async Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6)> CollectAsync<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this Task<OperationResult<TResult1>> result1, Task<OperationResult<TResult2>> result2, Task<OperationResult<TResult3>> result3, Task<OperationResult<TResult4>> result4, Task<OperationResult<TResult5>> result5, Task<OperationResult<TResult6>> result6)
        {
            await Task.WhenAll(result1, result2, result3, result4, result5, result6);
            return (await result1, await result2, await result3, await result4, await result5, await result6);
        }

        /// <summary>
        /// Sugar use to fill results after <see cref="CollectAsync{TResult1, TResult2, TResult3, TResult4,TResult5,TResult6}(Task{OperationResult{TResult1}}, Task{OperationResult{TResult2}}, Task{OperationResult{TResult3}}, Task{OperationResult{TResult4}}, Task{OperationResult{TResult5}}, Task{OperationResult{TResult6}})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6)> results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, OperationResult<TResult4>, OperationResult<TResult5>, OperationResult<TResult6>, TResult> receiver)
        { var (result1, result2, result3, result4, result5, result6) = await results; return await results.InOnceAsync(receiver(result1, result2, result3, result4, result5, result6)); }


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6)> results, TResult result)
        => OncePriority(await results, result);

        #endregion

        #endregion

        #region -   Seven   -

        #region -   Sync   -

        /// <summary>
        /// Collect multi results dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult7"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <param name="result4"></param>
        /// <param name="result5"></param>
        /// <param name="result6"></param>
        /// <param name="result7"></param>
        /// <returns></returns>
        public static (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6, OperationResult<TResult7> result7) Collect<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6, OperationResult<TResult7> result7)
        => (result1, result2, result3, result4, result5, result6, result7);

        /// <summary>
        /// Sugar use to fill results after <see cref="Collect{TResult1, TResult2, TResult3, TResult4,TResult5,TResult6,TResult7}(OperationResult{TResult1}, OperationResult{TResult2}, OperationResult{TResult3}, OperationResult{TResult4}, OperationResult{TResult5}, OperationResult{TResult6}, OperationResult{TResult7})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult7"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static OperationResult<TResult> Into<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6, OperationResult<TResult7> result7) results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, OperationResult<TResult4>, OperationResult<TResult5>, OperationResult<TResult6>, OperationResult<TResult7>, TResult> receiver)
        => results.InOnce(receiver(results.result1, results.result2, results.result3, results.result4, results.result5, results.result6, results.result7));


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult7"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static OperationResult<TResult> InOnce<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult>(this (OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6, OperationResult<TResult7> result7) results, TResult result)
        => OncePriority(results, result);

        #endregion

        #region -   Async   -

        /// <summary>
        /// Collect multi returns dependent on first TResult1 then extension to take multi result of different type .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult7"></typeparam>
        /// <param name="result1"></param>
        /// <param name="result2"></param>
        /// <param name="result3"></param>
        /// <param name="result4"></param>
        /// <param name="result5"></param>
        /// <param name="result6"></param>
        /// <param name="result7"></param>
        /// <returns></returns>
        public static async Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6, OperationResult<TResult7> result7)> CollectAsync<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this Task<OperationResult<TResult1>> result1, Task<OperationResult<TResult2>> result2, Task<OperationResult<TResult3>> result3, Task<OperationResult<TResult4>> result4, Task<OperationResult<TResult5>> result5, Task<OperationResult<TResult6>> result6, Task<OperationResult<TResult7>> result7)
        {
            await Task.WhenAll(result1, result2, result3, result4, result5, result6, result7);
            return (await result1, await result2, await result3, await result4, await result5, await result6, await result7);
        }

        /// <summary>
        /// Sugar use to fill results after <see cref="CollectAsync{TResult1, TResult2, TResult3, TResult4,TResult5,TResult6,TResult7}(Task{OperationResult{TResult1}}, Task{OperationResult{TResult2}}, Task{OperationResult{TResult3}}, Task{OperationResult{TResult4}}, Task{OperationResult{TResult5}}, Task{OperationResult{TResult6}}, Task{OperationResult{TResult7}})"/> into DTO <see langword="new()"/>  or <see langword="anonymous"/>  .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult7"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static async Task<OperationResult<TResult>> IntoAsync<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6, OperationResult<TResult7> result7)> results,
            Func<OperationResult<TResult1>, OperationResult<TResult2>, OperationResult<TResult3>, OperationResult<TResult4>, OperationResult<TResult5>, OperationResult<TResult6>, OperationResult<TResult7>, TResult> receiver)
        { var (result1, result2, result3, result4, result5, result6, result7) = await results; return await results.InOnceAsync(receiver(result1, result2, result3, result4, result5, result6, result7)); }


        /// <summary>
        /// Call priority to get real <see cref="OperationResult{TResult}"/> with full Conditions result .
        /// </summary>
        /// <typeparam name="TResult1"></typeparam>
        /// <typeparam name="TResult2"></typeparam>
        /// <typeparam name="TResult3"></typeparam>
        /// <typeparam name="TResult4"></typeparam>
        /// <typeparam name="TResult5"></typeparam>
        /// <typeparam name="TResult6"></typeparam>
        /// <typeparam name="TResult7"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static async Task<OperationResult<TResult>> InOnceAsync<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult>(this Task<(OperationResult<TResult1> result1, OperationResult<TResult2> result2, OperationResult<TResult3> result3, OperationResult<TResult4> result4, OperationResult<TResult5> result5, OperationResult<TResult6> result6, OperationResult<TResult7> result7)> results, TResult result)
        => OncePriority(await results, result);

        #endregion

        #endregion




        /// <summary>
        /// Condition to collect many operation result into once , dependent on  Priority of <see cref="Statuses"/>
        /// <para>With <see cref="Statuses.Exception"/>  Will return first exception result  used <see cref="OperationResult{TResult}.SetException(Exception, string?)"/></para>
        /// <para>With <see cref="Statuses.Failed"/> ,<see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> Will return join of message  used <see cref="OperationResult{TResult}.SetSuccess(string)"/></para>
        /// <para>With <see cref="Statuses.Success"/> Will return TResult and  join of message used <see cref="OperationResult{TResult}.SetFailed(string, Statuses)"/> .</para>
        /// </summary>
        /// <typeparam name="TOneResult"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="oneResult">expected result</param>
        /// <param name="result">expected result</param>
        /// <returns><see cref="OperationResult{TResult}"/></returns>
        private static OperationResult<TResult> OnePriority<TOneResult, TResult>(TOneResult oneResult, TResult result)
        {
            OperationResult<TResult> operation = new();

            if (oneResult is not OperationResult Result)
                throw new ArgumentNullException(nameof(oneResult));

            if (Result.Status == Statuses.Exception)
                return operation.SetException(Result.Exception);

            if (Result.Status == Statuses.Failed || Result.Status == Statuses.Forbidden || Result.Status == Statuses.Unauthorized)
                return operation.SetFailed(Result.Message, Result.Status);

            return operation.SetSuccess(result, Result.Message);
        }




        /// <summary>
        /// Condition to collect many operation result into once , dependent on  Priority of <see cref="Statuses"/>
        /// <para>With <see cref="Statuses.Exception"/>  Will return first exception result  used <see cref="OperationResult{TResult}.SetException(Exception, string?)"/></para>
        /// <para>With <see cref="Statuses.Failed"/> ,<see cref="Statuses.Forbidden"/> and <see cref="Statuses.Unauthorized"/> Will return join of message  used <see cref="OperationResult{TResult}.SetSuccess(string)"/></para>
        /// <para>With <see cref="Statuses.Success"/> Will return TResult and  join of message used <see cref="OperationResult{TResult}.SetFailed(string, Statuses)"/> .</para>
        /// </summary>
        /// <typeparam name="TTupleResult"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result">expected result</param>
        /// <returns><see cref="OperationResult{TResult}"/></returns>
        private static OperationResult<TResult> OncePriority<TTupleResult, TResult>(TTupleResult results, TResult result) where TTupleResult : ITuple
        {
            OperationResult<TResult> operation = new();

            IEnumerable<OperationResult> listResult = Enumerable.Range(0, results.Length).Select(index => results[index]).Cast<OperationResult>();

            OperationResult? firstException = listResult.FirstOrDefault(result => result.Status == Statuses.Exception);
            if (firstException is not null)
                return operation.SetException(firstException.Exception);

            var maxasdFailded = listResult.Where(result => result.Status == Statuses.Failed || result.Status == Statuses.Forbidden ||
                 result.Status == Statuses.Unauthorized).ToList();

            Statuses? maxFailded = listResult.Where(result => result.Status == Statuses.Failed || result.Status == Statuses.Forbidden ||
            result.Status == Statuses.Unauthorized).MaxBy(result => (int)result.Status)?.Status;


            if (maxFailded is not null)
                return operation.SetFailed(String.Join(",",
                    listResult.Select((result, iter) => result.Message)), maxFailded);

            return operation.SetSuccess(result, String.Join(",",
                    listResult.Select((result, iter) => result.Message)));

        }

        ///// <summary>
        ///// Task-Wrapped-Operations use <see cref="Task.WhenAll"/>
        ///// </summary>
        ///// <param name="tasks"></param>
        ///// <returns></returns>
        //private static Task<TResult[]> WrappedOperations<TResult>(params Task<TResult>[] tasks)
        //    => Task.WhenAll(tasks);

    }
}
