using NFine.Application.ScheduleManage;
using NFine.Code;
using NFine.Domain.Entity.ScheduleManage;
using NFine.Web.Areas.SchoolManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class School_PRule_TeacherController : ControllerBase
    {
        public class SubmitModel
        {
            public int value { get; set; }
            public int dateIndex { get; set; }
            public int lessonIndex { get; set; }
        }

        private School_PRule_Teacher_App ruleApp = new School_PRule_Teacher_App();
        private School_PRule_TeacherTime_App teacherTimeruleApp = new School_PRule_TeacherTime_App();
        private School_Teachers_App teacherApp = new School_Teachers_App();

        public ActionResult TeacherRule()
        {
            return View();
        }

        public ActionResult GeTeacherRule(string F_Teacher, string F_Course, string F_Divis, string F_Year, string F_Class, string F_Semester)
        {
            var data = ruleApp.GetList(F_Teacher, F_Course, F_Divis, F_Year, F_Class, F_Semester);
            return Content(data.ToJson());
        }

        public ActionResult SetTeacherRule(string F_Teacher, string F_Course, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, List<SubmitModel> data)
        {
            SetRule(3, F_Teacher, F_Course, F_Divis, F_Grade, F_Year, F_Class, F_Semester, data);
            return Success("操作成功。");
        }

        public void SetRule(int F_Type, string F_Teacher, string F_Course, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, List<SubmitModel> datas)
        {
            ruleApp.DeleteForm(F_Teacher, F_Year, F_Semester, F_Grade, F_Course);
            if (datas == null)
                return;
            foreach (var data in datas)
            {
                Schedule_PRule_Teacher_Entity model = new Schedule_PRule_Teacher_Entity();
                model.Create();
                //model.F_Type = F_Type;
                model.F_Year = F_Year;
                model.F_SemesterId = F_Semester;
                model.F_GradeId = F_Grade;
                model.F_ClassId = F_Class;
                model.F_CourseId = F_Course;
                model.F_TeacherId = F_Teacher;

                model.F_WeekN = data.dateIndex;
                model.F_CourseIndex = data.lessonIndex;
                model.F_Rule = data.value;
                ruleApp.AddEntity(model);
            }
        }

        public ActionResult TeacherCourseTimeRule()
        {
            return View();
        }

        public ActionResult GetTeacherCourseTimeRule(Pagination pagination, string F_Teacher, string F_Divis, string F_Year, string F_Semester)
        {
            var data = new
            {
                rows = teacherTimeruleApp.GetList(pagination, F_Teacher, F_Divis, F_Year, F_Semester)
                .Select(t =>
                {
                    //var course = new School_Course_App().GetForm(t.F_CourseId);
                    var teacher = teacherApp.GetForm(t.F_TeacherId);
                    return new
                    {
                        //F_CourseId = t.F_CourseId,
                        //F_CourseName = course == null ? "" : course.F_Name,
                        F_Day1CourseTime = t.F_Day1CourseTime,
                        F_Day2CourseTime = t.F_Day2CourseTime,
                        F_Day3CourseTime = t.F_Day3CourseTime,
                        F_Day4CourseTime = t.F_Day4CourseTime,
                        F_Day5CourseTime = t.F_Day5CourseTime,
                        F_Day6CourseTime = t.F_Day6CourseTime,
                        F_Day7CourseTime = t.F_Day7CourseTime,
                        F_Divis_ID = t.F_Divis_ID,
                        //F_GradeId = t.F_GradeId,
                        F_Id = t.F_Id,
                        F_SemesterId = t.F_SemesterId,
                        F_TeacherId = t.F_TeacherId,
                        F_TeacherName = teacher == null ? "" : teacher.F_Name,
                        F_Year = t.F_Year
                    };
                }),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = teacherTimeruleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        public ActionResult UpdForm()
        {
            return View();
        }

        public ActionResult CopyForm()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Schedule_PRule_TeacherTime_Entity entity, string keyValue)
        {
            teacherTimeruleApp.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            teacherTimeruleApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        public ActionResult SubmitCopy(string F_Id, string F_DTeacherIds, string F_Divis, string F_Year, string F_Semester)
        {
            string[] F_DTeachers = F_DTeacherIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            teacherTimeruleApp.SubmitCopy(F_Id, F_DTeachers, F_Divis, F_Year, F_Semester);
            return Success("操作成功。");
        }
    }
}