using ZHXY.Application;
using System;
using ZHXY.Common;
using System.Web.Http;
using System.Linq;

namespace OpenApi.Controllers
{
    [RoutePrefix("api/School_Attendance")]
    public class School_AttendanceController : ApiControllerBase
    {
        private StudentAppService  StudentApp => new StudentAppService();
        private AttendanceRuleAppService  AttendanceRulesApp => new AttendanceRuleAppService();

        public class ReportSign4EGParam
        {
            public string F_APPKEY { get; set; }
            public string F_SESSIONKEY { get; set; }
            public string F_Sn { get; set; }
            public string F_CardNum { get; set; }
            public string F_Time { get; set; }
            public string F_School_Id { get; set; }
            public string F_Pos { get; set; }
            public int F_FlowType { get; set; }
            public string F_Course_PrepareID { get; set; }
        }

        [Route("ReportSign4EG")]
        public string ReportSign4EG(ReportSign4EGParam param)
        {
            try
            {
                if (!string.IsNullOrEmpty(param.F_School_Id) && !string.IsNullOrEmpty(param.F_CardNum) && !string.IsNullOrEmpty(param.F_Sn))
                {
                    //考勤逻辑
                    /**
                        * 1、先校验学号，若不是该行政班学生，提示无该学生
                        * 2、查询考勤规则
                        * 3、计算考勤结果 保存流水 计算结果 进统计表
                        * */
                    var body = new Object();

                    //先校验学号
                    var student = StudentApp.GetStudentByCardNum(param.F_CardNum);

                    if (student == null)
                    {
                        return Error("0000", "无效卡片");
                    }
                    else
                    {
                        var parent = new StuParentAppService().GetList(t => t.F_Stu_Id == student.F_Id).FirstOrDefault()?.Parent;
                        string inout = "进/出";
                        if (param.F_FlowType == 1) inout = "进";
                        if (param.F_FlowType == 1) inout = "出";
                        //ZHXY.Message.Bingo.PushMessage.SendMessage(student.F_Name + "家长您好，您的孩子于" + param.F_Time + " " + inout + "校，请知晓。", parent?.F_Id, parent?.F_RealName);
                        body = new
                        {
                            name = student.F_Name,
                            headIcon = student.F_FacePic_File,
                            studentNo = student.F_StudentNum,
                            result = AttendanceRulesApp.makeAttendanceLogs4EG(student, student.F_Class_ID, param.F_Sn, param.F_Course_PrepareID, param.F_Time, param.F_Pos)
                        };
                        return Success(body);
                    }
                }
                else
                {
                    throw new Exception("参数错误");
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionContext("1101", ex.Message);
            }
        }

        public static void SetCurrentOperator(string schoolCode)
        {
            if (string.IsNullOrEmpty(schoolCode)) throw new Exception("学校代码不能为空!");
            OperatorProvider.Set(new OperatorModel { SchoolCode = schoolCode });
        }
    }
}