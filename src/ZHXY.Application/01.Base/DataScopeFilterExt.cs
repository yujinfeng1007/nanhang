using ZHXY.Common;
using ZHXY.Domain;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace ZHXY.Application
{
    public static  class DataScopeFilterExt
    {
        public static string DataScopeFilter(this AppService app, string oldExpression)
        {
            var expression = "";
            var user = OperatorProvider.Current;
            var roles = user.Roles;
            var orgApp = new OrgService();
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
                                    expression += " or t.F_DepartmentId='" + org.Id + "'";
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
                        var orgs = new OrgService().GetListByParentId(currentUser.DepartmentId);
                        foreach (var org in orgs)
                        {
                            expression = expression.Or(t => t.F_DepartmentId == org.Id);
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