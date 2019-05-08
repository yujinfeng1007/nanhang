
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System;
using System.Configuration;
using ZHXY.Common;

namespace OpenApi
{
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
        public bool Ignore { get; set; }
        public HandlerAuthorizeAttribute(bool ignore = true) => Ignore = ignore;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (Ignore == false)
            {
                return;
            }
            var request = System.Web.HttpContext.Current.Request;
            string F_APPKEY = request.Form["F_APPKEY"]?.ToString();
            string F_SESSIONKEY = request.Form["F_SESSIONKEY"]?.ToString();
            string F_School_Id = request.Form["F_School_Id"]?.ToString();

            if (!ActionAuthorize(actionContext, F_APPKEY, F_SESSIONKEY))
            {
                throw new ExceptionContext("401", "验证失败！");
                ////此处暂时以401返回，可调整为其它返回
                //actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            SetCurrentOperator(F_School_Id);
            base.OnActionExecuting(actionContext);
        }

        private bool ActionAuthorize(HttpActionContext filterContext, string F_APPKEY, string F_SESSIONKEY)
        {
            if (string.IsNullOrEmpty(F_APPKEY) || string.IsNullOrEmpty(F_SESSIONKEY))
                return false;
            var user = getUser(F_APPKEY);
            if (user == null)
                return false;
            if (!validate(F_SESSIONKEY, user, filterContext.ActionDescriptor.ActionName))
                return false;
            return true;
        }

        class UserModel
        {
            public string AppKey { get; set; }
            public string CustCode { get; set; }
        }
        private UserModel getUser(string F_APPKEY)
        {
            var user = (UserModel)HttpContext.Current.Session[F_APPKEY];
            if (user == null)
            {
                // 从数据库中获取
                //.......

                user = new UserModel
                {
                    AppKey = ConfigurationManager.AppSettings["AppKey"],
                    CustCode = ConfigurationManager.AppSettings["CustCode"]
                };
                if (user == null)
                    return null;
                if (user.AppKey != F_APPKEY)
                    return null;
                HttpContext.Current.Session.Add(F_APPKEY, user);
            }
            return user;
        }

        private bool validate(string F_SESSIONKEY, UserModel user, string apiName)
        {
            var str = (user.AppKey + user.CustCode + apiName + DateTime.Now.ToString("yyMMdd")).ToLower();
            //var str = "05023587341147ff9e2fd0bcc410c25a113001reportsign4eg190108";
            var md5Str = Md5EncryptHelper.Encrypt(str, 32);
            var sign = Base64Encode(md5Str);
            if (F_SESSIONKEY != sign)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        public static void SetCurrentOperator(string schoolCode)
        {
            if (string.IsNullOrEmpty(schoolCode)) throw new Exception("学校代码不能为空!");
            OperatorProvider.Set(new OperatorModel { SchoolCode = schoolCode });
        }
    }
}