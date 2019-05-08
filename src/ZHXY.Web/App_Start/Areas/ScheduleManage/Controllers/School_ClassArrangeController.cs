using NFine.Application.ScheduleManage;
using NFine.Application.SchoolManage;
using NFine.Code;
using NFine.Domain.Entity.ScheduleManage;
using NFine.Web.Areas.SchoolManage;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class School_ClassArrangeController : ControllerBase
    {
        public class SubmitModel
        {
            public string teacherId { get; set; }
            public string courseId { get; set; }
            public int dateIndex { get; set; }
            public int lessonIndex { get; set; }
        }

        public class ImportModel
        {
            public string F_StudentNum { get; set; }
            public string F_ClassRoomNum { get; set; }
            public bool F_IsMoveCourse { get; set; }
            public int F_CourseIndex { get; set; }
            public string F_TeacherNum { get; set; }
            public int F_WeekN { get; set; }
            public string F_CourseNum { get; set; }
            public string F_ClassId { get; set; }
        }

        private School_Schedules_Time_App timeApp = new School_Schedules_Time_App();
        private School_Teachers_App teacherservice = new School_Teachers_App();
        private School_Course_App courseservice = new School_Course_App();
        private School_Class_Info_App classInfoApp = new School_Class_Info_App();
        private School_Students_App studentApp = new School_Students_App();
        private School_Classroom_App roomApp = new School_Classroom_App();

        private School_Class_PTimetable_App app = new School_Class_PTimetable_App();
        private School_ArrangeCourse_App arrangeApp = new School_ArrangeCourse_App();
        private Schedule_MoveClass_App moveClassApp = new Schedule_MoveClass_App();
        private Schedule_MoveClassStudent_App moveClassStudentApp = new Schedule_MoveClassStudent_App();
        //获取班级课表

        public ActionResult GetClassPTimetable(Pagination pagination, string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = app.GetClassPTimetable(F_Divis, F_Grade, F_Year, F_Class, F_Semester)
                .Select(t =>
                {
                    var teacher = new School_Teachers_App().GetForm(t.F_TeacherId);
                    var course = new School_Course_App().GetForm(t.F_CourseId);
                    string courseNameFormat = arrangeApp.FormatCourseName(F_Class, F_Semester, F_Year, t.F_CourseId);
                    string courseName = course == null ? "" : string.Format(courseNameFormat, course.F_Name);

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
            return Content(data.ToJson());
        }

        #region 获了年级课表

        private string getDayText(List<Schedule_Class_PTimetable_Entity> gdata, int f_Weeks
            , string F_Class, string F_Semester, string F_Year)
        {
            string day1 = "";
            gdata.Where(t => t.F_WeekN == f_Weeks).OrderBy(t => t.F_CourseIndex).ToList().ForEach(t =>
            {
                var teacher = teacherservice.GetForm(t.F_TeacherId);
                var course = courseservice.GetForm(t.F_CourseId);
                string courseNameFormat = arrangeApp.FormatCourseName(F_Class, F_Semester, F_Year, t.F_CourseId);
                string courseName = course == null ? "" : string.Format(courseNameFormat, course.F_Name);
                string text = t.F_CourseIndex + ":" + courseName + "(" + (teacher == null ? "" : teacher.F_Name) + ")" + "</br>";
                text = t.F_IsMoveCourse == true ? ("<span style='color:blue'>" + text + "</span>") : text;
                day1 += text;
            });
            return day1.TrimEnd("</br>".ToCharArray());
        }

        public ActionResult GetClassPTimetables(Pagination pagination, string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var teacherservice = new School_Teachers_App();
            var courseservice = new School_Course_App();
            List<object> objs = new List<object>();
            var datas = app.GetList(t => t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_GradeId == F_Grade);
            var gdatas = datas.Where(t => t.F_IsMoveCourse == false).GroupBy(t => t.F_ClassId);
            foreach (var gdata in gdatas)
            {
                var classtimeTable = gdata.ToList();
                var moveClassIds = moveClassApp.GetList(t => t.F_ClassIds.Contains(gdata.Key)).Select(t => t.F_Id).ToList();
                var movedata = datas.Where(t => moveClassIds.Contains(t.F_ClassId));

                var cousertime = classtimeTable.Count() + (movedata.Count() > 0 ? "+" + movedata.Count() : "");
                classtimeTable.AddRange(movedata);
                var fdata = classtimeTable.FirstOrDefault();
                var classEntity = new Application.SystemManage.OrganizeApp().GetForm(fdata.F_ClassId);
                objs.Add(new
                {
                    F_ClassName = classEntity == null ? "" : classEntity.F_FullName,
                    F_Id = classEntity.F_EnCode,
                    F_ClassId = fdata.F_ClassId,
                    F_CourseId = fdata.F_CourseId,
                    F_Divis_ID = fdata.F_Divis_ID,
                    F_GradeId = fdata.F_GradeId,
                    F_SemesterId = fdata.F_SemesterId,
                    F_CourseCount = cousertime,
                    F_Year = fdata.F_Year,
                    F_Day1 = getDayText(classtimeTable, 1, fdata.F_ClassId, F_Semester, F_Year),
                    F_Day2 = getDayText(classtimeTable, 2, fdata.F_ClassId, F_Semester, F_Year),
                    F_Day3 = getDayText(classtimeTable, 3, fdata.F_ClassId, F_Semester, F_Year),
                    F_Day4 = getDayText(classtimeTable, 4, fdata.F_ClassId, F_Semester, F_Year),
                    F_Day5 = getDayText(classtimeTable, 5, fdata.F_ClassId, F_Semester, F_Year),
                    F_Day6 = getDayText(classtimeTable, 6, fdata.F_ClassId, F_Semester, F_Year),
                    F_Day7 = getDayText(classtimeTable, 7, fdata.F_ClassId, F_Semester, F_Year)
                });
            }
            return Content(objs.ToJson());
        }

        #endregion 获了年级课表

        public ActionResult GetTimeJson(string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            OperatorModel user = OperatorProvider.Provider.GetCurrent();
            var data = timeApp.GetFormByOrg(F_Semester, F_Grade);
            return Content(data.ToJson());
        }

        #region 手动调整课表

        public ActionResult SubmitClassTimeTable(string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, List<SubmitModel> data)
        {
            var classEnity = classInfoApp.GetForm(F_Class);

            new School_Class_PTimetable_App().Delete(t => t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_ClassId == F_Class && t.F_IsMoveCourse == false);
            data = data ?? new List<SubmitModel>();
            List<Schedule_Class_PTimetable_Entity> list = new List<Schedule_Class_PTimetable_Entity>();
            data.ForEach(p =>
            {
                var model = new Schedule_Class_PTimetable_Entity
                {
                    F_Year = F_Year,
                    F_SemesterId = F_Semester,
                    F_GradeId = F_Grade,
                    F_Divis_ID = F_Divis,
                    F_ClassId = F_Class,
                    F_CourseId = p.courseId,
                    F_CourseIndex = p.lessonIndex,
                    F_TeacherId = p.teacherId,
                    F_WeekN = p.dateIndex,
                    F_IsMoveCourse = false,
                    F_ClassRoomId = classEnity == null ? null : classEnity.F_Classroom
                };
                model.Create();
                list.Add(model);
            });

            app.AddClassTimeTable(list);
            return Success("操作成功。");
        }

        public ActionResult cellValidate(int dateIndex, int lessonIndex, string courseid, string teacherId, string F_Year, string F_Semester, string F_Grade, string F_Class)
        {
            var data = arrangeApp.Validate(dateIndex, lessonIndex, teacherId, courseid, F_Year, F_Semester, F_Grade, F_Class, 10);
            if (data == null)
                return Success("操作成功。");
            else
            {
                return Error("周" + dateIndex + ",第" + lessonIndex + "节," + data);
            }
        }

        public ActionResult GetArrangeTip(Pagination pagination, string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new School_PRule_Grade_App().ArrangeRuleTip(pagination, keyword, F_Divis, F_Grade, F_Year, F_Class, F_Semester, F_CreatorTime_Start, F_CreatorTime_Stop);
            return Content(data.ToJson());
        }

        #endregion 手动调整课表

        // 发布课表
        public ActionResult PublishForm(string keyValue, string F_Semester, string F_Year)
        {
            string[] F_Id = keyValue.Split('|');
            for (int i = 0; i < F_Id.Length - 1; i++)
            {
                app.PublishClassTable(F_Id[i], F_Semester, F_Year);
            }
            return Success("操作成功。");
        }

        #region 导入导出课表
        ////导出excel
        //[HttpGet]
        //[HandlerAuthorize]
        //public FileResult export(string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        //{
        //    Dictionary<string, object> orgs = CacheConfig.GetOrganizeListByCache();
        //    var data = app.GetClassPTimetable(F_Divis, F_Grade, F_Year, F_Class, F_Semester);
        //    var gdata = data.OrderBy(t => t.F_WeekN).ThenBy(t => t.F_CourseIndex).GroupBy(t => t.F_ClassId);
        //    List<SheetItem> sheetItems = new List<SheetItem>();
        //    foreach (var items in gdata)
        //    {
        //        var item = items.FirstOrDefault();
        //        string className = GetPropertyValue(orgs[item.F_ClassId], "fullname").ToString();
        //        string title = "";
        //        title += item.F_Year;
        //        //title += item.F_SemesterId;
        //        title += GetPropertyValue(orgs[item.F_Divis_ID], "fullname");
        //        title += GetPropertyValue(orgs[item.F_GradeId], "fullname");
        //        title += className;
        //        title += "共" + items.Count() + "节";
        //        DataTable dt = ToExportDT(items.Where(t => t.F_ClassId == item.F_ClassId).ToList());
        //        sheetItems.Add(new SheetItem { SheetName = className, Title = title, dt = dt });
        //    }

        //    ///////////////////写流
        //    MemoryStream ms = new NPOIExcel().ToExcelMuchSheet(sheetItems);
        //    ms.Seek(0, SeekOrigin.Begin);
        //    string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //    return File(ms, "application/ms-excel", filename);
        //}

        //private DataTable ToExportDT(List<Schedule_Class_PTimetable_Entity> items)
        //{
        //    if (items == null || items.Count <= 0)
        //        return null;
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("第几节", Type.GetType("System.String"));
        //    dt.Columns.Add("周1", Type.GetType("System.String"));
        //    dt.Columns.Add("周2", Type.GetType("System.String"));
        //    dt.Columns.Add("周3", Type.GetType("System.String"));
        //    dt.Columns.Add("周4", Type.GetType("System.String"));
        //    dt.Columns.Add("周5", Type.GetType("System.String"));
        //    dt.Columns.Add("周6", Type.GetType("System.String"));
        //    dt.Columns.Add("周7", Type.GetType("System.String"));

        //    var item = items.FirstOrDefault();
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["第几节"] = "第" + (i) + "节";
        //        dt.Rows.Add(dr);

        //        var indexitems = items.Where(t => t.F_CourseIndex == i).ToList();
        //        foreach (var dayindexitem in indexitems)
        //        {
        //            //var teacher = new School_Teachers_App().GetForm(dayindexitem.F_TeacherId);
        //            var course = new School_Course_App().GetForm(dayindexitem.F_CourseId);
        //            string text = (course == null ? "" : course.F_Name);// + "(" + (teacher == null ? "" : teacher.F_Name) + ")";
        //            dr["周" + dayindexitem.F_WeekN] = text;
        //        }
        //    }
        //    return dt;
        //}

        /// <summary>
        /// excel导入页面
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath, string F_Divis, string F_Grade, string F_Year, string F_Semester)
        {
            List<ImportModel> list = new List<ImportModel>();
            FileStream fs = null;
            try
            {
                using (fs = System.IO.File.OpenRead(Server.MapPath(filePath)))
                {
                    IWorkbook workbook = null;
                    // 2007版本
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        int sheetCount = workbook.NumberOfSheets;//读取第一个sheet，当然也可以循环读取每个sheet
                        List<string> classIds = new List<string>();
                        for (int i = 0; i < sheetCount; i++)
                        {
                            var sheet = workbook.GetSheetAt(i);
                            var classId = sheet.SheetName;
                            classIds.Add(classId);
                            var datas = GetExcelDatas(sheet, classId);
                            list.AddRange(datas);
                        }
                        ImportDatas(classIds, list, F_Year, F_Semester, F_Divis, F_Grade);
                    }
                }
            }
            catch (Exception e)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return Error("导入失败");
            }
            return Success("导入成功。");
        }

        public void ImportDatas(List<string> classIds, List<ImportModel> datas, string F_Year, string F_Semester, string F_Divis, string F_Grade)
        {
            app.RestTimeTable(F_Year, F_Semester, F_Grade, classIds);
            // 导入行政课
            List<Schedule_Class_PTimetable_Entity> list = new List<Schedule_Class_PTimetable_Entity>();
            var xzdata = datas.Where(t => t.F_IsMoveCourse != true).GroupBy(t => new { t.F_WeekN, t.F_CourseIndex, t.F_ClassId }).ToList();
            xzdata.ForEach(p =>
            {
                var fdata = p.FirstOrDefault();
                var roomEntity = roomApp.GetFormByNo(fdata.F_ClassRoomNum);
                var courseEnity = courseservice.GetFormByCode(fdata.F_CourseNum);
                var teacherEntity = teacherservice.GetFormByF_Num(fdata.F_TeacherNum);
                var cpModel = new Schedule_Class_PTimetable_Entity
                {
                    F_ClassId = fdata.F_ClassId,
                    F_ClassRoomId = roomEntity == null ? null : roomEntity.F_Id,
                    F_CourseId = courseEnity == null ? null : courseEnity.F_Id,
                    F_CourseIndex = fdata.F_CourseIndex,
                    F_Divis_ID = F_Divis,
                    F_GradeId = F_Grade,
                    F_IsMoveCourse = fdata.F_IsMoveCourse,
                    F_SemesterId = F_Semester,
                    F_TeacherId = teacherEntity == null ? null : teacherEntity.F_Id,
                    F_WeekN = fdata.F_WeekN,
                    F_Year = F_Year
                };
                cpModel.Create();
                list.Add(cpModel);
            });

            // 导入走班课表
            List<Schedule_MoveClass_Entity> moveClassList = new List<Schedule_MoveClass_Entity>();
            List<Schedule_MoveClassStudent_Entity> moveClassStudentList = new List<Schedule_MoveClassStudent_Entity>();

            var zbdata = datas.Where(t => t.F_IsMoveCourse == true).GroupBy(t => new { t.F_TeacherNum, t.F_CourseNum }).ToList();
            zbdata.ForEach(p =>
            {
                //创建走班班级
                var fdata = p.FirstOrDefault();
                var courseEnity = courseservice.GetFormByCode(fdata.F_CourseNum);
                var teacherEntity = teacherservice.GetFormByF_Num(fdata.F_TeacherNum);

                var srcClassIds = p.GroupBy(t => t.F_ClassId).Select(t => t.FirstOrDefault().F_ClassId).Distinct();
                var moveclass = new Schedule_MoveClass_Entity
                {
                    F_ClassIds = string.Join(",", srcClassIds),
                    F_CourseTime = 0,
                    F_DivisId = F_Divis,
                    F_Name = courseEnity.F_Name,
                    F_ParentCourseId = courseEnity.F_ParentId,
                    F_RepeatCount = 0,
                    F_CourseId = courseEnity.F_Id,
                    F_GradeId = F_Grade,
                    F_SemesterId = F_Semester,
                    F_TeacherId = teacherEntity == null ? null : teacherEntity.F_Id,
                    F_Year = F_Year
                };
                moveclass.Create();
                moveClassList.Add(moveclass);
                //创建走班学生
                foreach (var mcs in p)
                {
                    var student = studentApp.GetFormBykeyValue(mcs.F_StudentNum);
                    moveClassStudentList.Add(new Schedule_MoveClassStudent_Entity
                    {
                        F_MoveClassId = moveclass.F_Id,
                        F_StudentId = student.F_Id,
                    });
                }
                // 走班班级进行排课
                var zbcdata = p.GroupBy(t => new { t.F_WeekN, t.F_CourseIndex }).ToList();
                zbcdata.ForEach(t =>
                {
                    var f = t.FirstOrDefault();
                    var roomEntity = roomApp.GetFormByNo(f.F_ClassRoomNum);

                    var cpModel = new Schedule_Class_PTimetable_Entity
                    {
                        F_ClassId = moveclass.F_Id,
                        F_ClassRoomId = roomEntity == null ? null : roomEntity.F_Id,
                        F_CourseId = courseEnity.F_Id,
                        F_CourseIndex = f.F_CourseIndex,
                        F_Divis_ID = F_Divis,
                        F_GradeId = F_Grade,
                        F_IsMoveCourse = f.F_IsMoveCourse,
                        F_SemesterId = F_Semester,
                        F_TeacherId = teacherEntity == null ? null : teacherEntity.F_Id,
                        F_WeekN = f.F_WeekN,
                        F_Year = F_Year
                    };
                    cpModel.Create();
                    list.Add(cpModel);
                });
            });
            moveClassStudentApp.import(moveClassStudentList);
            moveClassApp.AddDatas(moveClassList);
            app.import(list);
        }

        private List<ImportModel> GetExcelDatas(ISheet sheet, string f_classId)
        {
            List<ImportModel> list = new List<ImportModel>();
            IRow row = null;
            ICell cell = null;
            if (sheet != null)
            {
                int rowCount = sheet.LastRowNum;//总行数
                if (rowCount > 1)
                {
                    IRow firstRow = sheet.GetRow(0);//第一行
                    int cellCount = firstRow.LastCellNum;//列数

                    //填充行
                    for (int i = 1; i <= rowCount; i++)
                    {

                        row = sheet.GetRow(i);
                        if (row == null || row.Cells.Count <= 0) continue;
                        var studentNum = row.GetCell(0).GetCellValue();
                        if (string.IsNullOrEmpty(studentNum)) continue;
                        int week = Convert.ToInt32(row.GetCell(1).GetCellValue());

                        for (int j = 0; j < 12; j++)
                        {
                            int cellCourseIndex = 2 + 4 * j;
                            cell = row.GetCell(cellCourseIndex);
                            if (string.IsNullOrEmpty(cell.GetCellValue()))
                                continue;

                            var courseId = cell.GetCellValue();
                            var teacherId = row.GetCell(cellCourseIndex + 1).GetCellValue();
                            var classRoomId = row.GetCell(cellCourseIndex + 2).GetCellValue();
                            var isMoveClass = string.IsNullOrEmpty(row.GetCell(cellCourseIndex + 3).GetCellValue()) ? false : Convert.ToBoolean(row.GetCell(cellCourseIndex + 3).GetCellValue());
                            var data = new ImportModel
                            {
                                F_CourseIndex = (j + 1),
                                F_WeekN = week,
                                F_CourseNum = courseId,
                                F_TeacherNum = teacherId,
                                F_IsMoveCourse = isMoveClass,
                                F_ClassRoomNum = classRoomId,
                                F_StudentNum = studentNum,
                                F_ClassId = f_classId
                            };
                            list.Add(data);
                        }
                    }
                }
            }
            return list;
        }

        ////导入excel
        //[HttpPost]
        //[HandlerAjaxOnly]
        //[HandlerAuthorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult import(string filePath, string F_Divis, string F_Grade, string F_Year, string F_Semester)
        //{
        //    List<Schedule_Class_PTimetable_Entity> list = new List<Schedule_Class_PTimetable_Entity>();
        //    FileStream fs = null;
        //    try
        //    {
        //        using (fs = System.IO.File.OpenRead(Server.MapPath(filePath)))
        //        {
        //            IWorkbook workbook = null;
        //            // 2007版本
        //            if (filePath.IndexOf(".xlsx") > 0)
        //                workbook = new XSSFWorkbook(fs);
        //            // 2003版本
        //            else if (filePath.IndexOf(".xls") > 0)
        //                workbook = new HSSFWorkbook(fs);

        //            if (workbook != null)
        //            {
        //                int sheetCount = workbook.NumberOfSheets;//读取第一个sheet，当然也可以循环读取每个sheet
        //                for (int i = 0; i < sheetCount; i++)
        //                {
        //                    var sheet = workbook.GetSheetAt(i);
        //                    var classId = CacheConfig.GetOrganizeListByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "fullname").Equals(sheet.SheetName)).Key;
        //                    if (string.IsNullOrEmpty(classId))
        //                        continue;
        //                    app.RestTimeTable(F_Year, F_Semester, F_Grade, new List<string> { classId });
        //                    list.AddRange(GetExcelDatas(sheet, classId, F_Divis, F_Grade, F_Year, F_Semester));
        //                }
        //            }
        //        }
        //        app.import(list);
        //    }
        //    catch
        //    {
        //        if (fs != null)
        //        {
        //            fs.Close();
        //        }
        //        return Error("导入失败");
        //    }

        //    return Success("导入成功。");
        //}

        //private List<Schedule_Class_PTimetable_Entity> GetExcelDatas(ISheet sheet, string f_classId, string F_Divis, string F_Grade, string F_Year, string F_Semester)
        //{
        //    List<Schedule_Class_PTimetable_Entity> list = new List<Schedule_Class_PTimetable_Entity>();
        //    IRow row = null;
        //    ICell cell = null;
        //    if (sheet != null)
        //    {
        //        int rowCount = sheet.LastRowNum;//总行数
        //        if (rowCount > 1)
        //        {
        //            IRow firstRow = sheet.GetRow(0);//第一行
        //            int cellCount = firstRow.LastCellNum;//列数

        //            //填充行
        //            for (int i = 1; i <= rowCount; i++)
        //            {
        //                row = sheet.GetRow(i);
        //                if (row == null || row.Cells.Count <= 0) continue;
        //                for (int j = 1; j < (cellCount - row.FirstCellNum); j++)
        //                {
        //                    if (j < 0)
        //                        break;
        //                    cell = row.GetCell(j);
        //                    if (string.IsNullOrEmpty(cell.StringCellValue))
        //                        continue;
        //                    var courseId = CacheConfig.GetSchoolCourseByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "F_Name").Equals(cell.StringCellValue.ToString())).Key;
        //                    var teacher = new School_Class_Info_Teacher_App().GetList(t => t.F_ClassID == f_classId && t.F_CourseID == courseId).FirstOrDefault();
        //                    var teacherId = teacher != null ? teacher.F_Teacher : null;
        //                    var data = new Schedule_Class_PTimetable_Entity
        //                    {
        //                        F_Year = F_Year,
        //                        F_SemesterId = F_Semester,
        //                        F_Divis_ID = F_Divis,
        //                        F_GradeId = F_Grade,
        //                        F_ClassId = f_classId,
        //                        F_CourseIndex = i,
        //                        F_WeekN = j,
        //                        F_CourseId = courseId,
        //                        F_TeacherId = teacherId
        //                    };
        //                    data.Create();
        //                    list.Add(data);
        //                }
        //            }
        //        }
        //    }
        //    return list;
        //}

        #endregion 导入导出课表

        #region 自动排课

        public ActionResult AutoArrange(Pagination pagination, string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            //var data = app.AutoArrange(pagination, keyword, F_Divis, F_Grade, F_Year, F_Class, F_Semester, F_CreatorTime_Start, F_CreatorTime_Stop);
            arrangeApp.GradeAutoArrange(F_Grade, F_Year, F_Semester);
            return Success("操作成功。");
        }

        public ActionResult AutoArrangeForm()
        {
            return View();
        }

        #endregion 自动排课
    }
}