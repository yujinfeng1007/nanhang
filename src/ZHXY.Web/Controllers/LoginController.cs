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

        public LoginController(DutyService app)
        {
            DutyApp = app;
        }

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
                var userEntity = new UserAppService().CheckLogin(username, password);
                var duty = string.Empty;
                if (userEntity != null)
                {
                    var operatorModel = new OperatorModel();
                    operatorModel.UserId = userEntity.F_Id;
                    operatorModel.UserCode = userEntity.F_Account;
                    operatorModel.UserName = userEntity.F_RealName;
                    operatorModel.F_User_SetUp = userEntity.F_User_SetUp;
                    operatorModel.CompanyId = userEntity.F_OrganizeId; //学校ID
                   

                  

                    operatorModel.DepartmentId = userEntity.OrgId;
                    operatorModel.RoleId = userEntity.F_RoleId;
                    operatorModel.HeadIcon = userEntity.F_HeadIcon;
                    operatorModel.LoginIPAddress = Net.Ip;
                    operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);
                    operatorModel.LoginTime = DateTime.Now;
                    operatorModel.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
                    operatorModel.MobilePhone = userEntity.F_MobilePhone;
                    //operatorModel.F_Class = userEntity.F_Class;
                    if (userEntity.F_Account == "admin")
                    {
                        duty = "admin";
                        operatorModel.IsSystem = true;
                    }
                    else
                    {
                        duty=DutyApp.GetEnCode(userEntity.F_DutyId);
                        operatorModel.IsSystem = false;
                    }

                    //duty = userEntity.F_DutyId;
                    //权限判断，先看个人有没有设置数据权限，再看角色是否有数据权限
                    operatorModel.Roles = GetRoles(userEntity);
                    //}
                    //用户岗位 类型
                    operatorModel.Duty = userEntity.F_DutyId;
                    operatorModel.SchoolCode = schoolCode;
                    OperatorProvider.Set(operatorModel);
                    logEntity.Account = userEntity.F_Account;
                    logEntity.NickName = userEntity.F_RealName;
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
                var userEntity = new UserAppService().CheckLogin(username, password);
                var roleList = new SysUserRoleAppService().GetListByUserId(userEntity.F_Id);
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
                        UserId = userEntity.F_Id,
                        UserCode = userEntity.F_Account,
                        UserName = userEntity.F_RealName,
                        F_User_SetUp = userEntity.F_User_SetUp,
                        CompanyId = userEntity.F_OrganizeId //学校ID
                    };

                    //学校老师
               
                    op.DepartmentId = userEntity.OrgId;
                    op.RoleId = userEntity.F_RoleId;
                    op.HeadIcon = userEntity.F_HeadIcon;
                    op.LoginIPAddress = Net.Ip;
                    op.LoginIPAddressName = Net.GetLocation(op.LoginIPAddress);
                    op.LoginTime = DateTime.Now;
                    op.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
                    op.MobilePhone = userEntity.F_MobilePhone;

                    if (userEntity.F_Account == "admin")
                    {
                        duty = "admin";
                        op.IsSystem = true;
                    }
                    else
                    {
                        duty = DutyApp.GetEnCode(userEntity.F_DutyId);
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

                        if (userEntity.F_Data_Type.IsEmpty())
                        {
                            if (role.DataType != null)
                                dic.Add(role.DataType, role.DataDeps);
                        }
                        else
                        {
                            dic.Add(userEntity.F_Data_Type, userEntity.F_Data_Deps);
                        }

                        roles.Add(e.F_Role, dic);
                    }

                    op.Roles = roles;
                    //}
                    //用户岗位 类型
                    op.Duty = userEntity.F_DutyId;
                    //添加学校编码
                    op.SchoolCode = schoolCode;
                    OperatorProvider.Set(op);
                    log.Account = userEntity.F_Account;
                    log.UserId = userEntity.F_Id;
                    log.NickName = userEntity.F_RealName;
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
                        userEntity.F_User_SetUp,
                        userEntity.F_RealName,
                        userEntity.F_Class
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
            var userEntity = new UserAppService().CheckLogin(uname, pass);
            var operatorModel = new OperatorModel
            {
                UserId = userEntity.F_Id,
                UserCode = userEntity.F_Account,
                UserName = userEntity.F_RealName,
                F_User_SetUp = userEntity.F_User_SetUp,
                CompanyId = userEntity.F_OrganizeId, //学校ID
                DepartmentId = userEntity.OrgId,
                RoleId = userEntity.F_RoleId,
                HeadIcon = userEntity.F_HeadIcon,
                LoginIPAddress = Net.Ip
            };
            operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);
            operatorModel.LoginTime = DateTime.Now;
            operatorModel.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
            operatorModel.MobilePhone = userEntity.F_MobilePhone;

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

                if (userEntity.F_Data_Type.IsEmpty())
                {
                    if (role.DataType != null)
                        dic.Add(role.DataType, role.DataDeps);
                }
                else
                {
                    dic.Add(userEntity.F_Data_Type, userEntity.F_Data_Deps);
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
                    userEntity.F_User_SetUp,
                    userEntity.F_RealName,
                    userEntity.F_Class
                }
            }.ToJson());
        }

        /// <summary>
        /// 获取学校列表
        /// </summary>
        [HttpGet]
        public ActionResult GetSchoolList()
        {
            var qdNo = ConfigurationManager.AppSettings["qdNo"]; //渠道编码
            var url = ConfigurationManager.AppSettings["getSchoolListUrl"]; //运营平台接口地址
            //var result = HttpHelper.GetString($"{url}{qdNo}");
            var result = new HttpClient().GetStringAsync($"{url}{qdNo}").Result;
            var j = JObject.Parse(result);
            var isError = (bool)j.GetValue("IsError", StringComparison.InvariantCultureIgnoreCase);
            if (isError) throw new Exception("获取学校信息失败!");
            var data = j.GetValue("Data", StringComparison.InvariantCultureIgnoreCase)?.ToString();
            return Content(data);
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
            var list = sysUserRoleApp.GetListByUserId(user.F_Id);
            //获得数据权限,支持多角色
            foreach (var e in list)
            {
                var role = roleApp.Get(e.F_Role);
                var dic = new Dictionary<string, string>();

                if (user.F_Data_Type.IsEmpty())
                {
                    if (role.DataType != null)
                        dic.Add(role.DataType, role.DataDeps);
                }
                else
                {
                    dic.Add(user.F_Data_Type, user.F_Data_Deps);
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
                var exist = new UserAppService().Read<User>(p => p.F_Account.Equals(uid)).AnyAsync().Result;
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
            var app = new UserAppService();
            var user = app.Read<User>(p => p.F_Account.Equals(account)).FirstOrDefaultAsync().Result;
            if (user == null) throw new Exception("用户不存在!");
            var roleIds = app.Read<UserRole>(p => p.F_User.Equals(user.F_Id)).Select(p => p.F_Role).ToListAsync()
                .Result;
            //var canLogin = app.Read<SysRole>(p => roleIds.Contains(p.F_Id) && p.F_Type.Equals("1")).AnyAsync().Result;
            //if (!canLogin) throw new Exception("无权限登录后台系统！");
            var current = new OperatorModel
            {
                UserId = user.F_Id,
                UserCode = user.F_Account,
                UserName = user.F_RealName,
                F_User_SetUp = user.F_User_SetUp,
                CompanyId = user.F_OrganizeId,
                DepartmentId = user.OrgId,
                RoleId = user.F_RoleId,
                HeadIcon = user.F_HeadIcon,
                LoginIPAddress = Net.Ip,
                MobilePhone = user.F_MobilePhone,
                Duty = user.F_DutyId
            };
            current.LoginIPAddressName = Net.GetLocation(current.LoginIPAddress);
            current.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());

            if (user.F_Account == "admin") current.IsSystem = true;
            return current;

        }
        #endregion
    }
}
