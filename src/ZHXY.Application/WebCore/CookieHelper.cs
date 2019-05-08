using Newtonsoft.Json;
using System;
using System.Web;

namespace ZHXY.Application
{
    /// <summary>
    ///     cookie助手
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public static class CookieHelper
    {
        public static void AddCookie(this HttpResponseBase response, object userObject, string cookieName, int days)
        {
            var json = JsonConvert.SerializeObject(userObject);
            var userCookie = new HttpCookie(cookieName, json);
            userCookie.Expires.AddDays(days);
            response.Cookies.Add(userCookie);
        }

        public static void RemoveCookie(this HttpContextBase context, string cookieName)
        {
            if (context.Request.Cookies[cookieName] != null)
            {
                var user = new HttpCookie(cookieName)
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Value = null
                };
                context.Response.Cookies.Add(user);
            }
        }

        public static HttpCookie GetCookie(this HttpContextBase context, string cookieName)
        {
            if (context.Request.Cookies[cookieName] != null)
            {
                return context.Request.Cookies.Get(cookieName);
            }
            return null;
        }
    }
}