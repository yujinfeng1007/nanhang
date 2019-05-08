using System;
using System.Security.Cryptography;

namespace ZHXY.Common
{
    /// <summary>
    /// 随机数助手
    /// </summary>
    public class RandomHelper
    {
        private static int GetRandomSeed()
        {
            var bytes = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static Random GetRandom()
        {
            return new Random(GetRandomSeed());
        }

        /// <summary>
        /// 生成随机整数
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Next(int? maxValue = null)
        {
            var max = maxValue ?? int.MaxValue;
            return new Random(GetRandomSeed()).Next(max);
        }
    }
}