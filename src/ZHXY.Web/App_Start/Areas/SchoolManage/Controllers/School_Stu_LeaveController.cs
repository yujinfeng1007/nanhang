using NFine.Code;
using System;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class School_Stu_LeaveController : ControllerBase
    {
        private StudentLeaveService app = new StudentLeaveService();

        /// <summary>
        /// 获取请假申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public ActionResult List(string status, string keyWord, Pagination pag)
        {
            try
            {
                var self = OperatorProvider.Provider.GetCurrent();

                if (self == null || self.RoleId != "teacher") throw new Exception("您不是老师!");

                var list = app.GetListByTeacherID(self.UserId, status, keyWord, pag);

                var data = new
                {
                    rows = list,
                    records = pag.Records,
                    total = pag.Total
                };
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public ActionResult Approving(string id, string approvalOpinion, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
                if (string.IsNullOrEmpty(status)) throw new ArgumentNullException(nameof(status));
                app.Approving(id, approvalOpinion, status);
                return Success("审批完成");
            }
            catch (Exception ex)
            {
                return Error($"审批失败:{ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            try
            {
                var data = app.GetByID(id);
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}