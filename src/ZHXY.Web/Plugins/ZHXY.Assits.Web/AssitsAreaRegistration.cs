using System.Web.Mvc;

namespace ZHXY.Web.ScheduleManage
{
    public class AssitsAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Assits";
         
        public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute(
               AreaName,
              AreaName + "/{controller}/{action}",
              new { action = "Index" },
                new[] { "ZHXY.Assits.Web.Controllers" }
            );
    }
}