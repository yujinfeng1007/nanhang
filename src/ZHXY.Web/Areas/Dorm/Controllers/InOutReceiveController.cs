using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;

namespace ZHXY.Web.Dorm.Controllers
{

    public class InOutReceiveController : ZhxyWebControllerBase
	{
		private InOutReceiveAppService App { get; }

        public InOutReceiveController(InOutReceiveAppService inOutApp)
        {
            App = inOutApp;
        }
		
        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = App.GetList(pagination),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };

            return Content(data.ToJson());
        }
        
        [HttpGet]
        public ActionResult GetFormJson(string id)
        {
            var data = App.GetById(id);
            return Content(data.ToJson());
        }
        
      
        
        [HttpPost]
        public ActionResult DeleteForm(string id)
        {
            App.Delete(id);
            return Message("删除成功。");
        }
        
		 //导出excel
        [HttpGet]
        public FileResult export(string keyword)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if(!Ext.IsEmpty(keyword))
                parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);
            
            string exportSql = CreateExportSql("InOutReceive", parms);
            var users = App.GetDataTable(exportSql, dbParameter);
            ///////////////////写流
            var ms = new NPOIExcel().ToExcelStream(users, "列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }


        //导入excel
        [HttpPost]
        public ActionResult import(string filePath)
        {
            IDictionary<string, string[]> rules = new Dictionary<string, string[]>
            { { "F_Id", new string[] { "F_Id", "" } },
                { "F_SortCode", new string[] { "F_SortCode", "" } },
                { "F_DepartmentId", new string[] { "F_DepartmentId", "" } },
                { "F_DeleteMark", new string[] { "F_DeleteMark", "" } },
                { "F_CreatorUserId", new string[] { "F_CreatorUserId", "" } },
                { "F_Type", new string[] { "F_Type", "" } },
                { "F_ReceiveUser", new string[] { "F_ReceiveUser", "" } }
            };
            var list =ExcelToList< InOutReceive>(Server.MapPath(filePath), rules);
            if (list == null)
                return Error("导入失败");
            App.AddAndSave<InOutReceive>(list);
            return Message("导入成功。");
        }
   
	}
}