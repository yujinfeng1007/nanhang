using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    public class VisitApprovalController : ZhxyController
    {
        private VisitorService App { get; }

        public VisitApprovalController(VisitorService app) => App = app;

        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination)
        {
            var data = new
            {
                rows = App.NotCheckApply(pagination),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        /// <summary>
        /// 访客审批
        /// </summary>
        [HttpPost]
        public ActionResult ApprovalVisitor(VisitorApprovalDto input)
        {
            input.CurrentUserId = Operator.GetCurrent().Id;
            App.Approval(input);
            return Result.Success();
        }


        /// <summary>
        /// 审批类型：  1=通过，2=不通过
        /// </summary>
        /// <param name="VisitLogId"></param>
        /// <param name="UserId"></param>
        /// <param name="CheckType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckVisitor(string VisitLogId, string UserId, int CheckType)
        {
            App.CheckVisitor(UserId, CheckType, VisitLogId);
            return Result.Success();
        }

        /// <summary>
        /// 审批
        /// </summary>
        [HttpPost]
        public ActionResult Approval(string id, bool pass)
        {
            App.Approval(id, pass);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult ApprovalList(string[] ids, int pass)
        {
            App.ApprovalList(ids, pass);
            return Result.Success();
        }
    }
}