using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZHXY.Application;

namespace ZHXY.Web
{
    public class MvcApplication : HttpApplication
    { 
        protected void Application_Start()
        {
            DIHelper.SetMvcDependencyResolver();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // todo
        }
        protected void Application_Error(object sender, EventArgs e)
        {
           // todo
        }
    }
}