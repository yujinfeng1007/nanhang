using System;
using System.Linq;
using System.Linq.Expressions;

namespace ZHXY.Domain
{
    public interface IUnitWork : IDisposable
    {
        IUnitWork BeginTrans();

        int Commit();

        int Insert<T>(T entity) where T : class;

        int Update<T>(T entity) where T : class;

        T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class;

        IQueryable<T> QueryAsNoTracking<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}