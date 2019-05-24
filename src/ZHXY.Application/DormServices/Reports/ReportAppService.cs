using ZHXY.Domain;
using System;
using ZHXY.Common;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ZHXY.Application
{

    /// <summary>
    /// 报表服务
    /// </summary>
    public class ReportAppService : AppService
    {
        public ReportAppService(IZhxyRepository r) : base(r) { }

        /// <summary>
        /// 获取异常出入报表
        /// </summary>
        public void GetAbnormalAccess(GetAbnormalAccessDto dto)
        {





        }

        /// <summary>
        /// 首页-数据面板-图表
        /// </summary>
        public dynamic GetDefaultData()
        {
            var now = DateTime.Now;
            var startTime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            var endTime = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            var tableName = DateTime.Now.ToString("yyyyMM");

            // 考勤总人数
            var totalQty = Read<Student>(t => !string.IsNullOrEmpty(t.InOut)).Count();

            // 在寝人数
            var noOutQty = Read<Student>(t=>t.InOut=="0").Count();

            // 外出人数
            var noReturnQty = Read<Student>(t=>t.InOut=="1").Count();

            // 请假人数
            var leaveQty = Read<LeaveOrder>(t=>t.StartTime<=now && t.EndOfTime>=now).Count();

            // 已签到人数
            var signedQty =Convert.ToInt32(SqlHelper.ExecuteScalar(string.Format("SELECT COUNT(0) FROM dbo.[DHFLOW_{0}] WHERE date>='{1}' AND date<='{2}' AND inOut='0' GROUP BY personId ", tableName, startTime, endTime)));

            // 晚归人数
            var laterReturnQty = Read<LateReturnReport>().Count();

            // 请假待审批记录
            var leaveList = Read<LeaveOrder>(t => t.Status == "0").ToList();


            return new
            {
                TotalQty = totalQty,
                NoOutQty = noOutQty,
                InQty = noOutQty,
                OutQty = noReturnQty,
                LeaveQty = leaveQty,
                LeftPieChartData = new List<ChartsDataItemDto> {
                    new ChartsDataItemDto { Name="已签到",Value=signedQty},
                    new ChartsDataItemDto { Name="请假",Value=leaveQty},
                },
                RightPieChartData = new List<ChartsDataItemDto>
                { new ChartsDataItemDto { Name="其他异常",Value=0},
                    new ChartsDataItemDto { Name="晚归",Value=laterReturnQty},
                    new ChartsDataItemDto { Name="未归",Value=noReturnQty},
                    new ChartsDataItemDto { Name="未出",Value=noOutQty},
                },
                LeavesData = leaveList
            };

        }

        /// <summary>
        /// 学生移动端数据面板
        /// </summary>
        public dynamic GetMobileDataByStu(string userId, string startTime, string endTime)
        {
            var st = Convert.ToDateTime(startTime + " 00:00:00");
            var et = Convert.ToDateTime(endTime + " 23:59:59");
            // 未归次数            
            var noReturnQty = Read<NoReturnReport>(p => p.StudentId.Equals(userId) && p.OutTime >= st && p.OutTime <= et).Count();
            //晚归次数
            var laterReturnQty = Read<LateReturnReport>(p => p.StudentId.Equals(userId) && p.CreatedTime >= st && p.CreatedTime <= et).Count();
            //请假天数(已审批通过)   请假开始时间  and 请假结束时间  between startTime和endTime  
            var leaveDay = Read<LeaveOrder>(p => p.ApplicantId.Equals(userId) && p.Status.Equals("1") &&((p.StartTime >= st && p.StartTime <= et) || (p.StartTime >= st && p.StartTime <= et))).Sum(p => p.LeaveDays);


            return new
            {
                noReturnQty = noReturnQty,
                laterReturnQty = laterReturnQty,
                leaveDay = leaveDay ==null? 0:leaveDay
            };

        }


        /// <summary>
        /// 机构移动端数据面板   总人数，在寝人数，外出人数，请假人数
        /// </summary>
        public dynamic GetMobileDataByOrg(string orgId)
        {
            //判断登陆用户所属机构级别   院系Grade  、 年级Division 、 班级Class
            var category = Read<Organ>(p => p.Id.Equals(orgId)).Select(p => p.CategoryId).FirstOrDefault();
            //总人数
            int totalQty = 0;
          
            if (category.Equals("Grade")) {
                totalQty = Read<Student>().Where(p => p.GradeId.Equals(orgId)).Count();
            }else if (category.Equals("Division")){
                totalQty = Read<Student>().Where(p => p.DivisId.Equals(orgId)).Count();
            }else if (category.Equals("Class")){
                totalQty = Read<Student>().Where(p => p.ClassId.Equals(orgId)).Count();
            }


            //在寝人数   in_out 0-进  1-出
            int inQty = 0;

            if (category.Equals("Division"))
            {
                inQty = Read<Student>().Where(p => p.DivisId.Equals(orgId) && p.InOut.Equals("0")).Count();
            }
            else if (category.Equals("Grade"))
            {
                inQty = Read<Student>().Where(p => p.GradeId.Equals(orgId) && p.InOut.Equals("0")).Count();
            }
            else if (category.Equals("Class"))
            {
                inQty = Read<Student>().Where(p => p.ClassId.Equals(orgId) && p.InOut.Equals("0")).Count();
            }
            //外出人数  in_out 0-进  1-出
            int outQty = 0;

            if (category.Equals("Division"))
            {
                outQty = Read<Student>().Where(p => p.DivisId.Equals(orgId) && p.InOut.Equals("1")).Count();
            }
            else if (category.Equals("Grade"))
            {
                outQty = Read<Student>().Where(p => p.GradeId.Equals(orgId) && p.InOut.Equals("1")).Count();
            }
            else if (category.Equals("Class"))
            {
                outQty = Read<Student>().Where(p => p.ClassId.Equals(orgId) && p.InOut.Equals("1")).Count();
            }
            //请假人数
            int leaveQty = 0;

           //sql写法
           var leaveSql = new StringBuilder("select count(1) from zhxy_leave_order leave join zhxy_student stu on leave.leaveer_id = stu.id where leave.status = '1' ");
            if (category.Equals("Division"))
            {
                leaveSql = leaveSql.Append(" and stu.divis_id = '"+ orgId + "'");
            }
            else if (category.Equals("Grade"))
            {
                leaveSql = leaveSql.Append(" and stu.grade_id = '" + orgId + "'");
            }
            else if (category.Equals("Class"))
            {
                leaveSql = leaveSql.Append(" and stu.class_id = '" + orgId + "'");
            }

             leaveQty = R.Db.Database.SqlQuery<int>(leaveSql.ToString()).First();           

           
            return new
            {
                totalQty = totalQty,
                inQty = inQty,
                outQty = outQty,
                leaveQty = leaveQty
            };

        }


    }


    
}



public class ChartsDataItemDto
    {
        public string Name { get; set; }

        public decimal Value { get; set; }
    }

    /// <summary>
    /// 获取异常出入统计
    /// </summary>
    public class GetAbnormalAccessDto 
    {
        /// <summary>
        /// 0:自定义时间  1:本周 2:本月  
        /// </summary>
        public int Pattern { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string OrgId { get; set; }
        public DateTime StartTime
        {
            get
            {
                var d = DateTime.MinValue;
                switch (Pattern)
                {
                    case -1: DateTime.TryParse(Start, out d); break;
                    case 0: d = DateHelper.GetStartTimeOfWeek(); break;
                    case 1: d = DateHelper.GetStartTimeOfMonth(); break;
                }
                return d;
            }
        }
        public DateTime EndOfTime
        {
            get
            {
                var d = DateTime.Now;
                switch (Pattern)
                {
                    case -1: d = DateTime.TryParse(End, out d) ? d.AddDays(1).AddSeconds(-1) : DateTime.Now.Date.AddDays(1).AddSeconds(-1); break;
                    case 0: d = DateHelper.GetEndTimeOfWeek(); break;
                    case 1: d = DateHelper.GetEndTimeOfMonth(); break;
                }
                return d;
            }

        }
    }


