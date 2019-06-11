using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 机构负责人
    /// </summary>
    public class OrgLeaderController : BaseController
    {
        private OrgLeaderService App { get; }

        public OrgLeaderController(OrgLeaderService app) => App = app;

        [HttpGet]
        public async Task<ViewResult> Select() => await Task.Run(() =>View());

        /// <summary>
        /// 获取机构负责人
        /// </summary>
        [HttpGet]
        
        public async Task<ActionResult> Get(string orgId) => await Task.Run(() => Result.Success(App.Get(orgId)));

        /// <summary>
        /// 添加机构负责人
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Add(AddOrRemoveOrgLeaderDto input) => await Task.Run(() =>
        {
            App.Add(input);
            return Result.Success();
        });

        /// <summary>
        /// 移除机构负责人
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Remove(AddOrRemoveOrgLeaderDto input) => await Task.Run(() =>
        {
            App.Remove(input);
            return Result.Success();
        });
    }
}