using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;
using ZHXY.Application.DormServices.Gates;

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


        public JsonResult UpdIco(string userId)
        {
            var filepath = string.Empty;
            var existen = string.Empty;
            var mapPath = Configs.GetValue("MapPath") + DateTime.Now.ToString("yyyyMMdd") + "/";
            var basePath = Server.MapPath(mapPath);
            var files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
                var random = RandomHelper.GetRandom();
                var todayStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                for (var i = 0; i < files.Count; i++)
                {
                    var strRandom = random.Next(1000, 10000).ToString(); //生成编号
                    var uploadName = $"{todayStr}{strRandom}";
                    existen = files[i].FileName.Substring(files[i].FileName.LastIndexOf('.') + 1);
                  
                    var fullPath = $"{basePath}{uploadName}.{existen}";
                    files[i].SaveAs(fullPath);
                    filepath = $"http://{Request.Url.Host}:{Request.Url.Port}{mapPath}{uploadName}.{existen}";
                }
            }
            if(!string.IsNullOrEmpty( filepath))
            {
                App.UpdIco(userId,filepath);
                new UserToGateService().SendUserHeadIco(new string[] { userId });
            }
            return Json(new { state=ResultState.Success, userId, message = "上传成功！", url = filepath});

        }
    }
}