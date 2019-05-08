using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace ZHXY.Common
{
    /// <summary>
    ///     网络操作
    /// </summary>
    public class Net
    {

        #region Ip(获取Ip)

        /// <summary>
        ///     获取Ip
        /// </summary>
        public static string Ip
        {
            get
            {
                var result = string.Empty;
                if (HttpContext.Current != null)
                    result = GetWebClientIp();
                if (result.IsEmpty())
                    result = GetLanIp();
                return result;
            }
        }

        /// <summary>
        ///     获取Web客户端的Ip
        /// </summary>
        private static string GetWebClientIp()
        {
            var ip = GetWebRemoteIp();
            foreach (var hostAddress in Dns.GetHostAddresses(ip))
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            return string.Empty;
        }

        /// <summary>
        ///     获取Web远程Ip
        /// </summary>
        private static string GetWebRemoteIp() => HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                   HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        /// <summary>
        ///     获取局域网IP
        /// </summary>
        private static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            return string.Empty;
        }

        #endregion Ip(获取Ip)

        #region 获取mac地址


        /// <summary>
        /// 通过NetworkInterface读取网卡Mac
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMacByNetworkInterface()
        {
            var macs = new List<string>();
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var ni in interfaces) macs.Add(ni.GetPhysicalAddress().ToString());
            return macs;
        }

        #endregion 获取mac地址

        #region Ip城市(获取Ip城市)

        /// <summary>
        ///     获取IP地址信息
        /// </summary>
        /// <param name="ip">  </param>
        /// <returns>  </returns>
        public static string GetLocation(string ip)
        {
            string res;
            try
            {
                if (ip == "localtion" || ip == "127.0.0.1") return "本地";
                var url = $"https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?query={ip}&resource_id=6006&ie=utf8&oe=gbk&format=json";
                res = HttpMethods.HttpGet(url, Encoding.GetEncoding("GBK"));
                var resjson = res.ToObject<LocationResponse>();
                res = resjson.Data[0].Location;
            }
            catch
            {
                res = "";
            }
            return res;
        }

        /// <summary>
        ///     百度接口
        /// </summary>
        private class LocationResponse
        {
            public List<LocationData> Data { get; set; }
        }

        private class LocationData
        {
            public string Location { get; set; }
        }

        #endregion Ip城市(获取Ip城市)
    }
}