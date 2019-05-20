using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class CurrentUserController : ZhxyController
    {
        private StudentService studentService { get; }
        private TeacherService App { get; }
        private OrgService orgService { get;  }
        public CurrentUserController(TeacherService app
            , OrgService org,StudentService student)
        {
            orgService = org;
            studentService = student;
            App = app;
        }

        [HttpGet]
        public ActionResult GetCurrentUser()
        {
            var user = Operator.GetCurrent();
            if (user.IsEmpty())
                return null;
            if (user != null && user.IsSystem)
                user.Duty = "admin";
            //老师用户绑定班级
            var classes = App.GetBindClass(user.Id);
            user.Classes = classes.Select(p=>(object)p).ToList();
            var orgName = orgService.GetById(user.Organ)?.Name;
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
                UserId = user.Id,
                OrgName = orgName,//机构名称
                Num = getNum(user.Duty,user.Id)//学工号

            }.ToJson());
        }
        private string getNum(string dutyId, string userId)
        {
            if (dutyId.Contains("student"))
            {
                var stu =studentService.Read<Student>(p => p.UserId == userId).FirstOrDefault();
                return stu?.StudentNumber;
            }
            if (dutyId.Contains("teacher"))
            {
                var tea =App. Read<Teacher>(p => p.UserId == userId).FirstOrDefault();
               return tea?.JobNumber;
            }
            return "";
        }
    }
}