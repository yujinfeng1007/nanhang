using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace ZHXY.Web.Dorm.Controllers
{
    public class VisitLimitController : ZhxyWebControllerBase
    {

        private VisitDormLimitService service => new VisitDormLimitService();
        // GET: api/VisitLimit
        public ActionResult GetGridJson(Pagination pagination, string F_Building, string F_Floor)
        {
            var data = new
            {
                rows = service.GetGridJson(pagination, F_Building, F_Floor),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult GetFloor(string BuildingName)
        {
            return Result(service.GetFloor(BuildingName));
        }

        [HttpPost]
        public ActionResult SubmitForm(int TimesOfWeek, string Organ, string OrganGrade, string OrganCourts, string OrganClass, int AutoSet)
        {
            service.SubmitForm(TimesOfWeek, Organ, OrganGrade, OrganCourts, OrganClass, AutoSet);
            return Message("操作成功。");
        }

        /// <summary>
        /// 查询学院接口
        /// </summary>
        /// <param name="OrganName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindOrgan(string OrganName)
        {
            return Result(service.FindOrgan(OrganName));
        }

        /// <summary>
        /// 通过上级ID和当前组织机构名称，查询年级信息
        /// </summary>
        /// <param name="OrganId">上级组织机构ID</param>
        /// <param name="GradeName">年级名称</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindOrganGrade(string OrganId, string GradeName)
        {
            return Result(service.FindOrganGrade(OrganId, GradeName));
        }

        /// <summary>
        /// 通过上级ID和当前组织机构的名称，查询分院信息
        /// </summary>
        /// <param name="GradeId">上级组织机构ID</param>
        /// <param name="CourtName">分院名称</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindOrganCourts(string GradeId, string CourtName)
        {
            return Result(service.FindOrganCourts(GradeId, CourtName));
        }

        /// <summary>
        /// 通过上级ID和当前组织机构的名称，查询班级信息
        /// </summary>
        /// <param name="CourtsId">上级组织机构ID</param>
        /// <param name="ClassName">分院名称</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindOrganClass(string CourtsId, string ClassName)
        {
            return Result(service.FindOrganClass(CourtsId, ClassName));
        }
    }
}
