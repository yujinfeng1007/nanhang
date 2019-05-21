using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : ZhxyController
    {
        private RoleService App { get; }
        public RoleController(RoleService app) => App = app;

        [HttpGet]
        public ViewResult Power() => View();
        public ViewResult Menu() => View();

        [HttpGet]

        public ActionResult Load(string keyword)
        {
            var data = App.GetList(keyword);
            return Result.Success(data);
        }


        [HttpGet]

        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Result.Success(data);
        }

        [HttpPost]

        public ActionResult Update(UpdateRoleDto dto)
        {
            App.Update(dto);
            return Result.Success();
        }

        [HttpPost]

        public ActionResult Add(AddRoleDto dto)
        {
            App.Add(dto);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            App.Delete(id);
            return Result.Success();
        }
        [HttpPost]
        public ActionResult AddRoleUser(string roleId, string[] userId)
        {
            App.AddRoleUser(roleId, userId);
            return Result.Success();
        }
        [HttpPost]
        public ActionResult RemoveRoleUser(string roleId, string[] userId)
        {
            App.RemoveRoleUser(roleId, userId);
            return Result.Success();
        }


        [HttpPost]
        public ActionResult RemoveRoleFunc(string roleId, string funcId)
        {
            App.RemoveRoleFunc(roleId, new[] { funcId});
            return Result.Success();
        }
        [HttpGet]
        public ActionResult GetRoleFuncs(string roleId)
        {
            var data=App.GetRoleFuncs(roleId);
            return Result.Success(data);
        }
        [HttpPost]
        public ActionResult AddRoleFunc(string roleId,string[] funcs)
        {
            App.AddRoleFunc(roleId, funcs);
            return Result.Success();
        }

        [HttpGet]
        public ActionResult GetRoleMenus(string roleId)
        {
            var data = App.GetRoleMenus(roleId);
            return Result.Success(data);
        }
        [HttpPost]
        public ActionResult AddRoleMenu(string roleId, string[] menus)
        {
            App.AddRoleMenu(roleId, menus);
            return Result.Success();
        }


        public ActionResult GetExcludeFuncs(string roleId,string menuId)
        {
            var data=App.GetMenuFuncsExcludeRole(roleId, menuId);
            return Result.Success(data);
        }

        public ActionResult GetExcludeMenus(string roleId, string menuId)
        {
            var data = App.GetMenusExcludeRole(roleId, menuId);
            return Result.Success(data);
        }




    }
}