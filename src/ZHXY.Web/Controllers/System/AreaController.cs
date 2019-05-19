using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    /// <summary>
    /// 行政区域管理
    /// [OK]
    /// </summary>
    public class AreaController : ZhxyWebControllerBase
    {
        private PlaceAreaService App { get; }

        public AreaController(PlaceAreaService app) => App = app;

        [HttpGet]
        
        public ActionResult GetTreeSelectJson()
        {
            var data = App.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeSelectModel
                {
                    id = item.Id,
                    text = item.Name,
                    parentId = item.ParentId
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
            foreach (var province in data.Where(a => a.ParentId == "0"))
            {
                var areaProvince = new AreaChild { value = province.Id, label = province.Name };
                var listProvince = new List<AreaChild>();
                foreach (var itemCity in data.Where(b => b.ParentId == province.Id))
                {
                    var areaCity = new AreaChild { value = itemCity.Id, label = itemCity.Name };
                    var listCity = data.Where(c => c.ParentId == itemCity.Id).Select(itemArea => new AreaChild { value = itemArea.Id, label = itemArea.Name }).ToList();
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
            var data = App.GetList().Where(t => t.ParentId == F_ParentId);
            var list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.Id, text = item.Name });
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
                    id = item.Id,
                    text = item.Name,
                    isLeaf = false,
                    parentId = item.ParentId,
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
                var hasChildren = data.Count(t => t.ParentId == item.Id) != 0;
                treeModel.id = item.Id;
                treeModel.text = item.Name;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.ParentId;
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