//using log4net;
//using Quartz;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using TaskApi.NanHangReport;
//using ZHXY.Common;
//
//using ZHXY.Domain.Entity;

//namespace TaskApi.Job
//{
//    public class ReportJob : IJob
//    {
//        private ILog Logger { get; } = LogManager.GetLogger(typeof(ReportJob));
//        private static readonly string schoolCode = ConfigurationManager.AppSettings["NanHangSchoolCode"];
//        private readonly DateTime now = DateTime.Now;
//        private List<Student> stuList = new Repository<Student>(schoolCode).QueryAsNoTracking().ToList();
//        private List<DormStudent> dormStuList = new Repository<DormStudent>(schoolCode).QueryAsNoTracking().ToList();
//        private Repository<LateReturnReport> lateRepo = new Repository<LateReturnReport>(schoolCode);
//        private Repository<NoReturnReport> noReturnRepo = new Repository<NoReturnReport>(schoolCode);
//        private Repository<OriginalReport> originalRepo = new Repository<OriginalReport>(schoolCode);
//        private Repository<DormRule> ruleRepo = new Repository<DormRule>(schoolCode);
//        private Repository<NoOutReport> noOutRepo = new Repository<NoOutReport>(schoolCode);
//        public void Execute(IJobExecutionContext context)
//        {
//            try
//            {
//                int count = 0;
//                Stopwatch sw = new Stopwatch();
//                Console.WriteLine("<----开始分类原始报表异常数据---->");
//                //获取前一天的原始数据
//                var list = GetOriginalList(1);
//                //获取规则
//                var rule = GetRule();
//                #region 分类晚归
//                Console.WriteLine("<----开始推送晚归数据---->");
//                sw.Start();
//                count = PushToLateReturn(rule, list);
//                sw.Stop();
//                Console.WriteLine("<----晚归数据推送完成，耗时" + sw.ElapsedMilliseconds + "毫秒 总计" + count + "条数据---->");
//                #endregion
//                #region 分类未归
//                Console.WriteLine("<----开始推送未归数据---->");
//                sw.Restart();
//                count = PushToNoReturn(rule, list);
//                sw.Stop();
//                Console.WriteLine("<----未归数据推送完成，耗时" + sw.ElapsedMilliseconds  + "毫秒 总计" + count + "条数据---->");
//                #endregion
//                #region 分类长时间未出
//                Console.WriteLine("<----开始推送长时间未出数据---->");
//                sw.Restart();
//                count = PushToNoOut(rule, list);
//                sw.Stop();
//                Console.WriteLine("<----长时间未出数据推送完成，耗时" + sw.ElapsedMilliseconds + "毫秒 总计" + count + "条数据---->");
//                #endregion
//                Console.WriteLine("<----原始报表异常数据分类结束---->");

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("分类数据失败，原因为:" + ex.Message);
//            }
//        }
//        private bool IsBeforeDawn(DateTime time)
//        {
//            var hour=Convert.ToInt32( DateHelper.GetDateTimePart("hour", time));
//            return 0 <= hour && hour <= 6;
//        }
//        private DormRule GetRule()
//        {
//            var rule = ruleRepo.QueryAsNoTracking().FirstOrDefault();
//            if (rule == null) throw new Exception("还未设置规则");
//            return rule;
//        }
//        private List<OriginalReport> GetOriginalList(int day)
//        {
//            var end = now;
//            var start = end.AddDays(-day);
//            StringBuilder sb = new StringBuilder("select * from dhflow_");
//            if (DateHelper.IsExtendIntoNext("month", end, start))
//            {
//                sb.Append(start.ToString("yyyyMM"));
//                sb.Append(" where date>=@start");
//                sb.Append(" UNION ALL");
//                sb.Append(" select * from dhflow_");
//                sb.Append(end.ToString("yyyyMM"));
//                sb.Append(" where date<=@end");
//            }
//            else
//            {
//                sb.Append(start.ToString("yyyyMM"));
//                sb.Append(" where");
//                sb.Append(" date>=@start and date<=@end");
//            }
//            DbParameter[] parameters = new DbParameter[]
//            {
//                new SqlParameter("@start",start),
//                new SqlParameter("@end",end)
//            };
//            var data = originalRepo.GetDataTable(sb.ToString(), parameters);
//            var list = originalRepo.DataTableToList<OriginalReport>(data);
//            return list;

