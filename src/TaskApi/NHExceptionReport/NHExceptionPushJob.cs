using Common.Logging;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHXY.Common;
using ZHXY.Dorm.Device.NH;

namespace TaskApi.NHExceptionReport
{
    public class NHExceptionPushJob : IJob
    {
        private ILog Logger { get; } = LogManager.GetLogger(typeof(NHExceptionPushJob));
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("进入方法！");
            //DateTime Time = Convert.ToDateTime("2019-05-16 08:00:00");
            DateTime Time = DateTime.Now;
            ExceptionMoudle moudle = new ExceptionMoudle();
            var leaderList = moudle.sys_org_leader.ToList();
            foreach(var leader in leaderList)
            {
                HashSet<string> Ids = new HashSet<string>(); //当前组织机构下属所有组织机构的ID集合
                string OrgId = leader.org_id;
                string UserId = leader.user_id;
                var OrgIds = moudle.Sys_Organize.Where(p => p.F_ParentId.Equals(OrgId)).Select(p => p.F_Id).ToList();
                if (null == OrgIds || OrgIds.Count() == 0)
                {
                    continue;
                }
                foreach (var id in OrgIds)
                {
                    Ids.Add(id);
                    var SonOrgIds = moudle.Sys_Organize.Where(p => p.F_ParentId.Equals(id)).Select(p => p.F_Id).ToList();
                    if (null == SonOrgIds || SonOrgIds.Count() == 0)
                    {
                        continue;
                    }
                    foreach (var sid in SonOrgIds)
                    {
                        Ids.Add(sid);
                        var SonOfSonOrgIds = moudle.Sys_Organize.Where(p => p.F_ParentId.Equals(sid)).Select(p => p.F_Id).ToList();
                        if (null == SonOfSonOrgIds || SonOfSonOrgIds.Count() == 0)
                        {
                            continue;
                        }
                        foreach (var ssid in SonOfSonOrgIds)
                        {
                            Ids.Add(sid);
                            var SonOfSonOfSonIds = moudle.Sys_Organize.Where(p => p.F_ParentId.Equals(ssid)).Select(p => p.F_Id).ToList();
                            if(null == SonOfSonOfSonIds || SonOfSonOfSonIds.Count() == 0)
                            {
                                continue;
                            }
                            foreach (var sssid in SonOfSonOfSonIds)
                            {
                                Ids.Add(sssid);
                            }
                        }
                    }
                }
                Ids.Add(OrgId);
                if(null != Ids && Ids.Count() != 0)
                {
                    DateTime Yeatday = Time.Date.AddDays(-1).AddHours(2);
                    JObject jo = new JObject();
                    string userName = moudle.Sys_User.Where(p => p.F_Id.Equals(leader.user_id)).Select(p => p.F_Account).FirstOrDefault();
                    var NoReturnList = moudle.Dorm_NoReturnReport.Where(p => Yeatday < p.F_CreatorTime && Ids.Contains(p.F_Class)).ToList();
                    var LateReturnList = moudle.Dorm_LateReturnReport.Where(p => Yeatday < p.F_CreatorTime && Ids.Contains(p.F_Class)).ToList();
                    var NoOutList = moudle.Dorm_NoOutReport.Where(p => Yeatday < p.F_CreatorTime && Ids.Contains(p.F_Class)).ToList();
                    jo.Add("NoReturnList", NoReturnList.ToJson());
                    jo.Add("LateReturnList", LateReturnList.ToJson());
                    jo.Add("NoOutList", NoOutList.ToJson());
                    new PushAppMessage().PushReportMessage(userName, jo.ToJson(), "");
                    Console.WriteLine("NHExceptionPush 推送成功：" + userName);
                }
            }
            Console.WriteLine("  ********* NHExceptionPush 全部推送成功 ");
        }
    }
}
