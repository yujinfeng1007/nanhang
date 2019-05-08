using ZHXY.Application;
using ZHXY.Common;
using System.Web.Mvc;

namespace ZHXY.Web.Dorm.Controllers
{

    public class DormStudentController : ZhxyWebControllerBase
	{
   		     
	 
		private DormStudentAppService App { get; }

        public DormStudentController(DormStudentAppService app)
        {
            App = app;
        }
		
        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis,string F_Grade, string F_Class)
        {
            var data = new
            {
                rows = App.GetList(pagination, F_Year, F_Semester, F_Divis,F_Grade, F_Class),
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
        
        
        

   
	}
}