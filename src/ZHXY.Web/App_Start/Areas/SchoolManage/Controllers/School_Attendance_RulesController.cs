/*******************************************************************************
 * Author: mario
 * Description: School_Attendance_Rules  Controller类
********************************************************************************/

using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //考勤规则
    public class School_Attendance_RulesController : ControllerBase
    {
        private School_Attendance_Rules_App app = new School_Attendance_Rules_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_EnabledMark)
        {
            try
            {
                var data = new
                {
                    rows = app.GetList(pagination, keyword, F_DepartmentId, F_EnabledMark),
                    total = pagination.Total,
                    page = pagination.Page,
                    records = pagination.Records
                };
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(AttendanceRule entity, string keyValue)
        {
            if (Ext.IsEmpty(entity.F_AttendanceTime2))
            {
                entity.F_AttendanceTime2 = "&nbsp;";
            }
            if (Ext.IsEmpty(entity.F_AttendanceTime3))
            {
                entity.F_AttendanceTime3 = "&nbsp;";
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

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            string[] F_Id = keyValue.Split('|');
            for (int i = 0; i < F_Id.Length - 1; i++)
            {
                AttendanceRule entity = app.GetForm(F_Id[i]);
                entity.F_Id = F_Id[i];
                entity.F_EnabledMark = false;
                app.UpdateForm(entity);
            }
            return Success("禁用成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            string[] F_Id = keyValue.Split('|');
            for (int i = 0; i < F_Id.Length - 1; i++)
            {
                AttendanceRule entity = app.GetForm(F_Id[i]);
                entity.F_Id = F_Id[i];
                entity.F_EnabledMark = true;
                app.UpdateForm(entity);
            }
            return Success("启用成功。");
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

            string exportSql = CreateExportSql("School_Attendance_Rules", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Attendance_Rules列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Attendance_Rules列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            List<AttendanceRule> list = ExcelToList<AttendanceRule>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}