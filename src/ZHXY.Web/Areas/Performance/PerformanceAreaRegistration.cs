using System.Web.Mvc;

namespace ZHXY.Web.Performance
{
    public class PerformanceAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Performance";

        public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute(
               "PerformanceManage_default",
              "Performance/{controller}/{action}"
            );
    }
}