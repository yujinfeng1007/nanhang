/*******************************************************************************
 * Author: mario
 * Description: School_Charge_Scheme  Controller类
********************************************************************************/

using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //收费标准
    public class School_Charge_SchemeController : ControllerBase
    {
        private School_Charge_Scheme_App app = new School_Charge_Scheme_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Divis_ID, string F_Grade_ID, string F_Class_ID, int? F_Year)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_Divis_ID, F_Grade_ID, F_Class_ID, F_Year),
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
            //var creator = new UserEntity();
            //var modifier = new UserEntity();
            //Dictionary<string, UserEntity>  dic = CacheFactory.Cache().GetCache<Dictionary<string, UserEntity>>(Cons.USERS);
            //if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId,out creator)) {
            //    data.F_CreatorUserId = creator.F_RealName;
            //}
            //if (data.F_LastModifyUserId != null &&　dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            //{
            //    data.F_LastModifyUserId = modifier.F_RealName;
            //}
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ChargeScheme entity, string keyValue)
        {
            entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
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
            List<ChargeScheme> list = app.GetList(pagination, keyword, string.Empty, string.Empty, string.Empty, 0);

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
            //rules.Add("F_ID", new string[] { "标准ID", "" });
            //rules.Add("F_Title", new string[] { "收费主题", "" });
            //rules.Add("F_Type", new string[] { "收费类别", "" });
            //rules.Add("F_Year", new string[] { "年度", "" });
            //rules.Add("F_Divis_ID", new string[] { "学部ID", "" });
            //rules.Add("F_Grade_ID", new string[] { "年级ID", "" });
            //rules.Add("F_Class_ID", new string[] { "班级ID", "" });
            //rules.Add("F_Total_Fee", new string[] { "收费金额", "" });
            //rules.Add("F_Remark", new string[] { "备注", "" });
            //rules.Add("F_SortCode", new string[] { "序号", "" });
            //rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
            //rules.Add("F_EnabledMark", new string[] { "启用标记", "" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "修改者", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除者", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            System.Data.DataTable dt = ListToDataTable(list, rules);

            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Charge_Scheme列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Charge_Scheme列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_ID", new string[] { "标准ID", "" });
            //rules.Add("F_Title", new string[] { "收费主题", "" });
            //rules.Add("F_Type", new string[] { "收费类别", "" });
            //rules.Add("F_Year", new string[] { "年度", "" });
            //rules.Add("F_Divis_ID", new string[] { "学部ID", "" });
            //rules.Add("F_Grade_ID", new string[] { "年级ID", "" });
            //rules.Add("F_Class_ID", new string[] { "班级ID", "" });
            //rules.Add("F_Total_Fee", new string[] { "收费金额", "" });
            //rules.Add("F_Remark", new string[] { "备注", "" });
            //rules.Add("F_SortCode", new string[] { "序号", "" });
            //rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
            //rules.Add("F_EnabledMark", new string[] { "启用标记", "" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "修改者", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除者", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            List<ChargeScheme> list = ExcelToList<ChargeScheme>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}