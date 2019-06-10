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
            var data = new
            {
                dataItems = CacheService.GetDataItemListByCache(),
                duty = CacheService.GetDutyListByCache(),
                organize = CacheService.GetOrganizeListByCache(),
                role = CacheService.GetRoleListByCache(),
                authorizeMenu = CacheService.GetMenuList(clientType).ToString(),
                authorizeButton = (Dictionary<string, object>)CacheService.GetMenuButtonList()
            };
            return Json(data,JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult UserInfo()
        {
            var current = Operator.GetCurrent();
            return Json(new
            {
                UserCode = current?.Account,
                UserName = current?.Name,
                current?.HeadIcon
            }, JsonRequestBehavior.AllowGet);
        }
    }
}