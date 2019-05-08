using System;
using System.Web.Mvc;
using ZHXY.Common;

namespace ZHXY.Web
{
    /// <summary>
    /// 过滤器配置
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 注册全局过滤器
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogError());
            filters.Add(new ErrorHandle());
        }
    }


    /// <summary>
    /// 记录错误
    /// </summary>
    public class LogError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var error = filterContext.Exception;
            var logger=Logger.GetLogger("LogError");
            logger.Error(new {time=DateTime.Now,errorMessage= error.GetBaseException().Message, stackTrace = error.StackTrace });
            filterContext.ExceptionHandled = true;
        }
    }

    /// <summary>
    /// 处理错误
    /// </summary>
    public class ErrorHandle : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var handled = filterContext.ExceptionHandled;
            if (handled) return;

            // todo
            filterContext.Result = new ContentResult { Content = new { state = "error", message = filterContext.Exception.GetBaseException().Message }.ToCamelJson() };
           

            filterContext.ExceptionHandled = true;
        }
    }
}