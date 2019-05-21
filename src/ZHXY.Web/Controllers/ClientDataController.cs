using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    public class ClientDataController : Controller
    {
        public ResourceService App { get; }
        public ClientDataController(ResourceService app) => App = app;

        [HttpGet]
        public JsonResult Get()
        {
            var data = new
            {
                area = CacheService.GetAreaListByCache(),
                dataItems = CacheService.GetDataItemListByCache(),
                duty = CacheService.GetDutyListByCache(),
                organize = CacheService.GetOrganizeListByCache(),
                role = CacheService.GetRoleListByCache(),
                authorizeMenu = CacheService.GetMenuList().ToString()
            };
            if (Operator.GetCurrent() == null) return Json(data, JsonRequestBehavior.AllowGet);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMenu() => Result.Success(App.GetAllMenu());
    }
}