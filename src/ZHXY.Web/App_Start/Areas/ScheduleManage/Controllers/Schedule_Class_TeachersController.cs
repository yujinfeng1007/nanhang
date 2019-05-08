using NFine.Application.ScheduleManage;
using NFine.Application.SchoolManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.ScheduleManage;
using NFine.Domain.Entity.SchoolManage;
using NFine.Web.Areas.SchoolManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_Class_TeachersController : ControllerBase
    {
        private School_Class_Info_App classInfoApp = new School_Class_Info_App();
        private School_Students_App studentsApp = new School_Students_App();
        private Schedule_MoveClass_App moveClassapp = new Schedule_MoveClass_App();
        private Schedule_MoveClassStudent_App moveClassStuentapp = new Schedule_MoveClassStudent_App();
        private School_Teachers_App teacherApp = new School_Teachers_App();

        private OrganizeApp organizeApp = new OrganizeApp();
        private Schedule_WishCourseGroup_App studentCGApp = new Schedule_WishCourseGroup_App();

        public ActionResult Teachers()
        {
            return View();
        }

        // 查询当前年级下所有走班班级
        private List<ClassInfo> GetClassByGrade(string F_Grade)
        {
            List<ClassInfo> datas = new List<ClassInfo>();
            // 查询所有班级
            var gclass = organizeApp.GetListByCategory("Class", F_Grade);
            gclass.ForEach(t =>
            {
                var classInfo = classInfoApp.GetForm(t.F_Id);
                if (classInfo != null && ((classInfo.F_IsMoveClass ?? false) && classInfo.F_CourseCount != 3))
                    datas.Add(classInfo);
            });
            return datas;
        }

        private bool IsMoveClass(ClassInfo gclass, string courseName, string courseParentId)
        {
            if (!gclass.F_Courses.Contains(courseParentId))
            {
                if (gclass.F_MoveCourse.Contains(courseParentId))
                {
                    if (courseName.Contains("A"))
                        return true;
                    return false;
                }
                else if (courseName.Contains("B"))
                {
                    return true;
                }
            }
            return false;
        }

        private List<Student> GetClassStudent(List<ClassInfo> gclasslist)
        {
            List<Student> students = new List<Student>();
            foreach (var classInfo in gclasslist)
            {
                students.AddRange(studentsApp.GetListByF_Class_ID(classInfo.F_ClassID));
            }
            return students.Distinct().ToList();
        }

        public ActionResult GetGridJson(string ClassId)
        {
            List<object> objs = new List<object>();
            var orgentity = organizeApp.GetForm(ClassId);
            // 查询所有课目
            var courses = new School_Grade_Course_App().GetList(t => t.F_Grade.Equals(orgentity.F_ParentId) && t.School_Course_Entity.F_CourseType == "4");
            // 查询所有走班班级
            var gclasslist = GetClassByGrade(orgentity.F_ParentId);
            // 当前年级所有已选科的学生
            var courseStudents = GetClassStudent(gclasslist);
            // 所有
            foreach (var course in courses)
            {
                var sutdents = new List<Student>(courseStudents.ToArray()); // copy of t

                string classNames = "", classIds = "";
                foreach (var gclass in gclasslist)
                {
                    // 当前班级包括走班科目且当前科目为科目A，或者组合科目不包括当前科目
                    var ismoveClass = IsMoveClass(gclass, course.School_Course_Entity.F_Name, course.School_Course_Entity.F_ParentId);
                    if (ismoveClass)
                    {
                        classNames += gclass.F_Name + ",";
                        classIds += gclass.F_ClassID + ",";
                    }
                    else
                    {
                        sutdents.RemoveAll(p => p.F_Class_ID == gclass.F_ClassID);
                    }
                }
                classIds = classIds.TrimEnd(',');
                string teachNames = "";
                var moveClass = moveClassapp.GetList(t => t.F_ClassIds == classIds && t.F_CourseId == course.F_CourseId);
                moveClass.Select(q => q.F_TeacherId).Distinct().ToList().ForEach(
                   p =>
                   {
                       var teacher = teacherApp.GetForm(p);
                       teachNames += teacher.F_Name + ",";
                   });

                objs.Add(new
                {
                    F_Id = course.F_CourseId,
                    F_CourseId = course.F_CourseId,
                    F_ClassNames = classNames.TrimEnd(','),
                    F_ClassIds = classIds,
                    F_Students = sutdents.Count,
                    F_ClassQty = moveClass.Count,
                    F_TeacherNames = teachNames.TrimEnd(',')
                });
            }
            return Content(objs.ToJson());
        }

        public ActionResult GetSelectJson(string keyValue)
        {
            var data = new School_Course_Teacher_App().GetSelect(keyValue);

            List<object> list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.School_Teachers_Entity.F_Id, text = item.School_Teachers_Entity.F_Name });
            }
            return Content(list.ToJson());
        }

        public ActionResult GetFormJson(string keyValue, string F_ClassIds)
        {
            var moveClass = moveClassapp.GetList(t => t.F_ClassIds == F_ClassIds && t.F_CourseId == keyValue);
            string teacherIds = string.Join(",", moveClass.Select(q => q.F_TeacherId).Distinct().ToList());
            var data = new
            {
                F_ClassQTY = moveClass.Count,
                F_Teacher_ID = teacherIds,
                F_SemesterId = moveClass.Count > 0 ? moveClass[0].F_SemesterId : null,
                F_CourseTime = moveClass.Count > 0 ? moveClass[0].F_CourseTime : null,
                F_RepeatCount = moveClass.Count > 0 ? moveClass[0].F_RepeatCount : null
            };
            return Content(data.ToJson());
        }

        public ActionResult Submit(string ClassId, string F_SemesterId, string F_CourseId, string F_ClassIds, string F_TeacherIds, int F_ClassQTY, int F_RepeatCount, int F_CourseTime)
        {
            string[] classIds = F_ClassIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (classIds.Length <= 0)
                return Error("走班班级为空!");
            var orgentity = organizeApp.GetForm(ClassId);
            var F_Grade = organizeApp.GetForm(orgentity.F_ParentId); ;
            var F_DivisId = F_Grade.F_ParentId;

            moveClassapp.Delete(F_CourseId, F_ClassIds);
            var course = (Course)CacheConfig.GetSchoolCourseByCache()[F_CourseId];

            var students = GetMoveClassStudents(F_Grade.F_Id, course.F_Name, course.F_ParentId);

            string[] teachers = F_TeacherIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < F_ClassQTY; i++)
            {
                var moveclass = new Schedule_MoveClass_Entity
                {
                    F_Year = F_Grade.F_Year.ToString(),
                    F_SemesterId = F_SemesterId,
                    F_DivisId = F_DivisId,
                    F_GradeId = F_Grade.F_Id,
                    F_Name = course.F_Name + (i + 1),
                    F_CourseId = F_CourseId,
                    F_TeacherId = teachers[(int)(((float)teachers.Length / F_ClassQTY) * i)],
                    F_ClassIds = F_ClassIds,
                    F_ParentCourseId = course.F_ParentId,
                    F_CourseTime = F_CourseTime,
                    F_RepeatCount = F_RepeatCount
                };
                moveClassapp.SubmitForm(moveclass, null);

                int studeltmaxQTY = (int)Math.Ceiling((float)students.Count / F_ClassQTY);
                int studeltQTY = students.Count >= (studeltmaxQTY * (i + 1)) ? studeltmaxQTY : (students.Count - studeltmaxQTY * i);
                var classStudents = students.GetRange(studeltmaxQTY * i, studeltQTY);
                AddClassStudent(classStudents, moveclass.F_Id);
            }
            return Success("操作成功!");
        }

        private List<Student> GetMoveClassStudents(string F_GradeId, string courseName, string courseParentId)
        {
            // 查询所有走班班级
            var gclasslist = GetClassByGrade(F_GradeId);
            // 当前年级所有已选科的学生
            var courseStudents = GetClassStudent(gclasslist);

            // 所有
            var sutdents = new List<Student>(courseStudents.ToArray()); // copy of t
            foreach (var gclass in gclasslist)
            {
                // 当前班级包括走班科目且当前科目为科目A，或者组合科目不包括当前科目
                var ismoveClass = IsMoveClass(gclass, courseName, courseParentId);
                if (!ismoveClass)
                {
                    sutdents.RemoveAll(p => p.F_Class_ID == gclass.F_ClassID);
                }
            }
            return sutdents;
        }

        private void AddClassStudent(List<Student> students, string moveclassId)
        {
            for (int i = 0; i < students.Count; i++)
            {
                moveClassStuentapp.SubmitForm(new Schedule_MoveClassStudent_Entity
                {
                    F_MoveClassId = moveclassId,
                    F_StudentId = students[i].F_Id,
                }, null);
            }
        }
    }
}