
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
    /// 菜单管理
    /// </summary>
    public class ModuleController : BaseController
    {
        private MenuService App { get; }
        public ModuleController(MenuService app) => App = app;

        [HttpGet]
        
        public ActionResult GetTreeSelectJson()
        {
            var data = App.GetList();
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
        
        public ActionResult GetTree(string keyword, string belongSys)
        {
            var data = App.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(belongSys))
                    data = data.TreeWhere(t => t.Name.Contains(keyword) && t.BelongSys == belongSys);
                else
                    data = data.TreeWhere(t => t.Name.Contains(keyword));
            }
            else if (!string.IsNullOrEmpty(belongSys))
            {
                data = data.TreeWhere(t => t.BelongSys == belongSys);
            }

            var treeList = new List<GridTree>();
            foreach (var item in data)
            {
                var treeModel = new GridTree();
                var hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                treeModel.Id = item.Id;
                treeModel.IsLeaf = hasChildren;
                treeModel.ParentId = item.ParentId;
                treeModel.Expanded = false;
                treeModel.EntityJson = item.Serialize();
                treeList.Add(treeModel);
            }
            return Result.PagingRst(treeList.TreeGridJson().Deserialize<object>());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetById(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Module moduleEntity, string keyValue)
        {
            App.Submit(moduleEntity, keyValue);
            RedisCache.Clear();
            return Result.Success();
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            RedisCache.Clear();
            return Result.Success();
        }
    }
}