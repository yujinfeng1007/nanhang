using System.Web.Http;
using ZHXY.Application;
namespace ZHXY.Api.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [CustomApiExceptionFilter]
    [ApiCompressionFilter]
    public abstract class BaseApiController : ApiController
    {
       
    }
}