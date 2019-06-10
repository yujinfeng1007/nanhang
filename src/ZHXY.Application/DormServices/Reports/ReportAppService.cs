using ZHXY.Domain;
using System;
using ZHXY.Common;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;

namespace ZHXY.Application
{

    /// <summary>
    /// 报表服务
    /// </summary>
    public class ReportAppService : AppService
    {
        public ReportAppService(DbContext r) : base(r) { }

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
            var totalQty = Read<Student>().Count();


            // 在寝人数
            var inQty = Read<Student>(t => t.InOut == "0").Count();

            // 外出人数
            var outQty = Read<Student>(t=>t.InOut=="1" || string.IsNullOrEmpty(t.InOut)).Count();

            // 请假人数
            var leaveQty = Read<LeaveOrder>(t => t.StartTime <= now && t.EndOfTime >= now && t.Status=="1").Count();

            // 未出人数
            var noOutQty = Read<NoOutReport>().Count();

            // 未归人数
            var noReturnQty = Read<NoReturnReport>().Count();

            // 晚归人数
            var laterReturnQty = Read<LateReturnReport>().Count();

            // 请假待审批记录
            var leaveList = Read<LeaveOrder>(t => t.Status == "0").ToList();


            return new
            {
                TotalQty = totalQty,
                LeaveQty = noOutQty,
                InQty = inQty,
                OutQty = outQty,
                LeftPieChartData = new List<ChartsDataItemDto> {
                    new ChartsDataItemDto { Name="在寝",Value=inQty},
                    new ChartsDataItemDto { Name="外出",Value=outQty},
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
            var noReturnQty = Read<NoReturnReport>(p => p.StudentId.Equals(userId) && p.CreatedTime >= st && p.CreatedTime <= et).Count();
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
            var totalQty = 0;
          
            if (category.Equals("Grade")) {
                totalQty = Read<Student>().Where(p => p.GradeId.Equals(orgId)).Count();
            }else if (category.Equals("Division")){
                totalQty = Read<Student>().Where(p => p.DivisId.Equals(orgId)).Count();
            }else if (category.Equals("Class")){
                totalQty = Read<Student>().Where(p => p.ClassId.Equals(orgId)).Count();
            }


            //在寝人数   in_out 0-进  1-出
            var inQty = 0;

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
            var outQty = 0;

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
            var leaveQty = 0;

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

             leaveQty = R.Database.SqlQuery<int>(leaveSql.ToString()).First();
            //linq写法            
            //var sql = (from leave in Read<LeaveOrder>()
            //           join stu in Read<Student>() on leave.LeaveerId equals stu.Id
            //           where leave.Status == "1" && stu.ClassId ==""   
                      
            //           orderby leave.Id descending
            //           select leave.Id              
            //          ).Count();
           
            return new
            {
                totalQty = totalQty,
                inQty = inQty,
                outQty = outQty,
                leaveQty = leaveQty
            };
        }

        /// <summary>
        /// 通过组织机构ID，查询下属机构所包含的所有学生相关信息
        /// </summary>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public dynamic loadByOrg(string OrgId)
        {
            var date = DateTime.Now;
            var OrgList = new List<string> { OrgId };
            this.GetChildOrg(OrgId, OrgList);
            var StuList = Read<Student>(p => OrgList.Contains(p.ClassId)).Select(p => new { p.InOut, p.Id}).ToList();
            var totalQty = StuList.Count();
            var outQty = StuList.Count(p => string.IsNullOrEmpty(p.InOut) || p.InOut.Equals("1"));
            var inQty = StuList.Count(p => !string.IsNullOrEmpty(p.InOut) && p.InOut.Equals("0"));
            var leaveQty = StuList.Join(Read<LeaveOrder>(s => s.Status == "1"), stu => stu.Id, leave => leave.LeaveerId, (stu, leave) => new { leave.Id}).Count();
            return new {
                totalQty,
                inQty,
                outQty,
                leaveQty
            };
        }


        public dynamic ListOrgan(string userId)
        {
            var OrgLeader = Read<OrgLeader>(p => p.UserId.Equals(userId)).ToList();
            if(OrgLeader == null || OrgLeader.Count() == 0)
            {
                return "当前用户未绑定相关机构！";
            }
            else
            {
                return OrgLeader.Join(Read<Organ>(), lea => lea.OrgId, org => org.Id, (lea, org) => new
                {
                    org.Id,
                    org.Name
                }).ToList();
            }
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


