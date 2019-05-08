
using ZHXY.Application;
using ZHXY.Common;
using System.Web.Mvc;

namespace ZHXY.Dorm.Web.Controllers
{

    public class RepairController : ZhxyWebControllerBase
	{
		private DormRepairAppService App { get; }
        public RepairController(DormRepairAppService app) => App = app;

        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis,string F_Grade, string F_Class)
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
        
        [HttpGet]
        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Content(data.ToJson());
        }
        
       
	}
}