using System.Web.Mvc;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class CurrentUserController : Controller
    {
        [HttpGet]
        public ActionResult GetCurrentUser()
        {
            var user = Operator.Current;
            if (user.IsEmpty())
                return null;
            if (user != null && user.IsSystem)
                user.Duty = "admin";
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
                UserId =user.Id
            }.ToJson());
        }
    }
}