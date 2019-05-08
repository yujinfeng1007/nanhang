/*******************************************************************************
 * Author: mario
 * Description: School_Salary  Controller类
********************************************************************************/

using NFine.Application;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //教师工资表
    public class School_SalaryController : ControllerBase
    {
        private School_Salary_App app = new School_Salary_App();
        private School_Teachers_App treacherapp = new School_Teachers_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult getListByYearMonth(string F_Year_Month)
        {
            string UserId = OperatorProvider.Provider.GetCurrent().UserId;
            if (string.IsNullOrEmpty(UserId))
            {
                return Error("没有此人信息");
            }

            //先找到该人的工资信息
            //List< School_Salary_Entity> datas = app.GetList(null, null, UserId, null, null, F_Year_Month);
            List<Salary> datas = app.GetList(UserId, F_Year_Month);
            if (datas != null && datas.Count > 0)
            {
                return GetSalaryExamConfig(datas.Take(1).Skip(0).ToList(), "School_Salary");
            }
            return Content("[]");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Tearcher_ID, string F_Teacher_Num, string F_Divis_ID, string F_Year_Month)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_Tearcher_ID, F_Teacher_Num, F_Divis_ID, F_Year_Month),
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
            return data == null ? null : Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Salary entity, string keyValue)
        {
            //entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            var data = treacherapp.GetListSelect(entity.F_Teachers_ID);
            foreach (Teacher item in data)
            {
                entity.F_Name = item.F_Name;
                entity.F_Teachers_ID = item.F_Id;
                entity.F_Num = item.F_Num;
                entity.F_Divis_ID = item.F_Divis_ID;
            }
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
        public FileResult export(string keyword, string F_Divis_ID, string F_Year_Month, string F_Num)
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();

            //部门
            if (!string.IsNullOrWhiteSpace(F_Divis_ID))
            {
                parms.Add("F_Divis_ID", F_Divis_ID);
            }
            //工资年月
            if (!string.IsNullOrWhiteSpace(F_Year_Month))
            {
                string[] yearMonths = F_Year_Month.Split('-');
                if (yearMonths != null && yearMonths.Length > 1)
                {
                    parms.Add("F_Year", yearMonths[0]);
                    parms.Add("F_Month", yearMonths[1]);
                }
            }

            //生成参数
            var dbParameter = CreateParms(parms);
            string exportSql = CreateExportSql("School_Salary", parms);

            //姓名
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                exportSql += " and F_Name like '%" + keyword + "%'";
            }

            //学号
            if (!string.IsNullOrWhiteSpace(F_Num))
            {
                exportSql += " and F_Num like '%" + F_Num + "%'";
            }

            //获取数据
            var salarys = app.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);

            //写入文件流
            MemoryStream ms = new NPOIExcel().ToExcelStream(salarys, "工资列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "工资列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            List<Salary> list = ExcelToList<Salary>(Server.MapPath(filePath), "School_Salary");
            if (list == null || list.IsEmpty())
            {
                return Error("导入失败");
            }
            app.import(list);
            return Success("导入成功。");
        }
    }
}