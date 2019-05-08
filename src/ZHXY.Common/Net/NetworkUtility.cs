using System;
using System.Net;
using System.Web;

namespace ZHXY.Common
{
    /// <summary>
    ///     网络工具
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public class NetworkUtility
    {
        /// <summary>
        ///     获取IP地址的方法MVC
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIPAddress(HttpRequestBase request)
        {
            var szRemoteAddr = request.UserHostAddress;
            var szXForwardedFor = request.ServerVariables["X_FORWARDED_FOR"];
            string szIP;
            if (szXForwardedFor == null)
            {
                szIP = szRemoteAddr;
            }
            else
            {
                szIP = szXForwardedFor;
                if (szIP.IndexOf(",", StringComparison.Ordinal) > 0)
                {
                    var arIPs = szIP.Split(',');

                    foreach (var item in arIPs)
                        if (!IsPrivateIP(item))
                            return item;
                }
            }
            return szIP;
        }

        public static string GetIPAddress2(HttpContext context)
        {
            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.Split(',');
                if (addresses.Length != 0) return addresses[0];
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        ///     判断是否是内部网络私有IP地址
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private static bool IsPrivateIP(string ipAddress)
        {
            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }
    }
}