using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Web.Shared;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 闸机管理
    /// </summary>
    public class GateController : BaseController
    {
        private GateAppService App { get; }
        public GateController(GateAppService app) => App = app;

        public async Task<ViewResult> Buildings() => await Task.Run(() => View());



        [HttpGet]
        public ActionResult Load(Pagination p, string keyword)
        {
            var data = App.GetList(p, keyword);
            return Result.PagingRst(data, p.Records, p.Total);
        }

        public ActionResult SetStatus(string[] ids, int status)
        {
            App.SetStatus(ids, status);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult Add(AddGateDto input)
        {
            App.Add(input);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult Update(UpdateGateDto input)
        {
            App.Update(input);
            return Result.Success();
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            return Result.Success(App.GetById(id));
        }

        public ActionResult SynDevice()
        {
            var data = DHAccount.GetMachineInfo("001", "01;01,02,05,07_4,08_3,08_5,34,38;01@9,07,12", "0", "");
            var entity = new AddGateDto();
            if (data == null || data["data"].ToString() == "[]")  return Result.Success();
            var datas = (List<object>)data["data"].ToObject(typeof(List<object>));
            for (var i = 0; i < datas.Count; i++)
            {
                entity.DeviceNumber = data["data"][i]["id"]?.ToString();
                entity.Name = data["data"][i]["name"]?.ToString();
                entity.Status = data["data"][i]["online"] == null ? 0 : Convert.ToInt32(data["data"][i]["online"]);
                App.Sync(entity);
            }
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
        public ActionResult BindBuilding(string id ,string[] buildings)
        {
             App.BindBuilding(id,buildings);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult UnbindBuilding(string id, string buildingId)
        {
            App.UnbindBuilding(id, buildingId);
            return Result.Success();
        }

        [HttpGet]
        public ActionResult GetByBuilding(string id)
        {
            var data = App.GetByBuilding(id);
            return Result.Success(data);
        }


        [HttpGet]
        public ActionResult GetUsers(string id)
        {
            var data = App.GetUsers(id);
            return Result.Success(data);
        }
    }
}