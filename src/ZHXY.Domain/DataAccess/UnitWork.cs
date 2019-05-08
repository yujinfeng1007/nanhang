using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using ZHXY.Common;

namespace ZHXY.Domain
{
    /// <summary>
    ///     仓储实现
    /// </summary>
    public class UnitWork : IUnitWork
    {
        public DbContext DbContext { get; set; }
        private DbTransaction DbTransaction { get; set; }

        public UnitWork() => DbContext = new ZhxyDbContext();

        public UnitWork(string schoolCode) => DbContext = new ZhxyDbContext(schoolCode);

        public IUnitWork BeginTrans()
        {
            // 如果走事务,当前数据库上下文需要重新创建

            DbContext = new ZhxyDbContext();

            var dbConnection = ((IObjectContextAdapter)DbContext).ObjectContext.Connection;
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            DbTransaction = dbConnection.BeginTransaction();
            return this;
        }

        public IUnitWork BeginTrans(string schoolCode)
        {
            // 如果走事务,当前数据库上下文需要重新创建
            DbContext = new ZhxyDbContext(schoolCode);
            var dbConnection = ((IObjectContextAdapter)DbContext).ObjectContext.Connection;
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            DbTransaction = dbConnection.BeginTransaction();
            return this;
        }

        public int Commit()
        {
            try
            {
                var returnValue = DbContext.SaveChanges();
                DbTransaction?.Commit();
                return returnValue;
            }
            catch (DbEntityValidationException)
            {
                DbTransaction?.Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            DbTransaction?.Dispose();
            DbContext.Dispose();
        }

        public int Insert<T>(T t) where T : class
        {
            DbContext.Set<T>().Add(t);
            return DbTransaction == null ? Commit() : 0;
        }

        public int BatchInsert<T>(List<T> entities) where T : class
        {
            DbContext.Set<T>().AddRange(entities);
            return DbTransaction == null ? Commit() : 0;
        }

        public int Update<T>(T entity) where T : class
        {
            if (DbContext.Entry(entity).State == EntityState.Detached) DbContext.Set<T>().Attach(entity);
            var props = entity.GetType().GetProperties().Where(p => !p.GetMethod.IsVirtual || p.GetMethod.IsFinal)
                .ToArray();

            foreach (var prop in props)
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        DbContext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    else if (prop.PropertyType.FullName.ToLower().IndexOf("datetime", StringComparison.Ordinal) != -1)
                        if (DbContext.Entry(entity).Property(prop.Name).CurrentValue.ToDateOrNull() ==
                            DateTime.Parse("1900-1-1"))
                            DbContext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    DbContext.Entry(entity).Property(prop.Name).IsModified = true;
                }
                else
                {
                    if (prop.PropertyType.FullName.ToLower().IndexOf("boolean", StringComparison.Ordinal) != -1)
                    {
                        DbContext.Entry(entity).Property(prop.Name).CurrentValue = false;
                        DbContext.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                }

            return DbTransaction == null ? Commit() : 0;
        }

        public int Delete<T>(T entity) where T : class
        {
            if (DbContext.Entry(entity).State == EntityState.Detached) DbContext.Set<T>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
            DbContext.Set<T>().Remove(entity);
            return DbTransaction == null ? Commit() : 0;
        }

        public int Delete<T>(List<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                if (DbContext.Entry(entity).State == EntityState.Detached) DbContext.Set<T>().Attach(entity);
            }

            entities.ForEach(m => DbContext.Entry(m).State = EntityState.Deleted);
            DbContext.Set<T>().RemoveRange(entities);
            return DbTransaction == null ? Commit() : 0;
        }

        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entities = DbContext.Set<TEntity>().Where(predicate).ToList();
            DbContext.Set<TEntity>().RemoveRange(entities);
            return DbTransaction == null ? Commit() : 0;
        }

        #region query

        public T Find<T>(object keyValue) where T : class => DbContext.Set<T>().Find(keyValue);

        public T FindEntity<T>(Expression<Func<T, bool>> predicate) where T : class => DbContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);

        public IQueryable<T> QueryAsNoTracking<T>() where T : class => DbContext.Set<T>().AsNoTracking();

        public IQueryable<T> QueryAsNoTracking<T>(Expression<Func<T, bool>> predicate) where T : class => DbContext.Set<T>().AsNoTracking().Where(predicate);

        #endregion query
    }
}