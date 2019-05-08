/*******************************************************************************
 * Author: mario
 * Description: School_Students_StatusLog  Controller类
********************************************************************************/

using NFine.Application.SchoolManage;
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
    //学生学籍管理
    public class School_Students_StatusLogController : ControllerBase
    {
        private School_Students_StatusLog_App app = new School_Students_StatusLog_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop, string F_StudentId)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop, F_StudentId),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue, string F_StudentId)
        {
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(StudentStatusLog entity, string keyValue, string F_StudentId)
        {
            if (!string.IsNullOrEmpty(F_StudentId))
            {
                var datas = new School_Students_App().GetFormNoTracking(F_StudentId);
                entity.F_Class_ID = datas.F_Class_ID;
                entity.F_Divis_ID = datas.F_Divis_ID;
                entity.F_Grade_ID = datas.F_Grade_ID;
                entity.F_Name = datas.F_Name;
                entity.F_StudentNum = datas.F_StudentNum;
                entity.F_StudentId = F_StudentId;

                Student Stuentity = new Student();
                Stuentity.F_Id = datas.F_Id;
                Stuentity.F_CurStatu = entity.F_CurStatu;
                new School_Students_App().UpdateForm(Stuentity);
            }
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
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_Students_StatusLog", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Students_StatusLog列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Students_StatusLog列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_ID", new string[] { "ID", "" });
            //rules.Add("F_StudentId", new string[] { "学生Id", "" });
            //rules.Add("F_Name", new string[] { "姓名", "" });
            //rules.Add("F_StudentNum", new string[] { "学号", "" });
            //rules.Add("F_CurStatu", new string[] { "日常类型", "" });
            //rules.Add("F_Divis_ID", new string[] { "学部ID", "" });
            //rules.Add("F_Grade_ID", new string[] { "年级ID", "" });
            //rules.Add("F_Class_ID", new string[] { "归属班级", "" });
            //rules.Add("F_File", new string[] { "变更凭证", "" });
            //rules.Add("F_Description", new string[] { "变更备注", "" });
            //rules.Add("F_Statu", new string[] { "审批状态", "" });
            //rules.Add("F_Auditor", new string[] { "审批人", "" });
            //rules.Add("F_AuditDTM", new string[] { "审批时间", "" });
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
            List<StudentStatusLog> list = ExcelToList<StudentStatusLog>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}