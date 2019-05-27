using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Web.Dorm.Controllers
{
    public class DeviceController : ZhxyController
    {
        private DeviceService App { get; }

        public DeviceController(DeviceService app) => App = app;

        public async Task<ViewResult> Buildings() => await Task.Run(() => View());

        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = App.GetList(pagination, keyword),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Device entity)
        {
            App.SubmitForm(entity);
            return Result.Success();
        }

        [HttpGet]
        public ActionResult FreeBuildings(string id)
        {
            var data = App.GetNotBoundBuildings(id);
            return Result.Success(data);
        }

        [HttpGet]
        public ActionResult BoundBuildings(string id)
        {
            var data = App.GetBoundBuildings(id);
            return Result.Success(data);
        }

        [HttpPost]
        public ActionResult BindBuilding(string id, string[] buildings)
        {
            App.BindBuilding(id, buildings);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult UnbindBuilding(string id, string buildingId)
        {
            App.UnbindBuilding(id, buildingId);
            return Result.Success();
        }
    }
}