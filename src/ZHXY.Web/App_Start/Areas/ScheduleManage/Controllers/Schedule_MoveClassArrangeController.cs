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
    public class Schedule_MoveClassArrangeController : ControllerBase
    {
        public class CellValueModel
        {
            public string TeacherId { get; set; }
            public string CourseId { get; set; }
            public string ClassId { get; set; }
        }

        public class SubmitModel
        {
            public string classId { get; set; }
            public string teacherId { get; set; }
            public string courseId { get; set; }
            public int dateIndex { get; set; }
            public int lessonIndex { get; set; }
        }

        private School_MoveClass_PTimetable_App app = new School_MoveClass_PTimetable_App();
        private School_PRule_Weeks_App pruleweekapp = new School_PRule_Weeks_App();
        private School_ArrangeCourse_App arrangeCourseapp = new School_ArrangeCourse_App();
        private School_Classroom_App classRoomApp = new School_Classroom_App();

        public ActionResult GetArrangeTip(Pagination pagination, string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = app.GetAllRule(F_Divis, F_Grade, F_Year, F_Semester);
            return Content(data.ToJson());
        }

        public ActionResult SubmitClassTimeTable(string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, List<SubmitModel> data)
        {
            new School_Class_PTimetable_App().Delete(t => t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_GradeId == F_Grade && t.F_IsMoveCourse == true);
            if (data == null)
                data = new List<SubmitModel>();
            List<Schedule_Class_PTimetable_Entity> list = new List<Schedule_Class_PTimetable_Entity>();
            data.ForEach(p =>
            {
                var room = arrangeCourseapp.GetMoveClass(p.dateIndex, p.lessonIndex, F_Year, F_Semester);
                var model = new Schedule_Class_PTimetable_Entity
                {
                    F_Year = F_Year,
                    F_SemesterId = F_Semester,
                    F_GradeId = F_Grade,
                    F_Divis_ID = F_Divis,
                    F_ClassId = p.classId,
                    F_CourseId = p.courseId,
                    F_CourseIndex = p.lessonIndex,
                    F_TeacherId = p.teacherId,
                    F_WeekN = p.dateIndex,
                    F_IsMoveCourse = true,
                    F_ClassRoomId = room == null ? null : room.F_Id
                };
                model.Create();
                list.Add(model);
            });

            new School_Class_PTimetable_App().AddClassTimeTable(list);
            return Success("操作成功。");
        }

        public ActionResult cellValidate(int dateIndex, int lessonIndex, string courseid, string teacherId, string F_Year, string F_Semester, string F_Grade, string F_Class, string cellClass)
        {
            var data = arrangeCourseapp.Validate(dateIndex, lessonIndex, teacherId, courseid, F_Year, F_Semester, F_Grade, F_Class, 11);
            if (data != null)
                return Error("周" + dateIndex + ",第" + lessonIndex + "节," + data);
            if (!string.IsNullOrEmpty(cellClass))
            {
                var classIds = cellClass.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var students = new Schedule_MoveClassStudent_App().GetList(t => classIds.Contains(t.F_MoveClassId));
                students.AddRange(new Schedule_MoveClassStudent_App().GetList(t => t.F_MoveClassId == F_Class));
                var rStudentsCount = students.GroupBy(t => t.F_StudentId).Where(t => t.Count() > 1).Count();
                if (rStudentsCount > 0)
                    return Error("周" + dateIndex + ",第" + lessonIndex + "节,学生时间冲突！");
            }
            var room = arrangeCourseapp.GetMoveClass(dateIndex, lessonIndex, F_Year, F_Semester);
            if (room == null)
                return Error("周" + dateIndex + ",第" + lessonIndex + "节,无可安排的走班教室！");
            return Success("操作成功。");
        }

        //public ActionResult pickUpcellValidate(string courseid, string teacherId, string F_Year, string F_Semester, string F_Grade, string F_Class)
        //{ List<object> objs = new List<object>();
        //    for (int i = 0; i < 7; i++)
        //    {
        //        for (int x = 0; x < 12; x++)
        //        {
        //            string state = "error";
        //            var data =arrangeCourseapp. Validate(i+1, x+1, teacherId, courseid, F_Year, F_Semester, F_Grade, F_Class);
        //            if (data == null)
        //                state = "success";
        //            objs.Add(new
        //            {
        //                lessonIndex = x+1,
        //                dateIndex = i+1,
        //                state = state
        //            });
        //        }
        //    }
        //    return Content( objs.ToJson());
        //}

        //获取班级课表

        public ActionResult GetClassPTimetable(Pagination pagination, string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            ////单双周课程设置
            //var weeksRule = pruleweekapp.GetList(t => t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_ClassId == F_Class);

            var data = app.GetMoveClassPTimetable(F_Divis, F_Grade, F_Year, F_Semester)
                .Select(t =>
                {
                    var teacher = new School_Teachers_App().GetForm(t.F_TeacherId);
                    var course = new School_Course_App().GetForm(t.F_CourseId);
                    //var moveClass = new Schedule_MoveClass_App().GetForm(t.F_ClassId);
                    string courseName = course == null ? "" : course.F_Name;
                    var room = classRoomApp.GetForm(t.F_ClassRoomId);
                    //courseName = moveClass.F_Name + "_" + courseName + "_" + (room == null ? "未安排" : room.F_Name);
                    courseName = courseName + "_" + (room == null ? "未安排" : room.F_Name);

                    //var weekdata = weeksRule.Where(p => p.F_Course1Id == t.F_CourseId).FirstOrDefault();
                    //if (weekdata != null)
                    //{
                    //    var course2 = new School_Course_App().GetForm(weekdata.F_Course2Id);
                    //    courseName = "单周" + courseName + "/双周" + (course2 == null ? "" : course2.F_Name);
                    //}
                    return new
                    {
                        F_ClassId = t.F_ClassId,
                        F_CourseId = new CellValueModel { ClassId = t.F_ClassId, CourseId = t.F_CourseId, TeacherId = t.F_TeacherId }.ToJson(),
                        F_CourseIndex = t.F_CourseIndex,
                        F_Divis_ID = t.F_Divis_ID,
                        F_GradeId = t.F_GradeId,
                        F_SemesterId = t.F_SemesterId,
                        F_TeacherId = t.F_TeacherId,
                        F_WeekN = t.F_WeekN,
                        F_Year = t.F_Year,
                        F_Teacher = teacher == null ? "" : teacher.F_Name,
                        F_CourseName = courseName
                    };
                });
            return Content(data.ToJson());
        }
    }
}