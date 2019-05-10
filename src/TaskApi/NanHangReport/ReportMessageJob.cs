using Common.Logging;
using Quartz;
using System;
using System.Configuration;
using System.Linq;
using ZHXY.Common;

using ZHXY.Domain;

namespace TaskApi.Job
{
    public class ReportMessageJob : IJob
    {
        private ILog Logger { get; } = LogManager.GetLogger(typeof(ReportMessageJob));
        private static readonly string schoolCode = ConfigurationManager.AppSettings["NanHangSchoolCode"];
        private readonly DateTime now = DateTime.Now;
        private Repository<LateReturnReport> lateRepo = new Repository<LateReturnReport>();
        private Repository<NoReturnReport> noReturnRepo = new Repository<NoReturnReport>();
        private Repository<NoOutReport> noOutRepo = new Repository<NoOutReport>();
        private Repository<InOutReceive> inOutReceiveRepo = new Repository<InOutReceive>();
        private Repository<User> sysUserRepo = new Repository<User>();

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DateTime? time = DateTime.Now.AddHours(-1);
                //晚归
                var lateData = lateRepo.Query(t => t.F_CreatorTime >= time);
                var receiveUsers = inOutReceiveRepo.QueryAsNoTracking().ToList();
               
                var ids = receiveUsers.Where(t => t.F_Type == 2).FirstOrDefault()?.F_ReceiveUser.Split(',');
                ids = ids ?? new string[0];
                var userNames = sysUserRepo.QueryAsNoTracking(x => ids.Contains(x.F_Id)).ToList().Select(x => x.F_Account);

                //foreach (var data in lateData)
                //{
                var data = lateData.FirstOrDefault();
                foreach (var userName in userNames)
                {
                    new ZHXY.Dorm.Device.NH.PushAppMessage()
                        .PushReportMessage(userName, data.ToJson(), "1011");
                }
                //}

                //未归
                var noInDatas = noReturnRepo.Query(t => t.F_CreatorTime >= time);
                ids = receiveUsers.Where(t => t.F_Type == 1).FirstOrDefault()?.F_ReceiveUser.Split(',');
                ids = ids ?? new string[0];
                userNames = sysUserRepo.QueryAsNoTracking(x => ids.Contains(x.F_Id)).Select(x => x.F_Account);

                var noInData = noInDatas.FirstOrDefault();
                foreach (var userName in userNames)
                {
                    new ZHXY.Dorm.Device.NH.PushAppMessage()
                        .PushReportMessage(userName, noInData.ToJson(), "1012");
                }

                //未出
                var noOutDatas = noOutRepo.Query(t => t.F_CreatorTime >= time);
                ids = receiveUsers.Where(t => t.F_Type == 3).FirstOrDefault()?.F_ReceiveUser.Split(',');
                ids = ids ?? new string[0];
                userNames = sysUserRepo.QueryAsNoTracking(x => ids.Contains(x.F_Id)).Select(x => x.F_Account);
                var noOutData = noOutDatas.FirstOrDefault();
                foreach (var userName in userNames)
                {
                    new ZHXY.Dorm.Device.NH.PushAppMessage()
                        .PushReportMessage(userName, data.ToJson(), "1011");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}
