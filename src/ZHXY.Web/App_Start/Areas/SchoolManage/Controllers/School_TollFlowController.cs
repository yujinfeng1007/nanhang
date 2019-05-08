/*******************************************************************************
 * Author: mario
 * Description: School_TollFlow  Controller类
********************************************************************************/

using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //收费流水表
    public class School_TollFlowController : ControllerBase
    {
        private School_TollFlow_App app = new School_TollFlow_App();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        private ICache cache = CacheFactory.Cache();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string F_Grade_ID, string F_Divis_ID, string F_Class_ID, string F_Charge_Type, string F_PayType, string F_Toll_Account, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_Grade_ID, F_Divis_ID, F_Class_ID, F_Charge_Type, F_PayType, F_Toll_Account, F_CreatorTime_Start, F_CreatorTime_Stop),
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
            //Dictionary<string, UserEntity> dic = CacheFactory.Cache().GetCache<Dictionary<string, UserEntity>>(Cons.USERS);
            //if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId, out creator))
            //{
            //    data.F_CreatorUserId = creator.F_RealName;
            //}
            //if (data.F_LastModifyUserId != null && dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            //{
            //    data.F_LastModifyUserId = modifier.F_RealName;
            //}
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(TollFlow entity, string keyValue)
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
            cache.RemoveCache(Cons.USERS);
            cache.WriteCache(CacheConfig.GetUserList(), Cons.USERS);
            return Success("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string F_Grade_ID, string F_Divis_ID, string F_Class_ID, string F_Charge_Type, string F_PayType, string F_Toll_Account, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(F_Grade_ID))
                parms.Add("F_Grade_ID", F_Grade_ID);
            if (!Ext.IsEmpty(F_Divis_ID))
                parms.Add("F_Divis_ID", F_Divis_ID);
            if (!Ext.IsEmpty(F_Class_ID))
                parms.Add("F_Class_ID", F_Class_ID);
            if (!Ext.IsEmpty(F_Charge_Type))
                parms.Add("F_Charge_Type", F_Charge_Type);
            if (!Ext.IsEmpty(F_PayType))
                parms.Add("F_PayType", F_PayType);
            if (!Ext.IsEmpty(F_Toll_Account))
                parms.Add("F_Toll_Account", F_Toll_Account);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_TollFlow", parms);
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
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable users = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "流水列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "流水列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
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
        //    List<School_TollFlow_Entity> list = app.GetList(pagination, keyword, "", "", "");

        // //////////////////定义规则：字段名，表头名称，字典 //字段名->string[]{表头,字典}，若是一般字段 字典为空字符串
        // IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
        // //rules.Add("F_Id", new string[] { "编号", "" }); //rules.Add("F_RealName", new string[] {
        // "姓名", "" }); //rules.Add("F_Gender", new string[] { "性别", "104" });
        // //rules.Add("F_OrganizeId", new string[] { "公司", "F_OrganizeId" });
        // //rules.Add("F_DepartmentId", new string[] { "部门", "F_DepartmentId" });
        // //rules.Add("F_AreaId", new string[] { "地区", "F_AreaId" }); //rules.Add("F_RoleId", new
        // string[] { "角色", "F_RoleId" }); //rules.Add("F_DutyId", new string[] { "岗位", "F_DutyId"
        // }); //rules.Add("F_CreatorTime", new string[] { "创建时间", "" }); //rules.Add("F_HeadIcon",
        // new string[] { "头像", "" });

        // //所有字段代码 //rules.Add("F_Id", new string[] { "流水ID", "" }); //rules.Add("F_Divis_ID", new
        // string[] { "学部ID", "" }); //rules.Add("F_Grade_ID", new string[] { "年级ID", "" });
        // //rules.Add("F_Class_ID", new string[] { "班级ID", "" }); //rules.Add("F_StudentNum", new
        // string[] { "学号", "" }); //rules.Add("F_Students_ID", new string[] { "学生ID", "" });
        // //rules.Add("F_Fee", new string[] { "收费金额", "" }); //rules.Add("F_Users_ID", new string[]
        // { "付款人ID", "" }); //rules.Add("F_Users_Name", new string[] { "付款人姓名", "" });
        // //rules.Add("F_PayType", new string[] { "支付方式", "" }); //rules.Add("F_BankCode", new
        // string[] { "付款银行编码", "" }); //rules.Add("F_BankName", new string[] { "付款银行名称", "" });
        // //rules.Add("F_PayNum", new string[] { "支付凭证", "" }); //rules.Add("F_Toll_Account", new
        // string[] { "收款账户", "" }); //rules.Add("F_Toll_DTM", new string[] { "收款时间", "" });
        // //rules.Add("F_Toll_Statu", new string[] { "到帐状态", "" }); //rules.Add("F_Confirm_Statu",
        // new string[] { "确认状态", "" }); //rules.Add("F_Auditor", new string[] { "审核人", "" });
        // //rules.Add("F_AuditDTM", new string[] { "审核时间", "" }); //rules.Add("F_SortCode", new
        // string[] { "序号", "" }); //rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
        // //rules.Add("F_DeleteMark", new string[] { "删除标记", "" }); //rules.Add("F_EnabledMark", new
        // string[] { "启用标记", "" }); //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
        // //rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
        // //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
        // //rules.Add("F_LastModifyUserId", new string[] { "修改者", "" }); //rules.Add("F_DeleteTime",
        // new string[] { "删除时间", "" }); //rules.Add("F_DeleteUserId", new string[] { "删除者", "" });

        // //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头) System.Data.DataTable dt =
        // ListToDataTable(list, rules);

        //    ///////////////////写流
        //    MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_TollFlow列表");
        //    ms.Seek(0, SeekOrigin.Begin);
        //    string filename = "School_TollFlow列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_Id", new string[] { "流水ID", "" });
            //rules.Add("F_Divis_ID", new string[] { "学部ID", "" });
            //rules.Add("F_Grade_ID", new string[] { "年级ID", "" });
            //rules.Add("F_Class_ID", new string[] { "班级ID", "" });
            //rules.Add("F_StudentNum", new string[] { "学号", "" });
            //rules.Add("F_Students_ID", new string[] { "学生ID", "" });
            //rules.Add("F_Fee", new string[] { "收费金额", "" });
            //rules.Add("F_Users_ID", new string[] { "付款人ID", "" });
            //rules.Add("F_Users_Name", new string[] { "付款人姓名", "" });
            //rules.Add("F_PayType", new string[] { "支付方式", "" });
            //rules.Add("F_BankCode", new string[] { "付款银行编码", "" });
            //rules.Add("F_BankName", new string[] { "付款银行名称", "" });
            //rules.Add("F_PayNum", new string[] { "支付凭证", "" });
            //rules.Add("F_Toll_Account", new string[] { "收款账户", "" });
            //rules.Add("F_Toll_DTM", new string[] { "收款时间", "" });
            //rules.Add("F_Toll_Statu", new string[] { "到帐状态", "" });
            //rules.Add("F_Confirm_Statu", new string[] { "确认状态", "" });
            //rules.Add("F_Auditor", new string[] { "审核人", "" });
            //rules.Add("F_AuditDTM", new string[] { "审核时间", "" });
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
            List<TollFlow> list = ExcelToList<TollFlow>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}