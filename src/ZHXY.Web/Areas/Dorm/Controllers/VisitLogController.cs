using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    public class VisitLogController : BaseController
    {
        private VisitorService App { get; }

        public VisitLogController(VisitorService app) => App = app;


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
            return Result.Success(data);
        }

        [HttpPost]
        public ActionResult UploadImg()
        {
            var approveFilepath = string.Empty;//审批后的头像
            var existen = string.Empty;
            var mapPath = ConfigurationManager.AppSettings["MapPath"] + DateTime.Now.ToString("yyyyMMdd") + "/";
            var basePath = Server.MapPath(mapPath);
            var files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
                var random = RandomHelper.GetRandom();
                var todayStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                for (var i = 0; i < files.Count; i++)
                {
                    var strRandom = random.Next(1000, 10000).ToString(); //生成编号
                    var uploadName = $"{todayStr}{strRandom}";
                    existen = files[i].FileName.Substring(files[i].FileName.LastIndexOf('.') + 1);
                    var fullPath = $"{basePath}{uploadName}.{existen}";
                    files[i].SaveAs(fullPath);
                    approveFilepath = $"http://{Request.Url.Host}:{Request.Url.Port}{mapPath}{uploadName}.{existen}";
                }
            }
            return Result.Success(approveFilepath);
        }


        /// <summary>
        /// 提交访客信息
        /// </summary>
        [HttpPost]
        public ActionResult SubmitVisitor(VisitorApplySubmitDto input)
        {
            var img = ConfigurationManager.AppSettings["NH_DEFAULT_IMG"];
            return Result.Success(App.Submit(input, img));
        }

        /// <summary>
        ///  宿管查询所审批访客，学生查询所提交访客
        /// </summary>
        [HttpGet]
        public ActionResult GetVisitorList(VisitorApprovalListDto input)
        {
            input.CurrentUserId = Operator.GetCurrent().Id;
            var data = App.GetVisitorApprovalList(input);
            return Result.PagingRst(data, input.Records, input.Total);            
        }

        /// <summary>
        /// 获取访客审批详情
        /// </summary>
        [HttpGet]
        public ActionResult GetVisitor(string visitId)
        {             
            var data = App.GetVisitorApprovalDetail(visitId, Operator.GetCurrent().Id);
            return Result.Success(data);
        }

        /// <summary>
        /// 访客审批
        /// </summary>
        [HttpPost]
        public ActionResult ApprovalVisitor(VisitorApprovalDto input)
        {
            input.CurrentUserId = Operator.GetCurrent().Id;
            var img = ConfigurationManager.AppSettings["NH_DEFAULT_IMG"];
            return Result.Success(App.Approval(input, img));
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
        /// 查询访客访问的学生
        /// </summary>
        /// <param name="KeyWords"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchStudent(string KeyWords)
        {
            return Content(App.SearchStudents(KeyWords).ToJson());
        }

        /// <summary>
        /// 通过学生ID，查询学生所在的楼栋的宿管信息
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SupervisorByStudent(string StudentId)
        {
            return Result.Success(App.SupervisorByStudent(StudentId));
        }

        /// <summary>
        /// 查询学生额度
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchStudentLimit(string StudentId)
        {
            return Result.Success(App.SearchStudentLimit(StudentId));
        }

        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetForm(keyValue);
            return Content(data.ToJson());
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

        [HttpGet]
        public ActionResult GetBuilding(string KeyWords)
        {
            return Result.Success(App.GetBuilding(KeyWords));
        }

        [HttpGet]
        public ActionResult GetDetail(string id)
        {
            return Result.Success(App.GetDetail(id));
        }
    }
}