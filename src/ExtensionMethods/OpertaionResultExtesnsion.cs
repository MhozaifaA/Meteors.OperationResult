using Meteors.OperationResult;
using Meteors.OperationResult.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Meteors
{

    /// <summary>
    /// Helper extensions of <see cref="OperationResult{T}"/>.
    /// </summary>
    public static class OpertaionResultExtesnsion
    {

        /// <summary>
        /// Encapsulation object to <see cref="OperationResult{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns> <see cref="OperationResult{T}"/> </returns>
        public static OperationResult<T> ToOperationResult<T>(this T @object)
         => new OperationResult<T>().SetSuccess(@object);


        /// <summary>
        /// Set custom <see cref="OperationResultBase.StatusCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static OperationResult<T> WithStatusCode<T>(this OperationResult<T> result, int statusCode)
        {
            result.StatusCode = statusCode;
            return result;
        }

        /// <summary>
        /// Set custom <see cref="OperationResultBase.StatusCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static async Task<OperationResult<T>> WithStatusCodeAsync<T>(this Task<OperationResult<T>> result, int statusCode)
        {
            var _result = await result;
            _result.StatusCode = statusCode;
            return _result;
        }


        /// <summary>
        /// Return <see cref="JsonResult"/> with real result completely .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns><see cref="JsonResult"/></returns>
        public static JsonResult ToJsonResult<T>(this OperationResult<T> result)
        {
            return result.ToJsonResult(false); //extra to refactoring
        }

        /// <summary>
        /// Return <see cref="JsonResult"/> with real result completely .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="isBody">boolean value if return json complete body of operation</param>
        /// <returns></returns>
        public static JsonResult ToJsonResult<T>(this OperationResult<T> result, bool isBody = false)
        {
            return result.OperationResultType switch
            {
                OperationResultTypes.Success => result.GetValidResult(jsonMessage:null,hasResult: true, isBody),
                OperationResultTypes.Exist => result.GetValidResult(jsonMessage: result.Message,isBody: isBody),
                OperationResultTypes.NotExist => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                OperationResultTypes.Failed => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                OperationResultTypes.Forbidden => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                OperationResultTypes.Unauthorized => result.GetValidResult(jsonMessage: result.Message, isBody: isBody),
                OperationResultTypes.Exception => result.GetValidResult(jsonMessage: result.FullExceptionMessage, isBody: isBody),
                _ => throw new NotImplementedException($"Update source code to catch new {nameof(OperationResultTypes)} value: {result.OperationResultType}"),
            };
        }


        /// <summary>
        /// Return <see cref="JsonResult"/> with real result completely .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns><see cref="Task{JsonResult}"/></returns>
        public static Task<JsonResult> ToJsonResultAsync<T>(this Task<OperationResult<T>> result) => result.ToJsonResultAsync(false);


        /// <summary>
        /// Return <see cref="JsonResult"/> with real result completely .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns><see cref="Task{JsonResult}"/></returns>
        public static async Task<JsonResult> ToJsonResultAsync<T>(this Task<OperationResult<T>> result, bool isBody = false) => (await result).ToJsonResult(isBody);


        /// <summary>
        /// Return <see langword="real result"/> .
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="jsonMessage">String of type</param>
        /// <param name="hasResult"> boolean value if return json of main type T </param>
        /// <param name="isBody"> boolean value if return json complete body of operation</param>
        /// <returns><see cref="JsonResult"/></returns>
        private static JsonResult GetValidResult<T>(this OperationResult<T> result, string jsonMessage = null, bool hasResult = false, bool isBody = false)
        {
            if (!result.HasCustomStatusCode && (result.StatusCode??0) == 0)
                result.StatusCode = (int)result.OperationResultType;

            bool jsonMessageIsNullOrEmpty = jsonMessage.IsNullOrEmpty();
            if (isBody)
            {
                if (jsonMessageIsNullOrEmpty)
                    result.Message = result.OperationResultType.ToString();
              
                return new JsonResult(result) { StatusCode = result.StatusCode };
            }

            if (hasResult)
                return new JsonResult(result.Data) { StatusCode = result.StatusCode };

            if (jsonMessageIsNullOrEmpty)
                return new JsonResult(result.OperationResultType.ToString()) { StatusCode = result.StatusCode };

            return new JsonResult(jsonMessage) { StatusCode = result.StatusCode };
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
            await Task.WhenAll(result1);
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
        /// Condition to collect many operation result into once , dependent on  Priority of <see cref="OperationResultTypes"/>
        /// <para>With <see cref="OperationResultTypes.Exception"/>  Will return first exception result  used <see cref="OperationResult{TResult}.SetException(Exception)"/></para>
        /// <para>With <see cref="OperationResultTypes.Failed"/> ,<see cref="OperationResultTypes.Forbidden"/> and <see cref="OperationResultTypes.Unauthorized"/> Will return join of message  used <see cref="OperationResult{TResult}.SetSuccess(string)"/></para>
        /// <para>With <see cref="OperationResultTypes.Success"/> Will return TResult and  join of message used <see cref="OperationResult{TResult}.SetFailed(string, OperationResultTypes)"/> .</para>
        /// </summary>
        /// <typeparam name="TOneResult"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="oneResult">expected result</param>
        /// <param name="result">expected result</param>
        /// <returns><see cref="OperationResult{TResult}"/></returns>
        private static OperationResult<TResult> OnePriority<TOneResult, TResult>(TOneResult oneResult, TResult result)
        {
            OperationResult<TResult> operation = new();

            OperationResultBase Result = oneResult as OperationResultBase;

            if (Result.OperationResultType == OperationResultTypes.Exception)
                return operation.SetException(Result.Exception);

            if (Result.OperationResultType == OperationResultTypes.Failed || Result.OperationResultType == OperationResultTypes.Forbidden || Result.OperationResultType == OperationResultTypes.Unauthorized)
                return operation.SetFailed(String.Join(",",
                    Result.Message.IsNullOrEmpty().NestedIF(()=>$"Result {1} not contain Message or Success",()=> Result.Message)), Result.OperationResultType);

            return operation.SetSuccess(result, String.Join(",",
                  Result.Message.IsNullOrEmpty().NestedIF(()=>$"Result {1} not contain Message or Success",()=> Result.Message)));

        }




        /// <summary>
        /// Condition to collect many operation result into once , dependent on  Priority of <see cref="OperationResultTypes"/>
        /// <para>With <see cref="OperationResultTypes.Exception"/>  Will return first exception result  used <see cref="OperationResult{TResult}.SetException(Exception)"/></para>
        /// <para>With <see cref="OperationResultTypes.Failed"/> ,<see cref="OperationResultTypes.Forbidden"/> and <see cref="OperationResultTypes.Unauthorized"/> Will return join of message  used <see cref="OperationResult{TResult}.SetSuccess(string)"/></para>
        /// <para>With <see cref="OperationResultTypes.Success"/> Will return TResult and  join of message used <see cref="OperationResult{TResult}.SetFailed(string, OperationResultTypes)"/> .</para>
        /// </summary>
        /// <typeparam name="TTupleResult"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="results"></param>
        /// <param name="result">expected result</param>
        /// <returns><see cref="OperationResult{TResult}"/></returns>
        private static OperationResult<TResult> OncePriority<TTupleResult, TResult>(TTupleResult results, TResult result) where TTupleResult : ITuple
        {
            OperationResult<TResult> operation = new();

            IEnumerable<OperationResultBase> listResult = Enumerable.Repeat(0, results.Length).Select(index => results[index]).Cast<OperationResultBase>();

            OperationResultBase firstException = listResult.FirstOrDefault(result => result.OperationResultType == OperationResultTypes.Exception);
            if (firstException != null)
                return operation.SetException(firstException.Exception);

            if (listResult.Any(result => result.OperationResultType == OperationResultTypes.Failed || result.OperationResultType == OperationResultTypes.Forbidden || result.OperationResultType == OperationResultTypes.Unauthorized))
                return operation.SetFailed(String.Join(",",
                    listResult.Select((result, iter) => result.Message.IsNullOrEmpty().NestedIF(()=>$"Result {iter} not contain Message or Success", () => result.Message))), listResult.Max(result => result.OperationResultType));

            return operation.SetSuccess(result, String.Join(",",
                    listResult.Select((result, iter) => result.Message.IsNullOrEmpty().NestedIF(() => $"Result {iter} not contain Message or Success", () => result.Message))));

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
