using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Assists.Application;
using ZHXY.Common;

namespace ZHXY.Assits.Web.Controllers
{
    /// <summary>
    /// 辅助屏控制器
    /// </summary>
    public class AssitsDevicesController : ZhxyWebControllerBase
    {
        private AssistAppService App => new AssistAppService();

        /// <summary>
        /// 解绑
        /// </summary>
        [HttpPost]
        public ActionResult Unbind(string id)
        {
            App.Unbind(id);
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.DEVICES);
            return Message("解绑成功！");
        }

        [HttpGet]
        public ActionResult GetList(Pagination pagination, string keyword, string displayStyle, string status)
        {
            var data = App.GetList(pagination, keyword, displayStyle, status);
            return PagingResult(data, pagination.Records, pagination.Total);
        }

        /// <summary>
        /// 获取绑定的教室信息
        /// </summary>
        [HttpGet]
        public ActionResult GetBindInfo(string id)
        {
            return Result(App.GetBindInfo(id));
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        [HttpGet]
        public ActionResult Get(string id)
        {
            var data = App.Get(id);
            return Result(data);
        }

        [HttpGet]
        public ActionResult GetBySn(string sn)
        {
            var data = App.Get(sn);
            return Result(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UpdateDeviceDto dto)
        {
            App.Update(dto);
            Task.Run(() => CacheFactory.Cache().RemoveCache(SmartCampusConsts.DEVICES));
            return AjaxResult();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            Task.Run(() => CacheFactory.Cache().RemoveCache(SmartCampusConsts.DEVICES));
            App.Delete(id);
            return AjaxResult();
        }
    }
}