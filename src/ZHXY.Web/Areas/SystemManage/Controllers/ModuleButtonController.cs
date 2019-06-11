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
        private MenuService App { get; }
        private MenuService ModuleApp { get; }
        public ModuleButtonController(MenuService app, MenuService moduleService)
        {
            App = app;
            ModuleApp = moduleService;
        }
        [HttpGet]
        
        public ActionResult GetTreeSelectJson(string moduleId)
        {
            var data = App.GetButtonList(moduleId);
            var treeList = new List<SelectTree>();
            foreach (var item in data)
            {
                var treeModel = new SelectTree();
                treeModel.Id = item.Id;
                treeModel.Text = item.Name;
                treeModel.ParentId = item.ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        
        public ActionResult GetTreeGridJson(string moduleId)
        {
            var data = App.GetButtonList(moduleId);
            var treeList = new List<GridTree>();
            foreach (var item in data)
            {
                var treeModel = new GridTree();
                var hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
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
            var data = App.GetButtonById(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Button moduleButtonEntity, string keyValue)
        {
            App.SubmitButton(moduleButtonEntity, keyValue);
            RedisCache.Clear();
            RedisCache.Set( SysConsts.AUTHORIZEBUTTON, ModuleApp.GetMenuButtonList());
            return Result.Success();
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteButton(keyValue);
            RedisCache.Clear();
            return Result.Success();
        }

        [HttpGet]
        public ActionResult CloneButton() => View();

        [HttpGet]
        
        public ActionResult GetCloneButtonTreeJson()
        {
            var moduledata = ModuleApp.GetList();
            var buttondata = App.GetButtonList();
            var treeList = new List<ViewTree>();
            foreach (var item in moduledata)
            {
                var tree = new ViewTree();
                var hasChildren = moduledata.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.Id = item.Id;
                tree.Text = item.Name;
                tree.Value = item.Code;
                tree.ParentId = item.ParentId;
                tree.Isexpand = true;
                tree.Complete = true;
                tree.HasChildren = true;
                treeList.Add(tree);
            }
            foreach (var item in buttondata)
            {
                var tree = new ViewTree();
                var hasChildren = buttondata.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.Id = item.Id;
                tree.Text = item.Name;
                tree.Value = item.Code;
                if (item.ParentId == "0")
                {
                    tree.ParentId = item.ModuleId;
                }
                else
                {
                    tree.ParentId = item.ParentId;
                }
                tree.Isexpand = true;
                tree.Complete = true;
                tree.Showcheck = true;
                tree.HasChildren = hasChildren;
                if (item.Icon != string.Empty)
                {
                    tree.Img = item.Icon;
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