using System.Web.Mvc;
using ZHXY.Application;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class DicController : ZhxyWebControllerBase
    {
        private DicService App { get; }

        public DicController(DicService app) => App = app;


        [HttpGet]
        public JsonResult GetData()
        {
           var data= App.GetData();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}