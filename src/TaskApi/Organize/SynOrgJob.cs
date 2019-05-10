using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;
using log4net;
using System.Net.Http;

namespace TaskApi
{
    public class SynOrgJob : IJob
    {
        private string _url = ConfigurationManager.AppSettings["SynOrganizeUrl"];

        private readonly string GetAllOrgInfoApiName = "/Models/jxzfzhxy/BaseInterfaceService.svc/GetAllOrgInfo?AccessTokenKey=3A15625FDF0BA91484BAABA5B70F7A5D";
        private readonly string GetOrgInfoByLastUpdatedTimeApiName = "/Models/jxzfzhxy/BaseInterfaceService.svc/GetOrgInfoByLastUpdatedTime";
        private readonly string GetAllUserInfoApiName = "/Models/jxzfzhxy/BaseInterfaceService.svc/GetAllUserInfo";
        private readonly string GetUserInfoByLastUpdatedTimeApiName = "/Models/jxzfzhxy/BaseInterfaceService.svc/GetUserInfoByLastUpdatedTime";

        private readonly ILog _logger = Logger.GetLogger(typeof(SynOrgJob));

        private Dictionary<string, string> schoolDict = new Dictionary<string, string>();

        public SynOrgJob()
        {
            schoolDict.Add("tfxx", "43e0edf5-31dc-4027-9b93-f06b1b04a347,94f66d8e-b506-43be-b778-11d739d64f5e");//天妃小学
            schoolDict.Add("hsdfx", "4546b7d3-7467-442b-af59-0b168977ed8f,94f66d8e-b506-43be-b778-11d739d64f5e");//杭师大附小
            schoolDict.Add("zpxx", "bb141c69-2e21-4fce-b45a-2b6a7d54a467,94f66d8e-b506-43be-b778-11d739d64f5e");//乍浦小学
        }

        // 静态锁
        private static readonly object locker = new object();

        public void Execute(IJobExecutionContext context)
        {
            lock (locker)
            {
                Console.Write("----------同步组织机构/用户开始-----------\r\n");

                try
                {
                    SnyOrg();
                    SnyUpdOrg();
                    //同步用户
                    SnyUpdUser();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    _logger.Error(e);
                }
                Console.Write("----------同步组织机构/用户结束-----------\r\n");
            }
        }

        #region 同步组织机构

