using NFine.Application.ScheduleManage;
using NFine.Application.SchoolManage;
using NFine.Code;
using NFine.Domain.Entity.ScheduleManage;
using NFine.Domain.Entity.SchoolManage;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_WithCourseResultController : ControllerBase
    {
        private Schedule_WishCourseGroup_App app = new Schedule_WishCourseGroup_App();
        private School_Students_App studentApp = new School_Students_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            var model = new
            {
                F_Year = data.Schedule_WishCourseTask_Entity.F_Year,
                F_SemesterId = data.Schedule_WishCourseTask_Entity.F_SemesterId,
                F_DivisId = data.Schedule_WishCourseTask_Entity.F_DivisId,
                F_GradeId = data.Schedule_WishCourseTask_Entity.F_GradeId,

                F_CourseGroups = data.F_TaskCourseGroupID,
                F_ClassId = data.School_Students_Entity.F_Class_ID,

                F_TaskId = data.F_TaskId,
                F_StudentId = data.F_StudentID
            };
            return Content(model.ToJson());
        }

        // 学生选科情况
        public ActionResult GetStudentCourseGridJson(Pagination pagination, string F_TaskId, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = app.GetStudentCourseList(pagination, F_TaskId);
            return Content(data.ToJson());
        }

        // 未选科学生
        public ActionResult GetStudentGridJson(string F_GradeId, string F_ClassId, string F_TaskId, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var data = app.GetForm(keyValue);
                return Content(studentApp.GetList(t => t.F_Id == data.F_StudentID).ToJson());
            }
            var expression = ExtLinq.True<Student>();
            if (!string.IsNullOrEmpty(F_GradeId))
            {
                expression = expression.And(t => t.F_Grade_ID == F_GradeId);
            }

            if (!string.IsNullOrEmpty(F_ClassId))
            {
                expression = expression.And(t => t.F_Class_ID == F_ClassId);
            }
            var students = studentApp.GetList(expression);
            var datas = app.GetList(t => t.F_TaskId == F_TaskId);
            datas.ForEach(p => students.RemoveAll(t => t.F_Id == p.F_StudentID));
            return Content(students.ToJson());
        }

        public ActionResult SubmitForm(string F_StudentID, string F_TaskCourseGroupID, string F_TaskId, string keyValue)
        {
            Schedule_WishCourseGroup_Entity entity = new Schedule_WishCourseGroup_Entity();
            entity.F_StudentID = F_StudentID;
            entity.F_TaskCourseGroupID = F_TaskCourseGroupID;
            entity.F_TaskId = F_TaskId;
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        //发布选科结果
        public ActionResult PublishForm()
        {
            return View();
        }
    }
}