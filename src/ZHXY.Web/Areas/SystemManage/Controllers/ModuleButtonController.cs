using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class ModuleButtonController : ZhxyWebControllerBase
    {
        private SysButtonAppService App { get; }
        public ModuleButtonController(SysButtonAppService app) => App = app;
        [HttpGet]
        
        public ActionResult GetTreeSelectJson(string moduleId)
        {
            var data = App.GetList(moduleId);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        
        public ActionResult GetTreeGridJson(string moduleId)
        {
            var data = App.GetList(moduleId);
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeGridModel();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
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
        public ActionResult SubmitForm(SysButton moduleButtonEntity, string keyValue)
        {
            App.SubmitForm(moduleButtonEntity, keyValue);
            CacheFactory.Cache().RemoveCache();
            CacheFactory.Cache().WriteCache(SysCacheAppService.GetMenuButtonList(), SYS_CONSTS.AUTHORIZEBUTTON);
            return Message("操作成功。");
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteForm(keyValue);
            //CacheFactory.Cache().RemoveCache(Cons.AUTHORIZEBUTTON);
            //CacheFactory.Cache().WriteCache(MvcApplication.GetMenuButtonList(), Cons.AUTHORIZEBUTTON);
            CacheFactory.Cache().RemoveCache();
            //CacheFactory.Cache().WriteCache(MvcApplication.GetMenuButtonList(), Cons.AUTHORIZEBUTTON);
            return Message("删除成功。");
        }

        [HttpGet]
        public ActionResult CloneButton() => View();

        [HttpGet]
        
        public ActionResult GetCloneButtonTreeJson()
        {
            var moduledata = new SysModuleAppService().GetList();
            var buttondata = App.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (var item in moduledata)
            {
                var tree = new TreeViewModel();
                var hasChildren = moduledata.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                treeList.Add(tree);
            }
            foreach (var item in buttondata)
            {
                var tree = new TreeViewModel();
                var hasChildren = buttondata.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                if (item.F_ParentId == "0")
                {
                    tree.parentId = item.F_ModuleId;
                }
                else
                {
                    tree.parentId = item.F_ParentId;
                }
                tree.isexpand = true;
                tree.complete = true;
                tree.showcheck = true;
                tree.hasChildren = hasChildren;
                if (item.F_Icon != string.Empty)
                {
                    tree.img = item.F_Icon;
                }
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpPost]
        
        public ActionResult SubmitCloneButton(string moduleId, string Ids)
        {
            App.SubmitCloneButton(moduleId, Ids);
            CacheFactory.Cache().RemoveCache();
            //CacheFactory.Cache().WriteCache(MvcApplication.GetMenuButtonList(), Cons.AUTHORIZEBUTTON);
            return Message("克隆成功。");
        }
    }
}