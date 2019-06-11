using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZHXY.Common
{
    /// <summary>
    ///     MD5加密
    /// </summary>
    public static class Md5EncryptHelper
    {
        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="str">  加密字符 </param>
        /// <param name="code"> 加密位数16/32 </param>
        /// <returns>  </returns>
        public static string Encrypt(string str, int code)
        {
            var buffer = Encoding.Default.GetBytes(str);
            buffer = MD5.Create().ComputeHash(buffer);
            var result = BitConverter.ToString(buffer).Replace("-", "");
            if (code == 16) result = result.Substring(0, 16);
            return result;
        }

        /// <summary>
        ///     计算文件内容的MD5值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ComputeMd5(Stream stream)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                stream.Seek(0, SeekOrigin.Begin);
                var hashBytesNew = md5.ComputeHash(stream);
                stream.Seek(0, SeekOrigin.Begin);

                // make a hex string of the hash for display or whatever
                var sb = new StringBuilder();
                foreach (var b in hashBytesNew) sb.Append(b.ToString("x2").ToLower());
                return sb.ToString();
            }
        }

        /// <summary>
        ///     计算文本类型的MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeMd5(this string str)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = Encoding.UTF8.GetBytes(str);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            var sTemp = "";
            for (var i = 0; i < bytHash.Length; i++) sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            return sTemp.ToLower();
        }
    }
}