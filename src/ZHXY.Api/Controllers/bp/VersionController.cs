using ZHXY.Common;
using System.Web.Http;
using Newtonsoft.Json;
using ZHXY.Application;
using ZHXY.Application.RequestDto.Api;

namespace ZHXY.Api.Controllers
{
    /// <summary>
    /// 版本
    /// </summary>
    [RoutePrefix("Api/School_Version")]
    public class VersionController : BaseBpController
    {
        /// <summary>
        /// 3.3.检测是否有新的版本信息
        /// </summary>
        [HttpPost]
        [Route("CheckVersion")]
        public IHttpActionResult CheckVersion(CheckVersionInput input)
        {
            var iinter = new ApiResult();
            var Url = "" + Configs.GetValue("port") + "/Api/School_Version/checkVersion";
            var isPost = true;
            try
            {
                var parament = "F_currentVersion=" + input.F_currentVersion + "";
                var getversion = WebHelper.SendRequest(Url, parament, isPost).Replace("\r\n", "");
                return Json(JsonConvert.DeserializeObject(getversion, iinter.GetType()));
            }
            catch (System.Exception ex)
            {
                iinter.IsError = true;
                iinter.ErrorCodeValue = "0001";
                iinter.ErrorMsgInfo = ex.Message;
                iinter.Body = "";
                return Json(iinter);
            }
        }
    }
}