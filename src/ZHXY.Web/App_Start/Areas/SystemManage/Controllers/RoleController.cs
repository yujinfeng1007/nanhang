using NFine.Application;
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
    public class RoleController : ControllerBase
    {
        private RoleApp roleApp = new RoleApp();
        private RoleAuthorizeApp roleAuthorizeApp = new RoleAuthorizeApp();
        private ModuleApp moduleApp = new ModuleApp();
        private ModuleButtonApp moduleButtonApp = new ModuleButtonApp();
        private ICache cache = CacheFactory.Cache();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJson(string F_RoleId)
        {
            List<object> list = new List<object>();
            var data = roleApp.GetListByRoleId(F_RoleId);
            foreach (Role item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            //for (int i = 0; i < RoleId.Length; i++)
            //{
            //    var datas = roleApp.GetForm(RoleId[i]);
            //    list.Add(new { id = datas.F_Id, text = datas.F_FullName });
            //}

            //var data = roleApp.GetList();

            //foreach (RoleEntity item in data)
            //{
            //    list.Add(new { id = item.F_Id, text = item.F_FullName });
            //}
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFullNameById(string F_Id)
        {
            var data = roleApp.GetListById(F_Id);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = roleApp.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCheckBoxJson(string keyword)
        {
            var data = roleApp.GetList();
            var checkedBoxs = new Sys_User_Role_App().GetListByUserId(keyword);
            string roleIds = string.Empty;
            foreach (SysUserRole s in checkedBoxs)
            {
                roleIds += s.F_Role + ",";
            }
            List<CheckBoxSelectModel> list = new List<CheckBoxSelectModel>();
            foreach (Role r in data)
            {
                CheckBoxSelectModel fieldItem = new CheckBoxSelectModel();
                fieldItem.value = r.F_Id;
                fieldItem.text = r.F_FullName;
                if (roleIds.IndexOf(r.F_Id) != -1)
                    fieldItem.ifChecked = true;
                list.Add(fieldItem);
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = roleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Role roleEntity, string permissionIds, string orgids, string keyValue)
        {
            if ("Diy".Equals(roleEntity.F_Data_Type))
                roleEntity.F_Data_Deps = orgids;
            else
                roleEntity.F_Data_Deps = string.Empty;
            roleApp.SubmitForm(roleEntity, permissionIds.Split(','), keyValue);
            cache.RemoveCache();
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
                roleApp.DeleteForm(F_Id[i]);
            }
            cache.RemoveCache();
            return Success("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export()
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();
            string exportSql = CreateExportSql("Sys_Role", parms);
            DbParameter[] dbParameter = CreateParms(parms);
            DataTable users = roleApp.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "角色表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "角色表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }
    }
}