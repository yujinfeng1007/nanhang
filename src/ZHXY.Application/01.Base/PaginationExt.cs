using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;

namespace ZHXY.Application
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
            var sord=pag.GetOrdering<T>();
            pag.Records = query.CountAsync().Result;
            return query.OrderBy(sord).Skip(pag.Skip).Take(pag.Rows);
        }

        /// <summary>
        ///     检查排序字段
        /// </summary>
        public static string GetOrdering<T>(this Pagination pag) where T : class, new()
        {
            //ordering
            if (string.IsNullOrEmpty(pag.Sidx)) return "false";
            if (typeof(T).GetProperties().Select(p => p.Name.ToLower()).ToArray().Contains(pag.Sidx.ToLower()))
            {
                return "false";
            }
            else
            {
               return $"{pag.Sidx} {pag.Sord}";
            }
            
            
        }
    }
}