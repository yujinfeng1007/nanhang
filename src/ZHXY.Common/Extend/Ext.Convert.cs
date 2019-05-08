using System;

namespace ZHXY.Common
{
    public static partial class Ext
    {

        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="value"> 值 </param>
        public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);
               
        /// <summary>
        /// 对象是否为空
        /// </summary>
        public static bool IsEmpty(this object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                return false;
            return true;
        }


        #region 日期转换

        /// <summary>
        ///     转换为日期
        /// </summary>
        /// <param name="data"> 数据 </param>
        public static DateTime ToDate(this object data)
        {
            if (data == null)
                return DateTime.MinValue;
            return DateTime.TryParse(data.ToString(), out var result) ? result : DateTime.MinValue;
        }

        /// <summary>
        ///     转换为可空日期
        /// </summary>
        /// <param name="data"> 数据 </param>
        public static DateTime? ToDateOrNull(this object data)
        {
            if (data == null)
                return null;
            var isValid = DateTime.TryParse(data.ToString(), out var result);
            if (isValid)
                return result;
            return null;
        }

        #endregion 日期转换

        #region 布尔转换

        /// <summary>
        ///     转换为布尔值
        /// </summary>
        /// <param name="data"> 数据 </param>
        public static bool ToBool(this object data)
        {
            if (data == null)
                return false;
            var value = GetBool(data);
            if (value != null)
                return value.Value;
            return bool.TryParse(data.ToString(), out var result) && result;
        }

        /// <summary>
        ///     获取布尔值
        /// </summary>
        private static bool? GetBool(this object data)
        {
            switch (data.ToString().Trim().ToLower())
            {
                case "0":
                    return false;

                case "1":
                    return true;

                case "是":
                    return true;

                case "否":
                    return false;

                case "yes":
                    return true;

                case "no":
                    return false;

                default:
                    return null;
            }
        }

       

        #endregion 布尔转换
    }
}