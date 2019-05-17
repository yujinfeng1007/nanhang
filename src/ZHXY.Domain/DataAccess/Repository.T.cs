using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using ZHXY.Common;

namespace ZHXY.Domain
{
    /// <inheritdoc />
    /// <summary>
    ///     仓储实现
    /// </summary>
    public class Repository<T> : IRepositoryBase<T> where T : class, new()
    {
        public DbContext DbContext { get; set; }

        public Repository() => DbContext = new ZhxyDbContext();

       

        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += $"Property: {error.PropertyName} Error: {error.ErrorMessage}" +
                           Environment.NewLine;
            return msg;
        }

        public int Insert(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return DbContext.SaveChanges();
        }

        public void BatchDelete(Func<object, bool> p) => throw new NotImplementedException();

        public int BatchInsert(List<T> ts)
        {
            DbContext.Set<T>().AddRange(ts);
            return DbContext.SaveChanges();
        }

        public int Update(T entity)
        {
            try
            {
                if (DbContext.Entry(entity).State == EntityState.Detached) DbContext.Set<T>().Attach(entity);
                var props = entity.GetType().GetProperties().Where(p => !p.GetMethod.IsVirtual || p.GetMethod.IsFinal)
                    .ToArray();
                foreach (var prop in props)
                    if (prop.GetValue(entity, null) != null)
                    {
                        if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                            DbContext.Entry(entity).Property(prop.Name).CurrentValue = null;
                        else if (prop.PropertyType.FullName != null && prop.PropertyType.FullName.ToLower().IndexOf("datetime", StringComparison.Ordinal) !=
                                 -1)
                            if (DbContext.Entry(entity).Property(prop.Name).CurrentValue.ToDateOrNull() ==
                                DateTime.Parse("1900-1-1"))
                                DbContext.Entry(entity).Property(prop.Name).CurrentValue = null;
                        DbContext.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                    else
                    {
                        if (prop.PropertyType.FullName != null && prop.PropertyType.FullName.ToLower().IndexOf("boolean", StringComparison.Ordinal) != -1)
                        {
                            DbContext.Entry(entity).Property(prop.Name).CurrentValue = false;
                            DbContext.Entry(entity).Property(prop.Name).IsModified = true;
                        }
                    }

                return DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int Update(string strSql) => DbContext.Database.ExecuteSqlCommand(strSql);

        public int BatchUpdate(List<T> entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("Db.Set<T>()");

                return DbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public int BatchUpdate(T entity, Expression<Func<T, bool>> predicate)
        {
            try
            {
                // 批量更新
                var entitys = DbContext.Set<T>().Where(predicate);
                entitys.ToList().ForEach(item =>
                {
                    var props = entity.GetType().GetProperties()
                        .Where(p => !p.GetMethod.IsVirtual || p.GetMethod.IsFinal).ToArray();
                    foreach (var prop in props)
                        if (prop.GetValue(entity, null) != null)
                        {
                            var fv = prop.GetValue(entity, null);
                            if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                                fv = null;
                            else if (prop.PropertyType.FullName.ToLower()
                                         .IndexOf("datetime", StringComparison.Ordinal) != -1)
                                if (DbContext.Entry(entity).Property(prop.Name).CurrentValue.ToDateOrNull() ==
                                    DateTime.Parse("1900-1-1"))
                                    fv = null;
                            DbContext.Entry(item).Property(prop.Name).CurrentValue = fv;
                            DbContext.Entry(item).State = EntityState.Modified;
                        }
                        else
                        {
                            if (prop.PropertyType.FullName.ToLower().IndexOf("boolean", StringComparison.Ordinal) == -1)
                                continue;
                            DbContext.Entry(item).Property(prop.Name).CurrentValue = false;
                            DbContext.Entry(item).State = EntityState.Modified;
                        }
                });
                return DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (DbContext.Entry(entity).State == EntityState.Detached) DbContext.Set<T>().Attach(entity);
            DbContext.Set<T>().Remove(entity);
            return DbContext.SaveChanges();
        }

        public int BatchDelete(Expression<Func<T, bool>> predicate)
        {
            var list = DbContext.Set<T>().Where(predicate).ToList();
            DbContext.Set<T>().RemoveRange(list);
            return DbContext.SaveChanges();
        }

        public T Find(object keyValue) => DbContext.Set<T>().Find(keyValue);

        public T FirstOrDefault(Expression<Func<T, bool>> predicate) => DbContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate = null) => predicate == null ? DbContext.Set<T>() : DbContext.Set<T>().Where(predicate);

        public IQueryable<T> QueryAsNoTracking(Expression<Func<T, bool>> predicate = null) => predicate == null ? DbContext.Set<T>().AsNoTracking() : DbContext.Set<T>().AsNoTracking().Where(predicate);

        public List<T> FindList(string strSql) => DbContext.Database.SqlQuery<T>(strSql).ToList();

        public List<T> FindList(string strSql, object[] dbParameter) => DbContext.Database.SqlQuery<T>(strSql, dbParameter).ToList();

        public List<T> FindList(Expression<Func<T, bool>> predicate, Pagination pagination)
        {
            if (pagination == null) throw new ArgumentNullException(nameof(pagination));
            if (string.IsNullOrWhiteSpace(pagination.Sidx)) throw new ArgumentException(nameof(pagination.Sidx));
            var query = DbContext.Set<T>().AsNoTracking().AsQueryable();
            if (predicate != null) query = query.Where(predicate);
            pagination.Records = query.Count();
            return query.OrderBy(pagination.Sidx).Skip(pagination.Rows * (pagination.Page - 1)).Take(pagination.Rows)
                .ToList();
        }



        /// <summary>
        ///     执行 sql 语句,返回 DataTable。若无参数，DbParameter[]传null
        /// </summary>
        public DataTable GetDataTable(string sql, DbParameter[] dbParameter)
        {
            var conn = new SqlConnection();
            try
            {
                conn = (SqlConnection)DbContext.Database.Connection;
                var cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                if (dbParameter!=null)
                {
                    cmd.Parameters.AddRange(dbParameter);
                }
                var adapter = new SqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                adapter.Dispose();
                return table;
            }
            catch (Exception ex)
            {
                throw new Exception("执行失败：" + ex.Message+ "sql:"+sql);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 将DataTable转换为对象集合
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public List<TObject> DataTableToList<TObject>(DataTable dataTable)
        {
            var list = new List<TObject>();
            var targetType = typeof(TObject);
            PropertyInfo[] allPropertyArray = targetType.GetProperties();
            foreach (DataRow rowElement in dataTable.Rows)
            {
                var element = Activator.CreateInstance<TObject>();
                foreach (DataColumn columnElement in dataTable.Columns)
                {
                    foreach (var property in allPropertyArray)
                    {
                        if (property.Name.Trim().ToUpper().Equals(columnElement.ColumnName.Trim().ToUpper()))
                        {
                            if (rowElement[columnElement.ColumnName] == DBNull.Value)
                            {
                                property.SetValue(element, null, null);
                            }
                            else
                            {
                                property.SetValue(element, rowElement
                               [columnElement.ColumnName], null);
                            }
                        }
                    }
                }
                list.Add(element);
            }
            return list;
        }

        public void Delete(string id)
        {
            var entity = DbContext.Set<T>().Find(id);
            if (null == entity) throw new Exception("未找到对象!");
            DbContext.Set<T>().Remove(entity);
            DbContext.SaveChanges();
        }
    }
}