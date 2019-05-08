using NFine.Code;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class School_Bill_VouController : Controller
    {
        // GET: /SchoolManage/School_Bill_Vou/
        private School_Bill_Voucher_App app = new School_Bill_Voucher_App();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetFormByF_Voucher_Num(string F_Charge_ID, string F_Voucher_Num, string type)
        {
            var data = app.GetFormByF_Charge_ID(F_Charge_ID, F_Voucher_Num, type);
            return Content(data.ToJson());
        }
    }
}