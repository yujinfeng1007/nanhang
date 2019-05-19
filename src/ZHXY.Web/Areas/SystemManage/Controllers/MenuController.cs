using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class MenuController : ZhxyWebControllerBase
    {
        private MenuService App { get; }
        public MenuController(MenuService app) => App = app;
        [HttpPost]
        public ActionResult Add(AddMenuDto dto)
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
        public ActionResult Update(UpdateMenuDto dto)
        {
            App.Update(dto);
            return Resultaat.Success();
        }
        [HttpGet]
        public ActionResult GetMenu(string nodeId, int n_level = 0)
        {
            var data=App.GetMenu(nodeId, n_level);
            return Resultaat.Success(data);
        }

        [HttpPost]
        public ActionResult AddBtn(AddBtnDto dto)
        {
            App.AddBtn(dto);
            return Resultaat.Success();
        }
        [HttpPost]
        public ActionResult DeleteBtn(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                App.DeleteBtn(id.Split(','));
            }
            return Resultaat.Success();
        }
        [HttpPost]
        public ActionResult UpdateBtn(UpdateBtnDto dto)
        {
            App.UpdateBtn(dto);
            return Resultaat.Success();
        }

        [HttpGet]
        public ActionResult GetMenuBth(string menuId = null)
        {
            var data = App.GetMenuBth(menuId);
            return Resultaat.Success(data);
        }

    }
}