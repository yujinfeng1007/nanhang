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

using NFine.Application.ScheduleManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.ScheduleManage;
using NFine.Domain.Entity.SchoolManage;
using NFine.Domain.Model;
using NFine.Web.Areas.SchoolManage;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class School_Class_TimetableController : ControllerBase
    {
        private School_Schedules_App app = new School_Schedules_App();
        private School_Teachers_App teacher_app = new School_Teachers_App();
        private School_Class_Info_App classApp = new School_Class_Info_App();
        private School_PRule_Weeks_App pruleweekapp = new School_PRule_Weeks_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Source, string F_YWJL, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                //rows = app.GetList(pagination, keyword, F_Source, F_YWJL, F_CreatorTime_Start, F_CreatorTime_Stop),
                //total = pagination.total,
                //page = pagination.page,
                //records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            //将用户id替换成姓名
            // var creator = new object();
            // var modifier = new object();
            // Dictionary<string, object>  dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.USERS);
            //if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId,out creator)) {
            //     data.F_CreatorUserId = creator.GetType().GetProperty("fullname").GetValue(creator, null).ToString();
            // }
            // if (data.F_LastModifyUserId != null &&　dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            // {
            //     data.F_LastModifyUserId = modifier.GetType().GetProperty("fullname").GetValue(modifier, null).ToString();
            // }
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = string.Empty;//createExportSql("School_Class_PTimetable", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable users = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        public ActionResult GetClassTimetable(string F_Semester, string F_Year, string F_Class, string startDate, string endDate)
        {
            var datas = app.GetClassTimetable(F_Semester, F_Year, F_Class, startDate, endDate);
            List<SchoolSchedulesModel> list = new List<SchoolSchedulesModel>();
            foreach (var data in datas)
            {
                list.Add(ToModel(data));
            }
            return Content(list.ToJson());
        }

        public SchoolSchedulesModel ToModel(Schedule entity)
        {
            var teacher = teacher_app.GetForm(entity.F_Teacher1);
            string F_Teacher1Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher2);
            string F_Teacher2Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher3);
            string F_Teacher3Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher4);
            string F_Teacher4Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher5);
            string F_Teacher5Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher6);
            string F_Teacher6Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher7);
            string F_Teacher7Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher8);
            string F_Teacher8Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher9);
            string F_Teacher9Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher10);
            string F_Teacher10Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher11);
            string F_Teacher11Name = teacher == null ? string.Empty : teacher.F_Name;
            teacher = teacher_app.GetForm(entity.F_Teacher12);
            string F_Teacher12Name = teacher == null ? string.Empty : teacher.F_Name;

            return new SchoolSchedulesModel
            {
                F_Week = entity.F_Week,
                F_Course1 = entity.F_Course1,
                F_Course10 = entity.F_Course10,
                F_Course11 = entity.F_Course11,
                F_Course12 = entity.F_Course12,
                F_Course2 = entity.F_Course2,
                F_Course3 = entity.F_Course3,
                F_Course4 = entity.F_Course4,
                F_Course5 = entity.F_Course5,
                F_Course6 = entity.F_Course6,
                F_Course7 = entity.F_Course7,
                F_Course8 = entity.F_Course8,
                F_Course9 = entity.F_Course9,
                F_Id = entity.F_Id,
                F_Teacher1 = entity.F_Teacher1,
                F_Teacher10 = entity.F_Teacher10,

                F_Teacher10Name = F_Teacher10Name,
                F_Teacher11 = entity.F_Teacher11,
                F_Teacher11Name = F_Teacher11Name,
                F_Teacher12 = entity.F_Teacher12,
                F_Teacher12Name = F_Teacher12Name,
                F_Teacher1Name = F_Teacher1Name,
                F_Teacher2 = entity.F_Teacher2,
                F_Teacher2Name = F_Teacher2Name,
                F_Teacher3 = entity.F_Teacher3,
                F_Teacher3Name = F_Teacher3Name,
                F_Teacher4 = entity.F_Teacher4,
                F_Teacher4Name = F_Teacher4Name,
                F_Teacher5 = entity.F_Teacher5,
                F_Teacher5Name = F_Teacher5Name,
                F_Teacher6 = entity.F_Teacher6,
                F_Teacher6Name = F_Teacher6Name,
                F_Teacher7 = entity.F_Teacher7,
                F_Teacher7Name = F_Teacher7Name,
                F_Teacher8 = entity.F_Teacher8,
                F_Teacher8Name = F_Teacher8Name,
                F_Teacher9 = entity.F_Teacher9,
                F_Teacher9Name = F_Teacher9Name,
            };
        }

        public ActionResult SubmitForm(string F_Semester, string F_Class, string sourceDay, string sourceLesson, string setType, string targetDay, string targetLesson, string newCourse)
        {
            app.AddOrUpdCourse(F_Semester, F_Class, sourceDay, sourceLesson, setType, targetDay, targetLesson, newCourse);
            return Success("操作成功。");
        }

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
        public ActionResult import(string filePath, string F_Divis, string F_Grade, string F_Year, string F_Semester, string F_Class)
        {
            List<Schedule> list = new List<Schedule>();
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
                        for (int i = 0; i < sheetCount; i++)
                        {
                            var sheet = workbook.GetSheetAt(i);
                            var startTime = Convert.ToDateTime(sheet.SheetName);
                            if (string.IsNullOrEmpty(F_Class))
                                continue;
                            // 清除已发布的数据
                            app.DeleteDayAfterSchedules(F_Year, F_Semester, F_Class, startTime, startTime.AddDays(7));

                            list.AddRange(GetExcelDatas(sheet, startTime, F_Class, F_Divis, F_Grade, F_Year, F_Semester));
                        }
                    }
                }
                app.importWeek(list);
            }
            catch
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return Error("导入失败");
            }

            return Success("导入成功。");
        }

        private List<Schedule> GetExcelDatas(ISheet sheet, DateTime startTime, string F_Class, string F_Divis, string F_Grade, string F_Year, string F_Semester)
        {
            //单双周课程设置
            var weeksRule = pruleweekapp.GetList(t => t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_ClassId == F_Class);

            var classEnity = classApp.GetForm(F_Class);

            List<Schedule> list = new List<Schedule>();
            IRow row = null;
            if (sheet != null)
            {
                int rowCount = sheet.LastRowNum;//总行数
                if (rowCount > 1)
                {
                    IRow firstRow = sheet.GetRow(0);//第一行
                    int cellCount = firstRow.LastCellNum;//列数

                    row = sheet.GetRow(0);
                    if (row == null || row.Cells.Count <= 0)
                        return list;
                    for (int j = 1; j < (cellCount - row.FirstCellNum); j++)
                    {
                        if (j < 0)
                            break;

                        DateTime day = startTime.AddDays(j - 1);
                        int week = Convert.ToInt32(day.DayOfWeek) == 0 ? 7 : Convert.ToInt32(day.DayOfWeek);

                        Schedule entity = new Schedule();
                        entity.Create(false);
                        entity.F_Class = F_Class;
                        entity.F_Classroom = classEnity == null ? null : classEnity.F_Classroom;
                        entity.F_Semester = F_Semester;
                        entity.F_GradeId = F_Grade;
                        entity.F_Date = day;
                        entity.F_Week = week.ToString();

                        if (rowCount > 1)
                        {
                            var courseName = sheet.GetRow(1).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course1 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher1 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID1 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 2)
                        {
                            var courseName = sheet.GetRow(2).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course2 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher2 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID2 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 3)
                        {
                            var courseName = sheet.GetRow(3).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course3 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher3 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID3 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 4)
                        {
                            var courseName = sheet.GetRow(4).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course4 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher4 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID4 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 5)
                        {
                            var courseName = sheet.GetRow(5).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course5 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher5 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID5 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 6)
                        {
                            var courseName = sheet.GetRow(6).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course6 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher6 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID6 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 7)
                        {
                            var courseName = sheet.GetRow(7).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course7 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher7 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID7 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 8)
                        {
                            var courseName = sheet.GetRow(8).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course8 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher8 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID8 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 9)
                        {
                            var courseName = sheet.GetRow(9).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course9 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher9 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID9 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 10)
                        {
                            var courseName = sheet.GetRow(10).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course10 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher10 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID10 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 11)
                        {
                            var courseName = sheet.GetRow(11).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course11 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher11 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID11 = Guid.NewGuid().ToString();
                        }
                        if (rowCount > 12)
                        {
                            var courseName = sheet.GetRow(12).GetCell(j).StringCellValue;
                            var pt = GetWeeksPtimetable(F_Class, courseName, weeksRule, day);
                            entity.F_Course12 = pt == null ? null : pt.F_CourseId;
                            entity.F_Teacher12 = pt == null ? null : pt.F_TeacherId;
                            entity.F_Course_PrepareID12 = Guid.NewGuid().ToString();
                        }
                        list.Add(entity);
                    }
                }
            }
            return list;
        }

        //获取双周课程
        private Schedule_Class_PTimetable_Entity GetWeeksPtimetable(string F_Class, string courseName, List<Schedule_PRule_Weeks_Entity> weeksRule, DateTime day)
        {
            if (string.IsNullOrEmpty(courseName))
                return new Schedule_Class_PTimetable_Entity();
            var courseId = CacheConfig.GetSchoolCourseByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "F_Name").Equals(courseName)).Key;

            //单双周
            if (weeksRule.Count > 0)
            {
                //获取指定时间是当年的第几周
                GregorianCalendar gc = new GregorianCalendar();
                int weekOfYear = gc.GetWeekOfYear(day, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                //双周
                if (weekOfYear % 2 == 0)
                {
                    var weekdata = weeksRule.Where(t => t.F_Course1Id == courseId).FirstOrDefault();
                    if (weekdata != null)
                    {
                        string teacher2Id = new School_Class_Info_Teacher_App().GetTeacherByClassCourse(F_Class, weekdata.F_Course2Id);
                        return new Schedule_Class_PTimetable_Entity { F_CourseId = weekdata.F_Course2Id, F_TeacherId = teacher2Id };
                    }
                }
            }

            string teacherId = new School_Class_Info_Teacher_App().GetTeacherByClassCourse(F_Class, courseId);
            return new Schedule_Class_PTimetable_Entity { F_CourseId = courseId, F_TeacherId = teacherId };
        }
    }
}