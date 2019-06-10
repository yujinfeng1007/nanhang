using ZHXY.Application;
using ZHXY.Common;
using System.Web.Mvc;

namespace ZHXY.Web.Dorm.Controllers
{

    public class InOutReceiveController : BaseController
	{
		private InOutReceiveAppService App { get; }

        public InOutReceiveController(InOutReceiveAppService app) => App = app;

        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
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
        public ActionResult GetFormJson(string id)
        {
            var data = App.GetById(id);
            return Content(data.ToJson());
        }
        
      
        
        [HttpPost]
        public ActionResult DeleteForm(string id)
        {
            App.Delete(id);
            return Result.Success();
        }
        
	
       
	}
}