using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZHXY.Common;

namespace ZHXY.Application
{
    public static class TreeQuery
    {
        public static List<T> TreeWhere<T>(this List<T> entityList, Predicate<T> condition, string keyValue = "F_Id",
            string parentId = "F_ParentId") where T : class
        {
            var locateList = entityList.FindAll(condition);
            var parameter = Expression.Parameter(typeof(T), "t");
            var treeList = new List<T>();
            var pids = new List<string>();
            foreach (var entity in locateList)
            {
                treeList.Add(entity);
                var pId = entity.GetType().GetProperty(parentId).GetValue(entity, null).ToString();
                while (true)
                {
                    if (string.IsNullOrEmpty(pId) || pId == "0" || pids.Contains(pId)) break;
                    pids.Add(pId);
                    var upLambda = Expression.Equal(parameter.Property(keyValue), Expression.Constant(pId))
                        .ToLambda<Predicate<T>>(parameter).Compile();
                    var upRecord = entityList.Find(upLambda);
                    if (upRecord != null)
                    {
                        treeList.Add(upRecord);
                        pId = upRecord.GetType().GetProperty(parentId).GetValue(upRecord, null).ToString();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return treeList.Distinct().ToList();
        }

        public static List<T> TreeWhereNoParent<T>(this List<T> entityList, Predicate<T> condition,
            string keyValue = "F_Id", string parentId = "F_ParentId") where T : class
        {
            var locateList = entityList.FindAll(condition);
            var parameter = Expression.Parameter(typeof(T), "t");
            var treeList = locateList;
            
            return treeList.Distinct().ToList();
        }




        public async static System.Threading.Tasks.Task<List<T>> TreeWhereTask<T>(this List<T> entityList, Predicate<T> condition, string keyValue = "F_Id",
            string parentId = "F_ParentId") where T : class
        {
            var treeList = new List<T>();
            await System.Threading.Tasks.Task.Run(() =>
            {
                var locateList = entityList.FindAll(condition);
                var parameter = Expression.Parameter(typeof(T), "t");

                var pids = new List<string>();
                foreach (var entity in locateList)
                {
                    treeList.Add(entity);
                    var pId = entity.GetType().GetProperty(parentId).GetValue(entity, null).ToString();
                    while (true)
                    {
                        if (string.IsNullOrEmpty(pId) || pId == "0" || pids.Contains(pId)) break;
                        pids.Add(pId);
                        var upLambda = Expression.Equal(parameter.Property(keyValue), Expression.Constant(pId))
                            .ToLambda<Predicate<T>>(parameter).Compile();
                        var upRecord = entityList.Find(upLambda);
                        if (upRecord != null)
                        {
                            treeList.Add(upRecord);
                            pId = upRecord.GetType().GetProperty(parentId).GetValue(upRecord, null).ToString();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            });
            return treeList.Distinct().ToList();
        }
    }
}