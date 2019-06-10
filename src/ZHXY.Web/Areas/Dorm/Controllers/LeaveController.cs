using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 请假控制器
    /// </summary>
    public class LeaveController : ZhxyController
    {

        public async Task<ViewResult> ApproveForm() => await Task.Run(() => View());
        public async Task<ViewResult> BulkApproveForm() => await Task.Run(() => View());

        private ILeaveService App { get; }

        public LeaveController(ILeaveService app) => App = app;

        /// <summary>
        /// 获取老师
        /// </summary>
        [HttpGet]
        public ActionResult GetTeachers() => Result.Success(App.GetTeachers());

        /// <summary>
        /// 学生获取审批详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDetail(string id)
        {
            return Result.Success(App.GetOrderDetail(id));
        }


        /// <summary>
        /// 请假申请
        /// </summary>
        [HttpPost]
        public ActionResult Apply(LeaveRequestDto input)
        {
            App.Request(input);
            return Result.Success();
        }
      


        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public ActionResult Get([Required(ErrorMessage = "请假Id不能为空!")]string id)
        {
            return Result.Success(App.GetApprovalDetail(id, Operator.GetCurrent().Id));
        }

        /// <summary>
        /// 请假审批
        /// </summary>
        [HttpPost]
        public ActionResult Approval(LeaveApprovalDto input)
        {
            input.CurrentUserId = Operator.GetCurrent().Id;
            App.Approval(input);
            return Result.Success();
        }

        /// <summary>
        /// 批量审批
        /// </summary>
        [HttpPost]
        public ActionResult OneKeyApproval(OneKeyApprovalDto input)
        {
            input.CurrentUserId = Operator.GetCurrent().Id;
            App.OneKeyApproval(input);
            return Result.Success();
        }

        /// <summary>
        /// 学生端获取请假列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLeaveHistory(GetLeaveHistoryDto input)  
        {
            var data = App.GetLeaveHistory(input);
            return Result.PagingRst(data, input.Records, input.Total);
        }

        /// <summary>
        /// 获取审批列表
        /// </summary>
        [HttpGet]
        public ActionResult Load(GetApprovalListDto input)
        {
            input.CurrentUserId = Operator.GetCurrent().Id;
            var data = App.GetApprovalList(input);
            return Result.PagingRst(data, input.Records, input.Total);
        }

        /// <summary>
        /// 获取审批人列表
        /// </summary>
        [HttpGet]
        public  ActionResult LoadFinals(string search)
        {
            return Result.Success(App.GetFinalJudgeList(search));
        }

        /// <summary>
        /// 添加审批人
        /// </summary>
        [HttpPost]
        public ActionResult AddApprover(AddApproverDto dto)
        {
            App.AddApprover(dto);
            return Result.Success();
        }

        /// <summary>
        /// 获取上级审批信息
        /// </summary>
        [HttpGet]
        public ActionResult GetPreApprove(string id)
        {
            return Result.Success(App.GetPrevApprove(id));
        }

        [HttpPost]
        public ActionResult SuspendLeave(string orderId)
        {
            App.SuspendLeave(orderId);
            return Result.Success();
        }

    }
}