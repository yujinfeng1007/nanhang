using System.Web.Mvc;

namespace ZHXY.Web.Report
{
    public class ReportManageAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Report";

        public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute(
               "ZhxyReportManage",
              "ReportManage/{controller}/{action}",
              new { action = "Index" }
            );
    }
}