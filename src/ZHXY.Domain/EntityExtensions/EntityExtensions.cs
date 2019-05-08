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
        public static void Create<T>(this T t, string deptId = null, string creatorId = null) where T : ICreateAuditable
        {
            t.Id = Guid.NewGuid().ToString("N").ToUpper();
            t.CreatedTime = DateTime.Now;
            t.OwnerDeptId = deptId;
            t.CreatedByUserId = creatorId;
        }

        public static void Modify<T>(this T t, string modifier = null) where T : IModifiedAuditable
        {
            t.LastModifiedTime = DateTime.Now;
            t.LastModifiedByUserId = modifier;
        }

        public static void MarkAsDeleted<T>(this T t, string deleteOperator = null) where T : IDeleteAuditable
        {
            t.DeletedTime = DateTime.Now;
            t.DeletedByUserId = deleteOperator;
            t.IsDeleted = true;
        }

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