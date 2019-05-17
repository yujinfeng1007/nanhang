
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
            var areaApp = new SysPlaceAreaAppService();
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
                fieldItem.encode = item.EnCode;
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
                var fieldItem = new FieldItem { encode = item.EnCode, fullname = item.Name };
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

       
        public static string ToMenuJson(List<Menu> data, string parentId)
        {
            var sbJson = new StringBuilder();
            sbJson.Append("[");
            var entitys = data.FindAll(t => t.ParentId == parentId).Select(p=>new
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
                F_BelongSys=p.BelongSys
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