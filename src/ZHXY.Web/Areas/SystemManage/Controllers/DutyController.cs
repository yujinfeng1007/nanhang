using System.Web.Mvc;
using ZHXY.Application;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    public class DutyController : ZhxyWebControllerBase
    {
        private DutyService App { get; }

        public DutyController(DutyService app) => App = app;

        [HttpGet]
        public ActionResult Load(string keyword)
        {
            var data = App.GetList(keyword);
            return Resultaat.Success(data);
        }


        [HttpGet]
        public ActionResult Get(string id)
        {
            var data = App.Get(id);
            return Resultaat.Success(data);
        }

        [HttpPost]
        public ActionResult Update(UpdateDutyDto dto)
        {
            App.Update(dto);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult Add(AddDutyDto dto)
        {
            App.Add(dto);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult Delete(string  id)
        {
            App.Delete(id);
            return Resultaat.Success();
        }

    }
}