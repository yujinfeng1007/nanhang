using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ZHXY.Domain
{
    public static class EFExtensions
    {
        public static int SaveChanges(this DbContext db) => db.SaveChanges();
        public static async Task<int> SaveChangesAsync(this DbContext db) => await db.SaveChangesAsync();

        public static void Attach<T>(this DbContext db, T t) where T : class
        {
            if (db.Entry(t).State == EntityState.Detached) db.Set<T>().Attach(t);
        }

        #region insert remove update

        public static void Add<T>(this DbContext db, T t) where T : class => db.Set<T>().Add(t);

        public static void AddRange<T>(this DbContext db, IEnumerable<T> entityList) where T : class => db.Set<T>().AddRange(entityList);

        public static void Remove<T>(this DbContext db, T t) where T : class
        {
            db.Attach(t);
            db.Set<T>().Remove(t);
        }

        public static void Remove<T>(this DbContext db, string key) where T : class
        {
            var t = db.Set<T>().Find(key);
            if (t != null) db.Remove(t);
        }

        public static void RemoveRange<T>(this DbContext db, IEnumerable<T> entityList) where T : class
        {
            var enumerable = entityList.ToList();
            foreach (var t in enumerable) db.Attach(t);
            db.Set<T>().RemoveRange(enumerable);
        }

        #endregion insert remove

        #region query

        public static T Find<T>(this DbContext db, dynamic key) where T : class => db.Set<T>().Find(key);

        public static async Task<T> FindAsync<T>(this DbContext db, dynamic key) where T : class => await db.Set<T>().FindAsync(key);

        public static IQueryable<T> Query<T>(this DbContext db, Expression<Func<T, bool>> expression = null) where T : class => expression == null ? db.Set<T>().AsQueryable() : db.Set<T>().Where(expression);

        public static IQueryable<T> QueryAsNoTracking<T>(this DbContext db, Expression<Func<T, bool>> expression = null) where T : class => expression == null
                ? db.Set<T>().AsNoTracking()
                : db.Set<T>().AsNoTracking().Where(expression);

        #endregion query
    }
}