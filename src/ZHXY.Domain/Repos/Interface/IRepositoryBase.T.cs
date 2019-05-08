using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ZHXY.Common;

namespace ZHXY.Domain
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        DbContext DbContext { get; }

        int Insert(TEntity entity);

        int BatchInsert(List<TEntity> entityList);

        int Update(TEntity entity);

        int Update(string strSql);

        int BatchUpdate(List<TEntity> entity);

        int BatchUpdate(TEntity entity, Expression<Func<TEntity, bool>> predicate);

        int Delete(TEntity entity);

        int BatchDelete(Expression<Func<TEntity, bool>> predicate);

        TEntity Find(object keyValue);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null);

        IQueryable<TEntity> QueryAsNoTracking(Expression<Func<TEntity, bool>> predicate = null);

        List<TEntity> FindList(string strSql);

        List<TEntity> FindList(string strSql, object[] dbParameter);

        List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination);


        DataTable GetDataTable(string sql, DbParameter[] dbParameter);

        List<TObject> DataTableToList<TObject>(DataTable dataTable);
        void Delete(string id);
    }
}