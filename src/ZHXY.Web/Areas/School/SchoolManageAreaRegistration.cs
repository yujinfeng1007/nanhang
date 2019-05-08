using System.Web.Mvc;

namespace ZHXY.Web.School
{
    public class SchoolManageAreaRegistration : AreaRegistration
    {
        public override string AreaName => "School";

        public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute(
               "ZhxySchoolManage",
              "SchoolManage/{controller}/{action}",
              new { action = "Index"}
            );
    }
}