using System.Configuration;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    public class VisitApprovalController : BaseController
    {
        private VisitorService App { get; }

        public VisitApprovalController(VisitorService app) => App = app;

        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination)
        {
            var data = new
            {
                rows = App.NotCheckApply(pagination),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult ApprovalList(string[] ids, int pass)
        {
            var img = ConfigurationManager.AppSettings["NH_DEFAULT_IMG"];
            App.ApprovalList(ids, pass, img);
            return Result.Success();
        }
    }
}