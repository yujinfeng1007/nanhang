using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
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
            var data = App.Get(id);
            return Resultaat.Success(data);
        }
      
      

        [HttpPost]
        
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            new SysUserLogOnAppService().RevisePassword(userPassword, keyValue);
            return Message("重置密码成功。");
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                var userEntity = new User { Id = F_Id[i], EnabledMark = false };
                App.Update(userEntity);
            }
            return Message("账户禁用成功。");
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                var userEntity = new User { Id = F_Id[i], EnabledMark = true };
                App.Update(userEntity);
            }
            return Message("账户启用成功。");
        }

     
     
        public JsonResult GetUserPassword(string userid, string password)
        {
            var IsOk = App.VerifyPwd(userid, password);
            return Json(IsOk);
        }


        /// <summary>
        /// 获取机构下的用户
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetOrgUsers(string orgId, string keyword) => await Task.Run(() => Resultaat.Success(App.GetByOrg(orgId, keyword)));

    }
}