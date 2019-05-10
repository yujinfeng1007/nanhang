using System.Web.Http;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Api.Controllers
{
    /// <summary>
    /// 机构
    /// </summary>
    public class OrgController : BaseApiController
    {
        /// <summary>
        /// 返回所有有效机构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [IpFilter]
        public IHttpActionResult GetAll()
        {
            var app = new OrgAppService();
            var orgs = app.GetList();
            return Json(orgs);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        [HttpGet]
        public IHttpActionResult GetById(string F_Id)
        {
            if (!F_Id.IsEmpty())
            {
                var app = new SysUserAppService();
                var user = app.Get(F_Id);
                return Json(user);
            }
            else
            {
                return BadRequest("缺少参数F_Id");
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        [HttpGet]
        public IHttpActionResult GetByUser([FromUri]User user)
        {
            if (!user.IsEmpty())
            {
                var app = new SysUserAppService();
                var tmp = app.Get(user.F_Id);
                return Json(tmp);
            }
            else
            {
                return BadRequest("缺少参数F_Id");
            }
        }
    }
}