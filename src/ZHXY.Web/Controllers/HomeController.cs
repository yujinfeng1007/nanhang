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
        private ReportAppService ReportApp { get; }
        private MenuService ModuleApp { get; }
        private DicService DicItemApp { get; }
        private RoleService RoleApp { get; }
        private OrgService OrgApp { get; }

        public HomeController(ReportAppService reportApp,MenuService moduleApp, DicService dicItemApp,RoleService roleApp,OrgService orgApp)
        {
            ReportApp = reportApp;
            ModuleApp = moduleApp;
            DicItemApp = dicItemApp;
            RoleApp = roleApp;
            OrgApp = orgApp;
        }

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
            return Result.Success(ReportApp.GetDefaultData());
        }


        [HttpGet]
        public ActionResult ClientData(string clientType)
        {
            var data = new
            {
                dataItems = DicItemApp.GetDataItemListByCache(),
                duty = DicItemApp.GetDutyListByCache(),
                organize = OrgApp.GetOrganizeListByCache(),
                role = RoleApp.GetRoleListByCache(),
                authorizeMenu = ModuleApp.GetMenuList(clientType).ToString(),
                authorizeButton = (Dictionary<string, object>)ModuleApp.GetMenuButtonList()
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