using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class School_MailBoxController : ControllerBase
    {
        public ActionResult OutBox() => View();

        public ActionResult DraftBox() => View();

        public ActionResult SendBox() => View();

        public ActionResult InBox() => View();

        public ActionResult RecycleBox() => View();

        public ActionResult MailDetail() => View();

        public ActionResult MailContents() => View();
    }
}