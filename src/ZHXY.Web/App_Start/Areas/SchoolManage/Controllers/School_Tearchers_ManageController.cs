/*******************************************************************************
 * Author: mario
 * Description: School_Tearchers_Manage  Controller类
********************************************************************************/

using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //教师日常管理
    public class School_Tearchers_ManageController : ControllerBase
    {
        private School_Tearchers_Manage_App app = new School_Tearchers_Manage_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop, string keyValue)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop, keyValue),
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
            var data = app.GetForm(keyValue);
            //将用户id替换成姓名
            var creator = new User();
            var modifier = new User();
            Dictionary<string, User> dic = CacheFactory.Cache().GetCache<Dictionary<string, User>>(Cons.USERS);
            if (data.F_CreatorUserId != null)
            {
                if (dic.TryGetValue(data.F_CreatorUserId, out creator))
                {
                    data.F_CreatorUserId = creator.F_RealName;
                }
            }
            if (data.F_LastModifyUserId != null)
            {
                if (dic.TryGetValue(data.F_LastModifyUserId, out modifier))
                {
                    data.F_LastModifyUserId = modifier.F_RealName;
                }
            }
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(TearcherManage entity, string keyValue, string F_Teachers_ID)
        {
            if (!string.IsNullOrEmpty(F_Teachers_ID))
                entity.F_Teachers_ID = F_Teachers_ID;
            //entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword)
        {
            /////////////////获得数据集合
            Pagination pagination = new Pagination();
            //排序
            pagination.Sord = "desc";
            //排序字段
            pagination.Sidx = "F_CreatorTime desc";
            pagination.Rows = 1000000;
            pagination.Page = 1;
            List<TearcherManage> list = app.GetList(pagination, keyword, string.Empty, string.Empty, string.Empty, string.Empty);

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
            //rules.Add("F_Id", new string[] { "教师管理记录ID", "" });
            //rules.Add("F_Teachers_ID", new string[] { "教师ID", "" });
            //rules.Add("F_Year", new string[] { "年度", "" });
            //rules.Add("F_Term", new string[] { "学期", "" });
            //rules.Add("F_Type", new string[] { "管理类型", "" });
            //rules.Add("F_Remark", new string[] { "管理备注", "" });
            //rules.Add("F_Fee", new string[] { "涉及费用", "" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "创建人", "" });
            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "修改人", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标志", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除用户", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            System.Data.DataTable dt = ListToDataTable(list, rules);

            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Tearchers_Manage列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Tearchers_Manage列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

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
            //rules.Add("F_Id", new string[] { "教师管理记录ID", "" });
            //rules.Add("F_Teachers_ID", new string[] { "教师ID", "" });
            //rules.Add("F_Year", new string[] { "年度", "" });
            //rules.Add("F_Term", new string[] { "学期", "" });
            //rules.Add("F_Type", new string[] { "管理类型", "" });
            //rules.Add("F_Remark", new string[] { "管理备注", "" });
            //rules.Add("F_Fee", new string[] { "涉及费用", "" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "创建人", "" });
            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "修改人", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标志", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除用户", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            List<TearcherManage> list = ExcelToList<TearcherManage>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}