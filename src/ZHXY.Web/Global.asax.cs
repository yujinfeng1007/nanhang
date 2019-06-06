using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZHXY.Application;
using ZHXY.Common;

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
            var lastError = Server.GetLastError();
            if (null != lastError)
            {
                Response.StatusCode = 200;
                if ((lastError.GetBaseException() is NoLoggedInException))
                {
                    Response.Write("<script>top.location.pathname = '/Login/Index';</script>");
                }
                else
                {
                    Response.Write(new {state="error",message=lastError.GetBaseException().Message }.Serialize());
                }
                Server.ClearError();
                Response.End();
            }
           
        }
    }
}