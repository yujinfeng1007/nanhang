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
            if (Operator.Current == null || Operator.Current == null)
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
            return true;
        }
    }
}