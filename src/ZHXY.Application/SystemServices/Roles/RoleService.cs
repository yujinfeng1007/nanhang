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

        public List<User> GetRoleUsers(string roleId)
        {
            var users= Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.SecondKey.Equals(roleId)).Select(p => p.FirstKey).ToArrayAsync().Result;
            return Read<User>(p => users.Contains(p.Id)).ToListAsync().Result;
        }

        public string[] GetRoleUsersId(string roleId)
        {
            return Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.SecondKey.Equals(roleId)).Select(p => p.FirstKey).ToArrayAsync().Result;
        }

        public string[] GetRoleMenusId(string roleId)
        {
            return Read<Relevance>(p => p.Name.Equals(Relation.RoleMenu) && p.SecondKey.Equals(roleId)).Select(p => p.FirstKey).ToArrayAsync().Result;
        }

        public string[] GetRoleButtonsId(string roleId)
        {
            return Read<Relevance>(p => p.Name.Equals(Relation.RoleButton) && p.SecondKey.Equals(roleId)).Select(p => p.FirstKey).ToArrayAsync().Result;
        }

        public void AddUser(string roleId,string [] userIds)
        {
            var existingUsers = GetRoleUsersId(roleId);
            var addUsers=userIds.Except(existingUsers);
            foreach (var item in addUsers)
            {
                Add(new Relevance { Name = Relation.UserRole, FirstKey = item, SecondKey = roleId });
            }
            SaveChanges();
        }

        public void RemoveUser(string roleId, string[] userIds)
        {
            var removeList = Query<Relevance>(p =>
              p.Name.Equals(Relation.UserRole) &&
              p.SecondKey.Equals(roleId)  &&
              userIds.Contains(p.FirstKey)
              ).ToArrayAsync().Result;
            DelAndSave<Relevance>(removeList);
        }

        public void AddMenu(string roleId,string[] menus)
        {
            var existingMenus = GetRoleMenusId(roleId);
            var addMenus = menus.Except(existingMenus);
            foreach (var item in addMenus)
            {
                Add(new Relevance { Name = Relation.RoleMenu, FirstKey = roleId, SecondKey = item });
            }
            SaveChanges();
        }

        public void RemoveMenu(string roleId, string[] menus)
        {
            var removeList = Query<Relevance>(p =>
             p.Name.Equals(Relation.RoleMenu) &&
             p.SecondKey.Equals(roleId) &&
             menus.Contains(p.FirstKey)
             ).ToArrayAsync().Result;
            DelAndSave<Relevance>(removeList);
        }

        public void AddButton(string roleId, string[] buttons)
        {
            var existingMenus = GetRoleMenusId(roleId);
            var addButtons = buttons.Except(existingMenus);
            foreach (var item in addButtons)
            {
                Add(new Relevance { Name = Relation.RoleButton, FirstKey = roleId, SecondKey = item });
            }
            SaveChanges();
        }

        public void RemoveButton(string roleId, string[] buttons)
        {
            var removeList = Query<Relevance>(p =>
             p.Name.Equals(Relation.RoleButton) &&
             p.SecondKey.Equals(roleId) &&
             buttons.Contains(p.FirstKey)
             ).ToArrayAsync().Result;
            DelAndSave<Relevance>(removeList);
        }



        public void SetMenu(string roleId,string[] menus)
        {
            var roles = Query<Relevance>(p => p.Name.Equals(Relation.RoleMenu) && p.FirstKey.Equals(roleId)).ToArrayAsync().Result;
            Del<Relevance>(roles);
            foreach (var item in menus)
            {
                Add(new Relevance { Name = Relation.RoleMenu, FirstKey = roleId, SecondKey = item });
            }
            SaveChanges();
        }

        public void SetButton(string roleId, string[] buttons)
        {
            var roles = Query<Relevance>(p => p.Name.Equals(Relation.RoleButton) && p.FirstKey.Equals(roleId)).ToArrayAsync().Result;
            Del<Relevance>(roles);
            foreach (var item in buttons)
            {
                Add(new Relevance { Name = Relation.RoleButton, FirstKey = roleId, SecondKey = item });
            }
            SaveChanges();
        }

        public void SetUser(string roleId, string[] users)
        {
            var roles = Query<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.SecondKey.Equals(roleId)).ToArrayAsync().Result;
            Del<Relevance>(roles);
            foreach (var item in users)
            {
                Add(new Relevance { Name = Relation.UserRole, FirstKey = item, SecondKey = roleId });
            }
            SaveChanges();
        }

    }
}