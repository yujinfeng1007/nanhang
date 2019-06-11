using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Web.Shared;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 机构管理
    /// </summary>
    public class OrganizeController : BaseController
    {
        private OrgService App { get; }
        private UserService UserApp { get; }
        private SysRoleAppService RoleApp { get; }

        public OrganizeController(OrgService app, UserService userApp, SysRoleAppService roleApp)
        {
            App = app;
            UserApp = UserApp;
            RoleApp = roleApp;
        }

        [HttpGet]
        public ActionResult GetSelectJson(string F_OrgId)
        {
            var list = new List<object>();
            var data = App.GetListByOrgId(F_OrgId);
            foreach (var item in data)
            {
                list.Add(new { id = item.Id, text = item.Name });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        public ActionResult GetFullNameById(string F_Id)
        {
            var data = App.GetListById(F_Id);
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult GetTreeSelectJson()
        {
            var data = App.GetList();
            var treeList = new List<SelectTree>();
            foreach (var item in data)
            {
                var treeModel = new SelectTree
                {
                    Id = item.Id,
                    Text = item.Name,
                    ParentId = item.ParentId,
                    Data = item
                };
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        /// <summary>
        /// 根据类别出机构下拉（非树状结构）
        /// </summary>
        /// <param name="parentId"> 父机构ID </param>
        /// <param name="keyword">  机构类别ID </param>
        /// <returns>  </returns>
        [HttpGet]
        public ActionResult GetSelectJsonByCategoryId(string keyword, string parentId)
        {
            var data = App.GetList();
            if (!string.IsNullOrEmpty(keyword))
                data = data.Where(t => t.Type == keyword).ToList();
            if (!parentId.IsEmpty())
                data = data.Where(t => t.ParentId == parentId).ToList();
            var list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.Id, text = item.Name });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        public ActionResult GetTreeJson(string keyword)
        {
            var data = App.GetList();
            var data_deeps = "";
            var role = RoleApp.Get(keyword);
            if (!role.IsEmpty())
            {
                data_deeps = role.F_Data_Deps;
            }
            else
            {
                var user = UserApp.GetById(keyword);
            }

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
                tree.Complete = false;
                tree.HasChildren = hasChildren;
                tree.Showcheck = true;
                if (!data_deeps.IsEmpty() && (data_deeps.IndexOf(item.Id, StringComparison.Ordinal) != -1))
                    tree.Checkstate = 1;
                else
                    tree.Checkstate = 0;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        public ActionResult GetTree(string keyword)
        {
            var data = App.GetList();
            if (!string.IsNullOrEmpty(keyword))
                data = data.TreeWhere(t => t.Name.Contains(keyword));
            var treeList = new List<GridTree>();
            foreach (var item in data)
            {
                var treeModel = new GridTree();
                var hasChildren = data.Count(t => t.ParentId == item.Id) != 0;
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
        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Result.Success(data);
        }

        [HttpPost]
        public ActionResult Update(UpdateOrgDto dto)
        {
            App.Update(dto);
            return Result.Success();
        }

        public ActionResult Add(AddOrgDto dto)
        {
            App.Add(dto);
            return Result.Success();
        }

        [HttpPost]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            App.Delete(id);
            return Result.Success();
        }


        /// <summary>
        /// 获取老师机构
        /// author:yujf
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetTeacherOrg(string nodeId = "3", int n_level = 0) => await Task.Run(() =>
             {
                 var result = App.GetTeacherOrg(nodeId, n_level);
                 return Result.Success(result);
             });

        /// <summary>
        /// 获取老师机构
        /// author:yujf
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetStudentOrg(string nodeId = "2", int n_level = 0) => await Task.Run(() =>
        {
            var result = App.GetTeacherOrg(nodeId, n_level);
            return Result.Success(result);
        });

        [HttpGet]
        public async Task<ActionResult> GetSubOrg(string nodeId = null, int n_level = 0) => await Task.Run(() =>
        {
            var result = App.GetChildOrg(nodeId, n_level);
            return Result.Success(result);
        });


    }
}