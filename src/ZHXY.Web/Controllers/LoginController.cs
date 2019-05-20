using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using SSO_ClientSDK;
using System.Data.Entity;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    /// <summary>
    /// 登录控制器
    /// </summary>

    public class LoginController : Controller
    {
        private DutyService DutyApp { get; }
        private RoleService RoleApp { get; }
        private UserService UserApp { get; }

        public LoginController(DutyService app, UserService userApp, RoleService roleApp)
        {
            DutyApp = app;
            UserApp = userApp;
            RoleApp = roleApp;
        }

        #region view

        [HttpGet]
        public virtual ActionResult Index2()
        {
            Licence.IsLicence();
            if (Operator.GetCurrent().IsEmpty())
            {
                return View("index");
            }
            return Redirect("/iPadFrame.html");
        }

        [HttpGet]
        public virtual ActionResult Index(string role = null)
        {
            Licence.IsLicence();

            if (Operator.GetCurrent().IsEmpty())
            {
                return View();
            }
            else
            {
                return Redirect("/iPadFrame.html");
            }
        }

        #endregion view

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileContentResult GetAuthCode()
        {
            var (code, img) = CaptchaGenerator.Gen();
            HttpContext.Session.Add("zhxy_verify_code", Md5EncryptHelper.ComputeMd5(code.ToLower()));
            return File(img, @"image/png");
        }

        /// <summary>
        /// 登出
        /// </summary>
        [HttpGet]
        public ActionResult OutLogin()
        {
            if (Operator.GetCurrent() != null)
            {
                WriteLog(new AddLogDto
                {
                    IPAddress = Net.Ip,
                    IPAddressName = Net.GetLocation(Net.Ip),
                    ModuleName = "系统登录",
                    Type = DbLogType.Exit.ToString(),
                    Account = Operator.GetCurrent().UserCode,
                    UserId = Operator.GetCurrent().Id,
                    NickName = Operator.GetCurrent().UserName,
                    Result = true,
                    Description = "安全退出系统",
                });
            }
            Session.Abandon();
            Session.Clear();
            Operator.Remove();
            return View("Index");
            //return Redirect(ConfigurationManager.AppSettings["LogoutURL"]);
        }

        /// <summary>
        /// 移动端登出
        /// </summary>
        [HttpGet]
        public ActionResult OutLoginMobile()
        {
            Session.Abandon();
            Session.Clear();
            Operator.Remove();
            if ("on".Equals(Configs.GetValue("ifSso")))
            {
                Session.Remove("authorizationState");
                return Redirect(Paths.ResourceServerBaseAddress + Paths.LogoutPath);
            }
            return Content("退出成功");
        }

        /// <summary>
        /// 移动端登录
        /// </summary>
        [HttpPost]
        public ActionResult CheckLogin(string username, string password, string code, string schoolCode)
        {
            var logEntity = new AddLogDto
            {
                ModuleName = "系统登录",
                Type = DbLogType.Login.ToString(),
                IPAddress = Net.Ip,
                IPAddressName = Net.GetLocation(Net.Ip),
            };
            try
            {
                CheckVerifyCode(code);
                var userEntity = UserApp.CheckLogin(username, password);
                var duty = string.Empty;
                if (userEntity != null)
                {
                    var operatorModel = new CurrentUser();
                    operatorModel.Id = userEntity.Id;
                    operatorModel.UserCode = userEntity.Account;
                    operatorModel.UserName = userEntity.Name;
                    operatorModel.SetUp = userEntity.UserSetUp;
                    operatorModel.Organ = userEntity.OrganId;
                    operatorModel.HeadIcon = userEntity.HeadIcon;
                    operatorModel.LoginIPAddress = Net.Ip;
                    operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);
                    operatorModel.LoginTime = DateTime.Now;
                    operatorModel.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
                    operatorModel.MobilePhone = userEntity.MobilePhone;
                    //operatorModel.F_Class = userEntity.F_Class;
                    if (userEntity.Account == "admin")
                    {
                        duty = "admin";
                        operatorModel.IsSystem = true;
                    }
                    else
                    {
                        duty = DutyApp.GetCode(userEntity.DutyId);
                        operatorModel.IsSystem = false;
                    }

                    //duty = userEntity.F_DutyId;
                    //权限判断，先看个人有没有设置数据权限，再看角色是否有数据权限
                    operatorModel.Roles = UserApp.GetUserRolesId(userEntity.Id);
                    //}
                    //用户岗位 类型
                    operatorModel.Duty = userEntity.DutyId;
                    Operator.Set(operatorModel);
                    logEntity.Account = userEntity.Account;
                    logEntity.NickName = userEntity.Name;
                    logEntity.Result = true;
                    logEntity.Description = "登录成功";

                    WriteLog(logEntity);
                }

                return Json(new { state = ResultState.Success, message = "登录成功。", data = duty });
            }
            catch (Exception ex)
            {
                logEntity.Account = username;
                logEntity.NickName = username;
                logEntity.Result = false;
                logEntity.Description = "登录失败，" + ex.Message;
                WriteLog(logEntity);
                return Content(new { state = ResultState.Error, message = logEntity.Description }.ToJson());
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        [HttpPost]
        public ActionResult CheckLoginType(string username, string password, string code)
        {
            CheckVerifyCode(code);
            var user = UserApp.CheckLogin(username, password);
            var duty = string.Empty;
            if (user != null)
            {
                var op = new CurrentUser
                {
                    Id = user.Id,
                    UserCode = user.Account,
                    UserName = user.Name,
                    SetUp = user.UserSetUp,
                    Organ = user.OrganId,
                    HeadIcon = user.HeadIcon,
                    LoginIPAddress = Net.Ip,
                    LoginTime = DateTime.Now,
                    LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString()),
                    MobilePhone = user.MobilePhone,
                };
                op.LoginIPAddressName = Net.GetLocation(op.LoginIPAddress);
                op.Roles = UserApp.GetUserRolesId(user.Id);
                op.Duty = user.DutyId;
                Operator.Set(op);
            }
            return Resultaat.Success(new { duty, user.UserSetUp, user.Name });
        }


        #region private

        /// <summary>
        /// 检查验证码
        /// </summary>
        private void CheckVerifyCode(string code)
        {
            return;
            //var ctx = System.Web.HttpContext.Current;
            //var verifyCode = ctx.GetSection("zhxy_verify_code")?.ToString();
            //verifyCode = verifyCode ?? Session["zhxy_verify_code"]?.ToString();
            //if (string.IsNullOrEmpty(verifyCode) || MD5Helper.GetMD5(code.ToLower()) != verifyCode)
            //    throw new Exception("验证码错误，请重新输入");
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="input"></param>
        private void WriteLog(AddLogDto input) => new LogService().AddLog(input);

        #endregion private

        #region 南航单点登录
        public void Index3()
        {
            if (Operator.GetCurrent() == null)
            {
                Certification();
            }
            else
            {
                Response.Redirect("/Home/Index");
            }
            Response.End();
        }
        private void Certification()
        {
            var returnURL = ConfigurationManager.AppSettings["LocalLoginUrl"];
            var siteId = Convert.ToInt32(ConfigurationManager.AppSettings["SiteID"]);
            var key = ConfigurationManager.AppSettings["Key"];
            var publicKey = WebUtility.HtmlEncode(ConfigurationManager.AppSettings["PublicKey"]);
            var passportLoginURL = ConfigurationManager.AppSettings["PassportURL"];
            var timeDiff = Convert.ToInt32(ConfigurationManager.AppSettings["TimeDiff"]);
            var client = new SsoClient(siteId, key, DateTime.Now.ToString(), returnURL, timeDiff);
            var ok = client.CheckLogin(publicKey);
            if (ok)
            {
                var uid = client.GetUID().ToLower();
                var exist = UserApp.Read<User>(p => p.Account.Equals(uid)).AnyAsync().Result;
                if (!exist) throw new Exception("无权进入本系统!");
                var user = BuildCurrent(uid);
                Operator.Set(user);
                Response.Redirect(returnURL);
                return;
            }
            Response.Redirect(client.LoginURL(passportLoginURL));
        }

        private CurrentUser BuildCurrent(string account)
        {
            var user = UserApp.Read<User>(p => p.Account.Equals(account)).FirstOrDefaultAsync().Result;
            if (user == null) throw new Exception("用户不存在!");
            var current = new CurrentUser
            {
                Id = user.Id,
                UserCode = user.Account,
                UserName = user.Name,
                SetUp = user.UserSetUp,
                Organ = user.OrganId,
                HeadIcon = user.HeadIcon,
                LoginIPAddress = Net.Ip,
                MobilePhone = user.MobilePhone,
                Duty = user.DutyId
            };
            current.LoginIPAddressName = Net.GetLocation(current.LoginIPAddress);
            current.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());

            if (user.Account == "admin") current.IsSystem = true;
            return current;

        }
        #endregion
    }
}
