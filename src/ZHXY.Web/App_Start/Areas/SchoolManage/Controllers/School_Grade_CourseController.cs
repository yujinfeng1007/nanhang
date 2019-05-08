/*******************************************************************************
 * Author: mario
 * Description: School_Grade_Course  Controller类
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
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //科目年级表
    public class School_Grade_CourseController : ControllerBase
    {
        private School_Grade_Course_App app = new School_Grade_Course_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop),
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
            // var creator = new object();
            // var modifier = new object();
            // Dictionary<string, object>  dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.USERS);
            //if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId,out creator)) {
            //     data.F_CreatorUserId = creator.GetType().GetProperty("fullname").GetValue(creator, null).ToString();
            // }
            // if (data.F_LastModifyUserId != null &&　dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            // {
            //     data.F_LastModifyUserId = modifier.GetType().GetProperty("fullname").GetValue(modifier, null).ToString();
            // }
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(GradeCourse entity, string keyValue)
        {
            var org = new OrganizeApp().GetForm(keyValue);
            entity.F_Grade_Code = org.F_EnCode;
            entity.F_Grade_Name = org.F_FullName;
            entity.F_School = OperatorProvider.Provider.GetCurrent().CompanyId;
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridSelectByGrade(string F_ClassID)
        {
            var org = new OrganizeApp().GetForm(F_ClassID);

            var data = app.GetListByGrade(org.F_ParentId);
            List<Course> list = new List<Course>();
            foreach (var item in data)
            {
                var Course = new School_Course_App().GetForm(item.F_CourseId);
                list.Add(Course);
            }

            list = new School_Course_App().FilterCourse(list, F_ClassID);
            return Content(list.ToJson());
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

            string exportSql = CreateExportSql("School_Grade_Course", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Grade_Course列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Grade_Course列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_Id", new string[] { "ID", "" });
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
            //rules.Add("F_Memo", new string[] { "备注", "" });
            //rules.Add("F_Name", new string[] { "科目名称", "" });
            //rules.Add("F_Code", new string[] { "科目代码", "" });
            //rules.Add("F_Grade", new string[] { "年级ID", "" });
            //rules.Add("F_Grade_Name", new string[] { "年级名称", "" });
            //rules.Add("F_School", new string[] { "学校ID", "" });
            //rules.Add("F_Grade_Code", new string[] { "年级代码", "" });
            //rules.Add("F_Divis", new string[] { "学部ID", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            List<GradeCourse> list = ExcelToList<GradeCourse>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        // 获取年级科目 by ben
        public ActionResult GetCourseSelectJson(string Grade_ID, string F_Divis_ID)
        {
            var expression = ExtLinq.True<GradeCourse>();
            if (!string.IsNullOrWhiteSpace(Grade_ID))
                expression = expression.And(p => p.F_Grade == Grade_ID);
            if (!string.IsNullOrWhiteSpace(F_Divis_ID))
                expression = expression.And(p => p.F_Divis == F_Divis_ID);
            var data = app.GetList(expression).Select(t => t.School_Course_Entity).GroupBy(t => t.F_Id).Select(t => t.First()).ToList();
            return Content(data.ToJson());
        }
    }
}