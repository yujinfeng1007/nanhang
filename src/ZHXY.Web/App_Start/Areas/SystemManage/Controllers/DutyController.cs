using NFine.Application;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SystemManage;
using NFine.Web.Areas.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class DutyController : ControllerBase
    {
        private DutyApp dutyApp = new DutyApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = dutyApp.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCheckBoxJson(string keyword)
        {
            var data = dutyApp.GetList();
            var checkedBoxs = new School_Subjects_App().GetForm(keyword);
            List<CheckBoxSelectModel> list = new List<CheckBoxSelectModel>();
            foreach (Role r in data)
            {
                CheckBoxSelectModel fieldItem = new CheckBoxSelectModel();
                fieldItem.value = r.F_Id;
                fieldItem.text = r.F_FullName;
                if (checkedBoxs != null)
                {
                    if (!string.IsNullOrEmpty(checkedBoxs.F_SignDutyId))
                    {
                        if (checkedBoxs.F_SignDutyId.IndexOf(r.F_Id) != -1)
                            fieldItem.ifChecked = true;
                    }
                }
                list.Add(fieldItem);
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dutyApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Role roleEntity, string keyValue)
        {
            dutyApp.SubmitForm(roleEntity, keyValue);
            CacheFactory.Cache().RemoveCache(Cons.DUTY);
            CacheFactory.Cache().WriteCache(CacheConfig.GetDutyList(), Cons.DUTY);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            string[] F_Id = keyValue.Split('|');
            for (int i = 0; i < F_Id.Length - 1; i++)
            {
                dutyApp.DeleteForm(F_Id[i]);
                CacheFactory.Cache().RemoveCache(Cons.DUTY);
                CacheFactory.Cache().WriteCache(CacheConfig.GetDutyList(), Cons.DUTY);
            }
            //dutyApp.DeleteForm(keyValue);
            //CacheFactory.Cache().RemoveCache(Cons.DUTY);
            //CacheFactory.Cache().WriteCache(MvcApplication.GetDutyList(), Cons.DUTY);
            return Success("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export()
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("F_Category", "2");
            string exportSql = CreateExportSql("Sys_Role", parms);
            DbParameter[] dbParameter = CreateParms(parms);
            DataTable users = dutyApp.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "岗位表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "岗位表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }
    }
}