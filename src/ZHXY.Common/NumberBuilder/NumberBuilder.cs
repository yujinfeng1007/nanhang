using System;

namespace ZHXY.Common
{
    /// <summary>
    /// 编号生成器
    /// </summary>
    public class NumberBuilder
    {
        /// <summary>
        /// 自动生成18位编号 201008251145409865
        /// </summary>
        public static string Build_18bit() => $"{ DateTime.Now.ToString("yyyyMMddHHmmss") }{ RandomHelper.GetRandom().Next(1,9999).ToString().PadLeft(4,'0') }";
    }
}