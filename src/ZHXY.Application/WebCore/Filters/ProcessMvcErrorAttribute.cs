using System;
using System.Text;
using System.Web.Mvc;
using log4net;
using ZHXY.Common;

namespace ZHXY.Application
{
    public class ProcessMvcErrorAttribute : HandleErrorAttribute
    {
        private static ILog FileLogger { get; } = LogHelper.GetLogger(typeof(ProcessMvcErrorAttribute));
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                // 1.统一返回
                var baseException = filterContext.Exception.GetBaseException();
                filterContext.Result = new ContentResult { Content = new { state = ResultState.Error, message = baseException.Message }.ToJson(), ContentEncoding = Encoding.UTF8, ContentType = "application/json" };
                filterContext.ExceptionHandled = true;

                // 2.记录异常信息
                var req = filterContext.HttpContext.Request;
                var errorMessage = new
                {
                    req.RequestType,
                    req.Path,
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ErrorMessage = baseException.Message
                };
                FileLogger.Error(errorMessage);


            }
        }
    }
}