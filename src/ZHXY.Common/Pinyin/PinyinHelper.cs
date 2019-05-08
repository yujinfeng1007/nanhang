using Microsoft.International.Converters.PinYinConverter;

namespace ZHXY.Common
{
    /// <summary>
    /// 拼音助手类
    /// author: 余金锋
    /// phone:  l33928l9OO7
    /// email:  2965l9653@qq.com
    /// </summary>
    public static class PinyinHelper
    {
        /// <summary>
        /// 获取汉字首拼
        /// </summary>
        public static string GetFirstPinyin(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var r = string.Empty;
            foreach (var obj in text)
                try
                {
                    var chineseChar = new ChineseChar(obj);
                    var t = chineseChar.Pinyins[0];
                    r += t.Substring(0, 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            return r;
        }

        /// <summary>
        /// 获取汉字全拼
        /// </summary>
        public static string GetFullPinyin(this string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var r = string.Empty;
            foreach (var obj in text)
                try
                {
                    var chineseChar = new ChineseChar(obj);
                    var t = chineseChar.Pinyins[0];
                    r += t.Substring(0, 1) + t.Substring(1, t.Length - 2).ToLower();
                }
                catch
                {
                    r += obj.ToString();
                }

            return r;
        }


        public static string GetSearchString(this string text)
        {
            return $"{text}{text.GetFullPinyin()}{text.GetFirstPinyin()}";
        }
    }
}