using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 通过宿管查询宿管相关楼栋和楼栋学生相关信息
    /// </summary>
    public class SupervisorController : ZhxyController
    {
        private VisitorService App { get; }

        public SupervisorController(VisitorService app) => App = app;

        /// <summary>
        /// 通过宿管ID，查询宿管管理的楼栋列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDorm()
        {
            return Content(App.GetDorm().ToJson());
        }

        [HttpGet]
        public ActionResult StatisticsByBuild(string BuildingId)
        {
            return Content(App.StatisticsByBuild(BuildingId));
        }
    }
}