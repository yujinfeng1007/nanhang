using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    public class CurrentUserController : ZhxyWebControllerBase
    {
        private TeacherService App { get; }
        public CurrentUserController(TeacherService app) => App = app;

        [HttpGet]
        public ActionResult GetCurrentUser()
        {
            var user = Operator.Current;
            if (user.IsEmpty())
                return null;
            if (user != null && user.IsSystem)
                user.Duty = "admin";
            //老师用户绑定班级
            var classes = App.GetBindClass(user.Id);
            user.Classes = classes.ToJson();
            return Content(new
            {
                user.Duty,
                user.HeadIcon,
                user.Id,
                user.IsSystem,
                user.LoginIPAddress,
                user.LoginIPAddressName,
                user.LoginTime,
                user.LoginToken,
                user.MobilePhone,
                user.Organ,
                user.Roles,
                user.SetUp,               
                user.UserCode,
                user.UserName,
                user.Classes,
                UserId =user.Id
            }.ToJson());
        }
    }
}