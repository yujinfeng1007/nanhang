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
    public class UserController : ControllerBase
    {
        private RoleApp roleApp = new RoleApp();
        private UserApp userApp = new UserApp();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        private Sys_User_Role_App userroleApp = new Sys_User_Role_App();
        private ICache cache = CacheFactory.Cache();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_DutyId, string F_CreatorTime_Start, string F_CreatorTime_Stop, string F_Contains)
        {
            var data = new
            {
                rows = userApp.GetList(pagination, keyword, F_DepartmentId, F_DutyId, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJsonByOrg(string F_DutyId)
        {
            var data = userApp.GetFormByOrg(F_DutyId);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        public ActionResult GetCurrentUser()
        {
            OperatorModel user = OperatorProvider.Provider.GetCurrent();
            if (Ext.IsEmpty(user))
                return null;
            if (user.IsSystem)
                user.Duty = "admin";
            else
                user.Duty = new DutyApp().GetForm(user.Duty).F_EnCode;
            return Content(user.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = userApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(User userEntity, UserLogOn userLogOnEntity, string orgids, string keyValue)
        {
            if ("Diy".Equals(userEntity.F_Data_Type))
                userEntity.F_Data_Deps = orgids;
            else
                userEntity.F_Data_Deps = string.Empty;
            userApp.SubmitForm(userEntity, userLogOnEntity, keyValue);
            //cache.RemoveCache(Cons.USERS);
            //cache.WriteCache(MvcApplication.GetUserList(), Cons.USERS);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateForm(User userEntity, string keyValue)
        {
            userEntity.F_Id = keyValue;
            userApp.UpdateForm(userEntity);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SubmitSetUp(User userEntity)
        {
            userApp.SubmitSetUp(userEntity);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            userApp.DeleteForm(keyValue);
            cache.RemoveCache(Cons.USERS);
            cache.WriteCache(CacheConfig.GetUserList(), Cons.USERS);
            return Success("删除成功。");
        }

        [HttpGet]
        public ActionResult RevisePassword()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        //[ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            userLogOnApp.RevisePassword(userPassword, keyValue);
            return Success("重置密码成功。");
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
                User userEntity = new User();
                userEntity.F_Id = F_Id[i];
                userEntity.F_EnabledMark = false;
                userApp.UpdateForm(userEntity);
            }
            return Success("账户禁用成功。");
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
                User userEntity = new User();
                userEntity.F_Id = F_Id[i];
                userEntity.F_EnabledMark = true;
                userApp.UpdateForm(userEntity);
            }
            return Success("账户启用成功。");
        }

        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }

        ////导出excel
        //[HttpGet]
        //[HandlerAuthorize]
        //public FileResult export(string keyword)
        //{
        //    /////////////////获得数据集合
        //    Pagination pagination = new Pagination();
        //    //排序
        //    pagination.sord = "desc";
        //    //排序字段
        //    pagination.sidx = "F_CreatorTime desc";
        //    pagination.rows = 1000000;
        //    pagination.page = 1;
        //    List<UserEntity> users = userApp.GetList(pagination, keyword, "", "", "");

        // //////////////////定义规则：字段名，表头名称，字典 //字段名->string[]{表头,字典}，若是一般字段 字典为空字符串
        // IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
        // rules.Add("F_Id", new string[] { "编号", "" }); rules.Add("F_RealName", new string[] { "姓名",
        // "" }); rules.Add("F_Gender", new string[] { "性别", "104" }); rules.Add("F_OrganizeId", new
        // string[] { "公司", "F_OrganizeId" }); rules.Add("F_DepartmentId", new string[] { "部门",
        // "F_DepartmentId" }); //rules.Add("F_AreaId", new string[] { "地区", "F_AreaId" });
        // rules.Add("F_RoleId", new string[] { "角色", "F_RoleId" }); rules.Add("F_DutyId", new
        // string[] { "岗位", "F_DutyId" }); rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
        // rules.Add("F_HeadIcon", new string[] { "头像", "" });

        // //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头) System.Data.DataTable dt =
        // ListToDataTable(users, rules);

        //    ///////////////////写流
        //    MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "用户列表");
        //    ms.Seek(0, SeekOrigin.Begin);
        //    string filename = "用户列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //    return File(ms, "application/ms-excel", filename);
        //}

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword, string F_DutyId, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            //if(!Ext.IsEmpty(keyword))
            //    parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("Sys_User", parms);
            if (!Ext.IsEmpty(keyword))
            {
                exportSql += " and t.F_RealName like '%" + keyword + "%' or t.F_Account like '%" + keyword + "%' or t.F_MobilePhone like '%" + keyword + "%' ";
            }
            if (!Ext.IsEmpty(F_DutyId))
            {
                exportSql += "and t.F_DutyId='" + F_DutyId + "'";
            }
            if (!Ext.IsEmpty(F_DepartmentId))
            {
                exportSql += "and t.F_DepartmentId='" + F_DepartmentId + "'";
            }
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
            exportSql += " and t.F_Account != 'admin'";
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable users = userApp.getDataTable(exportSql, dbParameter);
            Dictionary<string, object> roles = CacheConfig.GetRoleListByCache();

            foreach (DataRow item in users.Rows)
            {
                var userroledata = userroleApp.GetListByUserId(item["用户主键"].ToString());
                string RoleId = string.Empty;
                object tmp = string.Empty;
                foreach (SysUserRole userroleentity in userroledata)
                {
                    Role role = roleApp.GetForm(userroleentity.F_Role);
                    if (role != null && roles.TryGetValue(userroleentity.F_Role, out tmp))
                    {
                        RoleId += GetPropertyValue(tmp, "fullname") + ",";
                    }
                }
                item["角色主键"] = RoleId;
            }
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "用户列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "用户列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<User>(Server.MapPath(filePath), "Sys_User");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            userApp.import(list);
            return Success("导入成功。");
        }

        public JsonResult GetUserPassword(string userid, string password)
        {
            bool IsOk = userApp.GetPwd(userid, password);
            return Json(IsOk);
        }
    }
}