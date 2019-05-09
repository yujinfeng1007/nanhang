using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Dorm.Device.DH;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 闸机管理
    /// </summary>
    public class GateController : ZhxyWebControllerBase
    {
        private GateAppService App { get; }
        public GateController(GateAppService app) => App = app;

        public async Task<ViewResult> Buildings() => await Task.Run(() => View());



        [HttpGet]
        public ActionResult Load(Pagination p, string keyword)
        {
            var data = App.GetList(p, keyword);
            return Resultaat.PagingRst(data, p.Records, p.Total);
        }

        public ActionResult SetStatus(string[] ids, int status)
        {
            App.SetStatus(ids, status);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult Add(AddGateDto input)
        {
            App.Add(input);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult Update(UpdateGateDto input)
        {
            App.Update(input);
            return Resultaat.Success();
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            return Resultaat.Success(App.GetById(id));
        }

        public ActionResult SynDevice()
        {
            var data = DHAccount.GetMachineInfo("001", "", "", "");
            var entity = new SyncGateDto();
            if( data["data"].ToString()=="[]")
                return Message("无数据！");
            entity.DeviceNumber = data["data"]["id"]?.ToString();
            entity.Name = data["data"]["name"]?.ToString();
            entity.Status = data["data"]["online"] == null ? 0 : Convert.ToInt32(data["data"]["online"]);
            App.Sync(entity);
            return Resultaat.Success();
        }

        [HttpGet]
        public ActionResult FreeBuildings(string id)
        {
            var data = App.GetNotBoundBuildings(id);
            return Resultaat.Success(data);
        }

        [HttpGet]
        public ActionResult BoundBuildings(string id)
        {
            var data = App.GetBoundBuildings(id);
            return Resultaat.Success(data);
        }

        [HttpPost]
        public ActionResult BindBuilding(string id ,string[] buildings)
        {
             App.BindBuilding(id,buildings);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult UnbindBuilding(string id, string buildingId)
        {
            App.UnbindBuilding(id, buildingId);
            return Resultaat.Success();
        }



    }
}