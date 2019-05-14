using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public class SysCacheAppService : AppService
    {
        public SysCacheAppService(IZhxyRepository r) : base(r)
        {
        }


     
     

        public static object GetAreaListChild()
        {
            var areaApp = new SysPlaceAreaAppService();
            var data = areaApp.GetList();
            var list = new List<AreaChild>();
            foreach (var itemprovince in data.Where(a => a.F_ParentId == "0"))
            {
                var areaprovince = new AreaChild { value = itemprovince.F_Id, label = itemprovince.F_FullName };
                var listprovince = new List<AreaChild>();
                foreach (var itemcity in data.Where(b => b.F_ParentId == itemprovince.F_Id))
                {
                    var areacity = new AreaChild { value = itemcity.F_Id, label = itemcity.F_FullName };
                    var listcity = new List<AreaChild>();
                    foreach (var itemArea in data.Where(c => c.F_ParentId == itemcity.F_Id))
                    {
                        var areaArea = new AreaChild { value = itemArea.F_Id, label = itemArea.F_FullName };
                        listcity.Add(areaArea);
                    }
                    areacity.children = listcity;
                    listprovince.Add(areacity);
                }
                areaprovince.children = listprovince;
                list.Add(areaprovince);
            }
            return list;
        }

        public static List<AreaChild> GetAreaListChildByCache()
        {
            var cache = CacheFactory.Cache();
            if (CacheFactory.Cache().GetCache<List<AreaChild>>(SYS_CONSTS.AREACHILD).IsEmpty())
            {
                cache.WriteCache((List<AreaChild>)GetAreaListChild(), SYS_CONSTS.AREACHILD);
            }

            return cache.GetCache<List<AreaChild>>(SYS_CONSTS.AREACHILD);
        }

        /// <summary>
        /// 字典缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetDataItemList()
        {
            var itemDetails = new SysDicItemAppService().GetList();
            var dic = new Dictionary<string, object>();
            foreach (var item in new SysDicAppService() .GetAll())
            {
                var tempDictionary = new Dictionary<string, string>();
                var details = itemDetails.FindAll(t => t.F_ItemId.Equals(item.F_Id));
                foreach (var i in details)
                {
                    try
                    {
                        tempDictionary.Add(i.F_ItemCode, i.F_ItemName);
                    }
                    catch
                    {
                        // ignored
                    }
                }
                dic.Add(item.F_EnCode, tempDictionary);
            }
            return dic;
        }

        public static Dictionary<string, object> GetDataItemListByCache()
        {
            var cache = CacheFactory.Cache();
            if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SYS_CONSTS.DATAITEMS).IsEmpty())
            {
                cache.WriteCache((Dictionary<string, object>)GetDataItemList(), SYS_CONSTS.DATAITEMS);
            }

            return cache.GetCache<Dictionary<string, object>>(SYS_CONSTS.DATAITEMS);
        }

       


        public static object GetMenuButtonList()
        {
            //var roleId = OperatorProvider.Current.RoleId;
            var roles = OperatorProvider.Current.Roles;
            var app = new SysRoleAuthorizeAppService();
            var data = new List<SysButton>();
            foreach (var e in roles)
            {
                data = data.Union(app.GetButtonList(e.Key), new ModuleButtonComparer()).ToList();
            }

            var dataModuleId = data.Distinct(new ExtList<SysButton>("F_ModuleId"));
            var dictionary = new Dictionary<string, object>();
            foreach (var item in dataModuleId)
            {
                var buttonList = data.Where(t => t.F_ModuleId.Equals(item.F_ModuleId)).ToList();
                dictionary.Add(item.F_ModuleId, buttonList);
            }
            return dictionary;
        }

        /// <summary>
        /// 菜单缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetMenuList()
        {
            //var roleId = OperatorProvider.Current.RoleId;
            var app = new SysRoleAuthorizeAppService();
            if (OperatorProvider.Current.IsSystem)
            {
                return ToMenuJson(app.GetEnableMenuList("0"), "0");
            }
            var roles = OperatorProvider.Current.Roles;
            var data = new List<SysModule>();
            foreach (var e in roles)
            {
                data = data.Union(app.GetEnableMenuList(e.Key), new ModuleComparer()).ToList();
            }
            return ToMenuJson(data, "0");
        }
       

        public static string ToMenuJson(List<SysModule> data, string parentId)
        {
            var sbJson = new StringBuilder();
            sbJson.Append("[");
            var entitys = data.FindAll(t => t.F_ParentId == parentId);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    var strJson = item.ToJson();
                    strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.F_Id) + string.Empty);
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }

    }
}