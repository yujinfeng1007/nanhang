using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain.Entity;
using ZHXY.Web.Shared;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 数据字典项管理
    /// [OK]
    /// </summary>
    public class ItemsTypeController : BaseController
    {
        private DicService App { get; }
        private DicService DicItemApp { get; }

        public ItemsTypeController(DicService app,DicService dicItemApp)
        {
            App = app;
            DicItemApp = dicItemApp;
        }

        [HttpGet]
        
        public ActionResult GetTreeSelectJson()
        {
            var data = App.GetAll();
            var treeList = new List<SelectTree>();
            foreach (var item in data)
            {
                var treeModel = new SelectTree
                {
                    Id = item.Id,
                    Text = item.Name,
                    ParentId = item.ParentId
                };
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        
        public ActionResult GetTree()
        {
            var data = App.GetAll();
            var treeList = new List<ViewTree>();
            foreach (var item in data)
            {
                var tree = new ViewTree();
                var hasChildren = data.Count(t => t.ParentId == item.Id) != 0;
                tree.Id = item.Id;
                tree.Text = item.Name;
                tree.Value = item.Code;
                tree.ParentId = item.ParentId;
                tree.Isexpand = true;
                tree.Complete = true;
                tree.HasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        
        public ActionResult GetTreeGridJson()
        {
            var data = App.GetAll();
            var treeList = new List<GridTree>();
            foreach (var item in data)
            {
                var treeModel = new GridTree();
                var hasChildren = data.Count(t => t.ParentId == item.Id) != 0;
                treeModel.Id = item.Id;
                treeModel.IsLeaf = hasChildren;
                treeModel.ParentId = item.ParentId;
                treeModel.Expanded = hasChildren;
                treeModel.EntityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetById(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Dic itemsEntity, string keyValue)
        {
            App.Submit(itemsEntity, keyValue);
            RedisCache.Remove(SysConsts.DATAITEMS);
            RedisCache.Set(SysConsts.DATAITEMS, DicItemApp.GetDataItemList());
            return Result.Success();
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            RedisCache.Remove(SysConsts.DATAITEMS);
            RedisCache.Set( SysConsts.DATAITEMS, DicItemApp.GetDataItemList());
            return Result.Success();
        }
    }
}