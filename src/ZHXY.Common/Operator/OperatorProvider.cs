using System.Web;

namespace ZHXY.Common
{
    public static class OperatorProvider
    {
        private static string LoginUserKey { get; } = "zhxy";
        private static string LoginProvider { get; } = Configs.GetValue("LoginProvider");

        public static OperatorModel Current
        {
            get
            {
                try
                {
                    var content = LoginProvider == "Cookie" ? HttpContext.Current.Response.Cookies.Get(LoginUserKey)?.Value : HttpContext.Current.Session[LoginUserKey]?.ToString();
                    return content.ToObject<OperatorModel>();
                }
                catch 
                {
                    return default;
                }
               
            }
        }

        /// <summary>
        /// 设置当前用户
        /// </summary>
        /// <param name="operatorModel"></param>
        public static void Set(OperatorModel operatorModel)
        {
            if (LoginProvider == "Cookie")
            {
                var cookie = HttpContext.Current.Request.Cookies[LoginUserKey] ?? new HttpCookie(LoginUserKey, operatorModel.ToJson());
                HttpContext.Current.Response.AppendCookie(cookie);
            }
            else
                HttpContext.Current.Session[LoginUserKey] = operatorModel.ToJson(); 
        }

        /// <summary>
        /// 移除当前用户
        /// </summary>
        public static void Remove()
        {
            if (LoginProvider == "Cookie")
                HttpContext.Current.Response.Cookies.Remove(LoginUserKey.Trim());
            else
                HttpContext.Current.Session.Contents.Remove(LoginUserKey.Trim());
        }
    }
}