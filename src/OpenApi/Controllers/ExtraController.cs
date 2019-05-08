using System;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
namespace OpenApi.Controllers
{
    [RoutePrefix("api/Extra")]
    public class ExtraController : ApiControllerBase
    {
        /// <summary>
        /// 7.1 获取学年学期信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string YearSemester(Parm input)
        {
            try
            {
                SchSemesterAppService app = new SchSemesterAppService();
                var data = app.GetList().Select(p =>
                 new
                 {
                     id = p.F_Id,
                     name = p.F_Year+"学年"+ p.F_Name,
                     startTime = p.F_Start_Time,
                     endTime = p.F_End_Time,
                 });
                return Success(data);
            }
            catch (Exception ex)
            {
                return Error("0001",ex.Message);
            }
        }
        /// <summary>
        ///  6.2 获取班级考勤
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public string ClassAttendance(Parm input)
        {
            try
            {
                AttendanceRuleAppService RuleApp = new AttendanceRuleAppService();
                return Success(RuleApp.GetClassSignDetailInfoV2(input.ClassId, input.Date));
            }
            catch (Exception ex)
            {
                return Error("0001", ex.Message);
            }
        }
        /// <summary>
        /// 5.2 获取学生个人考勤信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public string PersonAttendance(Parm input)
        {
            try
            {
                AttendanceRuleAppService RuleApp = new AttendanceRuleAppService();
                var student = new StudentAppService().GetByStuNum(input.StudentCode);
                if (student == null) throw new Exception("班级中没有该学生");
                return Success(RuleApp.getMySignInfoV2(student,null, input.Date));
            }
            catch (Exception ex)
            {
                return Error("0001", ex.Message);
            }
        }
        public class Parm
        {
            public string ClassId { get; set; }
            public string Date { get; set; }
            public string StudentCode { get; set; }
        }
    }
}