using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ZHXY.Common;
using System.Data.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleService : AppService
    {
        public RelevanceService RelevanceApp { get; }

        public RoleService(IZhxyRepository r) : base(r)
        {
        }

        public RoleService(IZhxyRepository r, RelevanceService relevanceApp) : base(r)
        {
            RelevanceApp = relevanceApp;
        }

        public List<Role> GetList(string keyword = null)
        {
            var query = Read<Role>();
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.OrderBy(t => t.SortCode).ToListAsync().Result;
        }

        public dynamic GetById(string id) => Get<Role>(id);
        public void Add(AddRoleDto dto)
        {
            var role = dto.MapTo<Role>();
            AddAndSave(role);
        }

        public void Update(UpdateRoleDto dto)
        {
            var role = Get<Role>(dto.Id);
            dto.MapTo(role);
            SaveChanges();
        }

        public void Delete(string id)
        {
            DelAndSave<Role>(id);
        }

        public List<User> GetRoleUser(string roleId)
        {
            var users = RelevanceApp.GetRoleUser(roleId);
            return Read<User>(p => users.Contains(p.Id)).ToListAsync().Result;
        }
       
        public void AddRoleUser(string roleId, string[] userIds)
        {
            var existingUsers = RelevanceApp.GetRoleUser(roleId);
            var addUsers = userIds.Except(existingUsers);
            foreach (var item in addUsers)
            {
                Add(new Relevance { Name = Relation.UserRole, FirstKey = item, SecondKey = roleId });
            }
            SaveChanges();
        }

        public void RemoveRoleUser(string roleId, string[] userIds)
        {
            var removeList = Query<Relevance>(p =>
              p.Name.Equals(Relation.UserRole) &&
              p.SecondKey.Equals(roleId) &&
              userIds.Contains(p.FirstKey)
              ).ToArrayAsync().Result;
            DelAndSave<Relevance>(removeList);
        }
    }
}