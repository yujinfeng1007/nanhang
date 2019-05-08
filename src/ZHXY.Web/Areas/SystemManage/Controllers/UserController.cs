using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 用户管理
    /// [OK]
    /// </summary>
    public class UserController : ZhxyWebControllerBase
    {
        private SysUserAppService App { get; }

        public UserController(SysUserAppService app) => App = app;

        #region view

        [HttpGet]
        public ActionResult Info() => View();

        #endregion view

        [HttpGet]
        
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_DutyId, string F_CreatorTime_Start, string F_CreatorTime_Stop, string F_Contains)
        {
            var data = new
            {
                rows = App.GetList(pagination, keyword, F_DepartmentId, F_DutyId, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetList(Pagination pagination, string keyword, string F_DepartmentId, string F_DutyId, string F_CreatorTime_Start, string F_CreatorTime_Stop, string F_Contains)
        {
            var data = new
            {
                rows = App.GetList(pagination, keyword, F_DepartmentId, F_DutyId, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJsonByOrg(string F_DutyId)
        {
            var data = App.GetByOrg(F_DutyId);
            return Content(data.ToJson());
        }

        public ActionResult GetListByOrg(string orgId)
        {
            var data = App.GetByOrg(orgId,false);
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
        public ActionResult SubmitForm(User userEntity, UserLogin userLogOnEntity, string orgids, string keyValue)
        {
            if ("Diy".Equals(userEntity.F_Data_Type))
                userEntity.F_Data_Deps = orgids;
            else
                userEntity.F_Data_Deps = string.Empty;
            App.Submit(userEntity, userLogOnEntity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult UpdateForm(User userEntity, string keyValue)
        {
            userEntity.F_Id = keyValue;
            if (userEntity.F_EnabledMark == null)
                userEntity.F_EnabledMark = true;
            App.Update(userEntity);
            return Message("操作成功。");
        }

        [HttpPost]
        
        public ActionResult SubmitSetUp(User userEntity)
        {
            App.SubmitSetUp(userEntity);
            return Message("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.USERS);
            CacheFactory.Cache().WriteCache(SysCacheAppService.GetUserList(), SmartCampusConsts.USERS);
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

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                var userEntity = new User { F_Id = F_Id[i], F_EnabledMark = false };
                App.Update(userEntity);
            }
            return Message("账户禁用成功。");
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                var userEntity = new User { F_Id = F_Id[i], F_EnabledMark = true };
                App.Update(userEntity);
            }
            return Message("账户启用成功。");
        }

        [HttpGet]
        [HandlerAuthorize]
        public FileResult Export(string keyword, string F_DutyId, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();

            var dbParameter = CreateParms(parms);

            var exportSql = CreateExportSql("Sys_User", parms);
            if (!keyword.IsEmpty())
            {
                exportSql += " and t.F_RealName like '%" + keyword + "%' or t.F_Account like '%" + keyword + "%' or t.F_MobilePhone like '%" + keyword + "%' ";
            }
            if (!F_DutyId.IsEmpty())
            {
                exportSql += "and t.F_DutyId='" + F_DutyId + "'";
            }
            if (!F_DepartmentId.IsEmpty())
            {
                exportSql += "and t.F_DepartmentId='" + F_DepartmentId + "'";
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
            exportSql += " and t.F_Account != 'admin'";
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
            var ms = new NPOIExcel().ToExcelStream(users, "用户列表");
            ms.Seek(0, SeekOrigin.Begin);
            var filename = "用户列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Import(string filePath)
        {
            var list = ExcelToList<User>(Server.MapPath(filePath), "Sys_User");
            if (list == null)
                return Error("导入失败");
            App.Import(list);
            return Message("导入成功。");
        }

        public JsonResult GetUserPassword(string userid, string password)
        {
            var IsOk = App.VerifyPwd(userid, password);
            return Json(IsOk);
        }


        [HttpGet]
        public async Task<ActionResult> GetByOrg(string orgId,string keyword) => await Task.Run(() => Resultaat.Success(App.GetUserByOrg(orgId, keyword)));
    }
}