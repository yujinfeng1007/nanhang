using System;
using System.Web;

namespace ZHXY.Common
{
    public sealed class Licence
    {
        public static bool IsLicence()
        {
            var mac = Md5EncryptHelper.Encrypt(Net.GetMacByNetworkInterface().ToJson(), 32);
            try
            {
                var host = HttpContext.Current.Request.Url.Host.ToLower();
                //if (!mac.Equals("localhost"))
                //    return false;
                if (host.Equals("localhost"))
                    return true;
                if (host.Equals("192.168.0.128"))
                    return true;
                if (host.Equals("192.168.0.111"))
                    return true;
                if (host.Equals("192.168.0.112"))
                    return true;
                if (host.Equals("192.168.2.153"))
                    return true;
                if (host.Equals("192.168.0.100"))
                    return true;
                if (host.Equals("192.168.1.128"))
                    return true;
                if (host.Equals("192.168.1.125"))
                    return true;
                if (host.Equals("192.168.1.126"))
                    return true;
                if (host.Equals("192.168.1.118"))
                    return true;
                if (host.Equals("192.168.1.120"))
                    return true;
                if (host.Equals("192.168.1.102"))
                    return true;
                if (host.Equals("192.168.1.105"))
                    return true;
                if (host.Equals("192.168.0.105"))
                    return true;
                if (host.Equals("192.168.1.122"))
                    return true;
                if (host.Equals("192.168.0.106"))
                    return true;
                if (host.Equals("192.168.1.106"))
                    return true;
                if (host.Equals("192.168.0.108"))
                    return true;
                if (host.Equals("192.168.0.103"))
                    return true;
                if (host.Equals("192.168.0.102"))
                    return true;
                if (host.Equals("192.168.0.101"))
                    return true;
                if (host.Equals("192.168.1.133"))
                    return true;
                if (host.Equals("192.168.1.130"))
                    return true;
                if (host.Equals("zjrhxx.ronghuai.cn"))
                    return true;
                if (host.Equals("www.zjrhjy.cn"))
                    return true;
                if (host.Equals("61.164.123.34"))
                    return true;
                if (host.Equals("zhxy.huawangtech.com"))
                    return true;
                throw new Exception("Licence无效或过期");
            }
            catch
            {
                return true;
            }
        }
    }
}