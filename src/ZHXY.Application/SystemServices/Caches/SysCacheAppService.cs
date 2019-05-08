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
        public static DbContext MyDb => new ZhxyDbContext();

        /// <summary>
        /// 地区缓存
        /// </summary>
        /// <returns></returns>
        public static object GetAreaList()
        {
            var areaApp = new SysPlaceAreaAppService();
            var data = areaApp.GetList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new FieldItem
                {
                    encode = item.F_EnCode,
                    fullname = item.F_FullName,
                    parentid = item.F_ParentId,
                    level = item.F_Layers
                };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        public static Dictionary<string, object> GetAreaListByCache()
        {
            var cache = CacheFactory.Cache();
            if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.AREA).IsEmpty())
            {
                cache.WriteCache((Dictionary<string, object>)GetAreaList(), SmartCampusConsts.AREA);
            }
            return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.AREA);
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
            if (CacheFactory.Cache().GetCache<List<AreaChild>>(SmartCampusConsts.AREACHILD).IsEmpty())
            {
                cache.WriteCache((List<AreaChild>)GetAreaListChild(), SmartCampusConsts.AREACHILD);
            }

            return cache.GetCache<List<AreaChild>>(SmartCampusConsts.AREACHILD);
        }

        /// <summary>
        /// 字典缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetDataItemList()
        {
            var itemDetails = new SysDicItemAppService().GetList();
            var dic = new Dictionary<string, object>();
            foreach (var item in new SysDicAppService().GetList())
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
            if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.DATAITEMS).IsEmpty())
            {
                cache.WriteCache((Dictionary<string, object>)GetDataItemList(), SmartCampusConsts.DATAITEMS);
            }

            return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.DATAITEMS);
        }

        /// <summary>
        /// 岗位缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetDutyList()
        {
            var dutyApp = new SysDutyAppService();
            var data = dutyApp.GetList();
            var dictionary = new Dictionary<string, object>();

            foreach (var item in data)
            {
                var fieldItem = new FieldItem();
                fieldItem.encode = item.F_EnCode;
                fieldItem.fullname = item.F_FullName;
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        public static Dictionary<string, object> GetDutyListByCache()
        {
            var cache = CacheFactory.Cache();
            if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.DUTY).IsEmpty())
            {
                cache.WriteCache((Dictionary<string, object>)GetDutyList(), SmartCampusConsts.DUTY);
            }

            return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.DUTY);
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

        /// <summary>
        /// 机构缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetOrganizeList()
        {
            var organizeApp = new SysOrganizeAppService();
            var data = organizeApp.GetList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new FieldItem { encode = item.F_EnCode, fullname = item.F_FullName };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        public static Dictionary<string, object> GetOrganizeListByCache()
        {
            var cache = CacheFactory.Cache();
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.ORGANIZE);
            CacheFactory.Cache().WriteCache(GetOrganizeList(), SmartCampusConsts.ORGANIZE);
            return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.ORGANIZE);
        }

        /// <summary>
        /// 角色缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetRoleList()
        {
            var roleApp = new SysRoleAppService();
            var data = roleApp.GetList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new FieldItem { encode = item.F_EnCode, fullname = item.F_FullName };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        public static Dictionary<string, object> GetRoleListByCache()
        {
            var cache = CacheFactory.Cache();
            if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.ROLE).IsEmpty())
            {
                cache.WriteCache((Dictionary<string, object>)GetRoleList(), SmartCampusConsts.ROLE);
            }

            return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.ROLE);
        }

    
        /// <summary>
        /// 用户缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetUserList()
        {
            var dictionary = new Dictionary<string, object>();
            new SysUserAppService().GetAll().ForEach(item =>
            {
                var fieldItem = new FieldItem { fullname = item.F_RealName };
                dictionary.Add(item.F_Id, fieldItem);
            });
            return dictionary;

        }

        public static Dictionary<string, object> GetUserListByCache()
        {
            var cache = CacheFactory.Cache();
            if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.USERS).IsEmpty())
            {
                cache.WriteCache((Dictionary<string, object>)GetUserList(), SmartCampusConsts.USERS);
            }

            return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.USERS);
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

        public static void ClearOrgCache() => CacheFactory.Cache().RemoveCache(SmartCampusConsts.ORGANIZE);
    }
}