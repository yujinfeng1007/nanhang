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
        public static DbContext Db => new ZhxyDbContext();
        
        /// <summary>
        /// 岗位缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetDutyList()
        {

            //var dutyApp = new DutyService(new ZhxyDbContext());
            var data = new SysDicItemAppService(new ZhxyDbContext()).GetItemList("Duty");
            var dictionary = new Dictionary<string, object>();

            foreach (var item in data)
            {
                var fieldItem = new
                {
                    encode = item.F_ItemCode,
                    fullname = item.F_ItemName
                };
                dictionary.Add(item.F_ItemCode, fieldItem);
            }
            return dictionary;
        }

        public static Dictionary<string, object> GetDutyListByCache()
        {
            if (RedisCache.Get<Dictionary<string, object>>(SysConsts.DUTY).IsEmpty())
            {
                RedisCache.Set(SysConsts.DUTY,(Dictionary<string, object>)GetDutyList());
            }

            return RedisCache.Get<Dictionary<string, object>>(SysConsts.DUTY);
        }



        /// <summary>
        /// 角色缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetRoleList()
        {
            var roleApp = new SysRoleAppService(new ZhxyDbContext());
            var data = roleApp.GetList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new  { encode = item.F_EnCode, fullname = item.F_FullName };
                dictionary.Add(item.F_Id, fieldItem);
            }
            return dictionary;
        }

        public static Dictionary<string, object> GetRoleListByCache()
        {
            if (RedisCache.Get<Dictionary<string, object>>(SysConsts.ROLE).IsEmpty())
            {
                RedisCache.Set(SysConsts.ROLE, (Dictionary<string, object>)GetRoleList());
            }

            return RedisCache.Get<Dictionary<string, object>>(SysConsts.ROLE);
        }


        /// <summary>
        /// 字典缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetDataItemList()
        {
            var itemDetails = new SysDicItemAppService(new ZhxyDbContext()).GetList();
            var dic = new Dictionary<string, object>();
            foreach (var item in new SysDicAppService(new ZhxyDbContext()).GetList())
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
            if (RedisCache.Get<Dictionary<string, object>>(SysConsts.DATAITEMS).IsEmpty())
            {
                RedisCache.Set(SysConsts.DATAITEMS, (Dictionary<string, object>)GetDataItemList());
            }

            return RedisCache.Get<Dictionary<string, object>>(SysConsts.DATAITEMS);
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
                var fieldItem = new { encode = item.Code, fullname = item.Name };
                dictionary.Add(item.Id, fieldItem);
            }
            return dictionary;
        }

        public static Dictionary<string, object> GetOrganizeListByCache()
        {
            RedisCache.Remove(SysConsts.ORGANIZE);
            RedisCache.Set(SysConsts.ORGANIZE, GetOrganizeList());
            return RedisCache.Get<Dictionary<string, object>>(SysConsts.ORGANIZE);
        }


        public static object GetMenuListByType(string clientType)
        {
            var app = new SysRoleAuthorizeAppService(new ZhxyDbContext());
            if (Operator.GetCurrent().IsSystem)
            {
                return ToMenuJson(app.GetMenuList("0", clientType), "0");
            }
            var roles = Operator.GetCurrent().Roles;
            var data = new List<SysModule>();
            foreach (var e in roles)
            {
                data = data.Union(app.GetMenuList(e, clientType)).ToList();
            }
            return ToMenuJson(data, "0");
        }
        /// <summary>
        /// 菜单缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetMenuList(string clientType)
        {
            clientType = string.IsNullOrEmpty(clientType) ? "2" : clientType;
            return GetMenuListByType(clientType);
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

        // 菜单按钮
        public static object GetMenuButtonList()
        {
            //var roleId = OperatorProvider.Current.RoleId;
            var roles = Operator.GetCurrent().Roles;
            var app = new SysRoleAuthorizeAppService(new ZhxyDbContext());
            var data = new List<SysButton>();
            foreach (var e in roles)
            {
                data = data.Union(app.GetButtonList(e)).ToList();
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
    }
}