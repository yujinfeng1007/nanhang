using Quartz;
using System;
using System.Linq;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;

namespace TaskApi.DH
{
    public class DHCancelSurveyJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine(" ************  开始执行南航项目：访客撤控 ： " + DateTime.Now);
            var db = new EFContext();
            var dateTime = DateTime.Now;
            var ApplyList = db.Set<VisitorApply>().Where(p => dateTime > p.VisitEndTime && p.SurveyStatus == "0" && p.DhId != null).ToList();
            foreach(var apply in ApplyList)
            {
                // 闸机Id列表
                var zjids = db.Set<Relevance>().Where(p => p.SecondKey == apply.BuildingId && p.Name == Relation.GateBuilding).Select(p => p.FirstKey).ToList();
                var channelIds = db.Set<Gate>().Where(t => zjids.Contains(t.Id)).Select(p => p.DeviceNumber+ "$7$0$0").ToArray();
                var ResultStr = DHAccount.CancelSurvey(channelIds, apply.DhId);
                var jo = ResultStr.Parse2JObject();
                if (jo.Value<bool>("success"))
                {
                    apply.SurveyStatus = "1";
                    Console.WriteLine("撤控成功：" + ResultStr + "  DhId = " + apply.DhId);
                }
                else
                {
                    Console.WriteLine("撤控失败：" + ResultStr + "  DhId = " + apply.DhId);
                }
                if(apply.VisitType == "1")
                {
                    Console.WriteLine("校外访客删除：" + DHAccount.PUSH_DH_DELETE_PERSON(new string[] { apply.DhId }));
                }
            }
            db.SaveChanges();
            db.Dispose();
        }
    }
}
