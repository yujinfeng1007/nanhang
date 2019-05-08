using System.Web.Http;
using ZHXY.Application;
namespace ZHXY.Api.Controllers
{
    /// <summary>
    /// 学生
    /// </summary>
    [RoutePrefix("Api/Students")]
    public class StudentsController : BaseApiController
    {
        /// <summary>
        /// 获取学生列表
        /// </summary>
        [Route("GetList")]
        [HttpGet]
        public IHttpActionResult GetList(string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var app = new StudentAppService();
            var data = app.GetList(F_CreatorTime_Start, F_CreatorTime_Stop);
            return Json(data);
        }
    }
}