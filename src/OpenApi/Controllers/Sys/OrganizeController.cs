
using OpenApi.Parms.Sys.Parms;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ZHXY.Application;
using ZHXY.Domain;

namespace OpenApi.Controllers.Sys
{
    [RoutePrefix("api/Sys/Organize")]
    public class OrganizeController : ApiControllerBase
    {
        private SysOrganizeAppService  orgApp => new SysOrganizeAppService();

        [Route("GetAllOrg")]
        public string PostGetAllOrg(GetAllOrgParam param)
        {
            try
            {
                var datas = new List<SysOrganize>();
                if (!string.IsNullOrEmpty(param.F_ParentId))
                {
                    datas = orgApp.GetListByParentId(param.F_ParentId);
                }
                else
                {
                    datas = orgApp.GetList();
                }
                var objs = datas.Select(t => new
                {
                    t.F_Id,
                    t.F_ParentId,
                    t.F_Layers,
                    t.F_EnCode,
                    t.F_FullName,
                    t.F_ShortName,
                    t.F_CategoryId
                });
                return Success(objs);
            }
            catch (System.Exception ex)
            {
                throw new ExceptionContext("1101", ex.Message);
            }
        }

        [Route("GetCreateTimeOrg")]
        public string PostGetCreateTimeOrg(GetCreateTimeOrgParam param)
        {
            try
            {
                var data = orgApp.GetList(t => t.F_CreatorTime >= param.StartTime && t.F_CreatorTime <= param.EndTime);
                var objs = data.Select(t => new
                {
                    t.F_Id,
                    t.F_ParentId,
                    t.F_Layers,
                    t.F_EnCode,
                    t.F_FullName,
                    t.F_ShortName,
                    t.F_CategoryId
                });
                return Success(objs);
            }
            catch (System.Exception ex)
            {
                throw new ExceptionContext("1101", ex.Message);
            }
        }
    }
}