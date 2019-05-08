using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZHXY.Domain;

namespace ZHXY.Data
{
    /// <inheritdoc />
    /// <summary>
    /// 仓储基类
    /// author: 余金锋
    /// phone:  l33928l9OO7
    /// email:  2965l9653@qq.com
    /// </summary>
    public abstract class Repоsitory : IRepоsitory
    {
        public DbContext Db { get; }

        protected Repоsitory(DbContext db) => Db = db;

        protected Repоsitory() => Db = new ZhxyDbContext();

        public int SaveChanges() => Db.SaveChanges();

        public async Task<int> SaveChangesAsync() => await Db.SaveChangesAsync();

        public void Attach<T>(T t) where T : class
        {
            if (Db.Entry(t).State == EntityState.Detached) Db.Set<T>().Attach(t);
        }

        #region insert remove update

        public void Add<T>(T t) where T : class => Db.Set<T>().Add(t);

        public void AddRange<T>(IEnumerable<T> entityList) where T : class => Db.Set<T>().AddRange(entityList);

        public void Remove<T>(T t) where T : class
        {
            Attach(t);
            Db.Set<T>().Remove(t);
        }

        public void Remove<T>(string key) where T : class
        {
            var t = Db.Set<T>().Find(key);
            if (t != null) Remove(t);
        }

        public void RemoveRange<T>(IEnumerable<T> entityList) where T : class
        {
            var enumerable = entityList.ToList();
            foreach (var t in enumerable) Attach(t);
            Db.Set<T>().RemoveRange(enumerable);
        }

        #endregion insert remove

        #region query

        public T Find<T>(dynamic key) where T : class => Db.Set<T>().Find(key);

        public async Task<T> FindAsync<T>(dynamic key) where T : class => await Db.Set<T>().FindAsync(key);

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> expression = null) where T : class => expression == null ? Db.Set<T>().AsQueryable() : Db.Set<T>().Where(expression);

        public IQueryable<T> QueryAsNoTracking<T>(Expression<Func<T, bool>> expression = null) where T : class => expression == null
                ? Db.Set<T>().AsNoTracking()
                : Db.Set<T>().AsNoTracking().Where(expression);

        #endregion query
    }
}