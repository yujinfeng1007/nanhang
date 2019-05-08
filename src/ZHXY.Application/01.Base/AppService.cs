using ZHXY.Common;
using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
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

        #region 增删改

        #region 添加

        public void Create<T>(T t) where T : class, ICreateAuditable, IEntity
        {
            t.Create(OperatorProvider.Current.DepartmentId, OperatorProvider.Current.UserId);
            R.Add(t);
        }

        public void Add<T>(T t) where T : class, IEntity => R.Add(t);
        public void Add<T>(IEnumerable<T> entityList) where T : class, IEntity => R.AddRange(entityList);

        #endregion 添加

        #region 添加并保存

        public void CreateAndSave<T>(T t) where T : class, ICreateAuditable, IEntity
        {
            Create(t);
            R.SaveChanges();
        }

        public async Task CreateAndSaveAsync<T>(T t) where T : class, ICreateAuditable, IEntity
        {
            Create(t);
            await R.SaveChangesAsync();
        }

        public void AddAndSave<T>(T t) where T : class, IEntity
        {
            R.Add(t);
            R.SaveChanges();
        }

        public void AddAndSave<T>(IEnumerable<T> ts) where T : class, IEntity
        {
            R.AddRange(ts);
            R.SaveChanges();
        }

        #endregion 添加并保存

        #region 删除

        public void Del<T>(IEnumerable<T> ts) where T : class, IEntity
        {
            R.RemoveRange(ts);
        }

        public void Del<T>(string[] ids) where T : BaseEntity, IEntity
        {
            var list = R.Query<T>(p => ids.Contains(p.Id)).ToList();
            R.RemoveRange(list);
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

        #endregion 物理删除并保存

        #region 修改

        public void Modify<T>(T t) where T : class, IModifiedAuditable, IEntity
        {
            R.Attach(t);
            t.Modify(OperatorProvider.Current.UserId);
        }

        #endregion 修改

        #endregion 增删改

        #region 查询

        public T Get<T>(string id) where T : class, IEntity => R.Find<T>(id);

        public async Task<T> GetAsync<T>(string id) where T : class, IEntity => await R.FindAsync<T>(id);

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity => R.Query(expression);

        public IQueryable<T> Read<T>(Expression<Func<T, bool>> expression = null) where T : class, IEntity => R.QueryAsNoTracking(expression);


        public IQueryable<T> ReadUsable<T>(Expression<Func<T, bool>> expression = null) where T : class, IDeleteAuditable, ICreateAuditable => R.QueryAsNoTracking(expression).Where(p => p.IsDeleted != true);

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

        #endregion 查询

        #region 保存更改

        public int SaveChanges() => R.SaveChanges();

        public async Task<int> SaveChangesAsync() => await R.SaveChangesAsync();

        #endregion 保存更改

        #region 其他方法

       

      
        ///// <summary>
        ///// 数据过滤器
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //public Expression<Func<TEntity, bool>> DataScopeFilter<TEntity>(Expression<Func<TEntity, bool>> oldExpression) where TEntity : class, ICreationAudited
        //{
        //    //1	所有数据	all
        //    //7	仅个人数据	Personal
        //    //8	仅当前机构数据（不含下级）	CurrentDep
        //    //9	自定义	Diy
        //    var expression = ExtLinq.False<TEntity>();
        //    var currentUser = OperatorProvider.Current;
        //    var roles = currentUser.Roles;
        //    if (OperatorProvider.Current.IsSystem) return oldExpression;
        //    foreach (var kvp in roles)
        //    {
        //        if (kvp.Value.Count() != 0)
        //        {
        //            var datatype = kvp.Value.First().Key;
        //            if ("all".Equals(datatype)) return oldExpression;

        //            if ("Personal".Equals(datatype))
        //            {
        //                expression = expression.Or(t => t.F_CreatorUserId == currentUser.UserId);
        //            }
        //            //本部门创建数据
        //            else if ("CurrentDep".Equals(datatype))
        //            {
        //                expression = expression.Or(t => t.F_DepartmentId == currentUser.DepartmentId);
        //            }
        //            //本部门及子部门创建数据
        //            else if ("CurrentDepAndSubDep".Equals(datatype))
        //            {
        //                expression = expression.Or(t => t.F_DepartmentId == currentUser.DepartmentId);
        //                var orgs = new SysOrganizeAppService().GetListByParentId(currentUser.DepartmentId);
        //                foreach (var org in orgs)
        //                {
        //                    expression = expression.Or(t => t.F_DepartmentId == org.F_Id);
        //                }
        //            }
        //            //自定义部门创建数据
        //            else if ("Diy".Equals(datatype))
        //            {
        //                var datadeps = kvp.Value.First().Value.Split(',');
        //                foreach (var item in datadeps)
        //                {
        //                    expression = expression.Or(t => t.F_DepartmentId == item);
        //                }
        //            }

        //        }
        //    }
        //    return expression.And(oldExpression);

        //}

        //public string DataScopeFilter(string oldExpression)
        //{
        //    var expression = "";
        //    var user = OperatorProvider.Current;
        //    var roles = user.Roles;
        //    var orgApp = new SysOrganizeAppService();
        //    if (OperatorProvider.Current.IsSystem)
        //    {
        //        return oldExpression;
        //    }
        //    else
        //    {
        //        foreach (var kvp in roles)
        //        {
        //            if (kvp.Value.Count() != 0)
        //            {
        //                var datatype = kvp.Value.First().Key;
        //                if ("all".Equals(datatype))
        //                {
        //                    return oldExpression;
        //                }
        //                else
        //                {
        //                    if ("Personal".Equals(datatype))
        //                    {
        //                        expression += " t.F_CreatorUserId='" + user.UserId + "'";
        //                    }
        //                    else if ("CurrentDep".Equals(datatype))
        //                    {
        //                        expression += " t.F_DepartmentId='" + user.DepartmentId + "'";
        //                    }
        //                    else if ("CurrentDepAndSubDep".Equals(datatype))
        //                    {
        //                        expression += " t.F_DepartmentId='" + user.DepartmentId + "'";
        //                        var orgs = orgApp.GetListByParentId(user.DepartmentId);
        //                        foreach (var org in orgs)
        //                        {
        //                            expression += " or t.F_DepartmentId='" + org.F_Id + "'";
        //                        }
        //                    }
        //                    //自定义部门创建数据
        //                    else if ("Diy".Equals(datatype))
        //                    {
        //                        var datadeps = kvp.Value.First().Value.Split(',');
        //                        var i = 1;
        //                        foreach (var item in datadeps)
        //                        {
        //                            if (i == 1)
        //                            {
        //                                expression += " t.F_DepartmentId='" + item + "'";
        //                                i++;
        //                            }
        //                            else
        //                                expression += " or t.F_DepartmentId='" + item + "'";
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        var exportSql = oldExpression + " and " + "(" + expression + ")";
        //        return exportSql;
        //    }
        //}


        #endregion 其他方法
    }

    public static  class DataScopeFilterExt
    {
        public static string DataScopeFilter(this AppService app, string oldExpression)
        {
            var expression = "";
            var user = OperatorProvider.Current;
            var roles = user.Roles;
            var orgApp = new SysOrganizeAppService();
            if (OperatorProvider.Current.IsSystem)
            {
                return oldExpression;
            }
            else
            {
                foreach (var kvp in roles)
                {
                    if (kvp.Value.Count() != 0)
                    {
                        var datatype = kvp.Value.First().Key;
                        if ("all".Equals(datatype))
                        {
                            return oldExpression;
                        }
                        else
                        {
                            if ("Personal".Equals(datatype))
                            {
                                expression += " t.F_CreatorUserId='" + user.UserId + "'";
                            }
                            else if ("CurrentDep".Equals(datatype))
                            {
                                expression += " t.F_DepartmentId='" + user.DepartmentId + "'";
                            }
                            else if ("CurrentDepAndSubDep".Equals(datatype))
                            {
                                expression += " t.F_DepartmentId='" + user.DepartmentId + "'";
                                var orgs = orgApp.GetListByParentId(user.DepartmentId);
                                foreach (var org in orgs)
                                {
                                    expression += " or t.F_DepartmentId='" + org.F_Id + "'";
                                }
                            }
                            //自定义部门创建数据
                            else if ("Diy".Equals(datatype))
                            {
                                var datadeps = kvp.Value.First().Value.Split(',');
                                var i = 1;
                                foreach (var item in datadeps)
                                {
                                    if (i == 1)
                                    {
                                        expression += " t.F_DepartmentId='" + item + "'";
                                        i++;
                                    }
                                    else
                                        expression += " or t.F_DepartmentId='" + item + "'";
                                }
                            }
                        }
                    }
                }
                var exportSql = oldExpression + " and " + "(" + expression + ")";
                return exportSql;
            }
        }
        public static  Expression<Func<TEntity, bool>> DataScopeFilter<TEntity>( this AppService app,Expression<Func<TEntity, bool>> oldExpression) where TEntity : class, ICreationAudited
        {
            //1	所有数据	all
            //7	仅个人数据	Personal
            //8	仅当前机构数据（不含下级）	CurrentDep
            //9	自定义	Diy
            var expression = ExtLinq.False<TEntity>();
            var currentUser = OperatorProvider.Current;
            var roles = currentUser.Roles;
            if (OperatorProvider.Current.IsSystem) return oldExpression;
            foreach (var kvp in roles)
            {
                if (kvp.Value.Count() != 0)
                {
                    var datatype = kvp.Value.First().Key;
                    if ("all".Equals(datatype)) return oldExpression;

                    if ("Personal".Equals(datatype))
                    {
                        expression = expression.Or(t => t.F_CreatorUserId == currentUser.UserId);
                    }
                    //本部门创建数据
                    else if ("CurrentDep".Equals(datatype))
                    {
                        expression = expression.Or(t => t.F_DepartmentId == currentUser.DepartmentId);
                    }
                    //本部门及子部门创建数据
                    else if ("CurrentDepAndSubDep".Equals(datatype))
                    {
                        expression = expression.Or(t => t.F_DepartmentId == currentUser.DepartmentId);
                        var orgs = new SysOrganizeAppService().GetListByParentId(currentUser.DepartmentId);
                        foreach (var org in orgs)
                        {
                            expression = expression.Or(t => t.F_DepartmentId == org.F_Id);
                        }
                    }
                    //自定义部门创建数据
                    else if ("Diy".Equals(datatype))
                    {
                        var datadeps = kvp.Value.First().Value.Split(',');
                        foreach (var item in datadeps)
                        {
                            expression = expression.Or(t => t.F_DepartmentId == item);
                        }
                    }

                }
            }
            return expression.And(oldExpression);

        }
    }
}