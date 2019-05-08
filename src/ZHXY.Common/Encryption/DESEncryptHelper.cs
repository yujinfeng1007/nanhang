using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZHXY.Common
{
    /// <summary>
    ///     DES加密、解密帮助类
    /// </summary>
    public class DESEncryptHelper
    {
        private static readonly string DESKey = "jiujiang_2019";

        #region ========加密========

        /// <summary>
        ///     加密
        /// </summary>
        public static string Encrypt(string text) => Encrypt(text, DESKey);

        /// <summary>
        ///     加密数据
        /// </summary>
        public static string Encrypt(string text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = Encoding.ASCII.GetBytes(Md5EncryptHelper.Encrypt(sKey, 32).Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(Md5EncryptHelper.Encrypt(sKey, 32).Substring(0, 8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (var b in ms.ToArray()) ret.AppendFormat("{0:X2}", b);
            return ret.ToString();
        }

        #endregion ========加密========

        #region ========解密========

        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="Text">  </param>
        /// <returns>  </returns>
        public static string Decrypt(string text) => !string.IsNullOrEmpty(text) ? Decrypt(text, DESKey) : "";

        /// <summary>
        ///     解密数据
        /// </summary>
        /// <param name="Text">  </param>
        /// <param name="sKey">  </param>
        /// <returns>  </returns>
        public static string Decrypt(string Text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            var len = Text.Length / 2;
            var inputByteArray = new byte[len];
            int x;
            for (x = 0; x < len; x++)
            {
                var i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(Md5EncryptHelper.Encrypt(sKey, 32).Substring(0, 8).Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(Md5EncryptHelper.Encrypt(sKey, 32).Substring(0, 8).Substring(0, 8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion ========解密========
    }
}