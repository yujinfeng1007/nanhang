using NFine.Application.ScheduleManage;
using NFine.Application.SchoolManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.ScheduleManage;
using NFine.Web.Areas.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using static NFine.Code.Excel.NPOIExcel;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_TimetableController : ControllerBase
    {
        private School_Class_PTimetable_App classTimetableApp = new School_Class_PTimetable_App();
        private School_MoveClass_PTimetable_App moveClassTimetableApp = new School_MoveClass_PTimetable_App();
        private School_ArrangeCourse_App arrangeApp = new School_ArrangeCourse_App();
        private School_Classroom_App classRoomApp = new School_Classroom_App();
        private School_Students_App studentApp = new School_Students_App();
        private Schedule_WishCourseGroup_App wishCourseGroupApp = new Schedule_WishCourseGroup_App();

        #region 获取班级课表

        public ActionResult ClassTimetable()
        {
            return View();
        }

        public ActionResult GetClassPTimetable(string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester)
        {
            var datas = classTimetableApp.GetClassPTimetable(F_Divis, F_Grade, F_Year, F_Class, F_Semester);
            var movedatas = moveClassTimetableApp.GetClassMovePTimetable(F_Divis, F_Grade, F_Year, F_Semester, F_Class);
            datas.AddRange(movedatas);
            var objs = datas.Select(t =>
              {
                  var teacher = new School_Teachers_App().GetForm(t.F_TeacherId);
                  var course = new School_Course_App().GetForm(t.F_CourseId);
                  string courseName = course == null ? "" : course.F_Name;

                  if (t.F_IsMoveCourse == true)
                  {
                      //var moveClass = new Schedule_MoveClass_App().GetForm(t.F_ClassId);
                      var room = classRoomApp.GetList(p => p.F_Id == t.F_ClassRoomId).FirstOrDefault();
                      //courseName = moveClass.F_Name + "_" + courseName+"_"+(room==null?"未安排":room.F_Name);
                      courseName = courseName + "_" + (room == null ? "未安排" : room.F_Name);
                  }
                  else
                  {
                      string courseNameFormat = arrangeApp.FormatCourseName(F_Class, F_Semester, F_Year, t.F_CourseId);
                      courseName = string.Format(courseNameFormat, courseName);
                  }
                  return new
                  {
                      F_ClassId = t.F_ClassId,
                      F_CourseId = t.F_CourseId,
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
            return Content(objs.ToJson());
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            Dictionary<string, object> orgs = CacheConfig.GetOrganizeListByCache();
            var datas = classTimetableApp.GetClassPTimetable(F_Divis, F_Grade, F_Year, F_Class, F_Semester);
            var movedatas = moveClassTimetableApp.GetClassMovePTimetable(F_Divis, F_Grade, F_Year, F_Semester, F_Class);
            datas.AddRange(movedatas);
            var gdata = datas.OrderBy(t => t.F_WeekN).ThenBy(t => t.F_CourseIndex).ToList();//.GroupBy(t => t.F_ClassId);
            //List<SheetItem> sheetItems = new List<SheetItem>();
            //foreach (var items in gdata)
            //{
                var item = gdata.FirstOrDefault();
                string className = GetPropertyValue(orgs[item.F_ClassId], "fullname").ToString();
                string title = "";
                title += item.F_Year;
                //title += item.F_SemesterId;
                title += GetPropertyValue(orgs[item.F_Divis_ID], "fullname");
                title += GetPropertyValue(orgs[item.F_GradeId], "fullname");
                title += className;
                title += "共" + gdata.Count() + "节";
                DataTable dt = ToExportDT(gdata);
            //    sheetItems.Add(new SheetItem { SheetName = className, Title = title, dt = dt });
            //}

            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        private DataTable ToExportDT(List<Schedule_Class_PTimetable_Entity> items)
        {
            if (items == null || items.Count <= 0)
                return null;
            DataTable dt = new DataTable();
            dt.Columns.Add("第几节", Type.GetType("System.String"));
            dt.Columns.Add("周1", Type.GetType("System.String"));
            dt.Columns.Add("周2", Type.GetType("System.String"));
            dt.Columns.Add("周3", Type.GetType("System.String"));
            dt.Columns.Add("周4", Type.GetType("System.String"));
            dt.Columns.Add("周5", Type.GetType("System.String"));
            dt.Columns.Add("周6", Type.GetType("System.String"));
            dt.Columns.Add("周7", Type.GetType("System.String"));

            var item = items.FirstOrDefault();
            for (int i = 1; i <= 12; i++)
            {
                DataRow dr = dt.NewRow();
                dr["第几节"] = "第" + (i) + "节";
                dt.Rows.Add(dr);

                var indexitems = items.Where(t => t.F_CourseIndex == i).ToList();
                foreach (var dayindexitem in indexitems)
                {
                    //var teacher = new School_Teachers_App().GetForm(dayindexitem.F_TeacherId);
                    var course = new School_Course_App().GetForm(dayindexitem.F_CourseId);
                    string text = (course == null ? "" : course.F_Name);// + "(" + (teacher == null ? "" : teacher.F_Name) + ")";
                    dr["周" + dayindexitem.F_WeekN] +=(string.IsNullOrEmpty(dr["周" + dayindexitem.F_WeekN].ToString())?"": ",")+text;
                }
            }
            return dt;
        }

        #endregion 获取班级课表

        #region 学生课表

        public ActionResult StudentTimetable()
        {
            return View();
        }

        public ActionResult GetClassStudents(string F_Divis, string F_Grade, string F_Year, string F_ClassId, string F_Semester)
        {
            var datas = studentApp.GetListByF_Class_ID(F_ClassId);
            var models = datas.Select(t =>
            {
                var courseGroup = wishCourseGroupApp.GetList(p => p.F_StudentID == t.F_Id
                  && p.Schedule_WishCourseTask_Entity.F_Year == F_Year
                  && p.Schedule_WishCourseTask_Entity.F_GradeId == F_Grade
                  && p.Schedule_WishCourseTask_Entity.F_SemesterId == F_Semester).FirstOrDefault();
                var Courses = courseGroup == null ? "" : courseGroup.Schedule_WCTask_Group_Entity.Schedule_CourseGroup_Entity.F_Courses;
                return new
                {
                    t.F_Id,
                    F_Name = t.F_Name,
                    F_CourseGroup = Courses
                };
            });
            return Content(models.ToJson());
        }

        public ActionResult GetStudentPTimetable(string F_Divis, string F_Grade, string F_Year, string StudentId, string F_Semester)
        {
            var student = studentApp.GetForm(StudentId);
            var datas = classTimetableApp.GetClassPTimetable(F_Divis, F_Grade, F_Year, student.F_Class_ID, F_Semester);
            var movedatas = moveClassTimetableApp.GetStudentMovePTimetable(F_Divis, F_Grade, F_Year, F_Semester, StudentId);
            datas.AddRange(movedatas);
            var objs = datas.Select(t =>
            {
                var teacher = new School_Teachers_App().GetForm(t.F_TeacherId);
                var course = new School_Course_App().GetForm(t.F_CourseId);
                string courseName = course == null ? "" : course.F_Name;

                if (t.F_IsMoveCourse == true)
                {
                    //var moveClass = new Schedule_MoveClass_App().GetForm(t.F_ClassId);
                    var room = classRoomApp.GetList(p => p.F_Id == t.F_ClassRoomId).FirstOrDefault();
                    //courseName = moveClass.F_Name + "_" + courseName+"_"+(room==null?"未安排":room.F_Name);
                    courseName = courseName + "_" + (room == null ? "未安排" : room.F_Name);
                }
                else
                {
                    string courseNameFormat = arrangeApp.FormatCourseName(t.F_ClassId, F_Semester, F_Year, t.F_CourseId);
                    courseName = string.Format(courseNameFormat, courseName);
                }
                return new
                {
                    F_ClassId = t.F_ClassId,
                    F_CourseId = t.F_CourseId,
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
            return Content(objs.ToJson());
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult exportStudent(string F_Divis, string F_Grade, string F_Year, string StudentId, string F_Semester)
        {
            Dictionary<string, object> orgs = CacheConfig.GetOrganizeListByCache();

            var student = studentApp.GetForm(StudentId);
            var datas = classTimetableApp.GetClassPTimetable(F_Divis, F_Grade, F_Year, student.F_Class_ID, F_Semester);
            var movedatas = moveClassTimetableApp.GetStudentMovePTimetable(F_Divis, F_Grade, F_Year, F_Semester, StudentId);
            datas.AddRange(movedatas);
            var gdata = datas.OrderBy(t => t.F_WeekN).ThenBy(t => t.F_CourseIndex).ToList();//.GroupBy(t => t.F_ClassId);                                                                                  //List<SheetItem> sheetItems = new List<SheetItem>();                                                                   //foreach (var items in gdata)
                                                                                            //{
            var item = gdata.FirstOrDefault();
            string className = GetPropertyValue(orgs[item.F_ClassId], "fullname").ToString();
            string title = "";
            title += item.F_Year;
            //title += item.F_SemesterId;
            title += GetPropertyValue(orgs[item.F_Divis_ID], "fullname");
            title += GetPropertyValue(orgs[item.F_GradeId], "fullname");
            title += className;
            title += "共" + gdata.Count() + "节";
            DataTable dt = ToExportDT(gdata);
            //    sheetItems.Add(new SheetItem { SheetName = className, Title = title, dt = dt });
            //}

            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        #endregion 学生课表

        #region 教师课表
        public ActionResult TeacherTimetable()
        {
            return View();
        }

        public ActionResult GetTeachers(string F_Divis, string F_Grade, string F_Year, string F_ClassId, string F_Semester)
        {
            var datas = studentApp.GetListByF_Class_ID(F_ClassId);
            var models = datas.Select(t =>
            {
                var courseGroup = wishCourseGroupApp.GetList(p => p.F_StudentID == t.F_Id
                  && p.Schedule_WishCourseTask_Entity.F_Year == F_Year
                  && p.Schedule_WishCourseTask_Entity.F_GradeId == F_Grade
                  && p.Schedule_WishCourseTask_Entity.F_SemesterId == F_Semester).FirstOrDefault();
                var Courses = courseGroup == null ? "" : courseGroup.Schedule_WCTask_Group_Entity.Schedule_CourseGroup_Entity.F_Courses;
                return new
                {
                    t.F_Id,
                    F_Name = t.F_Name,
                    F_CourseGroup = Courses
                };
            });
            return Content(models.ToJson());
        }

        public ActionResult GetTeacherPTimetable(string F_Divis,  string F_Year, string TeacherId, string F_Semester)
        {
            var datas = classTimetableApp.GetList(t=>t.F_Divis_ID==F_Divis&&t.F_Year==F_Year&&t.F_SemesterId== F_Semester&&t.F_TeacherId== TeacherId);
            var objs = datas.Select(t =>
            {
                var teacher = new School_Teachers_App().GetForm(t.F_TeacherId);
                var course = new School_Course_App().GetForm(t.F_CourseId);
                string courseName = course == null ? "" : course.F_Name;

                if (t.F_IsMoveCourse == true)
                {
                    //var moveClass = new Schedule_MoveClass_App().GetForm(t.F_ClassId);
                    var room = classRoomApp.GetList(p => p.F_Id == t.F_ClassRoomId).FirstOrDefault();
                    //courseName = moveClass.F_Name + "_" + courseName+"_"+(room==null?"未安排":room.F_Name);
                    courseName = courseName + "_" + (room == null ? "未安排" : room.F_Name);
                }
                else
                {
                    string courseNameFormat = arrangeApp.FormatCourseName(t.F_ClassId, F_Semester, F_Year, t.F_CourseId);
                    courseName = string.Format(courseNameFormat, courseName);
                }
                return new
                {
                    F_ClassId = t.F_ClassId,
                    F_CourseId = t.F_CourseId,
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
            return Content(objs.ToJson());
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult exportTeacher(string F_Divis, string F_Year, string TeacherId, string F_Semester)
        {
            Dictionary<string, object> orgs = CacheConfig.GetOrganizeListByCache();

            var datas = classTimetableApp.GetList(t => t.F_Divis_ID == F_Divis && t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_TeacherId == TeacherId);

            var gdata = datas.OrderBy(t => t.F_WeekN).ThenBy(t => t.F_CourseIndex).ToList();//.GroupBy(t => t.F_ClassId);                                                                                  //List<SheetItem> sheetItems = new List<SheetItem>();                                                                   //foreach (var items in gdata)
                                                                                            //{
            var item = gdata.FirstOrDefault();
            string className = GetPropertyValue(orgs[item.F_ClassId], "fullname").ToString();
            string title = "";
            title += item.F_Year;
            //title += item.F_SemesterId;
            title += GetPropertyValue(orgs[item.F_Divis_ID], "fullname");
            title += GetPropertyValue(orgs[item.F_GradeId], "fullname");
            title += className;
            title += "共" + gdata.Count() + "节";
            DataTable dt = ToExportDT(gdata);
            //    sheetItems.Add(new SheetItem { SheetName = className, Title = title, dt = dt });
            //}

            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        #endregion

        #region 教室课表
        public ActionResult ClassRoomTimetable()
        {
            return View();
        }

        public ActionResult GetClassRooms(string F_Divis, string F_Grade, string F_Year, string F_ClassId, string F_Semester)
        {
            var datas = studentApp.GetListByF_Class_ID(F_ClassId);
            var models = datas.Select(t =>
            {
                var courseGroup = wishCourseGroupApp.GetList(p => p.F_StudentID == t.F_Id
                  && p.Schedule_WishCourseTask_Entity.F_Year == F_Year
                  && p.Schedule_WishCourseTask_Entity.F_GradeId == F_Grade
                  && p.Schedule_WishCourseTask_Entity.F_SemesterId == F_Semester).FirstOrDefault();
                var Courses = courseGroup == null ? "" : courseGroup.Schedule_WCTask_Group_Entity.Schedule_CourseGroup_Entity.F_Courses;
                return new
                {
                    t.F_Id,
                    F_Name = t.F_Name,
                    F_CourseGroup = Courses
                };
            });
            return Content(models.ToJson());
        }

        public ActionResult GetClassRoomPTimetable( string F_Year, string ClassRoomId, string F_Semester)
        {
            var datas = classTimetableApp.GetList(t =>  t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_ClassRoomId == ClassRoomId);
            var objs = datas.Select(t =>
            {
                var teacher = new School_Teachers_App().GetForm(t.F_TeacherId);
                var course = new School_Course_App().GetForm(t.F_CourseId);
                string courseName = course == null ? "" : course.F_Name;

                if (t.F_IsMoveCourse == true)
                {
                    //var moveClass = new Schedule_MoveClass_App().GetForm(t.F_ClassId);
                    var room = classRoomApp.GetList(p => p.F_Id == t.F_ClassRoomId).FirstOrDefault();
                    //courseName = moveClass.F_Name + "_" + courseName+"_"+(room==null?"未安排":room.F_Name);
                    courseName = courseName + "_" + (room == null ? "未安排" : room.F_Name);
                }
                else
                {
                    string courseNameFormat = arrangeApp.FormatCourseName(t.F_ClassId, F_Semester, F_Year, t.F_CourseId);
                    courseName = string.Format(courseNameFormat, courseName);
                }
                return new
                {
                    F_ClassId = t.F_ClassId,
                    F_CourseId = t.F_CourseId,
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
            return Content(objs.ToJson());
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult exportClassRoom(string F_Year, string ClassRoomId, string F_Semester)
        {
            Dictionary<string, object> orgs = CacheConfig.GetOrganizeListByCache();
            var datas = classTimetableApp.GetList(t => t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_ClassRoomId == ClassRoomId);

            var gdata = datas.OrderBy(t => t.F_WeekN).ThenBy(t => t.F_CourseIndex).ToList();//.GroupBy(t => t.F_ClassId);                                                                                  //List<SheetItem> sheetItems = new List<SheetItem>();                                                                   //foreach (var items in gdata)
                                                                                            //{
            var item = gdata.FirstOrDefault();
            string className = GetPropertyValue(orgs[item.F_ClassId], "fullname").ToString();
            string title = "";
            title += item.F_Year;
            //title += item.F_SemesterId;
            title += GetPropertyValue(orgs[item.F_Divis_ID], "fullname");
            title += GetPropertyValue(orgs[item.F_GradeId], "fullname");
            title += className;
            title += "共" + gdata.Count() + "节";
            DataTable dt = ToExportDT(gdata);
            //    sheetItems.Add(new SheetItem { SheetName = className, Title = title, dt = dt });
            //}

            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        #endregion
    }
}