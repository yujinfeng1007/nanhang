using System.Linq;
using System.Linq.Dynamic;

namespace ZHXY.Common
{
    /// <summary>
    ///     查询扩展方法
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public static class PaginationExt
    {

        public static IQueryable<T> Paging<T>(this IQueryable<T> query, Pagination pag) where T : class, new()
        {
            pag.CheckSort<T>();
            pag.Records = query.Count();
            return query.OrderBy(pag.Sidx).Skip(pag.Rows * (pag.Page - 1)).Take(pag.Rows);
        }


        /// <summary>
        ///     检查排序字段是否合法
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="sidx">排序字段</param>
        /// <returns></returns>
        private static bool Check<T>(this string sidx) where T : class, new()
        {
            if (string.IsNullOrEmpty(sidx)) return false;
            var result = true;
            var arr = sidx.ToLower().Split(',');
            var props = typeof(T).GetProperties().Select(p => p.Name.ToLower()).ToArray();
            foreach (var item in arr)
            {
                var split = item.Trim().Split(' ');
                var fieldName = split[0].Trim();
                if (!props.Contains(fieldName))
                {
                    result = false;
                    break;
                }

                if (split.Length <= 1) continue;
                var sortDirection = split[1].Trim();
                if (sortDirection == "asc" || sortDirection == "desc") continue;
                result = false;
                break;
            }

            return result;
        }

        /// <summary>
        ///     检查排序字段
        /// </summary>
        public static void CheckSort<T>(this Pagination pag) where T : class, new()
        {
            pag.Sidx = $"{pag.Sidx} {pag.Sord}";
            pag.Sidx = Check<T>(pag.Sidx) ? pag.Sidx : "false";
        }
    }
}