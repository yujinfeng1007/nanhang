using NFine.Application.SchoolManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    /// <summary>
    /// 学生分班
    /// </summary>
    public class School_StudentClassController : ControllerBase
    {
        private School_Students_App app = new School_Students_App();

        public ActionResult GetGridJsonBySetClass(Pagination pagination, string keyword, string F_DepartmentId, string F_Grade, string F_Class, string F_Subjects_ID, string F_Year, bool IsSet)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_Grade, F_Year, F_Subjects_ID, F_Class, IsSet),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        public ActionResult SubmitForm(string keyValue, string F_Class_ID, string F_Grade_ID)
        {
            app.UpdClass(keyValue, F_Class_ID, F_Grade_ID);
            return Success("操作成功。");
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<Student>(Server.MapPath(filePath), "School_Students");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            var entitys = new List<Student>();
            foreach (var data in list)
            {
                var id = app.GetIdByStudentNum(data.F_StudentNum);
                entitys.Add(new Student
                {
                    F_Id = id,
                    F_Class_ID = data.F_Class_ID,
                    F_DepartmentId = data.F_Class_ID
                });
            }
            app.UpdClass(entitys);
            return Success("导入成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword, string F_DepartmentId, string F_Grade, string F_Class, string F_Year)
        {
            /////////////////获得数据集合
            Pagination pagination = new Pagination();
            //排序
            pagination.Sord = "desc";
            //排序字段
            pagination.Sidx = "F_DepartmentId asc,F_CreatorTime desc";
            pagination.Rows = 1000000;
            pagination.Page = 1;
            List<Student> list = app.GetList(pagination, keyword, F_DepartmentId, F_Grade, F_Year, null, null, true);

            //////////////////定义规则：字段名，表头名称，字典
            //字段名->string[]{表头,字典}，若是一般字段 字典为空字符串
            IDictionary<string, string[]> rules = new Dictionary<string, string[]>();

            //所有字段代码
            rules.Add("F_Id", new string[] { "学生ID", string.Empty });
            rules.Add("F_Year", new string[] { "年度", string.Empty });
            rules.Add("F_Divis_ID", new string[] { "学部", "F_OrganizeId" });
            rules.Add("F_Grade_ID", new string[] { "年级", "F_OrganizeId" });
            //rules.Add("F_Subjects_ID", new string[] { "班级类型", "" });
            rules.Add("F_Class_ID", new string[] { "班级", "F_OrganizeId" });
            rules.Add("F_StudentNum", new string[] { "学号", string.Empty });
            rules.Add("F_Name", new string[] { "姓名", string.Empty });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            System.Data.DataTable dt = ListToDataTable(list, rules);

            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "学生分班列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "学生分班列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }
    }
}