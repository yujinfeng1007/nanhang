using System.Web.Mvc;

namespace ZHXY.Web.Dorm
{
    public class DormAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Dorm";

        public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute(
                "Dorm_default",
                "Dorm/{controller}/{action}",
                 new { action = "Index" }
            );
    }
}