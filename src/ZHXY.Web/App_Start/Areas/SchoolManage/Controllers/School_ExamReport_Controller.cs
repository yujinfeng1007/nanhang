/*******************************************************************************
 * Author: 刘红刚
 * Description: School_ExamReport  Controller类
********************************************************************************/

using NFine.Application;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //学生考试成绩表
    public class School_ExamReportController : ControllerBase
    {
        public DbContextMulti db = new DbContextMulti();
        private School_ExamReport_App app = new School_ExamReport_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult getExamListByYearTopicNum(string F_Year, string F_StudentNum, string F_Topic, string DutyId, string UserId, string MobilePhone)
        {
            //var F_DutyId = new DutyApp().GetForm(DutyId);
            //UserId = OperatorProvider.Provider.GetCurrent().UserId;
            //MobilePhone= OperatorProvider.Provider.GetCurrent().MobilePhone;
            //string UserCode=OperatorProvider.Provider.GetCurrent().UserCode;
            //var aa=new List<School_EntrySignUp_Entity>();
            //string Topic = "";
            //if (F_DutyId!=null)
            //{
            //    if (F_DutyId.F_FullName.Equals("家长"))
            //    {
            //        aa = new School_EntrySignUp_App().GetList("", MobilePhone);
            //        foreach (School_EntrySignUp_Entity item in aa)
            //        {
            //            Topic += item.F_StudentNum + ",";
            //        }
            //    }
            //    else if (F_DutyId.F_FullName.Equals("老师"))
            //    {
            //        aa = new School_EntrySignUp_App().GetList(UserId, "");
            //        foreach (School_EntrySignUp_Entity item in aa)
            //        {
            //            Topic += item.F_StudentNum + ",";
            //        }
            //    }
            //}

            //if (string.IsNullOrEmpty(UserId))
            //{
            //    return Error("没有找到该联系人信息");
            //}
            //List<School_ExamReport_Entity> datas=new List<School_ExamReport_Entity>();
            ////先找到该人的考试信息
            //if (Topic.IndexOf(F_StudentNum) !=-1|| UserCode=="admin")
            //{
            //    datas = app.GetList(F_Year, F_StudentNum, F_Topic);
            //}
            var datas = app.GetList(F_Year, F_StudentNum, F_Topic);
            if (datas != null && datas.Count > 0)
            {   //规则匹配
                return GetSalaryExamConfig(datas, "School_ExamReport");
            }
            return Content("[]");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Divis_ID, string F_Grade_Id, string F_Class_Id, string F_Student_Num)
        {
            //var aa = db.School_ExamReport.Include("School_ExamTitle_Entity").Single(a => a.F_Title == "");
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_Year, F_Divis_ID, F_Grade_Id, F_Class_Id, F_Student_Num),
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
            if (string.IsNullOrEmpty(keyValue))
            {
                return Error("参数不能为空");
            }
            var data = app.GetForm(keyValue);
            if (data != null)
            {
                return Content(data.ToJson());
            }
            return Content("没有找到该用户信息");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ExamReport entity, string keyValue)
        {
            entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            app.SubmitForm(entity, keyValue);
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
        public FileResult export(string keyword, string Student_Num, string F_Divis_ID, string F_Grade, string F_Class, string F_Year)
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件

            if (!string.IsNullOrEmpty(F_Divis_ID))
            {
                parms.Add("F_Divis_ID", F_Divis_ID);
            }
            if (!string.IsNullOrEmpty(F_Grade))
            {
                parms.Add("F_Grade_ID", F_Grade);
            }
            if (!string.IsNullOrEmpty(F_Class))
            {
                parms.Add("F_Class_ID", F_Class);
            }
            if (!string.IsNullOrEmpty(F_Year))
            {
                parms.Add("F_Year", F_Year);
            }

            var dbParameter = CreateParms(parms);
            string exportSql = CreateExportSql("School_ExamReport", parms);

            //姓名Like条件
            if (!string.IsNullOrEmpty(keyword))
            {
                exportSql += " and F_RealName like '%" + keyword + "%'";
            }
            //学号like条件
            if (!string.IsNullOrWhiteSpace(Student_Num))
            {
                exportSql += " and F_StuNum like '%" + Student_Num + "%'";
            }

            //获取数据
            var users = app.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);

            //写入流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "学生成绩列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "学生成绩列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            //是不是需要从配置数据库里面读取设定的显示名称？？
            List<ExamReport> list = ExcelToList<ExamReport>(Server.MapPath(filePath), "School_ExamReport");
            if (list == null || list.IsEmpty())
            {
                return Error("导入失败");
            }
            app.import(list);
            return Success("导入成功。");
        }
    }
}