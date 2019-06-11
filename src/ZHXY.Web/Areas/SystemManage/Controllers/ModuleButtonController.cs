using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Web.Shared;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class ModuleButtonController : BaseController
    {
        private SysButtonAppService App { get; }
        private SysModuleAppService moduleAppService { get; }
        public ModuleButtonController(SysButtonAppService app, SysModuleAppService moduleService)
        {
            App = app;
            moduleAppService = moduleService;
        }
        [HttpGet]
        
        public ActionResult GetTreeSelectJson(string moduleId)
        {
            var data = App.GetList(moduleId);
            var treeList = new List<SelectTree>();
            foreach (var item in data)
            {
                var treeModel = new SelectTree();
                treeModel.Id = item.F_Id;
                treeModel.Text = item.F_FullName;
                treeModel.ParentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        
        public ActionResult GetTreeGridJson(string moduleId)
        {
            var data = App.GetList(moduleId);
            var treeList = new List<GridTree>();
            foreach (var item in data)
            {
                var treeModel = new GridTree();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
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
        public ActionResult SubmitForm(SysButton moduleButtonEntity, string keyValue)
        {
            App.SubmitForm(moduleButtonEntity, keyValue);
            RedisCache.Clear();
            RedisCache.Set( SysConsts.AUTHORIZEBUTTON, CacheService.GetMenuButtonList());
            return Result.Success();
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteForm(keyValue);
            RedisCache.Clear();
            return Result.Success();
        }

        [HttpGet]
        public ActionResult CloneButton() => View();

        [HttpGet]
        
        public ActionResult GetCloneButtonTreeJson()
        {
            var moduledata = moduleAppService.GetList();
            var buttondata = App.GetList();
            var treeList = new List<ViewTree>();
            foreach (var item in moduledata)
            {
                var tree = new ViewTree();
                var hasChildren = moduledata.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.Id = item.F_Id;
                tree.Text = item.F_FullName;
                tree.Value = item.F_EnCode;
                tree.ParentId = item.F_ParentId;
                tree.Isexpand = true;
                tree.Complete = true;
                tree.HasChildren = true;
                treeList.Add(tree);
            }
            foreach (var item in buttondata)
            {
                var tree = new ViewTree();
                var hasChildren = buttondata.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.Id = item.F_Id;
                tree.Text = item.F_FullName;
                tree.Value = item.F_EnCode;
                if (item.F_ParentId == "0")
                {
                    tree.ParentId = item.F_ModuleId;
                }
                else
                {
                    tree.ParentId = item.F_ParentId;
                }
                tree.Isexpand = true;
                tree.Complete = true;
                tree.Showcheck = true;
                tree.HasChildren = hasChildren;
                if (item.F_Icon != string.Empty)
                {
                    tree.Img = item.F_Icon;
                }
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpPost]
        
        public ActionResult SubmitCloneButton(string moduleId, string Ids)
        {
            App.SubmitCloneButton(moduleId, Ids);
            RedisCache.Clear();
            return Result.Success();
        }
    }
}