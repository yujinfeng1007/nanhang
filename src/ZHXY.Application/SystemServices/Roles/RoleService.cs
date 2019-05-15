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
            var query = Read<Role>(p => p.Category == 1);
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword) || p.EnCode.Contains(keyword));
            return query.OrderBy(t => t.SortCode).ToListAsync().Result;
        }

        public dynamic GetById(string id) => Get<Role>(id);
        public void Add(AddRoleDto dto)
        {
            var duty = dto.MapTo<Role>();
            duty.Category = 1;
            AddAndSave(duty);
        }

        public void Update(UpdateRoleDto dto)
        {
            var duty = Get<Role>(dto.Id);
            dto.MapTo(duty);
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