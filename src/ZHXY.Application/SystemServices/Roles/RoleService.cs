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
        public RoleService(IZhxyRepository  r):base(r)
        {
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

        public void Delete(string[] id)
        {
            foreach (var item in id)
            {
                Del<Role>(item);
            }
            SaveChanges();
        }

        public List<Role> GetUserRoles(string userId)
        {
            var roles= Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.FirstKey.Equals(userId)).Select(p => p.SecondKey).ToArrayAsync().Result;
            return Read<Role>(p => roles.Contains(p.Id)).ToListAsync().Result;
        }

        public string[] GetUserRolesId(string userId)
        {
            return Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.FirstKey.Equals(userId)).Select(p => p.SecondKey).ToArrayAsync().Result;
        }
    }
}