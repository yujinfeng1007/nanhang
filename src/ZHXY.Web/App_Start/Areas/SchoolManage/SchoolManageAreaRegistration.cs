using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage
{
    public class SchoolManageAreaRegistration : AreaRegistration
    {
        public override string AreaName { get { return "SchoolManage"; } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              AreaName + "_Default",
              AreaName + "/{controller}/{action}/{id}",
              new { area = AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
              new string[] { "NFine.Web.Areas." + AreaName + ".Controllers" }
            );
        }
    }
}