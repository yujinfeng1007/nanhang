using ZHXY.Application;
using ZHXY.Common;
using System.Web.Mvc;

namespace ZHXY.Dorm.Web.Controllers
{

    public class BedController : ZhxyWebControllerBase
    {
        private BedAppService App { get; }

        public BedController(BedAppService app) => App = app;

        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = new
            {
                rows = App.GetList(pagination),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }



    }
}