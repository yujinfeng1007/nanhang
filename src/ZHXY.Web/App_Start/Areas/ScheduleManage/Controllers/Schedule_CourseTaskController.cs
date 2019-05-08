using NFine.Application.ScheduleManage;
using NFine.Code.Excel;
using NFine.Domain.Entity.ScheduleManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using NFine.Application.SchoolManage;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_CourseTaskController : ControllerBase
    {
        private Schedule_WishCourseTask_App app = new Schedule_WishCourseTask_App();
        private Schedule_WishCourseGroup_App courseGroupApp = new Schedule_WishCourseGroup_App();
        private Schedule_WCTask_Group_App wtGroupApp = new Schedule_WCTask_Group_App();
        private School_Students_App studentApp = new School_Students_App();

        public ActionResult publishTask(string keyValue)
        {
            app.publishTask(keyValue);
            return Success("操作成功。");
        }

        public ActionResult Import()
        {
            return View();
        }

        public ActionResult exportStudent(string F_Semester, string F_Year, string F_Divis, string F_Grade)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            parms.Add("F_Divis_ID", F_Divis);
            parms.Add("F_Grade_ID", F_Grade);
            parms.Add("F_Year", F_Year);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = "select School_Students.F_Id as '编号',F_StudentNum as '学号',F_Name as '姓名','' as '选科' from School_Students "
                + " left join Schedule_WishCourseGroup on School_Students.F_Id=Schedule_WishCourseGroup.F_StudentID"
                + " left join Schedule_WCTask_Group on Schedule_WishCourseGroup.F_TaskCourseGroupID = Schedule_WCTask_Group.F_Id"
                + " left join Schedule_CourseGroup on Schedule_WCTask_Group.F_CourseGroupId = Schedule_CourseGroup.F_Id"
                + " where F_Divis_ID=@F_Divis_ID and F_Grade_ID=@F_Grade_ID and F_Year=@F_Year";
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "年级学生");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "学生选课列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        public ActionResult importStudentCourses(string filePath,string keyValue)
        {
            IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
            rules.Add("F_Id", new string[] { "编号", "" });
            rules.Add("F_StudentID", new string[] { "学号", "" });
            rules.Add("F_Memo", new string[] { "姓名", "" });
            rules.Add("F_TaskCourseGroupID", new string[] { "选科", "" });
            List<Schedule_WishCourseGroup_Entity> list = ExcelToList<Schedule_WishCourseGroup_Entity>(Server.MapPath(filePath), rules);
            foreach(var data in list)
            {
                var student= studentApp.GetFormByNum(data.F_StudentID);
                if (student == null)
                    continue;
                var group = wtGroupApp.GetList(t =>t.F_TaskId==keyValue&& t.Schedule_CourseGroup_Entity.F_GroupName == data.F_TaskCourseGroupID).FirstOrDefault();
                if (group == null)
                    continue;
                data.F_StudentID = student.F_Id;
                data.F_TaskCourseGroupID = group.F_Id;
                data.F_TaskId= keyValue;
                data.Create();
            }
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            courseGroupApp.import(list,false);
            return Success("导入成功。");
        }
    }
}