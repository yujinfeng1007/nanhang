using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 行政区域管理
    /// [OK]
    /// </summary>
    public class AreaController : ZhxyWebControllerBase
    {
        private SysPlaceAreaAppService App { get; }

        public AreaController(SysPlaceAreaAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetTreeSelectJson()
        {
            var data = App.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeSelectModel
                {
                    id = item.F_Id,
                    text = item.F_FullName,
                    parentId = item.F_ParentId
                };
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        
        public ActionResult GetTreeChildJson()
        {
            var data = App.GetList();
            var list = new List<AreaChild>();
            foreach (var province in data.Where(a => a.F_ParentId == "0"))
            {
                var areaProvince = new AreaChild { value = province.F_Id, label = province.F_FullName };
                var listProvince = new List<AreaChild>();
                foreach (var itemCity in data.Where(b => b.F_ParentId == province.F_Id))
                {
                    var areaCity = new AreaChild { value = itemCity.F_Id, label = itemCity.F_FullName };
                    var listCity = data.Where(c => c.F_ParentId == itemCity.F_Id).Select(itemArea => new AreaChild { value = itemArea.F_Id, label = itemArea.F_FullName }).ToList();
                    areaCity.children = listCity;
                    listProvince.Add(areaCity);
                }
                areaProvince.children = listProvince;
                list.Add(areaProvince);
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetSelectJsonByCategoryId(string F_ParentId)
        {
            var data = App.GetList().Where(t => t.F_ParentId == F_ParentId);
            var list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = App.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeGridModel
                {
                    id = item.F_Id,
                    text = item.F_FullName,
                    isLeaf = false,
                    parentId = item.F_ParentId,
                    expanded = false,
                    entityJson = item.ToJson()
                };
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
        
        public ActionResult GetLazyTreeGridJson(string keyword, string nodeid, int? n_level)
        {
            var index = 0;
            string parentId;
            if (nodeid.IsEmpty())
            {
                parentId = "0";
            }
            else
            {
                parentId = nodeid;
                if (n_level != null) index = (int)n_level;
            }
            var data = App.GetListByParentId(parentId);
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeGridModel();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0;
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
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(PlaceArea areaEntity, string keyValue)
        {
            App.SubmitForm(areaEntity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteForm(keyValue);
            return Message("删除成功。");
        }

    }
}