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
    public class LeaveController : ZhxyWebControllerBase
    {
        public LeaveAppService App { get; }
        public LeaveController(LeaveAppService app) => App = app;

        #region 请假

        #region for 家校通


        /// <summary>
        /// 获取老师
        /// </summary>
        [HttpGet]
        public ActionResult GetTeachers() => Resultaat.Success(App.GetTeachers());

        /// <summary>
        /// 获取审批详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDetail([Required(ErrorMessage = "请假Id不能为空!")]string id) => AjaxResult(App.GetRequestDetail(id));


        /// <summary>
        /// 请假申请
        /// </summary>
        [HttpPost]
        public ActionResult Apply(LeaveRequestDto input)
        {
            App.Request(input);
            return Resultaat.Success();
        }
        #endregion

        #region for web
        public async Task<ViewResult> ApproveForm() => await Task.Run(() => View());
        public async Task<ViewResult> BulkApproveForm() => await Task.Run(() => View());


        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public ActionResult Get([Required(ErrorMessage = "请假Id不能为空!")]string id) => Resultaat.Success(App.GetApprovalDetail(id, OperatorProvider.Current.UserId));

        /// <summary>
        /// 请假审批
        /// </summary>
        [HttpPost]
        public ActionResult Approval(LeaveApprovalDto input)
        {
            input.CurrentUserId = OperatorProvider.Current.UserId;
            App.Approval(input);
            return Resultaat.Success();
        }

        /// <summary>
        /// 批量审批
        /// </summary>
        [HttpPost]
        public ActionResult OneKeyApproval(OneKeyApprovalDto input)  
        {
            input.CurrentUserId = OperatorProvider.Current.UserId;
            App.OneKeyApproval(input);
            return Resultaat.Success();
        }

        /// <summary>
        /// 获取请假列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetLeaveHistory(GetLeaveHistoryDto input) => await Task.Run(() =>
        {
            var data= App.GetLeaveHistory(input);
            return Resultaat.PagingRst(data, input.Records,input.Total);
        });

        /// <summary>
        /// 获取审批列表
        /// </summary>
        [HttpGet]
        public ActionResult Load(GetApprovalListDto input)
        {
            input.CurrentUserId = OperatorProvider.Current.UserId;
            var data= App.GetApprovalList(input);
            return  Resultaat.PagingRst(data,input.Records,input.Total);
        }

        /// <summary>
        /// 获取审批人列表
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> LoadFinals(string search)
        {
            return await Task.Run(() => Resultaat.Success(App.GetFinalJudgeList(search)));
        }

        /// <summary>
        /// 添加审批人
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public void AddApprover(AddApproverDto input) => App.AddApprover(input);

        /// <summary>
        /// 获取上级审批信息
        /// </summary>
        [HttpGet]
        public ActionResult GetPreApprove(string id) => Resultaat.Success(App.GetPrevApprove(id));
        #endregion

        #endregion
        
        #region 销假
   

        [HttpGet]
        public async Task<ViewResult> Cancel() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ViewResult> CancelForm() => await Task.Run(() => View());

        [HttpGet]
        public ActionResult CancelList(GetCancelListDto input)
        {
            input.CurrentUserId = OperatorProvider.Current.UserId;
            var data = App.GetCanceList(input);
            return Resultaat.PagingRst(data, input.Records, input.Total);
        }

        /// <summary>
        /// 销假
        /// </summary>
        [HttpPost]
        public ActionResult CancelHoliday(CancelHolidayDto input)
        {
            input.OperatorId = OperatorProvider.Current.UserId;
            App.CancelHoliday(input);
            return Resultaat.Success();
        }
        #endregion
    }
}