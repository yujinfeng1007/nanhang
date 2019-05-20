
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class FilterIPController : ZhxyController
    {
        private FilterIpService App { get; }
        public FilterIPController(FilterIpService app) => App = app;

        [HttpGet]
        
        public ActionResult Load(string keyword)
        {
            var data = App.GetList(keyword);
            return Result.Success(data);
        }

        [HttpGet]
        
        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Result.Success(data);
        }

        [HttpPost]
        public ActionResult Add(AddFilterIpDto input)
        {
            App.Add(input);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult Update(UpdateFilterIpDto input)
        {
            App.Update(input);
            return Result.Success();
        }
        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            App.Delete(id);
            return Result.Success();
        }
    }
}