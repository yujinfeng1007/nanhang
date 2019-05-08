using System;
using System.Web.Http;
using ZHXY.Common;
using ZHXY.Application;

namespace ZHXY.Dorm.Web.ApiControllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [CustomApiExceptionFilter]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// 成功
        /// </summary>
        protected IHttpActionResult Success(object data = null)
        {
            var body = null == data ? "[]" : data.ToJson();
            return Json(new ApiResult
            {
                IsError = false,
                Body = body
            });
        }

        /// <summary>
        /// 错误
        /// </summary>
        protected IHttpActionResult Error(string errorCodeValue, string errorMsgInfo) => Json(new ApiResult
        {
            IsError = true,
            ErrorCodeValue = errorCodeValue,
            ErrorMsgInfo = errorMsgInfo,
        });

        /// <summary>
        /// 错误
        /// </summary>
        protected IHttpActionResult Error(string errorCodeValue) => Json(new ApiResult
        {
            IsError = true,
            ErrorCodeValue = errorCodeValue
        });

        /// <summary>
        /// 错误
        /// </summary>
        protected IHttpActionResult Error(Exception ex) => Json(new ApiResult
        {
            IsError = true,
            ErrorMsgInfo = ex.Message,
            Body = "[]"
        });
    }
}