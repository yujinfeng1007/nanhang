/*******************************************************************************
 * Author: mario
 * Description: School_Class_Info  Controller类
********************************************************************************/

using NFine.Application.SchoolManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //班级信息
    public class School_Class_InfoController : ControllerBase
    {
        public ActionResult Teachers()
        {
            return View();
        }

        public ActionResult Teacher_Select()
        {
            return View();
        }

        private School_Class_Info_App app = new School_Class_Info_App();
        private School_Course_Teacher_App courseApp = new School_Course_Teacher_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop),
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
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ClassInfo entity, string keyValue)
        {
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateForm(ClassInfo entity, string keyValue)
        {
            entity.F_ClassID = keyValue;
            app.UpdateForm(entity);
            return Success("操作成功。");
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
            IDictionary<string, string> parms = new Dictionary<string, string>();
            DbParameter[] dbParameter = CreateParms(parms);
            string exportSql = CreateExportSql("School_Class_Info", parms);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "班级信息表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "班级信息表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
            List<ClassInfo> list = ExcelToList<ClassInfo>(Server.MapPath(filePath), rules);
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult getClassDetailInfo(string F_Class)
        {
            var entity = app.GetFormByF_ClassID(F_Class);
            var class_teacher = new School_Class_Info_Teacher_App().GetFormByF_ClassID(entity.F_ClassID);
            Class_Info cla = new Class_Info();
            if (entity != null)
            {
                cla.F_ID = entity.F_Id;
                cla.F_LOGO = new School_Info_App().GetForm(entity.F_School).F_Logo;
                cla.F_Name = entity.F_Name;
                cla.F_Address = entity.F_Address;
                cla.F_Motto = entity.F_Motto;

                List<TeacherBaseModel> teabasemodelleader = new List<TeacherBaseModel>();
                if (class_teacher.Count > 0)
                {
                    if (class_teacher.First().F_Leader_Tea != null)
                    {
                        CourseTeacher courseteacher1 = courseApp.GetFromBy_Teacher(class_teacher.First().F_Leader_Tea).First();
                        TeacherBaseModel tea = new TeacherBaseModel();
                        tea.F_Id = courseteacher1.School_Teachers_Entity.F_Id;
                        tea.F_Name = courseteacher1.School_Teachers_Entity.F_Name;
                        tea.F_Subject = courseteacher1.School_Course_Entity.F_Name;
                        tea.F_HeadPic = courseteacher1.School_Teachers_Entity.teacherSysUser.F_HeadIcon;
                        teabasemodelleader.Add(tea);
                    }

                    if (class_teacher.First().F_Leader_Tea2 != null)
                    {
                        CourseTeacher courseteacher2 = courseApp.GetFromBy_Teacher(class_teacher.First().F_Leader_Tea2).First();
                        TeacherBaseModel tea2 = new TeacherBaseModel();
                        tea2.F_Id = courseteacher2.School_Teachers_Entity.F_Id;
                        tea2.F_Name = courseteacher2.School_Teachers_Entity.F_Name;
                        tea2.F_Subject = courseteacher2.School_Course_Entity.F_Name;
                        tea2.F_HeadPic = courseteacher2.School_Teachers_Entity.teacherSysUser.F_HeadIcon;
                        teabasemodelleader.Add(tea2);
                    }
                }
                cla.F_Leader_Tea = teabasemodelleader;

                List<TeacherBaseModel> teabasemodel = new List<TeacherBaseModel>();
                if (class_teacher.Count > 0)
                {
                    foreach (var item1 in class_teacher)
                    {
                        if (item1.School_Teachers_F_Leader_Tea != null && (item1.School_Teachers_F_Leader_Tea.F_Id == item1.School_Teachers_Entity.F_Id))
                            continue;
                        if (item1.School_Teachers_F_Leader_Tea2 != null && (item1.School_Teachers_F_Leader_Tea2.F_Id == item1.School_Teachers_Entity.F_Id))
                            continue;
                        TeacherBaseModel tea = new TeacherBaseModel();
                        tea.F_Id = item1.School_Teachers_Entity.F_Id;
                        tea.F_Name = item1.School_Teachers_Entity.F_Name;
                        tea.F_Subject = item1.School_Course_Entity.F_Name;
                        tea.F_HeadPic = item1.School_Teachers_Entity.teacherSysUser.F_HeadIcon;
                        teabasemodel.Add(tea);
                    }
                }
                cla.teachers = teabasemodel;

                List<StudentBaseModel> stubasemodel = new List<StudentBaseModel>();
                var stulist = new School_Students_App().GetListByF_Class_ID(F_Class);
                if (stulist.Count > 0)
                {
                    foreach (var item in stulist)
                    {
                        StudentBaseModel stu = new StudentBaseModel();
                        stu.F_Id = item.F_Id;
                        stu.F_Name = item.F_Name;
                        stu.F_StudentNum = item.F_StudentNum;
                        stu.F_HeadPic = item.studentSysUser.F_HeadIcon;
                        stubasemodel.Add(stu);
                    }
                }
                cla.students = stubasemodel;

                List<StudentBaseModel> stubasemodelduty = new List<StudentBaseModel>();
                var studutylist = new School_Class_Duty_App().GetListByF_Class_ID(F_Class);
                if (studutylist.Count > 0)
                {
                    foreach (var item in studutylist)
                    {
                        StudentBaseModel stu = new StudentBaseModel();
                        stu.F_Id = item.School_Students_Entity.F_Id;
                        stu.F_Name = item.School_Students_Entity.F_Name;
                        stu.F_StudentNum = item.School_Students_Entity.F_StudentNum;
                        stu.F_HeadPic = item.School_Students_Entity.studentSysUser.F_HeadIcon;
                        stubasemodelduty.Add(stu);
                    }
                }
                cla.dutys = stubasemodelduty;
                Dictionary<string, int> dic = new School_Attendance_Rules_App().getClassCheckInfo(F_Class, DateTime.Now);
                cla.F_CheckCount = dic["1"];
                cla.F_NoCheckCount = dic["2"];
                cla.F_DelayCount = dic["3"];
                cla.F_studentCount = new School_Students_App().GetList().Where(t => t.F_Class_ID == entity.F_ClassID).Count();
            }
            return Content(cla.ToJson());
        }

        #region Model

        public class Class_Info
        {
            public string F_ID { get; set; }
            public string F_Name { get; set; }
            public string F_LOGO { get; set; }
            public string F_Address { get; set; }
            public string F_Motto { get; set; }

            public List<TeacherBaseModel> F_Leader_Tea { get; set; }
            public int F_CheckCount { get; set; }
            public int F_NoCheckCount { get; set; }
            public int F_DelayCount { get; set; }
            public int F_studentCount { get; set; }
            public List<StudentBaseModel> students { get; set; }
            public List<TeacherBaseModel> teachers { get; set; }
            public List<StudentBaseModel> dutys { get; set; }
        }

        public class StudentBaseModel
        {
            public string F_Id { get; set; }
            public string F_Name { get; set; }
            public string F_StudentNum { get; set; }
            public string F_HeadPic { get; set; }
        }

        public class TeacherBaseModel
        {
            public string F_Id { get; set; }
            public string F_Name { get; set; }
            public string F_Subject { get; set; }
            public string F_HeadPic { get; set; }
        }

        #endregion Model
    }
}