using NFine.Code;
using NFine.Domain.Entity.SchoolManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class ClassTableController : ControllerBase
    {
        //我的课表
        [HandlerAuthorize]
        public ActionResult MyIndex()
        {
            OperatorModel teacher = OperatorProvider.Provider.GetCurrent();
            if ("teacherDuty".Equals(teacher.Duty))
            {
                List<Teacher> tea = new School_Teachers_App().GetList(t => t.F_User_ID == teacher.UserId).ToList();
                if (tea.Count() == 1)
                {
                    ViewData["id"] = tea.First().F_Id;
                    return View();
                }
                else
                {
                    return Error("没有这个老师");
                }
            }
            else
            {
                return Error("没有这个老师");
            }
        }
    }
}