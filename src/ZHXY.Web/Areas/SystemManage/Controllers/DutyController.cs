using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 岗位管理
    /// [OK]
    /// </summary>
    public class DutyController : ZhxyWebControllerBase
    {
        private SysDutyAppService App { get; }

        public DutyController(SysDutyAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetGridJson(string keyword)
        {
            var data = App.GetList(keyword);
            return Content(data.ToJson());
        }

      
        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.Get(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Role roleEntity, string keyValue)
        {
            App.SubmitForm(roleEntity, keyValue);
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.DUTY);
            CacheFactory.Cache().WriteCache(SysCacheAppService.GetDutyList(), SmartCampusConsts.DUTY);
            return Message("操作成功。");
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                App.Delete(F_Id[i]);
                CacheFactory.Cache().RemoveCache(SmartCampusConsts.DUTY);
                CacheFactory.Cache().WriteCache(SysCacheAppService.GetDutyList(), SmartCampusConsts.DUTY);
            }
            return Message("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export()
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("F_Category", "2");
            var exportSql = CreateExportSql("Sys_Role", parms);
            var dbParameter = CreateParms(parms);
            var users = App.GetDataTable(App.DataScopeFilter(exportSql), dbParameter);
            var ms = new NPOIExcel().ToExcelStream(users, "岗位表");
            ms.Seek(0, SeekOrigin.Begin);
            var filename = "岗位表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }
    }
}