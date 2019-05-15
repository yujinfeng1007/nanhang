using System.Web;

namespace ZHXY.Common
{
    public static class Operator
    {
        private static string LoginUserKey { get; } = "zhxy";

        public static CurrentUser Current
        {
            get
            {
                try
                {
                    var content = HttpContext.Current.Session[LoginUserKey]?.ToString();
                    return content.ToObject<CurrentUser>();
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
        /// <param name="user"></param>
        public static void Set(CurrentUser user)
        {
            HttpContext.Current.Session[LoginUserKey] = user.ToJson();
        }

        /// <summary>
        /// 移除当前用户
        /// </summary>
        public static void Remove()
        {
            HttpContext.Current.Session.Contents.Remove(LoginUserKey.Trim());
        }
    }
}