using NFine.Application.ScheduleManage;
using NFine.Code;
using NFine.Domain.Entity.ScheduleManage;
using NFine.Web.Areas.SchoolManage;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class School_PRule_CourseController : ControllerBase
    {
        public class SubmitModel
        {
            public int value { get; set; }
            public int dateIndex { get; set; }
            public int lessonIndex { get; set; }
        }

        private School_PRule_Course_App ruleApp = new School_PRule_Course_App();
        private School_PRule_TeacherTime_App teacherTimeruleApp = new School_PRule_TeacherTime_App();
        private School_Teachers_App teacherApp = new School_Teachers_App();

        public ActionResult CourseRule()
        {
            return View();
        }

        public ActionResult GeCourseRule(string F_Course, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester)
        {
            var data = ruleApp.GetList(F_Course, F_Divis, F_Grade, F_Year, F_Class, F_Semester);
            return Content(data.ToJson());
        }

        public ActionResult SetCourseRule(string F_Course, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, List<SubmitModel> data)
        {
            SetRule(2, null, F_Course, F_Divis, F_Grade, F_Year, F_Class, F_Semester, data);
            return Success("操作成功。");
        }

        public void SetRule(int F_Type, string F_Teacher, string F_Course, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, List<SubmitModel> datas)
        {
            ruleApp.DeleteForm(F_Year, F_Semester, F_Grade, F_Course);
            if (datas == null)
                return;
            foreach (var data in datas)
            {
                Schedule_PRule_Course_Entity model = new Schedule_PRule_Course_Entity();
                model.Create();
                //model.F_Type = F_Type;
                model.F_Year = F_Year;
                model.F_SemesterId = F_Semester;
                model.F_GradeId = F_Grade;
                model.F_ClassId = F_Class;
                model.F_CourseId = F_Course;
                //model.F_TeacherId = F_Teacher;

                model.F_WeekN = data.dateIndex;
                model.F_CourseIndex = data.lessonIndex;
                model.F_Rule = data.value;
                ruleApp.AddEntity(model);
            }
        }
    }
}