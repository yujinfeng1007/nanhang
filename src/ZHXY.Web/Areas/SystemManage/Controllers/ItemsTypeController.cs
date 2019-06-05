using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain.Entity;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 数据字典项管理
    /// [OK]
    /// </summary>
    public class ItemsTypeController : ZhxyController
    {
        private SysDicAppService App { get; }

        public ItemsTypeController(SysDicAppService app) => App = app;

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
        
        public ActionResult GetTreeJson()
        {
            var data = App.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (var item in data)
            {
                var tree = new TreeViewModel();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0;
                tree.id = item.F_Id;
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        
        public ActionResult GetTreeGridJson()
        {
            var data = App.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeGridModel();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysDic itemsEntity, string keyValue)
        {
            App.SubmitForm(itemsEntity, keyValue);
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.DATAITEMS);
            CacheFactory.Cache().WriteCache(CacheService.GetDataItemList(), SmartCampusConsts.DATAITEMS);
            return Result.Success();
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteForm(keyValue);
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.DATAITEMS);
            CacheFactory.Cache().WriteCache(CacheService.GetDataItemList(), SmartCampusConsts.DATAITEMS);
            return Result.Success();
        }
    }
}