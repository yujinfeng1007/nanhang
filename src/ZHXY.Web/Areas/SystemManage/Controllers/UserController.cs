using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Web.Shared;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 用户管理
    /// [OK]
    /// </summary>
    public class UserController : BaseController
    {
        private UserService App { get; }

        public UserController(UserService app) => App = app;

        #region view

        [HttpGet]
        public ActionResult Info() => View();

        #endregion view

        [HttpGet]

        public ActionResult GetGridJson(Pagination p, string keyword, string org_id, string duty_id)
        {
            var rows = App.GetList(p, keyword, org_id, duty_id);
            return Result.PagingRst(rows, p.Records, p.Total);
        }


        [HttpGet]

        public ActionResult GetFormJsonByOrg(string F_DutyId)
        {
            var data = App.GetByOrg(F_DutyId);
            return Content(data.ToJson());
        }

        public ActionResult GetListByOrg(string orgId)
        {
            var data = App.GetByOrg(orgId);
            return Content(data.ToJson());
        }

        [HttpGet]

        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetById(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(User userEntity, string F_RoleId, string keyValue)
        {
            App.Submit(userEntity, F_RoleId, keyValue);
            return Result.Success();
        }


        [HttpPost]
        [HandlerAuthorize]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue.Split('|'));
            return Result.Success();
        }

        [HttpGet]
        public ActionResult RevisePassword() => View();

        [HttpPost]

        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            App.RevisePassword(userPassword, keyValue);
            return Result.Success();

        }


        public JsonResult GetUserPassword(string userid, string password)
        {
            var IsOk = App.VerifyPwd(userid, password);
            return Json(IsOk);
        }
    }
}