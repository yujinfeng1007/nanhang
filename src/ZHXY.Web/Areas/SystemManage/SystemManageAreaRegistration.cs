using System.Web.Mvc;

namespace ZHXY.Web.SystemManage
{
    public class SystemManageAreaRegistration : AreaRegistration
    {
        public override string AreaName => "SystemManage";

        public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute(
               "ZhxySystemManage",
              "SystemManage/{controller}/{action}",
              new { action = "Index" }
            );
    }
}