        private void SnyOrg()
        {
            //var firstschool = schoolDict.First();
            //var service = new OrganizeRepository(firstschool.Key);
            //string[] strArray = firstschool.Value.Split(',');
            //string orgid = strArray[0];
            //var data = service.QueryAsNoTracking(t => t.F_Id == orgid).FirstOrDefault();
            //if (data == null)
            //{
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["RestSyn"]))
            {
                SynAllOrgInfo();
                SynUser();
            }
            //}
        }

        private void SynAllOrgInfo()
        {
            //var result = new HttpHelper(_url).Get<ResultModel>(null, GetAllOrgInfoApiName);
            //var result = HttpHelper.GetString(_url+GetAllOrgInfoApiName)?.ToObject<ResultModel>();
            var result = new HttpClient().GetStringAsync(_url + GetAllOrgInfoApiName).Result?.ToObject<ResultModel>();
            var datas = result.Result.ToList<OrgModel>();
            foreach (var dic in schoolDict)
            {
                Console.WriteLine("同步."+dic.Key);
                var pdatas = datas.Where(t => dic.Value.Contains(t.OrgId)).ToList();
                foreach (var data in pdatas)
                {
                    if (data != null)
                    {
                        InsertOrUpdOrg(dic.Key, data.OrgId, data.OrgId, data.OrgName, "0", "Division","0");
                        insertList(dic.Key, datas, data.OrgId);
                    }
                }
            }
        }

        private void insertList(string schoolCode, List<OrgModel> datas, string parentId)
        {
            var newlist = datas.Where(t => t.ParentOrgId == parentId);
            foreach (var data in newlist)
            {
                InsertOrUpdOrg(schoolCode, data.OrgId, data.OrgId, data.OrgName, data.ParentOrgId, GetOrgCateGoryId(data.Level),data.IS_DELETED);
                insertList(schoolCode, datas, data.OrgId);
            }
        }

        private string GetOrgCateGoryId(int Level)
        {
            if (Level == 0)
                return "Division";
            if (Level == 1)
                return "Grade";
            if (Level == 2)
                return "Class";
            return "1";
        }

        private void SnyUpdOrg()
        {
            Console.WriteLine("同步增量组织."+ DateTime.Now.AddMinutes(-Convert.ToInt32(ConfigurationManager.AppSettings["UpdatedBeforeTime"])).ToString("yyyy-MM-dd HH:mm:ss"));
            var parms = new Dictionary<string, string>();
            parms.Add("LastUpdatedTime", DateTime.Now.AddMinutes(-Convert.ToInt32(ConfigurationManager.AppSettings["UpdatedBeforeTime"])).ToString("yyyy-MM-dd HH:mm:ss"));

            var result = WebHelper.GetString(_url+ GetOrgInfoByLastUpdatedTimeApiName, parms )?.ToObject<ResultModel>();
            var datas = result.Result.ToList<OrgModel>();
            foreach (var data in datas)
            {
                foreach (var dic in schoolDict)
                {
                    if (UpdOrAddOrg(dic.Key, data.OrgId, data.OrgId, data.OrgName, data.ParentOrgId, GetOrgCateGoryId(data.Level), data.IS_DELETED))
                        break;
                }
            }
        }

        private bool UpdOrAddOrg(string schoolCode, string f_id, string f_EnCode, string f_FullName, string f_ParentId, string CateGoryId,string isDelete)
        {
            try
            {
                var db = new ZhxyDbContext();
                var parentdata = db.Set<Organ>().Where(t => t.Id == f_ParentId).FirstOrDefault();
                if (parentdata == null)
                {
                    return false;
                }
                InsertOrUpdOrg(schoolCode, f_id, f_EnCode, f_FullName, f_ParentId, CateGoryId, isDelete);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                _logger.Error(e);
            }
            return true;
        }

        private void InsertOrUpdOrg(string schoolCode, string f_id, string f_EnCode, string f_FullName, string f_ParentId, string CateGoryId,string isDelete)
        {
            try
            {
                using (var db = new UnitWork().BeginTrans())
                {
                    var entity = new Organ();
                    entity.EnCode = f_EnCode;
                    entity.Name = f_FullName;
                    entity.ParentId = f_ParentId;
                    entity.CategoryId = CateGoryId;
                    var data = db.QueryAsNoTracking<Organ>(t => t.Id == f_id).FirstOrDefault();
                    if (data != null)
                    {
                        db.Update(entity);
                    }
                    else
                    {
                        entity.Id = f_id;
                        db.Insert(entity);
                    }
                    if (CateGoryId == "Class")
                    {
                        AddOrUpdClassInfo(db, f_id, f_FullName);
                    }
                    db.Commit();
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                _logger.Error(e);
            }
        }

        private void AddOrUpdClassInfo(IUnitWork db, string classId, string className)
        {
            //var entity = new ClassInfo();
            //entity.F_Name = className;
            //entity.F_IsMoveClass = false;
            //var data = db.QueryAsNoTracking<ClassInfo>(t => t.F_ClassID == classId).FirstOrDefault();
            //if (data != null)
            //{
            //    entity.Modify(data.F_Id);
            //    db.Update(entity);
            //}
            //else
            //{
            //    entity.Create();
            //    entity.F_ClassID = classId;
            //    db.Insert(entity);
            //}
        }

        #endregion 同步组织机构

        #region 同步用户

        public void SynUser()
        {
            Console.WriteLine("同步所有用户.");
            //var result = new HttpHelper(_url).Get<ResultModel>(null, GetAllUserInfoApiName);
            var result=new HttpClient().GetStringAsync(_url + GetAllUserInfoApiName).Result?.ToObject<ResultModel>();
            var datas = result.Result.ToList<UserModel>();
            foreach (var data in datas)
            {
                foreach (var dic in schoolDict)
                {
                    if (UpdOrAddUser(dic.Key, data))
                        break;
                }
            }
        }

        private bool UpdOrAddUser(string schoolCode, UserModel data)
        {
            try
            {
                var db = new ZhxyDbContext();
                var parentdata = db.Set<Organ>().Where(t => t.Id == data.OrgId).FirstOrDefault();
                if (parentdata == null)
                {
                    return false;
                }
                var userService = new SynUserService(schoolCode);
                string userNum = string.IsNullOrEmpty(data.user_Num) ? data.UserId : data.user_Num;
                userService.UpdOrAdd(data, GetOrgCateGoryId(data.Level));
            }
            catch (Exception e)
            {
                Console.WriteLine("用户信息:" + data.UserId + ",name:" + data.UserName);

                Console.Write(e.Message);
                _logger.Error(e);
            }
            return true;
        }

        private void SnyUpdUser()
        {
            Console.WriteLine("同步增量用户." + DateTime.Now.AddMinutes(-Convert.ToInt32(ConfigurationManager.AppSettings["UpdatedBeforeTime"])).ToString("yyyy-MM-dd HH:mm:ss"));

            var parms = new Dictionary<string, string>();
            parms.Add("LastUpdatedTime", DateTime.Now.AddMinutes(-Convert.ToInt32(ConfigurationManager.AppSettings["UpdatedBeforeTime"])).ToString("yyyy-MM-dd HH:mm:ss"));
            var result = WebHelper.GetString(_url+GetUserInfoByLastUpdatedTimeApiName,parms)?.ToObject<ResultModel>();
            var datas = result.Result.ToList<UserModel>();
            foreach (var data in datas)
            {
                foreach (var dic in schoolDict)
                {
                    if (UpdOrAddUser(dic.Key, data))
                        break;
                }
            }
        }

        #endregion 同步用户
    }
}