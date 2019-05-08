using System.Web.Mvc;

namespace ZHXY.Web.Flow.Controllers
{
    public class FlowController : Controller
    {
        ////跳转到流程设计器
        //[HttpGet]
        //[HandlerAuthorize]
        //public virtual ActionResult Index()
        //{
        //    OperatorModel user = OperatorProvider.Current;
        //    if (user.IsEmpty())
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        if (WebUser.No != user.UserCode)
        //        {
        //            var emp = new BP.Port.Emp {No = user.UserCode, FK_Dept = user.DepartmentId};
        //            WebUser.SignInOfGener(emp);
        //            WebUser.IsWap = false;
        //            WebUser.Auth = ""; //设置授权人为空.

        //            BP.WF.Dev2Interface.Port_Login(emp.No);
        //        }
        //        return Redirect("/WF/Admin/CCBPMDesigner/Default.htm?SID=" + user.UserId + "&UserNo=" + user.UserName);
        //    }
        //}

        ////跳转到流程管理界面
        //[HttpGet]
        //[HandlerAuthorize]
        //public virtual ActionResult Console(string path)
        //{
        //    OperatorModel user = OperatorProvider.Current;
        //    if (user.IsEmpty())
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        if (WebUser.No != user.UserCode)
        //        {
        //            BP.Port.Emp emp = new BP.Port.Emp();
        //            emp.No = user.UserCode;
        //            emp.FK_Dept = user.DepartmentId;
        //            WebUser.SignInOfGener(emp);
        //            WebUser.IsWap = false;
        //            WebUser.Auth = ""; //设置授权人为空.

        //            BP.WF.Dev2Interface.Port_Login(emp.No);
        //        }
        //        return Redirect("/WF/" + path);
        //    }
        //}
    }
}