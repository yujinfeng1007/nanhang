using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Web.SystemManage.Controllers
{
    //缴费发票

    public class Sys_ARInvoiceController : ZhxyWebControllerBase
    {
        private ARInvoiceManageApp App { get; }

        public Sys_ARInvoiceController(ARInvoiceManageApp app) => App = app;
        [HttpGet]
        
        public ActionResult GetGridJson(Pagination pagination, string F_ARInvoiceNum, string F_Type, string F_Bill_Num, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = App.GetList(pagination, F_ARInvoiceNum, F_Type, F_Bill_Num, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysARInvoice entity, string keyValue)
        {
            App.SubmitForm(entity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteForm(keyValue);
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.USERS);
            CacheFactory.Cache().WriteCache(SysCacheAppService.GetUserList(), SmartCampusConsts.USERS);
            return Message("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string F_ARInvoiceNum, string F_Type, string F_Charge_ID, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!F_ARInvoiceNum.IsEmpty())
                parms.Add("F_Tax_Num", F_ARInvoiceNum);
            if (!F_Type.IsEmpty())
                parms.Add("F_Type", F_Type);
            if (!F_Charge_ID.IsEmpty())
                parms.Add("F_Charge_ID", F_Charge_ID);
            var dbParameter = CreateParms(parms);

            var exportSql = CreateExportSql("Sys_ARInvoice", parms);
            if (!string.IsNullOrEmpty(F_CreatorTime_Start))
            {
                var CreatorTime_Start = Convert.ToDateTime(F_CreatorTime_Start + " 00:00:00");
                exportSql += " and t.F_CreatorTime >= '" + CreatorTime_Start + "'";
            }

            if (!string.IsNullOrEmpty(F_CreatorTime_Stop))
            {
                var CreatorTime_Stop = Convert.ToDateTime(F_CreatorTime_Stop + " 23:59:59");
                exportSql += " and t.F_CreatorTime <= '" + CreatorTime_Stop + "'";
            }
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            var users = App.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            var ms = new NPOIExcel().ToExcelStream(users, "开票列表");
            ms.Seek(0, SeekOrigin.Begin);
            var filename = "开票列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<SysARInvoice>(Server.MapPath(filePath), "Sys_ARInvoice");
            if (list == null || list.IsEmpty())
            {
                return Error("导入失败");
            }
            App.Import(list);
            return Message("导入成功。");
        }
    }
}