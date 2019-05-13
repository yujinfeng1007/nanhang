using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using DotNetOpenAuth.OAuth2;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using SSO_ClientSDK;
using System.Data.Entity;
using System.Web;
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
        private UserService UserApp { get; }

        public LoginController(DutyService app, UserService userApp)
        {
            DutyApp = app;
            UserApp = userApp;
        }

        #region view

        [HttpGet]
        public virtual ActionResult Index2()
        {
            Licence.IsLicence();
            if (OperatorProvider.Current.IsEmpty())
            {
                return View("index");
            }
            return Redirect("/iPadFrame.html");
        }

        [HttpGet]
        public virtual ActionResult Index(string role = null)
        {
            Licence.IsLicence();

            if (OperatorProvider.Current.IsEmpty())
            {
                return View();
            }
            else
            {
                return Redirect("/iPadFrame.html");
            }
        }

        //家校通
        [HttpGet]
        public virtual ActionResult MobileIndex()
        {
            if ("on".Equals(Configs.GetValue("ifSso")))
            {
                if (OperatorProvider.Current.IsEmpty())
                {
                    return View();
                }
            }

            return View();
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
            if (OperatorProvider.Current != null)
            {
                WriteLog(new AddLogDto
                {
                    IPAddress = Net.Ip,
                    IPAddressName = Net.GetLocation(Net.Ip),
                    ModuleName = "系统登录",
                    Type = DbLogType.Exit.ToString(),
                    Account = OperatorProvider.Current.UserCode,
                    UserId = OperatorProvider.Current.UserId,
                    NickName = OperatorProvider.Current.UserName,
                    Result = true,
                    Description = "安全退出系统",
                });
            }
            Session.Abandon();
            Session.Clear();
            OperatorProvider.Remove();
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
            OperatorProvider.Remove();
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
            OperatorProvider.Set(new OperatorModel { SchoolCode = schoolCode });
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
                    var operatorModel = new OperatorModel();
                    operatorModel.UserId = userEntity.Id;
                    operatorModel.UserCode = userEntity.Account;
                    operatorModel.UserName = userEntity.Name;
                    operatorModel.F_User_SetUp = userEntity.UserSetUp;
                    operatorModel.CompanyId = userEntity.OrganId; //学校ID
                   

                  

                    operatorModel.DepartmentId = userEntity.OrganId;
                    operatorModel.RoleId = userEntity.RoleId;
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
                        duty=DutyApp.GetEnCode(userEntity.DutyId);
                        operatorModel.IsSystem = false;
                    }

                    //duty = userEntity.F_DutyId;
                    //权限判断，先看个人有没有设置数据权限，再看角色是否有数据权限
                    operatorModel.Roles = GetRoles(userEntity);
                    //}
                    //用户岗位 类型
                    operatorModel.Duty = userEntity.DutyId;
                    operatorModel.SchoolCode = schoolCode;
                    OperatorProvider.Set(operatorModel);
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
        public ActionResult CheckLoginType(string username, string password, string code, string schoolCode)
        {
            OperatorProvider.Set(new OperatorModel { SchoolCode = schoolCode });
            var log = new AddLogDto
            {
                ModuleName = "后台登录",
                Type = DbLogType.Login.ToString(),
                IPAddress = Net.Ip,
                IPAddressName = Net.GetLocation(Net.Ip),
            };
            try
            {
                CheckVerifyCode(code);
                var userEntity = UserApp.CheckLogin(username, password);
                var roleList = new SysUserRoleAppService().GetListByUserId(userEntity.Id);
                var F_Type = true;
                foreach (var item in roleList)
                {
                    if (new RoleService().Get(item.F_Role).Type.Equals("1"))
                    {
                        F_Type = true;
                        break;
                    }
                    else
                    {
                        F_Type = false;
                    }

                    //F_Type += new RoleApp().GetForm(item.F_Role).F_Type + ",";
                }

                if (F_Type != true)
                {
                    throw new Exception("无权限登录后台系统！");
                }

                var duty = string.Empty;
                if (userEntity != null)
                {
                    var op = new OperatorModel
                    {
                        UserId = userEntity.Id,
                        UserCode = userEntity.Account,
                        UserName = userEntity.Name,
                        F_User_SetUp = userEntity.UserSetUp,
                        CompanyId = userEntity.OrganId //学校ID
                    };

                    //学校老师
               
                    op.DepartmentId = userEntity.OrganId;
                    op.RoleId = userEntity.RoleId;
                    op.HeadIcon = userEntity.HeadIcon;
                    op.LoginIPAddress = Net.Ip;
                    op.LoginIPAddressName = Net.GetLocation(op.LoginIPAddress);
                    op.LoginTime = DateTime.Now;
                    op.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
                    op.MobilePhone = userEntity.MobilePhone;

                    if (userEntity.Account == "admin")
                    {
                        duty = "admin";
                        op.IsSystem = true;
                    }
                    else
                    {
                        duty = DutyApp.GetEnCode(userEntity.DutyId);
                        op.IsSystem = false;
                    }

                    //duty = userEntity.F_DutyId;
                    //权限判断，先看个人有没有设置数据权限，再看角色是否有数据权限
                    var roles = new Dictionary<string, Dictionary<string, string>>();

                    var sysUserRoleApp = new SysUserRoleAppService();
                    var roleApp = new RoleService();
                    var list = sysUserRoleApp.GetListByUserId(op.UserId);
                    //获得数据权限,支持多角色
                    foreach (var e in list)
                    {
                        var role = roleApp.Get(e.F_Role);
                        var dic = new Dictionary<string, string>();

                        if (userEntity.DataType.IsEmpty())
                        {
                            if (role.DataType != null)
                                dic.Add(role.DataType, role.DataDeps);
                        }
                        else
                        {
                            dic.Add(userEntity.DataType, userEntity.DataDeps);
                        }

                        roles.Add(e.F_Role, dic);
                    }

                    op.Roles = roles;
                    //}
                    //用户岗位 类型
                    op.Duty = userEntity.DutyId;
                    //添加学校编码
                    op.SchoolCode = schoolCode;
                    OperatorProvider.Set(op);
                    log.Account = userEntity.Account;
                    log.UserId = userEntity.Id;
                    log.NickName = userEntity.Name;
                    log.Result = true;
                    log.Description = "登录成功";


                }
                WriteLog(log);
                return Content(new
                {
                    state = ResultState.Success,
                    message = "登录成功。",
                    data = new
                    {
                        duty,
                        userEntity.UserSetUp,
                        userEntity.Name,
                        userEntity.Class
                    }
                }.ToJson());
            }
            catch (Exception ex)
            {
                log.Account = username;
                log.NickName = username;
                log.Result = false;
                log.Description = "登录失败，" + ex.Message;
                WriteLog(log);
                return Content(new { state = ResultState.Error, message = log.Description }.ToJson());
            }
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        [HttpPost]
        public ActionResult AdminLogin(string uname, string pass, string schoolCode)
        {
            OperatorProvider.Set(new OperatorModel { SchoolCode = schoolCode });
            var userEntity = UserApp.CheckLogin(uname, pass);
            var operatorModel = new OperatorModel
            {
                UserId = userEntity.Id,
                UserCode = userEntity.Account,
                UserName = userEntity.Name,
                F_User_SetUp = userEntity.UserSetUp,
                CompanyId = userEntity.OrganId, //学校ID
                DepartmentId = userEntity.OrganId,
                RoleId = userEntity.RoleId,
                HeadIcon = userEntity.HeadIcon,
                LoginIPAddress = Net.Ip
            };
            operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);
            operatorModel.LoginTime = DateTime.Now;
            operatorModel.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
            operatorModel.MobilePhone = userEntity.MobilePhone;

            operatorModel.IsSystem = true;

            var roles = new Dictionary<string, Dictionary<string, string>>();

            var sysUserRoleApp = new SysUserRoleAppService();
            var roleApp = new RoleService();
            var list = sysUserRoleApp.GetListByUserId(operatorModel.UserId);
            //获得数据权限,支持多角色
            foreach (var e in list)
            {
                var role = roleApp.Get(e.F_Role);
                var dic = new Dictionary<string, string>();

                if (userEntity.DataType.IsEmpty())
                {
                    if (role.DataType != null)
                        dic.Add(role.DataType, role.DataDeps);
                }
                else
                {
                    dic.Add(userEntity.DataType, userEntity.DataDeps);
                }

                roles.Add(e.F_Role, dic);
            }

            operatorModel.Roles = roles;

            return Content(new
            {
                state = ResultState.Success,
                message = "登录成功。",
                data = new
                {
                    duty = "admin",
                    userEntity.UserSetUp,
                    userEntity.Name,
                    userEntity.Class
                }
            }.ToJson());
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
        /// 获取角色
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, string>> GetRoles(User user)
        {
            //权限判断，先看个人有没有设置数据权限，再看角色是否有数据权限
            var roles = new Dictionary<string, Dictionary<string, string>>();
            var sysUserRoleApp = new SysUserRoleAppService();
            var roleApp = new RoleService();
            var list = sysUserRoleApp.GetListByUserId(user.Id);
            //获得数据权限,支持多角色
            foreach (var e in list)
            {
                var role = roleApp.Get(e.F_Role);
                var dic = new Dictionary<string, string>();

                if (user.DataType.IsEmpty())
                {
                    if (role.DataType != null)
                        dic.Add(role.DataType, role.DataDeps);
                }
                else
                {
                    dic.Add(user.DataType, user.DataDeps);
                }

                roles.Add(e.F_Role, dic);
            }

            return roles;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="input"></param>
        private void WriteLog(AddLogDto input) => new SysLogAppService().AddLog(input);

        #endregion private

        #region 南航单点登录
        public void Index3()
        {
            if (OperatorProvider.Current == null)
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
                OperatorProvider.Set(user);
                Response.Redirect(returnURL);
                return;
            }
            Response.Redirect(client.LoginURL(passportLoginURL));
        }

        private OperatorModel BuildCurrent(string account)
        {
            var user = UserApp.Read<User>(p => p.Account.Equals(account)).FirstOrDefaultAsync().Result;
            if (user == null) throw new Exception("用户不存在!");
            var roleIds = UserApp.Read<UserRole>(p => p.F_User.Equals(user.Id)).Select(p => p.F_Role).ToListAsync()
                .Result;
            var current = new OperatorModel
            {
                UserId = user.Id,
                UserCode = user.Account,
                UserName = user.Name,
                F_User_SetUp = user.UserSetUp,
                CompanyId = user.OrganId,
                DepartmentId = user.OrganId,
                RoleId = user.RoleId,
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
