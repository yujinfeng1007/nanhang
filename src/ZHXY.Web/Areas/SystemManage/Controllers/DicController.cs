using System.Web.Mvc;
using ZHXY.Application;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class DicController : ZhxyController
    {
        private DicService App { get; }

        public DicController(DicService app) => App = app;


        [HttpGet]
        public JsonResult GetData()
        {
           var data= App.GetData();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItems(string code)
        {
            var data=App.GetItems(code);
            return Resultaat.Success(data);
        }


        [HttpGet]
        public ActionResult Load()
        {
            var data = App.GetList();
            return Resultaat.Success(data);
        }


        public ActionResult Add(DicDto dto)
        {
            App.Add(dto);
            return Resultaat.Success();
        }

        public ActionResult Update(DicDto dto)
        {
            App.Update(dto);
            return Resultaat.Success();
        }

        public ActionResult Delete(string id)
        {
            App.Delete(id);
            return Resultaat.Success();
        }

        public ActionResult AddItem(DicItemDto dto)
        {
            App.AddItem(dto);
            return Resultaat.Success();
        }

        public ActionResult UpdateItem(DicItemDto dto)
        {
            App.UpdateItem(dto);
            return Resultaat.Success();
        }

        public ActionResult DeleteItem(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                App.DeleteItem(id.Split(','));
            }
            return Resultaat.Success();
        }
    }
}