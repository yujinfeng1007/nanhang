using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    [LoginAuthentication]
    public class HomeController : Controller
    {
        public ReportAppService App { get; }

        public HomeController(ReportAppService app) => App = app;

        [HttpGet]
        public async Task<ViewResult> Default() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ViewResult> Index() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ViewResult> Import() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ViewResult> About() => await Task.Run(() => View());

        //考勤模块跳转
        [HttpGet]
        public async Task<ViewResult> Kq() => await Task.Run(() => View());


        /// <summary>
        /// 默认页-数据面板-图表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDefaultDataView()
        {
            return Result.Success(App.GetDefaultData());
        }


        [HttpGet]
        public ActionResult ClientData(string clientType)
        {
            var data = new
            {
                dataItems = CacheService.GetDataItemListByCache(),
                duty = CacheService.GetDutyListByCache(),
                organize = CacheService.GetOrganizeListByCache(),
                role = CacheService.GetRoleListByCache(),
                authorizeMenu = CacheService.GetMenuList(clientType).ToString(),
                authorizeButton = (Dictionary<string, object>)CacheService.GetMenuButtonList()
            };
            return Result.Success(data);
        }


        [HttpGet]
        public ActionResult UserInfo()
        {
            var current = Operator.GetCurrent();
            return Result.Success(new
            {
                UserCode = current?.Account,
                UserName = current?.Name,
                current?.HeadIcon
            });
        }
    }
}