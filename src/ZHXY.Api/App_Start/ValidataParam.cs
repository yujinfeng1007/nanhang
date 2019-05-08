using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Api
{
    /// <summary>
    /// 验证数据模型
    /// </summary>
    public class ValidataParam : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errotMessage = new StringBuilder();
                actionContext.ModelState.Values.ToList().ForEach(v => v.Errors.ToList().ForEach(e => errotMessage.Append($"{e.ErrorMessage};")));

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(
                    content: new ApiResult
                    {
                        IsError = true,
                        ErrorCodeValue = "1000",
                        ErrorMsgInfo = errotMessage.ToString()
                    }.ToJson(),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json"
                );
                actionContext.Response = response;
            }
        }
    }
}