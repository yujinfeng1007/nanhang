using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 树结构帮助类
    /// author: 余金锋
    /// phone:  l33928l9OO7
    /// email:  2965l9653@qq.com
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        /// 获取所有下级节点的Id(递归),不包含自己
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="app"></param>
        /// <param name="result">结果</param>
        /// <param name="rootId">父节点id</param>
        public static void GetChildrenId<T>(this AppService app, List<string> result, string rootId) where T : class, ITreeable, IEntity
        {
            var currentList = app.Read<T>(p => p.ParentId.Equals(rootId)).Select(p => p.Id).ToList();
            result.AddRange(currentList);
            currentList.ForEach(e => app.GetChildrenId<T>(result, e));
        }

        /// <summary>
        /// 获取所有下级集合(递归)
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="app"></param>
        /// <param name="parentId">父节点</param>
        /// <returns></returns>
        public static T[] GetChildren<T>(this AppService app, string parentId) where T : class, ITreeable, IEntity
        {
            var query = app.Read<T>(p => p.ParentId.Equals(parentId));
            return query.ToList().Concat(query.ToList().SelectMany(p => app.GetChildren<T>(p.Id))).ToArray();
        }

        /// <summary>
        /// 获取所有子机构的Id(递归)
        /// </summary>
        public static void GetChildOrg(this AppService app, string rootId, List<string> result)
        {
            app.Read<Organ>(p => p.ParentId.Equals(rootId)).Select(p => p.Id).ToList().ForEach(e=>
            {
                result.Add(e);
                app.GetChildOrg(e,result);
            });
        }

        /// <summary>
        /// 获取子机构
        /// </summary>
        public static List<TreeView> GetChildOrg(this AppService app,  string nodeId=null ,int nodeLevel=0)
        {
            nodeLevel = string.IsNullOrWhiteSpace(nodeId) ? 0 : nodeLevel + 1;
            nodeId = string.IsNullOrWhiteSpace(nodeId) ? "0" : nodeId;
            return  app.Read<Organ>(p => p.ParentId.Equals(nodeId)).Select(p =>
                new TreeView
                {
                    Id = p.Id,
                    ParentId = p.ParentId,
                    Level = nodeLevel,
                    Loaded = false,
                    IsLeaf = !p.Children.Any(),
                    Expanded = false,
                    Name = p.Name,
                    ParentName = p.Parent.Name,
                    SortCode = p.SortCode ?? 0
                }).ToListAsync().Result;
        }

        /// <summary>
        /// 获取老师机构
        /// </summary>
        public static List<TreeView> GetTeacherOrg(this AppService app, string nodeId = "3", int nodeLevel = 0)
        {
            nodeLevel = "3" == nodeId ? 0 : nodeLevel + 1;
            return app.Read<Organ>(p => p.ParentId.Equals(nodeId)).Select(p =>
               new TreeView
               {
                   Id = p.Id,
                   ParentId = p.ParentId,
                   Level = nodeLevel,
                   Loaded = false,
                   IsLeaf = !p.Children.Any(),
                   Expanded = false,
                   Name = p.Name,
                   ParentName = p.Parent.Name,
                   SortCode = p.SortCode ?? 0
               }).ToListAsync().Result;
        }

        /// <summary>
        /// 获取学生机构
        /// </summary>
        public static List<TreeView> GetStudentOrg(this AppService app, string nodeId = "2", int nodeLevel = 0)
        {
            nodeLevel = "2" == nodeId ? 0 : nodeLevel + 1;
            return app.Read<Organ>(p => p.ParentId.Equals(nodeId)).Select(p =>
               new TreeView
               {
                   Id = p.Id,
                   ParentId = p.ParentId,
                   Level = nodeLevel,
                   Loaded = false,
                   IsLeaf = !p.Children.Any(),
                   Expanded = false,
                   Name = p.Name,
                   ParentName = p.Parent.Name,
                   SortCode = p.SortCode ?? 0
               }).ToListAsync().Result;
        }
    }
}