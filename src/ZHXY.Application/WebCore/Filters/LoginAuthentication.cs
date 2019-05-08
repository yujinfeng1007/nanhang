using System.Web.Mvc;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 登录验证
    /// </summary>
    public class LoginAuthentication : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (OperatorProvider.Current != null) return;                   // 已登录,验证通过=>直接返回
            filterContext.Result = new ContentResult() { Content = "<script>top.location.pathname = '/Login/Index';</script>" };
        }
    }
}