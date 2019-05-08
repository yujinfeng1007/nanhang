using ZHXY.Application;
using ZHXY.Domain;
using OpenApi.Parms.Sys.Parms;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OpenApi.Controllers.Sys
{
    [RoutePrefix("api/Sys/User")]
    public class UserController : ApiControllerBase
    {
        private SysUserAppService  userApp => new SysUserAppService();

        [Route("GetAllUser")]
        public string PostGetAllUser(GetAllUserParam param)
        {
            try
            {
                var datas = new List<SysUser>();
                if (!string.IsNullOrEmpty(param.F_ParentId))
                {
                    datas = userApp.GetList(t => t.F_OrganizeId == param.F_ParentId);
                }
                else
                {
                    datas = userApp.GetList();
                }
                var objs = datas.Select(t => new
                {
                    t.F_Id,
                    t.F_Account,
                    t.F_RealName,
                    t.F_NickName,
                    t.F_HeadIcon,
                    t.F_Gender,
                    t.F_Birthday,
                    t.F_MobilePhone,
                    t.F_Email,
                    t.F_OrganizeId
                });
                return Success(objs);
            }
            catch (System.Exception ex)
            {
                throw new ExceptionContext("1101", ex.Message);
            }
        }

        [Route("GetCreateTimeUser")]
        public string PostGetCreateTimeUser(GetCreateTimeUserParam param)
        {
            try
            {
                var data = userApp.GetList(t => t.F_CreatorTime >= param.StartTime && t.F_CreatorTime <= param.EndTime);
                return Success(data);
            }
            catch (System.Exception ex)
            {
                throw new ExceptionContext("1101", ex.Message);
            }
        }
    }
}