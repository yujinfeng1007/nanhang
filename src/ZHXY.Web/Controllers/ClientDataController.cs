using System.Collections.Generic;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    public class ClientDataController : Controller
    {
        [HttpGet]
        public JsonResult Get(string clientType)
        {
            var data = new data
            {
                dataItems = CacheService.GetDataItemListByCache(),
                duty = CacheService.GetDutyListByCache(),
                organize = CacheService.GetOrganizeListByCache(),
                role = CacheService.GetRoleListByCache()
            };

            if (Operator.GetCurrent() == null) return Json(data, JsonRequestBehavior.AllowGet);
            data.authorizeMenu = CacheService.GetMenuList(clientType).ToString();
            data.authorizeButton = (Dictionary<string, object>)CacheService.GetMenuButtonList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult UserInfo()
        {
            var current = Operator.GetCurrent();
            return Json(new
            {
                UserCode= current?.Account,
                UserName= current?.Name,
                current?.HeadIcon
            }, JsonRequestBehavior.AllowGet);
        }


        private class data
        {
            public Dictionary<string, object> authorizeButton;
            public string authorizeMenu;
            public Dictionary<string, object> dataItems;
            public Dictionary<string, object> duty;
            public Dictionary<string, object> organize;
            public Dictionary<string, object> role;
        }
    }
}