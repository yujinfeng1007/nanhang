using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 机构管理
    /// </summary>
    public class OrganizeController : ZhxyController
    {
        private OrgService App { get; }
        private UserService UserApp { get; }
        private RoleService RoleApp { get; }

        public OrganizeController(OrgService app, UserService userApp, RoleService roleApp)
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
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeSelectModel
                {
                    id = item.Id,
                    text = item.Name,
                    parentId = item.ParentId,
                    data = item
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
                data = data.Where(t => t.CategoryId == keyword).ToList();
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
            var role = RoleApp.GetById(keyword);
            if (!role.IsEmpty())
            {
                data_deeps = role.DataDeps;
            }
            else
            {
                var user = UserApp.GetById(keyword);
                if (!user.IsEmpty())
                    data_deeps = user.DataDeps;
            }

            var treeList = new List<TreeViewModel>();
            foreach (var item in data)
            {
                var tree = new TreeViewModel();
                var hasChildren = data.Count(t => t.ParentId == item.Id) != 0;
                tree.id = item.Id;
                tree.text = item.Name;
                tree.value = item.EnCode;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = false;
                tree.hasChildren = hasChildren;
                tree.showcheck = true;
                //tree.img = "";
                if (!data_deeps.IsEmpty() && (data_deeps.IndexOf(item.Id, StringComparison.Ordinal) != -1))
                    tree.checkstate = 1;
                else
                    tree.checkstate = 0;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        public ActionResult GetTreeGridJson(string keyword, string divisId, string gradeId, string classId, string schoolId)
        {
            var data = App.GetList();
            if (!string.IsNullOrEmpty(keyword))
                data = data.TreeWhere(t => t.Name.Contains(keyword));
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeGridModel();
                var hasChildren = data.Count(t => t.ParentId == item.Id) != 0;
                treeModel.id = item.Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.ParentId;
                treeModel.expanded = false;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
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