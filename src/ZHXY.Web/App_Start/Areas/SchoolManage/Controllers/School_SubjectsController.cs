/*******************************************************************************
 * Author: mario
 * Description: School_Subjects  Controller类
********************************************************************************/

using NFine.Application;
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
    //班级类型
    public class School_SubjectsController : ControllerBase
    {
        private School_Subjects_App app = new School_Subjects_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_Year, string F_Charge_Type, string F_Divis_ID, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            pagination.Sord = "asc";
            //排序字段
            pagination.Sidx = "F_SortCode asc";
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_Year, F_Charge_Type, F_Divis_ID, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetJsonByYDG(Subject entity)
        {
            var data = app.GetByYDG(entity);
            if (data.Count() != 0)
            {
                data = data.Where(t => t.F_SignDutyId.Contains(OperatorProvider.Provider.GetCurrent().Duty)).ToList();
                if (data.Count() == 0)
                {
                    throw new Exception("当前无权限报名！");
                }
            }
            Dictionary<string, object> dataitems = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.DATAITEMS);
            string F_YearText = string.Empty;
            object dic = new Dictionary<string, string>();
            if (dataitems.TryGetValue("Class_Type", out dic))
            {
                foreach (var item in data)
                {
                    ((Dictionary<string, string>)dic).TryGetValue(item.F_Class_Type, out F_YearText);
                    item.F_Title = F_YearText;
                }
            }
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            List<object> list = new List<object>();
            Dictionary<string, object> dataitems = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.DATAITEMS);
            string F_YearText = "";
            object dic = new Dictionary<string, string>();
            if (dataitems.TryGetValue("Class_Type", out dic))
            {
                ((Dictionary<string, string>)dic).TryGetValue(data.F_Class_Type, out F_YearText);
                data.F_Title = F_YearText;
                list.Add(new { id = data.F_Id, text = data.F_Title });
            }
            return Content(list.ToJson());
            //return Content(data.ToJson());
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

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJsonByDGY(string F_Divis_ID, string F_Grade_ID, string F_Year)
        {
            var data = app.GetFormByDGY(F_Divis_ID, F_Grade_ID, F_Year);
            List<object> list = new List<object>();
            foreach (Subject item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_Title });
            }
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Subject entity, string keyValue)
        {
            //entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            if (string.IsNullOrEmpty(keyValue))
                entity.F_EnabledMark = true;
            //entity.F_Title = entity.F_Class_Type;
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

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            string[] F_Id = keyValue.Split('|');
            for (int i = 0; i < F_Id.Length - 1; i++)
            {
                Subject entity = new Subject();
                entity.F_Id = F_Id[i];
                entity.F_EnabledMark = false;
                app.UpdateForm(entity);
            }
            return Success("禁用成功。");
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
                Subject entity = new Subject();
                entity.F_Id = F_Id[i];
                entity.F_EnabledMark = true;
                app.UpdateForm(entity);
            }
            return Success("启用成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword, string F_Year, string F_Charge_Type, string F_Divis_ID)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_Title", keyword);
            if (!Ext.IsEmpty(F_Year))
                parms.Add("F_Year", F_Year);
            if (!Ext.IsEmpty(F_Charge_Type))
                parms.Add("F_Charge_Type", F_Charge_Type);
            if (!Ext.IsEmpty(F_Divis_ID))
                parms.Add("F_Divis_ID", F_Divis_ID);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_Subjects", parms);
            exportSql += " and t.F_DeleteMark != 'true' ";
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable users = app.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "班级类型列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "班级类型列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<Subject>(Server.MapPath(filePath), "School_Subjects");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}