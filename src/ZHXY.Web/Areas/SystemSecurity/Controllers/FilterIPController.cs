
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemSecurity.Controllers
{
    public class FilterIPController : ZhxyWebControllerBase
    {
        private SysFilterIPAppService App { get; }
        public FilterIPController(SysFilterIPAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetGridJson(string keyword)
        {
            var data = App.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(FilterIP filterIPEntity, string keyValue)
        {
            App.SubmitForm(filterIPEntity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                App.DeleteForm(F_Id[i]);
            }
            //filterIPApp.DeleteForm(keyValue);
            return Message("删除成功。");
        }
    }
}