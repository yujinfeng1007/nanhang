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
            return Content(user.ToJson());
        }
    }
}