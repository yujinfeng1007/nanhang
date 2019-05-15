using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 用户管理
    /// [OK]
    /// </summary>
    public class UserController : ZhxyWebControllerBase
    {
        private UserService App { get; }

        public UserController(UserService app) => App = app;

        #region view

        [HttpGet]
        public ActionResult Info() => View();
        [HttpGet]
        public ActionResult RevisePassword() => View();
        #endregion view

        [HttpGet]
        public ActionResult Load(Pagination pag,string orgId, string keyword)
        {
            var rows = App.GetList(pag, orgId,keyword);
            return Resultaat.PagingRst(rows, pag.Records, pag.Total);
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Resultaat.Success(data);
        }


        [HttpPost]
        public ActionResult Add(AddUserDto dto)
        {
            App.Add(dto);
            return Resultaat.Success();
        }


        [HttpPost]
        public ActionResult Update(UpdateUserDto dto)
        {
            App.Update(dto);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                App.Delete(id.Split(','));
            }
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult RevisePassword(string userPassword, string keyValue)
        {
            App.RevisePassword(userPassword, keyValue);
            return Resultaat.Success(); 
        }

        [HttpPost]
        public ActionResult Disable(string id)
        {
            App.Disable(id);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult Enable(string id)
        {
            App.Enable(id);
            return Resultaat.Success();
        }

        /// <summary>
        /// 获取机构下的用户
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetOrgUsers(string orgId, string keyword) => await Task.Run(() => Resultaat.Success(App.GetByOrg(orgId, keyword)));


        [HttpPost]
        public ActionResult SetRole(string userId,string[] roleId)
        {
            App.SetRole(userId, roleId);
            return Resultaat.Success();
        }


        [HttpGet]
        public ActionResult GetUserData()
        {
            return Resultaat.Success(App.GetUserData(Operator.Current));
        }


       
    }
}