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
    /// 角色管理
    /// </summary>
    public class RoleController : BaseController
    {
        private RoleService App { get; }
        public RoleController(RoleService app) => App = app;

        [HttpGet]
        public ActionResult GetSelectJson(string F_RoleId)
        {
            var list = new List<object>();
            var data = App.GetListByRoleId(F_RoleId);
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
        public ActionResult List(Pagination p, string keyword)
        {
            var data = App.GetList(p, keyword);
            return Result.PagingRst(data,p.Records,p.Total);
        }

        [HttpGet]
        public ActionResult GetCheckBoxJson(string keyword)
        {
            var allRoles = App.GetList();
            var userRoles = App.GetUserRoles(keyword);
            var list = new List<CheckBox>();
            foreach (var role in allRoles)
            {
                var fieldItem = new CheckBox
                {
                    Value = role.Id,
                    Text = role.Name,
                    IfChecked = userRoles.Contains(role.Id)
                };
                list.Add(fieldItem);
            }
            return Content(list.ToJson());
        }

        [HttpGet]

        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.Get(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Role roleEntity, string permissionIds2, string permissionIds3, string permissionIds4, string orgids, string keyValue)
        {
            if ("Diy".Equals(roleEntity.DataType))
                roleEntity.DataDeps = orgids;
            else
                roleEntity.DataDeps = string.Empty;
            App.SubmitForm(roleEntity, permissionIds2.Split(','), permissionIds3.Split(','), permissionIds4.Split(','), keyValue);
            RedisCache.Clear();
            return Result.Success();
        }

        [HttpPost]

        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                App.DeleteForm(F_Id[i]);
            }
            RedisCache.Clear();
            return Result.Success();
        }


    }
}