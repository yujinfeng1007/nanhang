using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.Performance.Controllers
{
    /// <summary>
    /// 考核
    /// </summary>
    public class PerformanceController : ZhxyWebControllerBase
    {
        private PerformanceAppService App { get; }
        public PerformanceController(PerformanceAppService app) => App = app;

        public async Task<ViewResult> SysLogin() => await Task.Run(()=>View());
        public async Task<ViewResult> BuildingAccess() => await Task.Run(()=>View());


        /// <summary>
        /// 获取登录统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginStatistics(GetLoginStatisticsDto input)
        {
            var (list, recordCount, pageCount) = App.GetLoginStatistics(input);
            return Resultaat.PagingRst(list, recordCount, pageCount);
        }

        /// <summary>
        /// 获取登录详情
        /// </summary>
        [HttpGet]
        public ActionResult LoginDetail(GetLoginDetailDto input)
        {
            var list = App.GetLoginDetail(input);
            return Resultaat.Success(list);
        }

        /// <summary>
        /// 获取楼栋进出统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BuildingAccessStatistics(GetBuildingAccessStatisticsDto input)
        {
            var (list, recordCount, pageCount) = App.GetBuildingAccessStatistics(input);
            return Resultaat.PagingRst(list, recordCount, pageCount);
        }

        /// <summary>
        /// 获取楼栋进出详情
        /// </summary>
        public ActionResult BuildingAccessDetail(GetBuildingAccessDetailDto input)
        {
            var list = App.GetBuildingAccessDetail(input);
            return Resultaat.Success(list);
        }
    }


}