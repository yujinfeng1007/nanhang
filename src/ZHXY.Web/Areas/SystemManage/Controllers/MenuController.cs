using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class MenuController : ZhxyController
    {
        private MenuService App { get; }
        public MenuController(MenuService app) => App = app;
        [HttpPost]
        public ActionResult Add(AddMenuDto dto)
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
        public ActionResult Update(UpdateMenuDto dto)
        {
            App.Update(dto);
            return Result.Success();
        }
        [HttpGet]
        public ActionResult GetMenu(string nodeId, int n_level = 0)
        {
            var data=App.GetMenu(nodeId, n_level);
            return Result.Success(data);
        }

        [HttpPost]
        public ActionResult AddBth(AddFuncDto dto)
        {
            App.AddFunc(dto);
            return Result.Success();
        }
        [HttpPost]
        public ActionResult DeleteBtn(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                App.DeleteFunc(id.Split(','));
            }
            return Result.Success();
        }
        [HttpPost]
        public ActionResult UpdateBtn(UpdateFuncDto dto)
        {
            App.UpdateFunc(dto);
            return Result.Success();
        }

        [HttpGet]
        public ActionResult GetMenuBth(string menuId = null)
        {
            var data = App.GetMenuFunc(menuId);
            return Result.Success(data);
        }

    }
}