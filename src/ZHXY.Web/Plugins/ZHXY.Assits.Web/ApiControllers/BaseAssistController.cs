using System.Web.Http;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Assits.Web
{
    /// <summary>
    /// 辅助屏控制器基类
    /// </summary>
    [SchoolCodeAndSnFilter]
    [CustomApiExceptionFilter]
    public abstract class BaseAssistController : ApiController
    {
        protected IHttpActionResult SUCCESS(object data = null)
        {
            var body = null == data ? "[]" : data.ToJson();
            return Json(new ApiResult
            {
                Body = body
            });
        }
    }
}