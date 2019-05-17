using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApi.DH;
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
            ExceptionMoudle moudle = new ExceptionMoudle();
            // 测试用时间点   
            //DateTime QuartzTime = Convert.ToDateTime("2019-05-17 02:00:00");
            DateTime QuartzTime = DateTime.Now;
            string TableName = "DHFLOW_" + QuartzTime.Year + QuartzTime.Month.ToString().PadLeft(2, '0');
            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("南航项目：开始统计未归报表 --> " + DateTime.Now.ToLocalTime());
            //Step1  统计未归报表
            ProcessNoReturnException(QuartzTime, moudle, TableName);
            sw.Stop();
            Console.WriteLine("南航项目：统计未归报表 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
            sw.Restart();
            Console.WriteLine("南航项目：开始统计晚归报表 --> " + DateTime.Now.ToLocalTime());
            //Step2 统计晚归报表
            ProcessLateReturnException(QuartzTime, moudle, TableName);
            sw.Stop();
            Console.WriteLine("南航项目：统计晚归报表 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
            sw.Restart();
            Console.WriteLine("南航项目：开始统计长时间未出报表 --> " + DateTime.Now.ToLocalTime());
            //Step3: 统计长时间未出报表
            ProcessNotOutException(QuartzTime, moudle, TableName);
            sw.Stop();
            Console.WriteLine("南航项目：统计长时间未出报表 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// Step1: 统计未归报表
        /// </summary>
        /// <param name="QuartzTime"></param>
        /// <param name="moudle"></param>
        /// <param name="TableName"></param>
        public void ProcessNoReturnException(DateTime QuartzTime, ExceptionMoudle moudle, string TableName)
        {
            //查看所有人员当天的最后一条记录
            long StartTimestamp = DateHelper.ConvertDateTimeInt(QuartzTime.AddDays(-1));
            long EndTimestamp = DateHelper.ConvertDateTimeInt(QuartzTime);
            String UsersLastRecordSql = "select b.id,b.code,b.inout, b.swipDate, b.date,b.firstName name from (SELECT code, MAX(swipDate) swipDate FROM [dbo].[" + TableName + "] GROUP BY code HAVING MAX(swipDate) BETWEEN '" + StartTimestamp + "' and '" + EndTimestamp + "' ) a JOIN " + TableName + " b on a.code=b.code and a.swipDate = b.swipDate";
            var List = SqlHelper.ExecuteDataTable(UsersLastRecordSql).ToJson().ToList<LastRecordMoudle>().Where(p => p.inout == 1).ToList();
            List<Dorm_NoReturnReport> ReportList = new List<Dorm_NoReturnReport>();
            foreach (var noReturn in List)
            {
                var IdAndClass = moudle.School_Students.Where(s => s.F_StudentNum.Equals(noReturn.code)).Select(p => new
                {
                    Id = p.F_Id,
                    ClassId = p.F_Class_ID
                }).FirstOrDefault();
                if(IdAndClass == null)
                {
                    continue;
                }
                Dorm_NoReturnReport report = new Dorm_NoReturnReport();
                report.F_Id = Guid.NewGuid().ToString().Replace("-", "");
                report.F_CreatorTime = DateTime.Now;
                report.F_DeleteMark = false;
                report.F_EnabledMark = true;
                report.F_College = "root";
                report.F_Account = noReturn.code;
                report.F_OutTime = DateHelper.GetTime(noReturn.swipDate);
                report.F_Name = noReturn.name;
                report.F_StudentId = IdAndClass.Id;
                report.F_Class = IdAndClass.ClassId;
                report.F_Dorm = moudle.Dorm_DormStudent.Where(s => s.F_Student_ID.Equals(IdAndClass.Id)).Select(u => u.F_DormId).FirstOrDefault();
                report.F_DayCount = (decimal) DateHelper.DateDiff("hour", DateHelper.GetTime(noReturn.swipDate), QuartzTime);
                ReportList.Add(report);
            }
            moudle.Dorm_NoReturnReport.AddRange(ReportList);
            moudle.SaveChanges();
            Console.WriteLine(" **********  未归人员数量：" + ReportList.Count());
        }

        /// <summary>
        ///Step2： 统计晚归报表
        /// </summary>
        /// <param name="QuartzTime"></param>
        /// <param name="moudle"></param>
        /// <param name="TableName"></param>
        public void ProcessLateReturnException(DateTime QuartzTime, ExceptionMoudle moudle, string TableName)
        {
            //查看所有人员23:00到凌晨2点的最后一条记录
            DateTime StartTime = QuartzTime.AddDays(-1).Date.AddHours(WorkDayLateReturnTime);
            var DayOfWeek = QuartzTime.AddDays(-1).DayOfWeek.ToString();
            if (DayOfWeek.Equals("Saturday") || DayOfWeek.Equals("Friday"))
            {
                StartTime = QuartzTime.Date;
            }
            DateTime EndTime = QuartzTime.Date.AddHours(NotReturnTime);
            long StartTimestamp = DateHelper.ConvertDateTimeInt(StartTime);
            long EndTimestamp = DateHelper.ConvertDateTimeInt(EndTime);
            string UsersLastRecordSql = "select b.id,b.code,b.inout, b.swipDate, b.date,b.firstName name from (SELECT code, MAX(swipDate) swipDate FROM [dbo].[" + TableName + "] GROUP BY code HAVING MAX(swipDate) BETWEEN '" + StartTimestamp + "' and '" + EndTimestamp + "' ) a JOIN " + TableName + " b on a.code=b.code and a.swipDate = b.swipDate";
            var List = SqlHelper.ExecuteDataTable(UsersLastRecordSql).ToJson().ToList<LastRecordMoudle>().Where(p => p.inout == 0);
            List<Dorm_LateReturnReport> reportList = new List<Dorm_LateReturnReport>();
            foreach(var p in List)
            {
                var IDClassId = moudle.School_Students.Where(s => s.F_StudentNum.Equals(p.code)).Select(d => new {
                    Id = d.F_Id,
                    ClassId = d.F_Class_ID
                }).FirstOrDefault();
                if(null == IDClassId)
                {
                    continue;
                }
                Dorm_LateReturnReport r = new Dorm_LateReturnReport();
                r.F_Id = Guid.NewGuid().ToString().Replace("-", "");
                r.F_CreatorTime = DateTime.Now;
                r.F_DeleteMark = false;
                r.F_EnabledMark = true;
                r.F_College = "root";
                r.F_Account = p.code;
                r.F_InTime = DateHelper.GetTime(p.swipDate);
                r.F_Name = p.name;
                r.F_StudentId = IDClassId.Id;
                r.F_Class = IDClassId.ClassId;
                r.F_Time = (decimal)DateHelper.DateDiff("hour", StartTime, DateHelper.GetTime(p.swipDate));
                r.F_Dorm = moudle.Dorm_DormStudent.Where(s => s.F_Student_ID.Equals(IDClassId.Id)).Select(u => u.F_DormId).FirstOrDefault();
                reportList.Add(r);
            }
            moudle.Dorm_LateReturnReport.AddRange(reportList);
            moudle.SaveChanges();
            Console.WriteLine(" **********  晚归人员数量：" + reportList.Count());
        }

        /// <summary>
        /// Step3: 统计长时间未出报表
        /// 查询所有学生的最后一条流水记录为 ‘进’状态的数据，然后比对当前时间，计算未出宿舍的时间
        /// </summary>
        /// <param name="QuartzTime"></param>
        /// <param name="moudle"></param>
        /// <param name="TableName"></param>
        public void ProcessNotOutException(DateTime QuartzTime, ExceptionMoudle moudle, string TableName)
        {
            //查看所有人员当天的最后一条记录
            long StartTimestamp = DateHelper.ConvertDateTimeInt(QuartzTime.Date.AddDays(-(int) QuartzTime.Day+1));
            long EndTimestamp = DateHelper.ConvertDateTimeInt(QuartzTime);
            String UsersLastRecordSql = "select b.id,b.code,b.inout, b.swipDate, b.date,b.firstName name from (SELECT code, MAX(swipDate) swipDate FROM [dbo].[" + TableName + "] GROUP BY code HAVING MAX(swipDate) BETWEEN '" + StartTimestamp + "' and '" + EndTimestamp + "' ) a JOIN " + TableName + " b on a.code=b.code and a.swipDate = b.swipDate";
            var List = SqlHelper.ExecuteDataTable(UsersLastRecordSql).ToJson().ToList<LastRecordMoudle>().Where(p => p.inout == 0).ToList();
            List<Dorm_NoOutReport> ReportList = new List<Dorm_NoOutReport>();
            foreach(var noOut in List)
            {
                var FTime = DateHelper.DateDiff("hour", DateHelper.GetTime(noOut.swipDate), QuartzTime);
                if (FTime < 24)
                {
                    continue;
                }
                var IdAndClass = moudle.School_Students.Where(s => s.F_StudentNum.Equals(noOut.code)).Select(p => new
                {
                    Id = p.F_Id,
                    ClassId = p.F_Class_ID
                }).FirstOrDefault();
                if (IdAndClass == null)
                {
                    continue;
                }
                
                Dorm_NoOutReport report = new Dorm_NoOutReport();
                report.F_Id = Guid.NewGuid().ToString().Replace("-", "");
                report.F_CreatorTime = DateTime.Now;
                report.F_DeleteMark = false;
                report.F_EnabledMark = true;
                report.F_College = "root";
                report.F_Account = noOut.code;
                report.F_InTime = DateHelper.GetTime(noOut.swipDate);
                report.F_Name = noOut.name;
                report.F_StudentId = IdAndClass.Id;
                report.F_Class = IdAndClass.ClassId;
                report.F_Time = (decimal) FTime;
                report.F_Dorm = moudle.Dorm_DormStudent.Where(s => s.F_Student_ID.Equals(IdAndClass.Id)).Select(u => u.F_DormId).FirstOrDefault();
                ReportList.Add(report);
            }
            moudle.Dorm_NoOutReport.AddRange(ReportList);
            moudle.SaveChanges();
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
