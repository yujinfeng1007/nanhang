using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    //缴费发票

    public class Sys_ARInvoiceController : ControllerBase
    {
        private Sys_ARInvoice_App app = new Sys_ARInvoice_App();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        private ICache cache = CacheFactory.Cache();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string F_ARInvoiceNum, string F_Type, string F_Bill_Num, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_ARInvoiceNum, F_Type, F_Bill_Num, F_CreatorTime_Start, F_CreatorTime_Stop),
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
            //将用户id替换成姓名
            //var creator = new object();
            //var modifier = new object();
            //Dictionary<string, object> dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.USERS);
            //if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId, out creator))
            //{
            //    data.F_CreatorUserId = creator.GetType().GetProperty("fullname").GetValue(creator, null).ToString();
            //}
            //if (data.F_LastModifyUserId != null && dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            //{
            //    data.F_LastModifyUserId = modifier.GetType().GetProperty("fullname").GetValue(modifier, null).ToString();
            //}
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysARInvoice entity, string keyValue)
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
            cache.RemoveCache(Cons.USERS);
            cache.WriteCache(CacheConfig.GetUserList(), Cons.USERS);
            return Success("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string F_ARInvoiceNum, string F_Type, string F_Charge_ID, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(F_ARInvoiceNum))
                parms.Add("F_Tax_Num", F_ARInvoiceNum);
            if (!Ext.IsEmpty(F_Type))
                parms.Add("F_Type", F_Type);
            if (!Ext.IsEmpty(F_Charge_ID))
                parms.Add("F_Charge_ID", F_Charge_ID);
            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("Sys_ARInvoice", parms);
            if (!string.IsNullOrEmpty(F_CreatorTime_Start))
            {
                DateTime CreatorTime_Start = Convert.ToDateTime(F_CreatorTime_Start + " 00:00:00");
                exportSql += " and t.F_CreatorTime >= '" + CreatorTime_Start + "'";
            }

            if (!string.IsNullOrEmpty(F_CreatorTime_Stop))
            {
                DateTime CreatorTime_Stop = Convert.ToDateTime(F_CreatorTime_Stop + " 23:59:59");
                exportSql += " and t.F_CreatorTime <= '" + CreatorTime_Stop + "'";
            }
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable users = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "开票列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "开票列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            List<SysARInvoice> list = ExcelToList<SysARInvoice>(Server.MapPath(filePath), "Sys_ARInvoice");
            if (list == null || list.IsEmpty())
            {
                return Error("导入失败");
            }
            app.import(list);
            return Success("导入成功。");
        }
    }
}