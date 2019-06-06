using System.Data.Entity;
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

        public static IQueryable<T> PagingNoSort<T>(this IQueryable<T> query, Pagination pag) where T : class, new()
        {
            pag.Records = query.CountAsync().Result;
            return query.Skip(pag.Skip).Take(pag.Rows);
        }

        public static IQueryable<T> Paging<T>(this IQueryable<T> query, Pagination pag) where T : class, new()
        {
            var ordering=pag.GetOrdering<T>();
            pag.Records = query.CountAsync().Result;
            return query.OrderBy(ordering).Skip(pag.Skip).Take(pag.Rows);
        }

       
        /// <summary>
        /// 获取排序规则
        /// </summary>
        public static string GetOrdering<T>(this Pagination pag) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(pag.Sidx)) return "false";
            var sidx = pag.Sidx.ToLower();
            if (!typeof(T).GetProperties().Select(p => p.Name.ToLower()).ToArray().Contains(sidx)) return "false";

            if (string.IsNullOrWhiteSpace(pag.Sord))
            {
                return sidx;
            }
            else
            {
                return $"{sidx} {pag.Sord}";
            }

           
        }
    }
}