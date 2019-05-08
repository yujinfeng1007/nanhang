using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ZHXY.Domain
{
    public interface IUnitWork : IDisposable
    {
        IUnitWork BeginTrans();

        int Commit();

        int Insert<T>(T entity) where T : class;

        int BatchInsert<T>(List<T> entityList) where T : class;

        int Update<T>(T entity) where T : class;

        int Delete<T>(T entity) where T : class;

        int Delete<T>(List<T> entityList) where T : class;

        int Delete<T>(Expression<Func<T, bool>> predicate) where T : class;

        T Find<T>(object keyValue) where T : class;

        T FindEntity<T>(Expression<Func<T, bool>> predicate) where T : class;

        IQueryable<T> QueryAsNoTracking<T>() where T : class;

        IQueryable<T> QueryAsNoTracking<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}