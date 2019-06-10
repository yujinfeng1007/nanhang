
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 下发头像控制器
    /// </summary>
    public class XFTXController : ZhxyController
    {
        private UserToGateService App { get; }

        public XFTXController(UserToGateService app) => App = app;


        [HttpGet]
        public async Task<ViewResult> SBYH() => await Task.Run(()=> View());

        /// <summary>
        /// 下发头像
        /// </summary>
        [HttpPost]
        public ActionResult XF(string[] userId)
        {
            App.SendUserHeadIco(userId);
            return Result.Success();
        }

        /// <summary>
        /// 注销头像
        /// </summary>
        [HttpPost]
        public ActionResult ZX(string[] userId)
        {
            return Result.Success();
        }
    }
}