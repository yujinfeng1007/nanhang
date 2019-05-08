using System.Web.Mvc;

namespace ZHXY.Dorm.Web
{
    public class ScheduleManageAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Dorm";

        public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute(
               AreaName,
              AreaName + "/{controller}/{action}",
              new { action = "Index" },
                new[] { "ZHXY.Dorm.Web.Controllers" }
            );
    }
}