using System.Collections.Generic;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    public class VisitLogController : ZhxyController
    {
        private VisitorAppService App { get; }

        public VisitLogController(VisitorAppService app) => App = app;


        /// <summary>
        /// 表格
        /// </summary>
        /// <param name="pagination">分页相关</param>
        /// <param name="F_Building">楼栋</param>
        /// <param name="Time_Type">时间类型：0 = 未选择时间（默认今日）,1 = 今日，2 = 昨天，3 = 本周，4 = 本月，5 = 上周，6 = 上月，7 = 其他时间，需要手动筛选时间段</param>
        /// <param name="startTime">开始时间： Time_Type为7时生效</param>
        /// <param name="endTime">结束时间：Time_Type为7时生效</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string F_Building, int Time_Type, string startTime, string endTime)
        {

            if (Time_Type != 7) { var DateTimes = DateHelper.GetDateTimes(Time_Type); startTime = DateTimes["startTime"]; endTime = DateTimes["endTime"]; }
            var data = new
            {
                rows = App.GetList(pagination, F_Building, Time_Type, startTime, endTime),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        /// <summary>
        ///  通过当前用户，查询所有访客
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VisivorByStudent( Pagination pag, int status)
        {
            var data = App.VisivorByStudent(pag,Operator.GetCurrent().Id, status);
            return Resultaat.Success(data);
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
            return Message("操作成功");
        }

        /// <summary>
        /// 查询访客访问的学生
        /// </summary>
        /// <param name="KeyWords"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchStudent(string KeyWords)
        {
            return Result(App.SearchStudents(KeyWords).ToJson());
        }

        /// <summary>
        /// 通过学生ID，查询学生所在的楼栋的宿管信息
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SupervisorByStudent(string StudentId)
        {
            return Result(App.SupervisorByStudent(StudentId));
        }

        /// <summary>
        /// 查询学生额度
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchStudentLimit(string StudentId)
        {
            return Result(App.SearchStudentLimit(StudentId));
        }

        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetForm(keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 提交访客信息
        /// </summary>
        [HttpPost]
        public ActionResult Submit(AddVisitApplyDto input)
        {
            App.Submit(input);
            return Resultaat.Success();
        }

        /// <summary>
        /// 审批
        /// </summary>
        [HttpPost]
        public ActionResult Approval(string id, bool pass)
        {
            App.Approval(id, pass);
            return Resultaat.Success();
        }

        [HttpGet]
        public ActionResult GetBuilding(string KeyWords)
        {
            return Result(App.GetBuilding(KeyWords));
        }

        [HttpGet]
        public ActionResult GetDetail(string id)
        {
            return Result(App.GetDetail(id));
        }
    }
}