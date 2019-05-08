using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using log4net;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 自定义错误处理
    /// </summary>
    public class CustomApiExceptionFilter : ExceptionFilterAttribute
    {
        private static ILog FileLogger { get; } = Logger.GetLogger("ErrorLogAttribute");

        public override void OnException(HttpActionExecutedContext actContext)
        {
            // 1.记录异常信息

            var ex = actContext.Exception;
            var errorMessage = new
            {
                actContext.Request.RequestUri.LocalPath,
                actContext.Request.RequestUri.OriginalString,
                ErrorMessage = ex?.GetBaseException()?.Message
            };
            FileLogger.Error(errorMessage);

            // 2.统一返回
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(
                content: new ApiResult
                {
                    IsError = true,
                    ErrorCodeValue = "0001",
                    ErrorMsgInfo = ex.GetBaseException().Message
                }.ToJson(),
                encoding: Encoding.UTF8,
                mediaType: "application/json"
            );
            actContext.Response = response;
        }
    }
}