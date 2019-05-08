using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.SessionState;

namespace ZHXY.Api
{
    /// <summary>
    ///WebApi应用程序
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        /// 应用启动入口
        /// </summary>
        protected void Application_Start()
        {
            //ZHXY.Application.AutoFacExt.InitAutofac();//依赖注入初始化
            Bootstrapper.Run();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }
    }
}