using System.Web.Mvc;
using ZHXY.Application;
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

        /// <summary>
        /// 机构移动端首页数据接口
        /// </summary>
        [HttpPost]
        public ActionResult loadByOrg(string orgId)
        {
            return Result.Success(app.GetMobileDataByOrg(orgId));
        }
      
    }
}