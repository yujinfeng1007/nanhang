using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class CurrentUserController : BaseController
    {
        private StudentService StuApp { get; }
        private TeacherService TeacherApp { get; }
        private UserService UserApp { get; }
        private OrgService OrgApp { get;  }        
        public CurrentUserController(TeacherService app
            , OrgService org,StudentService student, UserService user)
        {
            OrgApp = org;
            StuApp = student;
            TeacherApp = app;
            UserApp = user;
        }

        [HttpGet]
        public ActionResult GetCurrentUser()
        {
            var user = Operator.GetCurrent();
            if (user.IsEmpty())
                return null;
            if (user != null && user.IsSystem)
                user.DutyId = "admin";
            //老师用户绑定班级
            var classes = TeacherApp.GetBindClass(user.Id);            
            var orgName = OrgApp.GetById(user.OrganId)?.Name;
            //缓存原因，重新取用户最新头像
            var userLatest = UserApp.GetById(user.Id);
            return Content(new
            {
                Duty= user.DutyId,
                //user.HeadIcon,
                userLatest?.HeadIcon,
                user.Id,
                user.IsSystem,
                LoginIPAddress= user.Ip,
                LoginIPAddressName= user.IpLocation,
                user.LoginTime,
                user.LoginToken,
                user.MobilePhone,
                Organ=  user.OrganId,
                user.Roles,
                user.SetUp,
                UserCode=  user.Account,
                UserName= user.Name,
                //user.Classes,
                Classes = classes.Select(p => new {
                    Id = p.Id,
                    CategoryId = p.Type,
                    Name = p.Name,
                    ParentName = p.Parent.Name
                }).ToList(),
                UserId = user.Id,
                OrgName = orgName,//机构名称
                Num = GetNum(user.DutyId,user.Id)//学工号

            }.ToJson());
        }
        private string GetNum(string dutyId, string userId)
        {
            if (dutyId.Contains("student"))
            {
                var stu =StuApp.Read<Student>(p => p.UserId == userId).FirstOrDefault();
                return stu?.StudentNumber;
            }
            if (dutyId.Contains("teacher"))
            {
                var tea =TeacherApp. Read<Teacher>(p => p.UserId == userId).FirstOrDefault();
               return tea?.JobNumber;
            }
            return "";
        }
    }
}