
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
    public class CacheService 
    {
        private static DbContext Db => new ZhxyDbContext();

        /// <summary>
        /// 地区缓存
        /// </summary>
        /// <returns></returns>
        public static object GetAreaList()
        {
            var data = Db.Set<Area>().ToListAsync().Result;
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new 
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

       

    }
}