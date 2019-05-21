using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Application.DormServices.Gates;
using System.IO;
using System;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 用户管理
    /// [OK]
    /// </summary>
    public class UserController : ZhxyController
    {
        private UserService App { get; }

        public UserController(UserService app) => App = app;

        #region view

        [HttpGet]
        public ActionResult Info() => View();
        [HttpGet]
        public ActionResult RevisePassword() => View();
        public ActionResult Role() => View();
        #endregion view

        [HttpGet]
        public ActionResult Load(Pagination pag,string orgId, string keyword)
        {
            var rows = App.GetList(pag, orgId,keyword);
            return Result.PagingRst(rows, pag.Records, pag.Total);
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Result.Success(data);
        }

        [HttpPost]
        public ActionResult Add(AddUserDto dto)
        {
            App.Add(dto);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult Update(UpdateUserDto dto)
        {
            App.Update(dto);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                App.Delete(id.Split(','));
            }
            return Result.Success();
        }

        [HttpPost]
        public ActionResult RevisePassword(string userPassword, string keyValue)
        {
            App.RevisePassword(userPassword, keyValue);
            return Result.Success(); 
        }

        [HttpPost]
        public ActionResult Disable(string id)
        {
            App.Disable(id);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult Enable(string id)
        {
            App.Enable(id);
            return Result.Success();
        }

        /// <summary>
        /// 获取机构下的用户
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetOrgUsers(string orgId, string keyword) => await Task.Run(() => Result.Success(App.GetByOrg(orgId, keyword)));


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

        [HttpPost]
        public ActionResult SetRole(string userId,string[] roleId)
        {
            App.SetRole(userId, roleId);
            return Result.Success();
        }


        [HttpGet]
        public ActionResult GetUserData()
        {
            return Result.Success(App.GetUserData(Operator.GetCurrent()));
        }


        public ActionResult GetUserRoles(string userId)
        {
            return Result.Success(App.GetUserRoles(userId));
        }

        public ActionResult GetExcludeRoles(string userId)
        {
            return Result.Success(App.GetRolesExcludeUser(userId));
        }

        public ActionResult AddRole(string userId,string[] roleId)
        {
            App.AddRole(userId, roleId);
            return Result.Success();
        }
        public ActionResult RemoveRole(string userId, string roleId)
        {
            App.RemoveRole(userId, new[] { roleId });
            return Result.Success();
        }
    }
}