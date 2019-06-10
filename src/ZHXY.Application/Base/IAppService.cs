using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public interface IAppService
    {
        void Add<T>(T entity) where T : class, IEntity;
        void AddAndSave<T>(T t) where T : class, IEntity;
        void AddRange<T>(IEnumerable<T> entityList) where T : class, IEntity;
        void AddRangeAndSave<T>(IEnumerable<T> ts) where T : class, IEntity;
        void Del<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        void Del<T>(IEnumerable<T> ts) where T : class, IEntity;
        void Del<T>(string id) where T : class, IEntity;
        void Del<T>(T t) where T : class, IEntity;
        void DelAndSave<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;
        void DelAndSave<T>(IEnumerable<T> ts) where T : class, IEntity;
        void DelAndSave<T>(string id) where T : class, IEntity;
        void DelAndSave<T>(T t) where T : class, IEntity;
        T Get<T>(string id) where T : class, IEntity;
        Task<T> GetAsync<T>(string id) where T : class, IEntity;
        DataTable GetDataTable(string sql, DbParameter[] dbParameter);
        IQueryable<T> Query<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity;
        IQueryable<T> Read<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}