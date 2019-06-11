
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
        private SysModuleAppService App { get; }
        public ModuleController(SysModuleAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetTreeSelectJson()
        {
            var data = App.GetList();
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
        
        public ActionResult GetTree(string keyword, string F_BelongSys)
        {
            var data = App.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(F_BelongSys))
                    data = data.TreeWhere(t => t.F_FullName.Contains(keyword) && t.F_BelongSys == F_BelongSys);
                else
                    data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            }
            else if (!string.IsNullOrEmpty(F_BelongSys))
            {
                data = data.TreeWhere(t => t.F_BelongSys == F_BelongSys);
            }

            var treeList = new List<GridTree>();
            foreach (var item in data)
            {
                var treeModel = new GridTree();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.Id = item.F_Id;
                treeModel.IsLeaf = hasChildren;
                treeModel.ParentId = item.F_ParentId;
                treeModel.Expanded = false;
                treeModel.EntityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Result.PagingRst(treeList.TreeGridJson().Deserialize<object>());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysModule moduleEntity, string keyValue)
        {
            App.SubmitForm(moduleEntity, keyValue);
            RedisCache.Clear();
            return Result.Success();
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteForm(keyValue);
            RedisCache.Clear();
            return Result.Success();
        }
    }
}