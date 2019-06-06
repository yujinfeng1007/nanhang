using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 应用服务基类,过时方法，请使用BassAppService
    /// </summary>
    public abstract class AppService
    {
        #region 属性
        /// <summary>
        /// 子类需要使用该仓储对象需要自行初始化
        /// </summary>
        protected IRepоsitory R { get; set; }

        #endregion 属性

        public AppService(IZhxyRepository r) => R = r;
        public AppService() { }

        public void AddRange<T>(IEnumerable<T> entityList) where T : class, IEntity => R.AddRange(entityList);
        public void Add<T>(T entity) where T : class, IEntity => R.Add(entity);

        public void AddAndSave<T>(T t) where T : class, IEntity
        {
            R.Add(t);
            R.SaveChanges();
        }

        public void AddRangeAndSave<T>(IEnumerable<T> ts) where T : class, IEntity
        {
            R.AddRange(ts);
            R.SaveChanges();
        }


        public void Del<T>(IEnumerable<T> ts) where T : class, IEntity
        {
            R.RemoveRange(ts);
        }

        public void Del<T>(string id) where T : class, IEntity
        {
            var entity = R.Find<T>(id);
            if (entity != null)
            {
                R.Remove(entity);
            }
        }

        public void Del<T>(T t) where T : class, IEntity
        {
            if (t != null)
            {
                R.Remove(t);
            }
        }

        public void Del<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            var removeList = R.Query(expression).ToListAsync().Result;
            R.RemoveRange(removeList);
        }
        public void DelAndSave<T>(IEnumerable<T> ts) where T : class, IEntity
        {
            R.RemoveRange(ts);
            R.SaveChanges();
        }

        public void DelAndSave<T>(string id) where T : class, IEntity
        {
            Del<T>(id);
            R.SaveChanges();
        }

        public void DelAndSave<T>(T t) where T : class, IEntity
        {
            R.Remove(t);
            R.SaveChanges();
        }
        public void DelAndSave<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            Del(expression);
            R.SaveChanges();
        }

        public T Get<T>(string id) where T : class, IEntity => R.Find<T>(id);

        public async Task<T> GetAsync<T>(string id) where T : class, IEntity => await R.FindAsync<T>(id);

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity => R.Query(expression);

        public IQueryable<T> Read<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity => R.QueryAsNoTracking(expression);

        public DataTable GetDataTable(string sql, DbParameter[] dbParameter)
        {
            var conn = new SqlConnection();
            try
            {
                conn = (SqlConnection)R.Db.Database.Connection;
                var cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                cmd.Parameters.AddRange(dbParameter);
                var adapter = new SqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                throw new Exception("执行失败：" + ex.Message);
            }
            finally
            {
                conn.Close();
            }


        }


        public int SaveChanges() => R.SaveChanges();

        public async Task<int> SaveChangesAsync() => await R.SaveChangesAsync();


    }
}