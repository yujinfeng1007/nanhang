using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : ZhxyWebControllerBase
    {
        private RoleService App { get; }
        public RoleController(RoleService app) => App = app;

        [HttpGet]
        public ViewResult Power() => View();

        [HttpGet]

        public ActionResult Load(string keyword)
        {
            var data = App.GetList(keyword);
            return Resultaat.Success(data);
        }


        [HttpGet]

        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Resultaat.Success(data);
        }

        [HttpPost]

        public ActionResult Update(UpdateRoleDto dto)
        {
            App.Update(dto);
            return Resultaat.Success();
        }

        [HttpPost]

        public ActionResult Add(AddRoleDto dto)
        {
            App.Add(dto);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            App.Delete(id);
            return Resultaat.Success();
        }
        [HttpPost]
        public ActionResult AddRoleUser(string roleId, string[] userId)
        {
            App.AddRoleUser(roleId, userId);
            return Resultaat.Success();
        }
        [HttpPost]
        public ActionResult RemoveRoleUser(string roleId, string[] userId)
        {
            App.RemoveRoleUser(roleId, userId);
            return Resultaat.Success();
        }

        [HttpGet]
        public ActionResult GetRoleFuncs(string roleId)
        {
            var data=App.GetRoleFuncs(roleId);
            return Resultaat.Success(data);
        }
        [HttpPost]
        public ActionResult AddRoleFunc(string roleId,string menuId, string[] funcs)
        {
            App.AddRoleFunc(roleId,menuId, funcs);
            return Resultaat.Success();
        }


        public ActionResult GetExcludeFuncs(string roleId,string menuId)
        {
            var data=App.GetMenuFuncsExcludeRole(roleId, menuId);
            return Resultaat.Success(data);
        }



    }
}