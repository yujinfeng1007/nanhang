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
        private SysDicAppService App { get; }

        public ItemsTypeController(SysDicAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetTreeSelectJson()
        {
            var data = App.GetList();
            var treeList = new List<SelectTree>();
            foreach (var item in data)
            {
                var treeModel = new SelectTree
                {
                    Id = item.F_Id,
                    Text = item.F_FullName,
                    ParentId = item.F_ParentId
                };
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        
        public ActionResult GetTreeJson()
        {
            var data = App.GetList();
            var treeList = new List<ViewTree>();
            foreach (var item in data)
            {
                var tree = new ViewTree();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0;
                tree.Id = item.F_Id;
                tree.Text = item.F_FullName;
                tree.Value = item.F_EnCode;
                tree.ParentId = item.F_ParentId;
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
            var data = App.GetList();
            var treeList = new List<GridTree>();
            foreach (var item in data)
            {
                var treeModel = new GridTree();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0;
                treeModel.Id = item.F_Id;
                treeModel.IsLeaf = hasChildren;
                treeModel.ParentId = item.F_ParentId;
                treeModel.Expanded = hasChildren;
                treeModel.EntityJson = item.ToJson();
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
            RedisCache.Remove(SysConsts.DATAITEMS);
            RedisCache.Set(SysConsts.DATAITEMS, CacheService.GetDataItemList());
            return Result.Success();
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteForm(keyValue);
            RedisCache.Remove(SysConsts.DATAITEMS);
            RedisCache.Set( SysConsts.DATAITEMS, CacheService.GetDataItemList());
            return Result.Success();
        }
    }
}