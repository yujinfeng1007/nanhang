using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 南航的异常消息推送控制器
    /// </summary>
    public class NHExceptionPushController : Controller
    {
        private MessageAppService App { get; }
        public NHExceptionPushController(MessageAppService app) => App = app;

       /// <summary>
       ///  异常报表 - 晚归
       /// </summary>
       /// <param name="OrgId"></param>
       /// <param name="ReportDate"></param>
       /// <returns></returns>
        [HttpGet]
        public object GetLateReturnReport(string OrgId, string ReportDate)
        {
            return App.GetLateReturnReport(OrgId, ReportDate);
        }

        /// <summary>
        ///  异常报表 - 未归
        /// </summary>
        /// <param name="OrgId"></param>
        /// <param name="ReportDate"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetNotReturnReport(string OrgId, string ReportDate)
        {
            return App.GetNotReturnReport(OrgId, ReportDate);
        }

        /// <summary>
        ///  异常报表 - 长时间未出
        /// </summary>
        /// <param name="OrgId"></param>
        /// <param name="ReportDate"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetNotOutReport(string OrgId, string ReportDate)
        {
            return App.GetNotOutReport(OrgId, ReportDate);
        }
    }
}