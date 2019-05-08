/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :School_Class_PTimetableMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-07-23 13:33:46
***************************************************************
* 修改履历    :
* 修改编号    :
*--------------------------------------------------------------
* 修改日期    :YYYY/MM/DD
* 修 改 者    :XXXXXX
* 修改内容    :
***************************************************************
*/

using NFine.Code;
using NFine.Web.Areas.SchoolManage;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class School_Teacher_TimetableController : ControllerBase
    {
        private School_Schedules_App app = new School_Schedules_App();
        private School_Class_Info_Teacher_App scitApp = new School_Class_Info_Teacher_App();

        public ActionResult GetTeacherTimetable(string F_Semester, string F_Year, string F_Teacher, string startDate, string endDate)
        {
            var datas = app.GetTeacherTimetable(F_Semester, F_Year, F_Teacher, startDate, endDate);
            return Content(datas.ToJson());
        }

        public ActionResult SubmitForm(string F_Semester, string F_Teacher, string F_Class, string targetClass, string sourceDay, string sourceLesson, string setType, string targetDay, string targetLesson)
        {
            string newCourse = null;
            if (setType == "1")
            {
                newCourse = scitApp.GetTeacherCourse(F_Teacher, targetClass);
                F_Class = targetClass;
            }
            app.AddOrUpdCourse(F_Semester, F_Class, sourceDay, sourceLesson, setType, targetDay, targetLesson, newCourse);
            return Success("操作成功。");
        }
    }
}