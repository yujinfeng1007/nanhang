
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using ZHXY.Common;

namespace OpenApi
{
    public class HandlerErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            WriteLog(actionExecutedContext);

            //2.返回调用方具体的异常信息
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
            else if (actionExecutedContext.Exception is ExceptionContext)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(actionExecutedContext.Exception.ToJson()),
                };
                actionExecutedContext.Response = resp;
            }
            else
            {
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(new ExceptionContext
                    {
                        ErrorCodeValue = "-1",
                        IsError = true,
                        ErrorMsgInfo = actionExecutedContext.Exception.Message
                    }.ToJson()),
                };
                actionExecutedContext.Response = resp;

                //actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            base.OnException(actionExecutedContext);
        }

        private void WriteLog(HttpActionExecutedContext context)
        {
            if (context == null)
                return;
            var log = Logger.GetLogger(context.ActionContext.ToString());
            log.Error(context.Exception);
        }
    }
}