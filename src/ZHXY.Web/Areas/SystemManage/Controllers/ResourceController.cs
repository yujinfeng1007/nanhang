using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class ResourceController : ZhxyController
    {
        private ResourceService App { get; }
        public ResourceController(ResourceService app) => App = app;



        public ActionResult AddMenu(AddMenuDto dto)
        {
            App.AddMenu(dto);
            return Result.Success();
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            App.Delete(id);
            return Result.Success();
        }
        [HttpPost]
        public ActionResult UpdateMenu(UpdateMenuDto dto)
        {
            App.UpdateMenu(dto);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult AddFunc(AddFuncDto dto)
        {
            App.AddFunc(dto);
            return Result.Success();
        }
        [HttpPost]
        public ActionResult UpdateFunc(UpdateFuncDto dto)
        {
            App.UpdateFunc(dto);
            return Result.Success();
        }

        [HttpGet]
        public ActionResult GetUserMenu(string userId)
        {
            var data = App.GetUserMenu(userId);
            return Result.Success(data);
        }

        [HttpGet]
        public ActionResult GetAllMenu()
        {
            var data = App.GetAllMenu();
            return Result.Success(data);
        }
    }
}