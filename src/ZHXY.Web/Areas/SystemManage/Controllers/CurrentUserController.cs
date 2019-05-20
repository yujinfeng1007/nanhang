using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class CurrentUserController : ZhxyController
    {
        private TeacherService App { get; }
        public CurrentUserController(TeacherService app) => App = app;

        [HttpGet]
        public ActionResult GetCurrentUser()
        {
            var user = Operator.GetCurrent();
            if (user.IsEmpty())
                return null;
            if (user != null && user.IsSystem)
                user.DutyId = "admin";
            //老师用户绑定班级
            var classes = App.GetBindClass(user.Id);
            user.Classes = classes.ToJson();
            return Content(new
            {
                user.DutyId,
                user.HeadIcon,
                user.Id,
                user.IsSystem,
                user.Ip,
                user.IpLocation,
                user.LoginTime,
                user.LoginToken,
                user.MobilePhone,
                user.OrganId,
                user.Roles,
                user.SetUp,               
                user.Account,
                user.Name,
                user.Classes,
                UserId =user.Id
            }.ToJson());
        }
    }
}