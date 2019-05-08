using NFine.Code;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class School_ExamSignController : Controller
    {
        // GET: /SchoolManage/School_ExamSign/

        private School_ExamSignUp_App app = new School_ExamSignUp_App();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[HandlerAjaxOnly]
        public ActionResult GetFormByExamNum(string F_ExamNum)
        {
            var data = app.GetFormByF_ExamNum(F_ExamNum);
            return Content(data.ToJson());
        }
    }
}