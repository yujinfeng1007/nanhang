using System;
using System.Security.Cryptography;
using System.Text;

namespace ZHXY.Dorm.Device.tools
{
    public class Md5Tool
    {
        /// <summary>
        /// MD5 32 位 小写加密规则
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md532(string str)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(str)));
            t2 = t2.Replace("-", "");
            return t2.ToLower();
        }
    }
}