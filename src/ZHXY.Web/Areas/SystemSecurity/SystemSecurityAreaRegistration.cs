using System.Web.Mvc;

namespace ZHXY.Web.SystemSecurity
{
    public class SystemSecurityAreaRegistration : AreaRegistration
    {
        public override string AreaName => "SystemSecurity";

        public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute(
                  "ZhxySystemSecurity",
                 "SystemSecurity/{controller}/{action}",
                 new {action = "Index" }
           );
    }
}