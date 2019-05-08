/*******************************************************************************
 * Author: mario
 * Description: School_Area  Controller类
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
    //School_Area
    public class School_AreaController : ControllerBase
    {
        private School_Area_App app = new School_Area_App();

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
        public ActionResult GetTreeSelectJson()
        {
            var data = app.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (SchoolArea item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeChildJson()
        {
            var data = app.GetList();
            List<SchoolAreaChild> list = new List<SchoolAreaChild>();
            foreach (SchoolArea itemprovince in data.Where(a => a.F_ParentId == "0"))
            {
                SchoolAreaChild areaprovince = new SchoolAreaChild();
                areaprovince.value = itemprovince.F_Id;
                areaprovince.label = itemprovince.F_FullName;
                List<SchoolAreaChild> listprovince = new List<SchoolAreaChild>();
                foreach (SchoolArea itemcity in data.Where(b => b.F_ParentId == itemprovince.F_Id))
                {
                    SchoolAreaChild areacity = new SchoolAreaChild();
                    areacity.value = itemcity.F_Id;
                    areacity.label = itemcity.F_FullName;
                    List<SchoolAreaChild> listcity = new List<SchoolAreaChild>();
                    foreach (SchoolArea itemArea in data.Where(c => c.F_ParentId == itemcity.F_Id))
                    {
                        SchoolAreaChild areaArea = new SchoolAreaChild();
                        areaArea.value = itemArea.F_Id;
                        areaArea.label = itemArea.F_FullName;
                        listcity.Add(areaArea);
                    }
                    areacity.children = listcity;
                    listprovince.Add(areacity);
                }
                areaprovince.children = listprovince;
                list.Add(areaprovince);
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJsonByCategoryId(string F_ParentId)
        {
            var data = app.GetList().Where(t => t.F_ParentId == F_ParentId);
            List<object> list = new List<object>();
            foreach (SchoolArea item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = app.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (SchoolArea item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                //bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.isLeaf = false;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = false;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            return Content(treeList.TreeGridJson());
        }

        //懒加载地区树
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetLazyTreeGridJson(string keyword, string nodeid, int? n_level)
        {
            int index = 0;
            string parentId;
            if (Ext.IsEmpty(nodeid))
            {
                parentId = "0";
            }
            else
            {
                parentId = nodeid;
                index = (int)n_level;
            }
            var data = app.GetListByParentId(parentId);
            var treeList = new List<TreeGridModel>();
            foreach (SchoolArea item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = false;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            return Content(treeList.LazyTreeGridJson(index, parentId));
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SchoolArea entity, string keyValue)
        {
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
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

            string exportSql = CreateExportSql("School_Area", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Area列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Area列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<SchoolArea>(Server.MapPath(filePath), "School_Area");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}