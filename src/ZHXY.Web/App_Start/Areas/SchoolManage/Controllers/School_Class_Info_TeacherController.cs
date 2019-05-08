/*******************************************************************************
 * Author: mario
 * Description: School_Class_Info_Teacher  Controller类
********************************************************************************/

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
    //班级任课老师关联表
    public class School_Class_Info_TeacherController : ControllerBase
    {
        private School_Class_Info_Teacher_App app = new School_Class_Info_Teacher_App();

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
        public ActionResult GetGridSelect(string F_ClassID)
        {
            var data = app.GetSelect(F_ClassID);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJsonByF_Leader_Tea(string F_Leader_Tea)
        {
            var data = app.GetForm(F_Leader_Tea);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SubmitForm(ClassTeacher entity, string F_Teacher, string keyValue)
        {
            app.SubmitForm(entity, F_Teacher, keyValue);
            CacheConfig.GetClassTeachersByCache();
            CacheFactory.Cache().RemoveCache(Cons.CLASSTEACHERS);
            CacheFactory.Cache().WriteCache(CacheConfig.GetClassTeachers(), Cons.CLASSTEACHERS);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            CacheFactory.Cache().RemoveCache(Cons.CLASSTEACHERS);
            CacheFactory.Cache().WriteCache(CacheConfig.GetClassTeachers(), Cons.CLASSTEACHERS);
            return Success("删除成功。");
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="keyword">  </param>
        /// <returns>  </returns>
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

            string exportSql = CreateExportSql("School_Class_Info_Teacher", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Class_Info_Teacher列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Class_Info_Teacher列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="filePath"> 文件路径 </param>
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
            List<ClassTeacher> list = ExcelToList<ClassTeacher>(Server.MapPath(filePath), rules);
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        /// <summary>
        /// 根据教师获取班级
        /// </summary>
        /// <param name="F_Teacher"> 老师的ID </param>
        /// <returns>  </returns>
        public ActionResult GetClassSelectJson(string F_Teacher)
        {
            var expression = ExtLinq.True<ClassTeacher>();

            if (!string.IsNullOrWhiteSpace(F_Teacher))
                expression = expression.And(p => p.School_Teachers_Entity.F_Id == F_Teacher);
            var data = app.GetList(expression).GroupBy(t => t.F_ClassID).Select(t => t.First().School_Class_Entity).ToList();

            return Content(data.ToJson());
        }
    }
}