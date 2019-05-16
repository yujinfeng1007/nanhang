using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    public class ClientDataController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            var data = new data();
            var cache = CacheFactory.Cache();
            data.area = SysCacheAppService.GetAreaListByCache();
            data.dataItems = SysCacheAppService.GetDataItemListByCache();
            data.duty = SysCacheAppService.GetDutyListByCache();
            data.organize = SysCacheAppService.GetOrganizeListByCache();
            data.role = SysCacheAppService.GetRoleListByCache();

            if (Operator.Current == null) return Json(data, JsonRequestBehavior.AllowGet);
            //菜单按钮权限
            var roles = Operator.Current.Roles;
            foreach (var e in roles)
            {
                var roleId = e;
                if (string.Equals(roleId, null, StringComparison.Ordinal)) continue;
                var menuCache = cache.GetCache<string>("menu_" + roleId);
                if (!string.IsNullOrEmpty(menuCache) && menuCache != "[]")
                {
                    data.authorizeMenu = cache.GetCache<string>("menu_" + roleId);
                }
                else
                {
                    data.authorizeMenu = SysCacheAppService.GetMenuList().ToString();
                    cache.WriteCache(data.authorizeMenu, "menu_" + roleId);
                }
                if (!cache.GetCache<Dictionary<string, object>>("button_" + roleId).IsEmpty())
                {
                    data.authorizeButton = cache.GetCache<Dictionary<string, object>>("button_" + roleId);
                }
                else
                {
                    data.authorizeButton = (Dictionary<string, object>)SysCacheAppService.GetMenuButtonList();
                    cache.WriteCache(data.authorizeButton, "button_" + roleId);
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult UserInfo()
        {
            var current = Operator.Current;
            return Json(new
            {
                current?.UserCode,
                current?.UserName,
                current?.HeadIcon
            }, JsonRequestBehavior.AllowGet);
        }


        private class data
        {
            public Dictionary<string, object> area;
            //public List< AreaChild> areachild;
            public Dictionary<string, object> authorizeButton;
            public string authorizeMenu;
            public Dictionary<string, string> classTeachers;
            public Dictionary<string, object> course;
            public Dictionary<string, object> dataItems;
            public Dictionary<string, string> devices;
            public Dictionary<string, object> duty;
            public Dictionary<string, object> organize;
            public Dictionary<string, object> role;
            public Dictionary<string, object> semester;
            public Dictionary<string, Dictionary<string, string>> schedulestime;
        }
    }
}