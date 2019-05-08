using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : ZhxyWebControllerBase
    {
        private SysRoleAppService App { get; }
        public RoleController(SysRoleAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetSelectJson(string F_RoleId)
        {
            var list = new List<object>();
            var data = App.GetListByRoleId(F_RoleId);
            foreach (var item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFullNameById(string F_Id)
        {
            var data = App.GetListById(F_Id);
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            pagination.Sidx = "F_CreatorTime desc";
            var data = new
            {
                rows = App.GetList(pagination, keyword),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetCheckBoxJson(string keyword)
        {
            var data = App.GetList();
            var checkedBox = new SysUserRoleAppService().GetListByUserId(keyword);
            var roleIds = string.Empty;
            foreach (var s in checkedBox)
            {
                roleIds += s.F_Role + ",";
            }
            var list = new List<CheckBoxSelectModel>();
            foreach (var r in data)
            {
                var fieldItem = new CheckBoxSelectModel();
                fieldItem.value = r.F_Id;
                fieldItem.text = r.F_FullName;
                if (roleIds.IndexOf(r.F_Id, StringComparison.Ordinal) != -1)
                    fieldItem.ifChecked = true;
                list.Add(fieldItem);
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.Get(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Role roleEntity, string permissionIds2, string permissionIds3, string permissionIds4, string orgids, string keyValue)
        {
            if ("Diy".Equals(roleEntity.F_Data_Type))
                roleEntity.F_Data_Deps = orgids;
            else
                roleEntity.F_Data_Deps = string.Empty;
            App.SubmitForm(roleEntity, permissionIds2.Split(','), permissionIds3.Split(','), permissionIds4.Split(','), keyValue);
            CacheFactory.Cache().RemoveCache();
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
                App.DeleteForm(F_Id[i]);
            }
            CacheFactory.Cache().RemoveCache();
            return Message("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export()
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();
            var exportSql = CreateExportSql("Sys_Role", parms);
            var dbParameter = CreateParms(parms);
            var users = App.GetDataTable(App.DataScopeFilter(exportSql), dbParameter);
            var ms = new NPOIExcel().ToExcelStream(users, "角色表");
            ms.Seek(0, SeekOrigin.Begin);
            var filename = "角色表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }
    }
}