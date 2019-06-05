using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Web.Controllers
{
    public class MobileDataViewController : Controller
    {
        private ReportAppService app = new ReportAppService(new ZhxyRepository());

        /// <summary>
        /// 学生移动端首页数据接口
        /// </summary>
        [HttpPost]        
        public ActionResult loadByStu(string userId,string startTime,string endTime)
        {                     
            return Result.Success(app.GetMobileDataByStu(userId, startTime, endTime));
        }

        [HttpPost]
        public ActionResult ListOrgan(string userId)
        {
            return Result.Success(app.ListOrgan(userId));
        }

        /// <summary>
        /// 机构移动端首页数据接口
        /// </summary>
        [HttpPost]
        public ActionResult loadByOrg(string orgId)
        {
            return Result.Success(app.loadByOrg(orgId));
            //return Result.Success(app.GetMobileDataByOrg(orgId));
        }
    }
}