//        }
//        private int PushToLateReturn(DormRule rule, List<OriginalReport> list)
//        {
//            var pushList = new List<LateReturnReport>();
//            var lateTime = Convert.ToDateTime(now.ToString("yyyy-MM-dd") + " " + rule.F_SetLateReturn);
//            if (!IsBeforeDawn(lateTime)) lateTime = lateTime.AddDays(-1);
//            foreach (var item in list)
//            {
//                if (item.InOut.Equals(((byte)InOutType.In).ToString()) && item.Date > lateTime)
//                {
//                    var student = stuList.FirstOrDefault(p => p.F_StudentNum.Equals(item.Code));
//                    if (student == null) continue;
//                    var dorm = dormStuList.FirstOrDefault(p => p.F_Student_ID.Equals(student?.F_Id));
//                    pushList.Add(new LateReturnReport()
//                    {
//                        F_College = item.DepartmentName,
//                        F_Account = student?.F_StudentNum,
//                        F_InTime = item.Date,
//                        F_Name = item.LastName + item.FirstName,
//                        F_Class = student?.F_Class_ID,
//                        F_Dorm = dorm?.F_DormId,
//                        F_Id = Guid.NewGuid().ToString("N"),
//                        F_CreatorTime = now
//                    });
//                }
//            }
//            lateRepo.BatchInsert(pushList);
//            return pushList.Count();
//        }
//        private int PushToNoReturn(DormRule rule, List<OriginalReport> list)
//        {
//            var pushList = new List<NoReturnReport>();
//            var groupList = list.GroupBy(p => p.Code);
//            foreach (var group in groupList)
//            {
//                var item = group.OrderByDescending(p => p.Date).FirstOrDefault();
//                if (item.InOut.Equals(((byte)InOutType.Out).ToString()))
//                {
//                    var student = stuList.FirstOrDefault(p => p.F_StudentNum.Equals(group.Key));
//                    if (student == null) continue;
//                    var dorm = dormStuList.FirstOrDefault(p => p.F_Student_ID.Equals(student?.F_Id));
//                    var origin = list.FirstOrDefault(p => p.Code.Equals(group.Key));
//                    pushList.Add(new NoReturnReport()
//                    {
//                        F_College = origin.DepartmentName,
//                        F_Account = student?.F_StudentNum,
//                        F_Name = origin.LastName + origin.FirstName,
//                        F_Class = student?.F_Class_ID,
//                        F_Dorm = dorm?.F_DormId,
//                        F_Id = Guid.NewGuid().ToString("N"),
//                        F_CreatorTime = now
//                    });
//                }
//            }
//            noReturnRepo.BatchInsert(pushList);
//            return pushList.Count();
//        }
//        private int PushToNoOut(DormRule rule, List<OriginalReport> list)
//        {
//            var pushList = new List<NoOutReport>();
//            var groupList = GetOriginalList(3).GroupBy(p => p.Code);
//            var dateTime = now;
//            foreach (var group in groupList)
//            {
//                var item = group.OrderByDescending(p => p.Date).FirstOrDefault();
//                var diff = DateHelper.DateDiff("hour", (DateTime)item.Date, dateTime);
//                if (item.InOut.Equals(((byte)InOutType.In).ToString()) && diff >= rule.F_SetNoOut)
//                {
//                    var student = stuList.FirstOrDefault(p => p.F_StudentNum.Equals(item.Code));
//                    if (student == null) continue;
//                    var dorm = dormStuList.FirstOrDefault(p => p.F_Student_ID.Equals(student?.F_Id));
//                    pushList.Add(new NoOutReport()
//                    {
//                        F_College = item.DepartmentName,
//                        F_Account = student?.F_StudentNum,
//                        F_InTime = item.Date,
//                        F_Name = item.LastName + item.FirstName,
//                        F_Class = student?.F_Class_ID,
//                        F_Dorm = dorm?.F_DormId,
//                        F_Id = Guid.NewGuid().ToString("N"),
//                        F_CreatorTime = now,
//                        F_Time = (int)diff
//                    });
//                }
//            }
//            noOutRepo.BatchInsert(pushList);
//            return pushList.Count();
//        }
//    }
//}
