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
        public static DbContext Db => new ZhxyDbContext();

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


        /// <summary>
        /// 字典缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetDataItemList()
        {
            var service = new DicService(new ZhxyRepository());
            var allitems = service.GetAllItems();
            var dic = new Dictionary<string, object>();
            foreach (var item in service.GetAll())
            {
                var tempDictionary = new Dictionary<string, string>();
                var details = allitems.Where(t => t.Code == item.Code);
                foreach (var i in details)
                {
                    try
                    {
                        tempDictionary.Add(i.Key, i.Value);
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
                dic.Add(item.Code, tempDictionary);
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
        /// 菜单缓存
        /// </summary>
        /// <returns>  </returns>
        public static object GetMenuList()
        {
            var app = new RoleAuthorizeService(new ZhxyRepository());
            if (Operator.GetCurrent().IsSystem)
            {
                return ToMenuJson(app.GetEnableMenuList("0"), SYS_CONSTS.DbNull);
            }
            if (Operator.GetCurrent().DutyId == "studentDuty")
            {
                return "[{"
    + "\"F_Id\": \"602119BB4BA04378B405395876F73B5A\","
    + "\"F_ParentId\": \"0\","
    + "\"F_EnCode\": null,"
    + "\"F_FullName\": \"常用\","
    + "\"F_Icon\": \"fa fa-chain\","
    + "\"F_Ico\": \"fa fa-chain\","
    + "\"F_UrlAddress\": \"Group\","
    + "\"F_Target\": \"blank\","
    + "\"F_IsMenu\": null,"
    + "\"F_IsExpand\": null,"
    + "\"F_IsPublic\": null,"
    + "\"F_SortCode\": 29,"
    + "\"F_BelongSys\": \"2\","
    + "\"ChildNodes\": [{"
    + "	\"F_Id\": \"804B991694324AC5BCCC0CED9AA72338\","
    + "	\"F_ParentId\": \"602119BB4BA04378B405395876F73B5A\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"请假V2\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"LeaveV2Form\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 101,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
    + "}, {"
    + "	\"F_Id\": \"D4ECF1448C6F405CAB1E711882409B1B\","
    + "	\"F_ParentId\": \"602119BB4BA04378B405395876F73B5A\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"校外访客登记\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"VisitorOutSchool\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 107,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
+ "}, {"
    + "	\"F_Id\": \"2D148E0F775B4B02BCEB1EE460545F5F\","
    + "	\"F_ParentId\": \"602119BB4BA04378B405395876F73B5A\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"校内互访\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"VisitorInfoInSchool\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 107,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
    + "}, {"
    + "	\"F_Id\": \"079CDBC55B2F4C7AA7DA7B924936B897\","
    + "	\"F_ParentId\": \"602119BB4BA04378B405395876F73B5A\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"访客授权\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"VisitorApproval\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 110,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
    + "}, {"
    + "	\"F_Id\": \"5D262277916E419FB175C3F9E06F0E95\","
    + "	\"F_ParentId\": \"602119BB4BA04378B405395876F73B5A\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"头像采集更新\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"SetPhotoInfo\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 123,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
    + "}]"
+ "}, {"
    + "\"F_Id\": \"12E9AD040D904E6C814B91C7888CFCE6\","
    + "\"F_ParentId\": \"0\","
    + "\"F_EnCode\": null,"
    + "\"F_FullName\": \"记录查询\","
    + "\"F_Icon\": \"fa fa-chain\","
    + "\"F_Ico\": \"fa fa-chain\","
    + "\"F_UrlAddress\": \"Group\","
    + "\"F_Target\": \"expand\","
    + "\"F_IsMenu\": true,"
    + "\"F_IsExpand\": false,"
    + "\"F_IsPublic\": false,"
    + "\"F_SortCode\": 30,"
    + "\"F_BelongSys\": \"2\","
    + "\"ChildNodes\": [{"
    + "	\"F_Id\": \"205D591EAD0C40A6A5D8F080512F01CD\","
    + "	\"F_ParentId\": \"12E9AD040D904E6C814B91C7888CFCE6\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"出入\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"InOut\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 1,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
    + "}, {"
    + "	\"F_Id\": \"50FE2F54203049AE888B2537E1273AFA\","
    + "	\"F_ParentId\": \"12E9AD040D904E6C814B91C7888CFCE6\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"晚归\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"LateIn\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 2,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
    + "}, {"
    + "	\"F_Id\": \"71DE61D4F046435588E2ED12A86C91E5\","
    + "	\"F_ParentId\": \"12E9AD040D904E6C814B91C7888CFCE6\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"未归\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"NotIn\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 3,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
    + "}, {"
    + "	\"F_Id\": \"6EC72287750E4AC596469088491C7E62\","
    + "	\"F_ParentId\": \"12E9AD040D904E6C814B91C7888CFCE6\","
    + "	\"F_EnCode\": null,"
    + "	\"F_FullName\": \"未出\","
    + "	\"F_Icon\": \"fa fa-chain\","
    + "	\"F_Ico\": \"fa fa-chain\","
    + "	\"F_UrlAddress\": \"NotOut\","
    + "	\"F_Target\": \"blank\","
    + "	\"F_IsMenu\": false,"
    + "	\"F_IsExpand\": false,"
    + "	\"F_IsPublic\": false,"
    + "	\"F_SortCode\": 4,"
    + "	\"F_BelongSys\": \"2\","
    + "	\"ChildNodes\": []"
    + "}]"
+ "}]";
            }
            if (Operator.GetCurrent().DutyId == "teacherDuty")
            {
                return "[{\"F_Id\":\"348b3964-7841-4af7-8fe1-7f91ae64e3bd\",\"F_ParentId\":\"0\",\"F_EnCode\":null,\"F_FullName\":\"学生管理\",\"F_Icon\":\"fafa-user\",\"F_Ico\":\"学生管理.png\",\"F_UrlAddress\":null,\"F_Target\":\"expand\",\"F_IsMenu\":null,\"F_IsExpand\":null,\"F_IsPublic\":null,\"F_SortCode\":3,\"F_BelongSys\":\"1\",\"ChildNodes\":[{\"F_Id\":\"D5092F0A87774E67BCA6B64689AFA160\",\"F_ParentId\":\"348b3964-7841-4af7-8fe1-7f91ae64e3bd\",\"F_EnCode\":null,\"F_FullName\":\"学生请假审核\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"/Dorm/Leave\",\"F_Target\":\"iframe\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":99,\"F_BelongSys\":\"1\",\"ChildNodes\":[]},{\"F_Id\":\"5871DE850C58400788C478C0CAEC2FCF\",\"F_ParentId\":\"348b3964-7841-4af7-8fe1-7f91ae64e3bd\",\"F_EnCode\":null,\"F_FullName\":\"不计考勤请假\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"/Dorm/SpecialLeave/Index\",\"F_Target\":\"iframe\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":898,\"F_BelongSys\":\"1\",\"ChildNodes\":[]},{\"F_Id\":\"82E90102D74249E38F53EC318C5D7461\",\"F_ParentId\":\"348b3964-7841-4af7-8fe1-7f91ae64e3bd\",\"F_EnCode\":\"\",\"F_FullName\":\"销假管理\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"/Dorm/Leave/Cancel\",\"F_Target\":\"iframe\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":999,\"F_BelongSys\":\"1\",\"ChildNodes\":[]}]},{\"F_Id\":\"255DE88C1FB543AD9D6131CE4DDADC7F\",\"F_ParentId\":\"0\",\"F_EnCode\":null,\"F_FullName\":\"宿舍安全管理\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":null,\"F_Target\":\"expand\",\"F_IsMenu\":true,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":11,\"F_BelongSys\":\"1\",\"ChildNodes\":[{\"F_Id\":\"3B9AA23BC32A4426A40BA65A240FDB72\",\"F_ParentId\":\"255DE88C1FB543AD9D6131CE4DDADC7F\",\"F_EnCode\":null,\"F_FullName\":\"报表统计分析\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":null,\"F_Target\":\"expand\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":2,\"F_BelongSys\":\"1\",\"ChildNodes\":[{\"F_Id\":\"CBA4CFFF31054FA881ED9D2D0B45742B\",\"F_ParentId\":\"3B9AA23BC32A4426A40BA65A240FDB72\",\"F_EnCode\":null,\"F_FullName\":\"未归报表\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"/Dorm/Report/NoReturn\",\"F_Target\":\"iframe\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":0,\"F_BelongSys\":\"1\",\"ChildNodes\":[]},{\"F_Id\":\"B62B8DE640A14ED8ABB16C9FFFBDB441\",\"F_ParentId\":\"3B9AA23BC32A4426A40BA65A240FDB72\",\"F_EnCode\":null,\"F_FullName\":\"原始报表\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"/Dorm/Report/Original\",\"F_Target\":\"iframe\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":0,\"F_BelongSys\":\"1\",\"ChildNodes\":[]},{\"F_Id\":\"DD4EDE4C84224F4AB9508847047435B0\",\"F_ParentId\":\"3B9AA23BC32A4426A40BA65A240FDB72\",\"F_EnCode\":null,\"F_FullName\":\"晚归报表\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"/Dorm/Report/LateReturn\",\"F_Target\":\"iframe\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":1,\"F_BelongSys\":\"1\",\"ChildNodes\":[]},{\"F_Id\":\"67B2AE40088B4440ADC26662B43EB6BD\",\"F_ParentId\":\"3B9AA23BC32A4426A40BA65A240FDB72\",\"F_EnCode\":null,\"F_FullName\":\"长时间未出报表\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"/Dorm/Report/NoOut\",\"F_Target\":\"iframe\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":3,\"F_BelongSys\":\"1\",\"ChildNodes\":[]}]}]},{\"F_Id\":\"602119BB4BA04378B405395876F73B5A\",\"F_ParentId\":\"0\",\"F_EnCode\":null,\"F_FullName\":\"常用\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"Group\",\"F_Target\":\"blank\",\"F_IsMenu\":null,\"F_IsExpand\":null,\"F_IsPublic\":null,\"F_SortCode\":29,\"F_BelongSys\":\"2\",\"ChildNodes\":[{\"F_Id\":\"804B991694324AC5BCCC0CED9AA72338\",\"F_ParentId\":\"602119BB4BA04378B405395876F73B5A\",\"F_EnCode\":null,\"F_FullName\":\"请假V2\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"LeaveV2Form\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":101,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"1753FCB94EE243C69233299BC468F3BA\",\"F_ParentId\":\"602119BB4BA04378B405395876F73B5A\",\"F_EnCode\":null,\"F_FullName\":\"请假审批V2\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"ApprovalV2\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":103,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"2D148E0F775B4B02BCEB1EE460545F5F\",\"F_ParentId\":\"602119BB4BA04378B405395876F73B5A\",\"F_EnCode\":null,\"F_FullName\":\"校内互访\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"VisitorInfoInSchool\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":107,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"D4ECF1448C6F405CAB1E711882409B1B\",\"F_ParentId\":\"602119BB4BA04378B405395876F73B5A\",\"F_EnCode\":null,\"F_FullName\":\"校外访客登记\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"VisitorOutSchool\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":107,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"079CDBC55B2F4C7AA7DA7B924936B897\",\"F_ParentId\":\"602119BB4BA04378B405395876F73B5A\",\"F_EnCode\":null,\"F_FullName\":\"访客授权\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"VisitorApproval\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":110,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"554EC6EF9F9A4B07ABD534400E78205D\",\"F_ParentId\":\"602119BB4BA04378B405395876F73B5A\",\"F_EnCode\":null,\"F_FullName\":\"访客审批\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"VisitList\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":130,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"554EC6EF9F9A4B07ABD534400E78205D\",\"F_ParentId\":\"602119BB4BA04378B405395876F73B5A\",\"F_EnCode\":null,\"F_FullName\":\"照片审批\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"ApprovalPhotos\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":130,\"F_BelongSys\":\"2\",\"ChildNodes\":[]}]},{\"F_Id\":\"12E9AD040D904E6C814B91C7888CFCE6\",\"F_ParentId\":\"0\",\"F_EnCode\":null,\"F_FullName\":\"记录查询\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"Group\",\"F_Target\":\"expand\",\"F_IsMenu\":true,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":30,\"F_BelongSys\":\"2\",\"ChildNodes\":[{\"F_Id\":\"205D591EAD0C40A6A5D8F080512F01CD\",\"F_ParentId\":\"12E9AD040D904E6C814B91C7888CFCE6\",\"F_EnCode\":null,\"F_FullName\":\"出入\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"InOut\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":1,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"50FE2F54203049AE888B2537E1273AFA\",\"F_ParentId\":\"12E9AD040D904E6C814B91C7888CFCE6\",\"F_EnCode\":null,\"F_FullName\":\"晚归\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"LateIn\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":2,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"71DE61D4F046435588E2ED12A86C91E5\",\"F_ParentId\":\"12E9AD040D904E6C814B91C7888CFCE6\",\"F_EnCode\":null,\"F_FullName\":\"未归\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"NotIn\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":3,\"F_BelongSys\":\"2\",\"ChildNodes\":[]},{\"F_Id\":\"6EC72287750E4AC596469088491C7E62\",\"F_ParentId\":\"12E9AD040D904E6C814B91C7888CFCE6\",\"F_EnCode\":null,\"F_FullName\":\"未出\",\"F_Icon\":\"fafa-chain\",\"F_Ico\":\"fafa-chain\",\"F_UrlAddress\":\"NotOut\",\"F_Target\":\"blank\",\"F_IsMenu\":false,\"F_IsExpand\":false,\"F_IsPublic\":false,\"F_SortCode\":4,\"F_BelongSys\":\"2\",\"ChildNodes\":[]}]}]";
            }
            return "[]";
            //var roles = Operator.Current.Roles;
            //var data = new List<Menu>();
            //foreach (var e in roles)
            //{
            //    data = data.Union(app.GetEnableMenuList(e), new ModuleComparer()).ToList();
            //}
            //return ToMenuJson(data, SYS_CONSTS.DbNull);
        }

        public static string ToMenuJson(List<Menu> data, string parentId)
        {
            var sbJson = new StringBuilder();
            sbJson.Append("[");
            var entitys = data.FindAll(t => t.ParentId.Equals(parentId)).ToList().Select(p => new
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