
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
            foreach (var item in new DicService().GetAll())
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

        public static object GetMenuButtonList()
        {
            var roles = Operator.Current.Roles;
            var app = new RoleAuthorizeService(new ZhxyRepository());
            var data = new List<Button>();
            foreach (var e in roles)
            {
                data = data.Union(app.GetButtonList(e), new ModuleButtonComparer()).ToList();
            }

            var dataModuleId = data.Distinct(new ExtList<Button>("F_ModuleId"));
            var dictionary = new Dictionary<string, object>();
            foreach (var item in dataModuleId)
            {
                var buttonList = data.Where(t => t.MenuId.Equals(item.MenuId)).ToList();
                dictionary.Add(item.MenuId, buttonList);
            }
            return dictionary;
        }

        /// <summary>
        /// 菜单缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetMenuList()
        {
            var app = new RoleAuthorizeService(new ZhxyRepository());
            if (Operator.Current.IsSystem)
            {
                return ToMenuJson(app.GetEnableMenuList("0"), "0");
            }
            var roles = Operator.Current.Roles;
            var data = new List<Menu>();
            foreach (var e in roles)
            {
                data = data.Union(app.GetEnableMenuList(e), new ModuleComparer()).ToList();
            }
            return ToMenuJson(data, "0");
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

        //public static Dictionary<string, object> GetSchoolCourseByCache()
        //{
        //    var cache = CacheFactory.Cache();
        //    if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.COURSE).IsEmpty())
        //    {
        //        cache.WriteCache((Dictionary<string, object>)GetSchoolCourseList(), SmartCampusConsts.COURSE);
        //    }

        //    return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.COURSE);
        //}

        //public static object GetSchoolCourseList()
        //{
        //    var courApp = new SchCourseAppService();

        //    var data = courApp.GetList();
        //    var dictionary = new Dictionary<string, object>();
        //    foreach (var item in data)
        //    {
        //        dictionary.Add(item.F_Id, item);
        //    }
        //    return dictionary;
        //}

        //public static Dictionary<string, string> GetSchoolDevicesByCache()
        //{
        //    var cache = CacheFactory.Cache();
        //    CacheFactory.Cache().RemoveCache(SmartCampusConsts.DEVICES);
        //    cache.WriteCache((Dictionary<string, string>)GetSchoolDevicesList(), SmartCampusConsts.DEVICES);
        //    return cache.GetCache<Dictionary<string, string>>(SmartCampusConsts.DEVICES);
        //}

        //public static object GetSchoolDevicesList()
        //{
        //    var data = new ElectronicBoardAppService().Query<ElectronicBoard>().Select(p => new { p.F_Sn, p.F_Class }).ToListAsync().Result;
        //    var dictionary = new Dictionary<string, string>();
        //    foreach (var item in data)
        //    {
        //        dictionary.Add(item.F_Sn, item.F_Class);
        //    }
        //    return dictionary;
        //}

        ///// <summary>
        ///// 学期缓存
        ///// </summary>
        ///// <returns>  </returns>
        //public static object GetSemesterList()
        //{
        //    var app = new SchSemesterAppService();
        //    var data = app.GetList();
        //    var dictionary = new Dictionary<string, object>();
        //    foreach (var item in data)
        //    {
        //        var o = new
        //        {
        //            fullname = item.F_Name,
        //            startTime = item.F_Start_Time,
        //            endTime = item.F_End_Time,
        //            nickname = item.F_Nickname
        //        };
        //        dictionary.Add(item.F_Id, o);
        //    }
        //    return dictionary;
        //}

        //public static Dictionary<string, object> GetSemesterListByCache()
        //{
        //    var cache = CacheFactory.Cache();
        //    if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.SEMESTER).IsEmpty())
        //    {
        //        cache.WriteCache((Dictionary<string, object>)GetSemesterList(), SmartCampusConsts.SEMESTER);
        //    }

        //    return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.SEMESTER);
        //}

        ///// <summary>
        ///// 用户缓存
        ///// </summary>
        ///// <returns>  </returns>
        //public static object GetUserList()
        //{
        //    var dictionary = new Dictionary<string, object>();
        //    new SysUserAppService().GetAll().ForEach(item =>
        //    {
        //        var fieldItem = new FieldItem { fullname = item.F_RealName };
        //        dictionary.Add(item.F_Id, fieldItem);
        //    });
        //    return dictionary;

        //}

        //public static Dictionary<string, object> GetUserListByCache()
        //{
        //    var cache = CacheFactory.Cache();
        //    if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.USERS).IsEmpty())
        //    {
        //        cache.WriteCache((Dictionary<string, object>)GetUserList(), SmartCampusConsts.USERS);
        //    }

        //    return cache.GetCache<Dictionary<string, object>>(SmartCampusConsts.USERS);
        //}

        public static string ToMenuJson(List<Menu> data, string parentId)
        {
            var sbJson = new StringBuilder();
            sbJson.Append("[");
            var entitys = data.FindAll(t => t.ParentId == parentId);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    var strJson = item.ToJson();
                    strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.Id) + string.Empty);
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }

        ///// <summary>
        ///// 班级老师缓存
        ///// </summary>
        //public static object GetClassTeachers()
        //{
        //    var query = MyDb.Set<Teacher>().AsNoTracking();
        //    var dic = query.Select(p => new { p.Id, p.Name }).ToDictionary(p => p.F_Id, p => p.F_Name);
        //    var teacherIds = query.Select(p => p.Id).ToArray();
        //    var classTeachers = MyDb.Set< ClassTeacher>().AsNoTracking().Where(p => teacherIds.Contains(p.F_Leader_Tea) && teacherIds.Contains(p.F_Leader_Tea2) && teacherIds.Contains(p.F_Teacher)).ToList();
        //    var data = classTeachers.GroupBy(p => new { p.F_ClassID, p.F_Leader_Tea, p.F_Leader_Tea2 }).Select(g => new
        //    {
        //        classId = g.Key.F_ClassID,
        //        leader_tea = dic[g.Key.F_Leader_Tea],
        //        leader_tea2 = dic[g.Key.F_Leader_Tea2],
        //        teachers = string.Join(",", g.Select(p => dic[p.F_Teacher]).ToArray())
        //    }).ToDictionary(p => p.classId, p => (new { p.leader_tea, p.leader_tea2, p.teachers }).ToJson());
        //    return data;
        //}

        //public static Dictionary<string, string> GetClassTeachersByCache()
        //{
        //    var cache = CacheFactory.Cache();
        //    if (CacheFactory.Cache().GetCache<Dictionary<string, string>>(SmartCampusConsts.CLASSTEACHERS).IsEmpty())
        //    {
        //        cache.WriteCache(GetClassTeachers(), SmartCampusConsts.CLASSTEACHERS);
        //    }
        //    return cache.GetCache<Dictionary<string, string>>(SmartCampusConsts.CLASSTEACHERS);
        //}

        //public static string[] GetOnlineDevices() => new ElectronicBoardAppService().Query<ElectronicBoard>(p => p.F_Device_Status.Equals("1")).Select(p => p.F_Sn).ToArray();

        //public static object GetOnlineDevicesByCache()
        //{
        //    var devs = GetOnlineDevices();
        //    var client = RedisHelper.GetDatabase();
        //    client.KeyDelete(SmartCampusConsts.DEVICES);
        //    foreach (var item in devs)
        //    {
        //        client.ListRightPush(SmartCampusConsts.DEVICES, item);
        //    }
        //    return client.ListRange(SmartCampusConsts.DEVICES);
        //}

        ///// <summary>
        /////课表缓存
        ///// </summary>
        //public static Dictionary<string, Dictionary<string, string>> GetSchedulesTimeByCache()
        //{
        //    var cache = CacheFactory.Cache();
        //    if (CacheFactory.Cache().GetCache<Dictionary<string, Dictionary<string, string>>>(SmartCampusConsts.SCHEDULESTIME).IsEmpty())
        //    {
        //        cache.WriteCache((Dictionary<string, Dictionary<string, string>>)GetSchedulesTimeList(), SmartCampusConsts.SCHEDULESTIME);
        //    }
        //    return cache.GetCache<Dictionary<string, Dictionary<string, string>>>(SmartCampusConsts.SCHEDULESTIME);
        //}

        ///// <summary>
        /////课表缓存
        ///// </summary>
        //public static object GetSchedulesTimeList()
        //{
        //    var schtimeApp = new SchScheduleTimeManageApp();
        //    var data = schtimeApp.GetList();
        //    var dictionary = new Dictionary<string, Dictionary<string, string>>();
        //    foreach (var item in data)
        //    {
        //        var dic = new Dictionary<string, string>();

        //        var key = item.F_Grande + item.F_Semester;

        //        #region 时间段

        //        var map1 = item.F_Course_StartTime1.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime1.ToDate().ToString("HH:mm");
        //        var map2 = item.F_Course_StartTime2.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime2.ToDate().ToString("HH:mm");
        //        var map3 = item.F_Course_StartTime3.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime3.ToDate().ToString("HH:mm");
        //        var map4 = item.F_Course_StartTime4.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime4.ToDate().ToString("HH:mm");
        //        var map5 = item.F_Course_StartTime5.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime5.ToDate().ToString("HH:mm");
        //        var map6 = item.F_Course_StartTime6.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime6.ToDate().ToString("HH:mm");
        //        var map7 = item.F_Course_StartTime7.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime7.ToDate().ToString("HH:mm");
        //        var map8 = item.F_Course_StartTime8.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime8.ToDate().ToString("HH:mm");
        //        var map9 = item.F_Course_StartTime9.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime9.ToDate().ToString("HH:mm");
        //        var map10 = item.F_Course_StartTime10.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime10.ToDate().ToString("HH:mm");
        //        var map11 = item.F_Course_StartTime11.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime11.ToDate().ToString("HH:mm");
        //        var map12 = item.F_Course_StartTime12.ToDate().ToString("HH:mm") + "-" + item.F_Course_EndTime12.ToDate().ToString("HH:mm");

        //        dic.Add("1", map1);
        //        dic.Add("2", map2);
        //        dic.Add("3", map3);
        //        dic.Add("4", map4);
        //        dic.Add("5", map5);
        //        dic.Add("6", map6);
        //        dic.Add("7", map7);
        //        dic.Add("8", map8);
        //        dic.Add("9", map9);
        //        dic.Add("10", map10);
        //        dic.Add("11", map11);
        //        dic.Add("12", map12);

        //        #endregion 时间段

        //        dictionary.Add(key, dic);
        //    }
        //    return dictionary;
        //}

        public static void ClearOrgCache() => CacheFactory.Cache().RemoveCache(SmartCampusConsts.ORGANIZE);
    }
}