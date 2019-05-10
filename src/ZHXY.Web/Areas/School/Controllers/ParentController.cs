using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.School.Controllers
{
    /// <summary>
    /// 家长管理
    /// </summary>
    public class ParentController : ZhxyWebControllerBase
    {
        private SysUserAppService App { get; }

        public ParentController(SysUserAppService app) => App = app;

        [HttpGet]

        public ActionResult GetGridJson(Pagination pagination, string F_Account, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = App.GetParentsList(pagination, F_Account, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop);
            return PagingResult(data, pagination.Records, pagination.Total);

        }

        [HttpGet]

        public ActionResult GetFormJson(string keyValue) => Content(App.Get(keyValue).ToJson());

        [HttpPost]

        public ActionResult SubmitForm(User userEntity, UserLogin userLogOnEntity, string keyValue)
        {
            userEntity.F_DepartmentId = "parent";
            userEntity.F_OrganizeId = "1";
            userEntity.F_DutyId = "parentDuty";
            userEntity.F_RoleId = "parent";
            userEntity.F_Account = userEntity.F_MobilePhone;
            userEntity.F_EnabledMark = true;
            App.ParentSubmit(userEntity, userLogOnEntity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult UpdataSubmitForm(User userEntity, UserLogin userLogOnEntity, string keyValue)
        {
            userEntity.F_DepartmentId = "parent";
            userEntity.F_OrganizeId = "1";
            userEntity.F_DutyId = "parentDuty";
            userEntity.F_RoleId = "parent";
            userEntity.F_Account = userEntity.F_MobilePhone;
            userEntity.F_EnabledMark = true;
            App.Submit(userEntity, userLogOnEntity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            CacheFactory.Cache().RemoveCache(SYS_CONSTS.USERS);
            CacheFactory.Cache().WriteCache(SysCacheAppService.GetUserList(), SYS_CONSTS.USERS);
            return Message("删除成功。");
        }

        [HttpGet]
        public ActionResult RevisePassword() => View();

        [HttpPost]

        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            new SysUserLogOnAppService().RevisePassword(userPassword, keyValue);
            return Message("重置密码成功。");
        }



        //导入excel
        [HttpPost]

        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Import(string filePath)
        {
            var list = ExcelToList<User>(Server.MapPath(filePath), "Sys_User");
            if (list == null) return Error("导入失败");
            App.Import(list);
            return Message("导入成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();

            var dbParameter = CreateParms(parms);

            var exportSql = CreateExportSql("Sys_User", parms);
            if (!keyword.IsEmpty())
            {
                exportSql += " and t.F_RealName like '%" + keyword + "%' or t.F_Account like '%" + keyword + "%' or t.F_MobilePhone like '%" + keyword + "%' ";
            }
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
            exportSql += " and t.F_DepartmentId='parent' and t.F_Account != 'admin'";
            var users = App.GetDataTable(exportSql, dbParameter);
            var roles = SysCacheAppService.GetRoleListByCache();

            foreach (DataRow item in users.Rows)
            {
                var useragent = new SysUserRoleAppService().GetListByUserId(item["用户主键"].ToString());
                var RoleId = string.Empty;
                object tmp = string.Empty;
                foreach (var adventitious in useragent)
                {
                    var role = new SysRoleAppService().Get(adventitious.F_Role);
                    if (role != null && roles.TryGetValue(adventitious.F_Role, out tmp))
                    {
                        RoleId += GetPropertyValue(tmp, "fullname") + ",";
                    }
                }
                item["角色主键"] = RoleId;
            }
            var ms = new NPOIExcel().ToExcelStream(users, "家长列表");
            ms.Seek(0, SeekOrigin.Begin);
            var filename = "家长列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }
    }
}