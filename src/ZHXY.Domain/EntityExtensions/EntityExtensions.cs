using System;
using System.Text;
using ZHXY.Common;

namespace ZHXY.Domain
{
    /// <summary>
    ///     实体扩展方法
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public static class EntityExtensions
    {
     


        public static string GetSearchString<T>(this T t, string[] fields) where T : IEntity
        {
            var sb = new StringBuilder();
            var type = typeof(T);
            foreach (var field in fields)
            {
                var prop = type.GetProperty(field);
                if (prop == null) continue;
                var val = prop.GetValue(t)?.ToString();
                sb.Append($"{val}{val.GetFirstPinyin()}{val.GetFullPinyin()}");
            }

            return sb.ToString();
        }
    }
}