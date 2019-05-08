using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class AreaController : ControllerBase
    {
        private AreaApp areaApp = new AreaApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = areaApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (Area item in data)
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
            var data = areaApp.GetList();
            List<AreaChild> list = new List<AreaChild>();
            foreach (Area itemprovince in data.Where(a => a.F_ParentId == "0"))
            {
                AreaChild areaprovince = new AreaChild();
                areaprovince.value = itemprovince.F_Id;
                areaprovince.label = itemprovince.F_FullName;
                List<AreaChild> listprovince = new List<AreaChild>();
                foreach (Area itemcity in data.Where(b => b.F_ParentId == itemprovince.F_Id))
                {
                    AreaChild areacity = new AreaChild();
                    areacity.value = itemcity.F_Id;
                    areacity.label = itemcity.F_FullName;
                    List<AreaChild> listcity = new List<AreaChild>();
                    foreach (Area itemArea in data.Where(c => c.F_ParentId == itemcity.F_Id))
                    {
                        AreaChild areaArea = new AreaChild();
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
            var data = areaApp.GetList().Where(t => t.F_ParentId == F_ParentId);
            List<object> list = new List<object>();
            foreach (Area item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = areaApp.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (Area item in data)
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
            var data = areaApp.GetListByParentId(parentId);
            var treeList = new List<TreeGridModel>();
            foreach (Area item in data)
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
            var data = areaApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Area areaEntity, string keyValue)
        {
            areaApp.SubmitForm(areaEntity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            areaApp.DeleteForm(keyValue);
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

            string exportSql = CreateExportSql("Sys_Area", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = areaApp.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "Sys_Area区域列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "Sys_Area区域列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }
    }
}