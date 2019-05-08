using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ZHXY.Domain
{
    /// <summary>
    ///     仓储接口
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public interface IRepоsitory
    {
        DbContext Db { get; }

        void Add<T>(T entity) where T : class;

        void AddRange<T>(IEnumerable<T> entityList) where T : class;

        void Remove<T>(T entity) where T : class;

        void Remove<T>(string key) where T : class;

        void RemoveRange<T>(IEnumerable<T> entityList) where T : class;

        T Find<T>(dynamic key) where T : class;

        Task<T> FindAsync<T>(dynamic key) where T : class;

        IQueryable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class;

        IQueryable<T> QueryAsNoTracking<T>(Expression<Func<T, bool>> expression = null) where T : class;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Attach<T>(T entity) where T : class;
    }
}