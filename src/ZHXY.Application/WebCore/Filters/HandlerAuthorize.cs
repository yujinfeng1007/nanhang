using System.Text;
using System.Web;
using System.Web.Mvc;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 权限验证
    /// </summary>
    public class HandlerAuthorize : AuthorizeAttribute
    {
        private bool Ignore { get; set; }

        public HandlerAuthorize(bool ignore = false) => Ignore = ignore;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore) return;
            if (OperatorProvider.Current == null || OperatorProvider.Current == null)
            {
                var sbScript = new StringBuilder();
                if (filterContext.HttpContext.Request.Browser.IsMobileDevice)
                {
                    filterContext.Result = new ContentResult() { Content = "#301#" };
                }
                else
                {
                    sbScript.Append("<script type='text/javascript'>alert('登录超时，请刷新页面！');</script>");
                    filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
                }
                return;
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var operatorProvider = OperatorProvider.Current;
            //var roleId = operatorProvider.RoleId;
            var moduleId = httpContext.GetCookie("cola_currentmoduleid")?.Value;
            var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
            var app = new SysRoleAuthorizeAppService();
            var result = false;
            foreach (var e in operatorProvider.Roles)
            {
                result = result || app.ActionValidate(e.Key, moduleId, action);
            }
            return result;
        }
    }
}