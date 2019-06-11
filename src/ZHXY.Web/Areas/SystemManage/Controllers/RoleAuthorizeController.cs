using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Web.Shared;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 角色授权
    /// </summary>
    public class RoleAuthorizeController : BaseController
    {
        private MenuService moduleService { get; }
        private RoleAuthorizeService roleAuthorizeService { get; }
        private MenuService buttonService { get; }

        public RoleAuthorizeController(MenuService moduleAppService,
            RoleAuthorizeService roleAuthorizeAppService,
            MenuService buttonAppService)
        {
            moduleService = moduleAppService;
            roleAuthorizeService = roleAuthorizeAppService;
            buttonService = buttonAppService;
        }
        public ActionResult GetPermissionTree(string roleId, string BeLong)
        {
            var moduledata = moduleService.GetList();
            if (!string.IsNullOrEmpty(BeLong))
            {
                moduledata = moduleService.GetList().Where(t => t.BelongSys.Equals(BeLong)).ToList();
            }
            var buttondata = buttonService.GetButtonList();
            var authorizedata = new List<RoleAuthorize>();
            if (!string.IsNullOrEmpty(roleId))
            {
                authorizedata = roleAuthorizeService.GetList(roleId);
            }
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
                tree.Showcheck = true;
                tree.Checkstate = authorizedata.Count(t => t.ItemId == item.Id);
                tree.HasChildren = true;
                tree.Img = item.Icon == string.Empty ? string.Empty : item.Icon;
                treeList.Add(tree);
            }
            foreach (var item in buttondata)
            {
                var tree = new ViewTree();
                var hasChildren = buttondata.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                tree.Id = item.Id;
                tree.Text = item.Name;
                tree.Value = item.Code;
                tree.ParentId = item.ParentId == "0" ? item.ModuleId : item.ParentId;
                tree.Isexpand = true;
                tree.Complete = true;
                tree.Showcheck = true;
                tree.Checkstate = authorizedata.Count(t => t.ItemId == item.Id);
                tree.HasChildren = hasChildren;
                tree.Img = item.Icon == string.Empty ? string.Empty : item.Icon;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
    }
}