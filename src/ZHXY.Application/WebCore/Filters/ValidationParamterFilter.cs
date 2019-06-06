using System.Linq;
using System.Text;
using System.Web.Http.Filters;
using ZHXY.Common;
namespace ZHXY.Application
{
    public class ValidationParamterFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errorMessage = new StringBuilder();
                actionContext.ModelState.Values.ToList().ForEach(v => v.Errors.ToList().ForEach(e => errorMessage.Append($"{(e.ErrorMessage.IsEmpty() ? e.Exception.GetBaseException().Message : e.ErrorMessage)};")));
                throw new System.Exception(errorMessage.ToString());
            }
        }
    }
}