using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace OpenApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //ZHXY.Application.AutoFacExt.InitAutofac();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new HandlerErrorAttribute());
            // 使api返回为json
            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss" });
        }

        public override void Init()
        {
            PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }
    }
}