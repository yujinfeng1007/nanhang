using NFine.Application.SchoolManage;
using NFine.Code;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class School_NoticeNotController : Controller
    {
        // GET: /SchoolManage/School_NoticeNot/
        private NoticeApp noticeapp = new NoticeApp();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string keyword, string F_IsFront)
        {
            var data = new
            {
                rows = noticeapp.GetListLimZJ(pagination, F_IsFront, keyword),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            //var data = noticeapp.GetList(keyword);
            return Content(data.ToJson());
        }
    }
}