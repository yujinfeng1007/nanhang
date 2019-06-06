using System;
using System.Web.Http;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Api
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [ValidationParamterFilter]
    [ProcessApiExceptionFilter]
    [CompressWebApiResultAttribute]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// 成功
        /// </summary>
        protected IHttpActionResult Success(object data = null)
        {
            var str = "[]";
            if (data != null)
            {
                if (data.GetType() == Type.GetType("System.String", true, true))
                {
                    str = data.ToString();
                }
                else
                {
                    str = data.ToJson();
                }
            }
            return Json(new ApiResult
            {
                IsError = false,
                Body = str
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

        ///// <summary>
        ///// 设置当前操作人
        ///// </summary>
        //protected static void SetCurrentOperator(string schoolCode)
        //{
        //    if (string.IsNullOrEmpty(schoolCode)) throw new Exception("学校代码不能为空!");
        //    OperatorProvider.Set(new OperatorModel { SchoolCode = schoolCode });
        //}
    }
}