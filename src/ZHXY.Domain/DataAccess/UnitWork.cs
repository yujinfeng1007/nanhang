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
        public DbContext Db { get; set; }
        private DbTransaction DbTransaction { get; set; }

        public UnitWork(DbContext db) => Db =db;
        public UnitWork() => Db =new ZhxyDbContext();


        public IUnitWork BeginTrans()
        {
            // 如果走事务,当前数据库上下文需要重新创建

            Db = new ZhxyDbContext();

            var dbConnection = ((IObjectContextAdapter)Db).ObjectContext.Connection;
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            DbTransaction = dbConnection.BeginTransaction();
            return this;
        }

        public int Commit()
        {
            try
            {
                var returnValue = Db.SaveChanges();
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
            Db.Dispose();
        }

        public int Insert<T>(T t) where T : class
        {
            Db.Set<T>().Add(t);
            return DbTransaction == null ? Commit() : 0;
        }

        public int BatchInsert<T>(List<T> entities) where T : class
        {
            Db.Set<T>().AddRange(entities);
            return DbTransaction == null ? Commit() : 0;
        }

        public int Update<T>(T entity) where T : class
        {
            if (Db.Entry(entity).State == EntityState.Detached) Db.Set<T>().Attach(entity);
            var props = entity.GetType().GetProperties().Where(p => !p.GetMethod.IsVirtual || p.GetMethod.IsFinal)
                .ToArray();

            foreach (var prop in props)
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        Db.Entry(entity).Property(prop.Name).CurrentValue = null;
                    else if (prop.PropertyType.FullName.ToLower().IndexOf("datetime", StringComparison.Ordinal) != -1)
                        if (Db.Entry(entity).Property(prop.Name).CurrentValue.ToDateOrNull() ==
                            DateTime.Parse("1900-1-1"))
                            Db.Entry(entity).Property(prop.Name).CurrentValue = null;
                    Db.Entry(entity).Property(prop.Name).IsModified = true;
                }
                else
                {
                    if (prop.PropertyType.FullName.ToLower().IndexOf("boolean", StringComparison.Ordinal) != -1)
                    {
                        Db.Entry(entity).Property(prop.Name).CurrentValue = false;
                        Db.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                }

            return DbTransaction == null ? Commit() : 0;
        }

        public int Delete<T>(T entity) where T : class
        {
            if (Db.Entry(entity).State == EntityState.Detached) Db.Set<T>().Attach(entity);
            Db.Entry(entity).State = EntityState.Deleted;
            Db.Set<T>().Remove(entity);
            return DbTransaction == null ? Commit() : 0;
        }

        public int Delete<T>(List<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                if (Db.Entry(entity).State == EntityState.Detached) Db.Set<T>().Attach(entity);
            }

            entities.ForEach(m => Db.Entry(m).State = EntityState.Deleted);
            Db.Set<T>().RemoveRange(entities);
            return DbTransaction == null ? Commit() : 0;
        }

        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entities = Db.Set<TEntity>().Where(predicate).ToList();
            Db.Set<TEntity>().RemoveRange(entities);
            return DbTransaction == null ? Commit() : 0;
        }

        #region query

        public T Find<T>(object keyValue) where T : class => Db.Set<T>().Find(keyValue);

        public T FindEntity<T>(Expression<Func<T, bool>> predicate) where T : class => Db.Set<T>().AsNoTracking().FirstOrDefault(predicate);

        public IQueryable<T> QueryAsNoTracking<T>() where T : class => Db.Set<T>().AsNoTracking();

        public IQueryable<T> QueryAsNoTracking<T>(Expression<Func<T, bool>> predicate) where T : class => Db.Set<T>().AsNoTracking().Where(predicate);

        #endregion query
    }
}