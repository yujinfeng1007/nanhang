/*******************************************************************************
 * Author: mario
 * Description: School_Classroom  Controller类
********************************************************************************/

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
    //教室信息表
    public class School_ClassroomController : ControllerBase
    {
        private School_Classroom_App app = new School_Classroom_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Classroom_Status, string F_Classroom_Type)
        {
            var data = new
            {
                rows = app.GetClassroomsInfo(pagination, keyword, F_Classroom_Status, F_Classroom_Type),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridSelect()
        {
            var data = app.GetList();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMoveClassGridSelect()
        {
            var data = app.GetList(t => t.F_IsMoveClass == true);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridSelectBy_Class(string keyValue)
        {
            var data = app.GetListBy_Class(keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 未绑定的教室列表
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridNoBindSelect()
        {
            var data = app.getlist();
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
        public ActionResult SubmitForm(Classroom entity, string keyValue)
        {
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
        public FileResult export(string keyword, string F_Classroom_Status, string F_Classroom_Type)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            //if(!Ext.IsEmpty(keyword))
            //    parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_Classroom", parms);
            if (!string.IsNullOrEmpty(F_Classroom_Status))
            {
                exportSql += " and t.F_Classroom_Status == '" + F_Classroom_Status + "' ";
            }
            if (!string.IsNullOrEmpty(F_Classroom_Type))
            {
                exportSql += " and t.F_Classroom_Type == '" + F_Classroom_Type + "' ";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                exportSql += " and (t.F_Name like '%" + keyword + "%' or t.F_Classroom_No like '%" + keyword + "%' )";
            }
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Classroom列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Classroom列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            List<Classroom> list = ExcelToList<Classroom>(Server.MapPath(filePath), "School_Classroom");

            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}