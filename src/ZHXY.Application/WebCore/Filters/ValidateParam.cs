using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using ZHXY.Common;
namespace ZHXY.Application
{
    public class ValidateParam : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errotMessage = new StringBuilder();
                actionContext.ModelState.Values.ToList().ForEach(v => v.Errors.ToList().ForEach(e => errotMessage.Append($"{e.ErrorMessage};")));

                actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                    content: new
                    {
                        state = "error",
                        message = errotMessage.ToString()
                    }.ToJson(),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json"
                )
                };
            }
        }
    }
}