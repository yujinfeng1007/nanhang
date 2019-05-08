/*******************************************************************************
 * Author: mario
 * Description: Sys_User  Controller类
********************************************************************************/

using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //用户表
    public class ParentsController : ControllerBase
    {
        private UserApp userApp = new UserApp();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        private ICache cache = CacheFactory.Cache();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string F_Account, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = userApp.GetParentsList(pagination, F_Account, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop),
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
            var data = userApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult SubmitForm(User userEntity, UserLogOn userLogOnEntity, string keyValue)
        {
            try
            {
                userEntity.F_DepartmentId = "parent";
                userEntity.F_OrganizeId = "1";
                userEntity.F_DutyId = "parentDuty";
                userEntity.F_RoleId = "parent";
                userEntity.F_Account = userEntity.F_MobilePhone;
                userEntity.F_EnabledMark = true;
                userApp.ParentSubmitForm(userEntity, userLogOnEntity, keyValue);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult UpdataSubmitForm(User userEntity, UserLogOn userLogOnEntity, string keyValue)
        {
            userEntity.F_DepartmentId = "parent";
            userEntity.F_OrganizeId = "1";
            userEntity.F_DutyId = "parentDuty";
            userEntity.F_RoleId = "parent";
            userEntity.F_Account = userEntity.F_MobilePhone;
            userEntity.F_EnabledMark = true;
            userApp.SubmitForm(userEntity, userLogOnEntity, keyValue);
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
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            userLogOnApp.RevisePassword(userPassword, keyValue);
            return Success("重置密码成功。");
        }

        //导出excel
        //[HttpGet]
        //[HandlerAuthorize]
        //public FileResult export(string keyword)
        //{
        //    //参数 字段名->string[]{"F_Id",value}
        //    IDictionary<string, string> parms = new Dictionary<string, string>();
        //    //过滤条件
        //    if (!Ext.IsEmpty(keyword))
        //        parms.Add("F_RealName", keyword);

        // DbParameter[] dbParameter = createParms(parms);

        //    string exportSql = createExportSql("Sys_User", parms);
        //    //string exportSql = "";
        //    //Console.WriteLine("exportSql==>" + exportSql);
        //    DataTable users = userApp.getDataTable(exportSql, dbParameter);
        //    ///////////////////写流
        //    MemoryStream ms = new NPOIExcel().ToExcelStream(users, "用户列表");
        //    ms.Seek(0, SeekOrigin.Begin);
        //    string filename = "家长列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //    return File(ms, "application/ms-excel", filename);
        //}

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            //////////////////定义规则：字段名，表头名称，字典
            //字段名->string[]{表头,字典}，若是一般字段 字典为空字符串
            IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
            //rules.Add("F_Id", new string[] { "编号", "" });
            //rules.Add("F_RealName", new string[] { "姓名", "" });
            //rules.Add("F_Gender", new string[] { "性别", "104" });
            //rules.Add("F_OrganizeId", new string[] { "公司", "F_OrganizeId" });
            //rules.Add("F_DepartmentId", new string[] { "部门", "F_DepartmentId" });
            //rules.Add("F_AreaId", new string[] { "地区", "F_AreaId" });
            //rules.Add("F_RoleId", new string[] { "角色", "F_RoleId" });
            //rules.Add("F_DutyId", new string[] { "岗位", "F_DutyId" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_HeadIcon", new string[] { "头像", "" });

            //所有字段代码
            //rules.Add("F_Id", new string[] { "用户主键", "" });
            //rules.Add("F_Account", new string[] { "账户", "" });
            //rules.Add("F_RealName", new string[] { "姓名", "" });
            //rules.Add("F_NickName", new string[] { "呢称", "" });
            //rules.Add("F_HeadIcon", new string[] { "头像", "" });
            //rules.Add("F_Gender", new string[] { "性别", "" });
            //rules.Add("F_Birthday", new string[] { "生日", "" });
            //rules.Add("F_MobilePhone", new string[] { "手机", "" });
            //rules.Add("F_Email", new string[] { "邮箱", "" });
            //rules.Add("F_WeChat", new string[] { "微信", "" });
            //rules.Add("F_ManagerId", new string[] { "主管主键", "" });
            //rules.Add("F_SecurityLevel", new string[] { "安全级别", "" });
            //rules.Add("F_Signature", new string[] { "个性签名", "" });
            //rules.Add("F_OrganizeId", new string[] { "组织主键", "" });
            //rules.Add("F_DepartmentId", new string[] { "部门主键", "" });
            //rules.Add("F_RoleId", new string[] { "角色主键", "" });
            //rules.Add("F_DutyId", new string[] { "岗位主键", "" });
            //rules.Add("F_IsAdministrator", new string[] { "是否管理员", "" });
            //rules.Add("F_SortCode", new string[] { "排序码", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标志", "" });
            //rules.Add("F_EnabledMark", new string[] { "有效标志", "" });
            //rules.Add("F_Description", new string[] { "描述", "" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "创建用户", "" });
            //rules.Add("F_LastModifyTime", new string[] { "最后修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "最后修改用户", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除用户", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            List<User> list = ExcelToList<User>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            userApp.import(list);
            return Success("导入成功。");
        }
    }
}