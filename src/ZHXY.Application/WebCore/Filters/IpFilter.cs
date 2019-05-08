using ZHXY.Common;
using System.Configuration;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ZHXY.Application
{
    /// <summary>
    /// IP过滤
    /// </summary>
    public class IpFilter : ActionFilterAttribute
    {
        public bool Ignore { get; set; }

        public IpFilter(bool ignore = true) => Ignore = ignore;

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            if (!ActionAuthorize(filterContext))
                //403 禁止访问
                filterContext.Response = filterContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Forbidden };
        }

        private bool ActionAuthorize(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            //string host = HttpContext.Current.Request.Url.Host.ToLower();
            var host = Net.Ip;
            var ips = ConfigurationManager.AppSettings["ips"];
            var result = false;
            if ("*".Equals(ips))
            {
                result = true;
            }
            else
            {
                var ipList = ips.Split(',');
                foreach (var ip in ipList)
                {
                    if (ip.Equals(host))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}