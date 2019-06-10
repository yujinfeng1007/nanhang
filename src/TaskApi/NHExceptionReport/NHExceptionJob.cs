using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;

namespace TaskApi.NHExceptionReport
{
    public class NHExceptionJob : IJob
    {
        private ILog Logger { get; } = LogManager.GetLogger(typeof(NHExceptionJob));
        public static int WorkDayLateReturnTime = 23; //23 工作日晚归时间点 （周日到周四） 当晚23:00
        public static int WeekendLateReturnTime = 24; //休息日晚归时间点 （周五、周六） 当晚24:00
        public static int NotReturnTime = 2; //未归时间点 次日凌晨2点
        public static string NotOutTime = "24"; //长时间未出  最近的一次打卡记录为进入宿舍且24小时内没有外出打卡记录
        public void Execute(IJobExecutionContext context)
        {
            // 测试用时间点   
            //DateTime QuartzTime = Convert.ToDateTime("2019-05-24 02:00:00");
            var QuartzTime = DateTime.Now;
            var TableName = "DHFLOW_" + QuartzTime.Year + QuartzTime.Month.ToString().PadLeft(2, '0');
            var db = new EFContext();
            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("南航项目：开始统计请假学生列表 --> " + DateTime.Now.ToLocalTime());
            //过滤：请假的人员
            var EndTime = QuartzTime.Date.AddDays(-1).AddHours(23);
            var LeaveListId = db.Set<LeaveOrder>().Where(p => p.EndOfTime > EndTime).Select(p => p.LeaveerId).ToList();
            Console.WriteLine("南航项目：开始统计未归报表 --> " + DateTime.Now.ToLocalTime());
            //Step1  统计未归报表
            ProcessNoReturnException(QuartzTime, TableName, LeaveListId, db);
            sw.Stop();
            Console.WriteLine("南航项目：统计未归报表 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
            sw.Restart();
            Console.WriteLine("南航项目：开始统计晚归报表 --> " + DateTime.Now.ToLocalTime());
            //Step2 统计晚归报表
            ProcessLateReturnException(QuartzTime, TableName, LeaveListId, db);
            sw.Stop();
            Console.WriteLine("南航项目：统计晚归报表 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
            sw.Restart();
            Console.WriteLine("南航项目：开始统计长时间未出报表 --> " + DateTime.Now.ToLocalTime());
            //Step3: 统计长时间未出报表
            ProcessNotOutException(QuartzTime, TableName, db);
            sw.Stop();
            Console.WriteLine("南航项目：统计长时间未出报表 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// Step1: 统计未归报表
        /// </summary>
        /// <param name="QuartzTime"></param>
        /// <param name="moudle"></param>
        /// <param name="TableName"></param>
        public void ProcessNoReturnException(DateTime QuartzTime, string TableName, List<string> LeaveListId, EFContext db)
        {
            //查看所有人员当天的最后一条记录
            var StartTimestamp = DateHelper.ConvertDateTimeInt(QuartzTime.AddDays(-1));
            var EndTimestamp = DateHelper.ConvertDateTimeInt(QuartzTime);
            var UsersLastRecordSql = "select b.id,b.code,b.inout, b.swipDate, b.date,b.firstName name from (SELECT code, MAX(swipDate) swipDate FROM [dbo].[" + TableName + "] GROUP BY code HAVING MAX(swipDate) BETWEEN '" + StartTimestamp + "' and '" + EndTimestamp + "' ) a JOIN " + TableName + " b on a.code=b.code and a.swipDate = b.swipDate";
            var List = SqlHelper.ExecuteDataTable(UsersLastRecordSql).ToJson().Deserialize<List<LastRecordMoudle>>().Where(p => p.inout == 1 && !LeaveListId.Contains(p.studentId)).ToList();
            var ReportList = new List<NoReturnReport>();
            foreach (var noReturn in List)
            {
                var IdAndClass = db.Set<Student>().Where(s => s.StudentNumber.Equals(noReturn.code)).FirstOrDefault();
                if(IdAndClass == null){continue;}
                var report = new NoReturnReport();
                report.Id = Guid.NewGuid().ToString().Replace("-", "");
                report.CreatedTime = QuartzTime.AddDays(-1).Date;
                report.College = "root";
                report.Account = noReturn.code;
                report.OutTime = DateHelper.GetTime(noReturn.swipDate);
                report.Name = noReturn.name;
                report.StudentId = IdAndClass.Id;
                report.ClassId = IdAndClass.ClassId;
                report.DormId = db.Set<DormStudent>().Where(s => s.StudentId.Equals(IdAndClass.Id)).Select(u => u.DormId).FirstOrDefault();
                report.DayCount = (decimal) DateHelper.DateDiff("hour", DateHelper.GetTime(noReturn.swipDate), QuartzTime);
                ReportList.Add(report);
            }
            db.Set<NoReturnReport>().AddRange(ReportList);
            db.SaveChanges();
            Console.WriteLine(" **********  未归人员数量：" + ReportList.Count());
        }

        /// <summary>
        ///Step2： 统计晚归报表
        /// </summary>
        /// <param name="QuartzTime"></param>
        /// <param name="moudle"></param>
        /// <param name="TableName"></param>
        public void ProcessLateReturnException(DateTime QuartzTime, string TableName, List<string> LeaveListId, EFContext db)
        {
            //查看所有人员23:00到凌晨2点的最后一条记录
            var StartTime = QuartzTime.AddDays(-1).Date.AddHours(WorkDayLateReturnTime);
            var DayOfWeek = QuartzTime.AddDays(-1).DayOfWeek.ToString();
            if (DayOfWeek.Equals("Saturday") || DayOfWeek.Equals("Friday"))
            {
                StartTime = QuartzTime.Date;
            }
            var EndTime = QuartzTime.Date.AddHours(NotReturnTime);
            var StartTimestamp = DateHelper.ConvertDateTimeInt(StartTime);
            var EndTimestamp = DateHelper.ConvertDateTimeInt(EndTime);
            var UsersLastRecordSql = "select b.id,b.code,b.inout, b.swipDate, b.date,b.firstName name from (SELECT code, MAX(swipDate) swipDate FROM [dbo].[" + TableName + "] GROUP BY code HAVING MAX(swipDate) BETWEEN '" + StartTimestamp + "' and '" + EndTimestamp + "' ) a JOIN " + TableName + " b on a.code=b.code and a.swipDate = b.swipDate";
            var List = SqlHelper.ExecuteDataTable(UsersLastRecordSql).ToJson().Deserialize<List<LastRecordMoudle>>().Where(p => p.inout == 0 && !LeaveListId.Contains(p.studentId));
            var reportList = new List<LateReturnReport>();
            foreach(var p in List)
            {
                var IDClassId = db.Set<Student>().Where(s => s.StudentNumber.Equals(p.code)).FirstOrDefault();
                if(null == IDClassId) {continue; }
                var r = new LateReturnReport();
                r.Id = Guid.NewGuid().ToString().Replace("-", "");
                r.CreatedTime = QuartzTime.AddDays(-1).Date;
                r.College = "root";
                r.Account = p.code;
                r.InTime = DateHelper.GetTime(p.swipDate);
                r.Name = p.name;
                r.StudentId = IDClassId.Id;
                r.Class = IDClassId.ClassId;
                r.F_Time = (decimal)DateHelper.DateDiff("hour", StartTime, DateHelper.GetTime(p.swipDate));
                r.DormId = db.Set<DormStudent>().Where(s => s.StudentId.Equals(IDClassId.Id)).Select(u => u.DormId).FirstOrDefault();
                reportList.Add(r);
            }
            db.Set<LateReturnReport>().AddRange(reportList);
            db.SaveChanges();
            Console.WriteLine(" **********  晚归人员数量：" + reportList.Count());
        }

        /// <summary>
        /// Step3: 统计长时间未出报表
        /// 查询所有学生的最后一条流水记录为 ‘进’状态的数据，然后比对当前时间，计算未出宿舍的时间
        /// </summary>
        /// <param name="QuartzTime"></param>
        /// <param name="moudle"></param>
        /// <param name="TableName"></param>
        public void ProcessNotOutException(DateTime QuartzTime, string TableName, EFContext db)
        {
            //查看所有人员当天的最后一条记录
            var StartTimestamp = DateHelper.ConvertDateTimeInt(QuartzTime.Date.AddDays(-(int) QuartzTime.Day+1));
            var EndTimestamp = DateHelper.ConvertDateTimeInt(QuartzTime);
            var UsersLastRecordSql = "select b.id,b.code,b.inout, b.swipDate, b.date,b.firstName name from (SELECT code, MAX(swipDate) swipDate FROM [dbo].[" + TableName + "] GROUP BY code HAVING MAX(swipDate) BETWEEN '" + StartTimestamp + "' and '" + EndTimestamp + "' ) a JOIN " + TableName + " b on a.code=b.code and a.swipDate = b.swipDate";
            var List = SqlHelper.ExecuteDataTable(UsersLastRecordSql).ToJson().Deserialize<List<LastRecordMoudle>>().Where(p => p.inout == 0).ToList();
            var ReportList = new List<NoOutReport>();
            foreach(var noOut in List)
            {
                var FTime = DateHelper.DateDiff("hour", DateHelper.GetTime(noOut.swipDate), QuartzTime);
                if (FTime < 24){continue; }
                var IdAndClass = db.Set<Student>().Where(s => s.StudentNumber.Equals(noOut.code)).FirstOrDefault();
                if (IdAndClass == null){continue; }

                var report = new NoOutReport();
                report.Id = Guid.NewGuid().ToString().Replace("-", "");
                report.CreatedTime = QuartzTime.AddDays(-1).Date;
                report.College = "root";
                report.Account = noOut.code;
                report.InTime = DateHelper.GetTime(noOut.swipDate);
                report.Name = noOut.name;
                report.StudentId = IdAndClass.Id;
                report.ClassId = IdAndClass.ClassId;
                report.Time = (decimal) FTime;
                report.DormId = db.Set<DormStudent>().Where(s => s.StudentId.Equals(IdAndClass.Id)).Select(u => u.DormId).FirstOrDefault();
                ReportList.Add(report);
            }
            db.Set<NoOutReport>().AddRange(ReportList);
            db.SaveChanges();
            Console.WriteLine(" **********  长时间未出人员数量：" + ReportList.Count());
        }


        public class LastRecordMoudle
        {
            public string id;
            public string code;
            public int inout;
            public string swipDate;
            public string date;
            public string name;
            public string studentId;
        }
    }
}
