
using System;
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
    public class CacheService : AppService
    {
        public static DbContext MyDb => new ZhxyDbContext();

        /// <summary>
        /// 地区缓存
        /// </summary>
        /// <returns></returns>
        public static object GetAreaList()
        {
            var areaApp = new PlaceAreaService();
            var data = areaApp.GetList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new FieldItem
                {
                    encode = item.Code,
                    fullname = item.Name,
                    parentid = item.ParentId,
                    level = item.Level
                };
                dictionary.Add(item.Id, fieldItem);
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
            var areaApp = new PlaceAreaService();
            var data = areaApp.GetList();
            var list = new List<AreaChild>();
            foreach (var itemprovince in data.Where(a => a.ParentId == "0"))
            {
                var areaprovince = new AreaChild { value = itemprovince.Id, label = itemprovince.Name };
                var listprovince = new List<AreaChild>();
                foreach (var itemcity in data.Where(b => b.ParentId == itemprovince.Id))
                {
                    var areacity = new AreaChild { value = itemcity.Id, label = itemcity.Name };
                    var listcity = new List<AreaChild>();
                    foreach (var itemArea in data.Where(c => c.ParentId == itemcity.Id))
                    {
                        var areaArea = new AreaChild { value = itemArea.Id, label = itemArea.Name };
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
            var service = new DicService(new ZhxyRepository());
            var allitems = service. GetAllItems();
            var dic = new Dictionary<string, object>();
            foreach (var item in service.GetAll())
            {
                var tempDictionary = new Dictionary<string, string>();
                var details = allitems.Where(t=>t.DicId==item.Id);
                foreach (var i in details)
                {
                    try
                    {
                        tempDictionary.Add(i.Key, i.Value);
                    }
                    catch(Exception e)
                    {
                        // ignored
                    }
                }
                dic.Add(item.Id, tempDictionary);
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
            var dutyApp = new DutyService(new ZhxyRepository());
            var data = dutyApp.GetList();
            var dictionary = new Dictionary<string, object>();

            foreach (var item in data)
            {
                var fieldItem = new FieldItem();
                fieldItem.encode = item.Code;
                fieldItem.fullname = item.Name;
                dictionary.Add(item.Id, fieldItem);
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

        /// <summary>
        /// 机构缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetOrganizeList()
        {
            var organizeApp = new OrgService();
            var data = organizeApp.GetList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new FieldItem { encode = item.EnCode, fullname = item.Name };
                dictionary.Add(item.Id, fieldItem);
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
            var roleApp = new RoleService(new ZhxyRepository());
            var data = roleApp.GetList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new FieldItem { encode = item.Code, fullname = item.Name };
                dictionary.Add(item.Id, fieldItem);
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


        //public static object GetMenuButtonList()
        //{
        //    var roles = Operator.Current.Roles;
        //    var app = new RoleAuthorizeService(new ZhxyRepository());
        //    var data = new List<Button>();
        //    foreach (var e in roles)
        //    {
        //        data = data.Union(app.GetButtonList(e), new ModuleButtonComparer()).ToList();
        //    }

        //    var dataModuleId = data.Select(t => t.MenuId).Distinct();
        //    var dictionary = new Dictionary<string, object>();
        //    foreach (var item in dataModuleId)
        //    {
        //        var buttonList = data.Where(t => t.MenuId.Equals(item)).ToList();
        //        dictionary.Add(item, buttonList);
        //    }
        //    return dictionary;
        //}

        /// <summary>
        /// 菜单缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetMenuList()
        {
            var app = new RoleAuthorizeService(new ZhxyRepository());
            if (Operator.Current.IsSystem)
            {
                return ToMenuJson(app.GetEnableMenuList("0"), SYS_CONSTS.DbNull);
            }
            var roles = Operator.Current.Roles;
            var data = new List<Menu>();
            foreach (var e in roles)
            {
                data = data.Union(app.GetEnableMenuList(e), new ModuleComparer()).ToList();
            }
            return ToMenuJson(data, SYS_CONSTS.DbNull);
        }

        public static string ToMenuJson(List<Menu> data, string parentId)
        {
            var sbJson = new StringBuilder();
            sbJson.Append("[");
            var entitys = data.FindAll(t => t.ParentId.Equals( parentId)).ToList().Select(p => new
            {
                F_Id = p.Id,
                F_ParentId = p.ParentId,
                F_FullName = p.Name,
                F_Icon = p.Icon,
                F_Ico = p.IconForWeb,
                F_UrlAddress = p.Url,
                F_Target = p.Target,
                F_IsMenu = p.IsMenu,
                F_IsExpand = p.IsExpand,
                F_IsPublic = p.IsPublic,
                F_SortCode = p.SortCode,
                F_BelongSys = p.BelongSys
            });

            if (entitys.Count() > 0)
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