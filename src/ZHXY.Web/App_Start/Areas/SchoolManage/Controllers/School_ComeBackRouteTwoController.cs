/*******************************************************************************
 * Author: mario
 * Description: School_ComeBackRouteTwo  Controller类
********************************************************************************/

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
    //School_ComeBackRouteTwo
    public class School_ComeBackRouteTwoController : ControllerBase
    {
        public ActionResult Country()
        {
            return View();
        }

        private School_ComeBackRouteTwo_App app = new School_ComeBackRouteTwo_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            pagination.Sord = "asc";
            //排序字段
            pagination.Sidx = "F_SortCode asc";
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
        public ActionResult GetCheckBoxJson(string keyword)
        {
            School_Area_App schoolareaapp = new School_Area_App();
            var data = schoolareaapp.GetListwhere().Where(t => t.F_ParentId == "110000"); ;
            List<CheckBoxSelectModel> list = new List<CheckBoxSelectModel>();
            foreach (SchoolArea r in data)
            {
                CheckBoxSelectModel fieldItem = new CheckBoxSelectModel();
                fieldItem.value = r.F_Id;
                fieldItem.text = r.F_FullName;
                if (keyword.IndexOf(r.F_Id) != -1)
                    fieldItem.ifChecked = true;
                list.Add(fieldItem);
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormEntity(ComeBackRouteTwo entity)
        {
            var data = app.GetByEntity(entity);
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
        public ActionResult SubmitForm(ComeBackRouteTwo entity, string keyValue)
        {
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
        public FileResult export(string keyword)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_ComeBackRouteTwo", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_ComeBackRouteTwo列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_ComeBackRouteTwo列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<ComeBackRouteTwo>(Server.MapPath(filePath), "School_ComeBackRouteTwo");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}