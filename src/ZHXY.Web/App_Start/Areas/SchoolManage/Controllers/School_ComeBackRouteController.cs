/*******************************************************************************
 * Author: mario
 * Description: School_ComeBackRoute  Controller类
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
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //接送路线
    public class School_ComeBackRouteController : ControllerBase
    {
        private School_ComeBackRoute_App app = new School_ComeBackRoute_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_Year, string F_ComeBackType, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            pagination.Sord = "asc";
            //排序字段
            pagination.Sidx = "F_SortCode asc";
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_Year, F_ComeBackType, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetJsonByPCC(ComeBackRoute entity)
        {
            var data = app.GetByPCC(entity);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectList()
        {
            ComeBackRoute entity = new ComeBackRoute();
            var data = app.GetByPCC(entity);
            List<object> list = new List<object>();
            foreach (ComeBackRoute item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_Title });
            }
            return Content(list.ToJson());
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
        public ActionResult SubmitForm(ComeBackRoute entity, string keyValue)
        {
            //entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            if (string.IsNullOrEmpty(keyValue))
                entity.F_EnabledMark = true;
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
                ComeBackRoute entity = new ComeBackRoute();
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
                ComeBackRoute entity = new ComeBackRoute();
                entity.F_Id = F_Id[i];
                entity.F_EnabledMark = true;
                app.UpdateForm(entity);
            }
            return Success("启用成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword, string F_Year, string F_ComeBackType)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_Title", keyword);
            if (!Ext.IsEmpty(F_Year))
                parms.Add("F_Year", F_Year);
            if (!Ext.IsEmpty(F_ComeBackType))
                parms.Add("F_ComeBackType", F_ComeBackType);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_ComeBackRoute", parms);
            exportSql += " and t.F_DeleteMark != 'true' ";
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable users = app.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "接送路线列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "接送路线列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<ComeBackRoute>(Server.MapPath(filePath), "School_ComeBackRoute");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}