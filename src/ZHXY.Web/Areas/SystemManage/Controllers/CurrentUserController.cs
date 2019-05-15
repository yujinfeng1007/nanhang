using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;


namespace ZHXY.Web.SystemManage.Controllers
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
            var data = App.GetBindClass(user.Id);
            user.Classes = data.ToJson();
            return Content(user.ToJson());
        }
       
    }
}