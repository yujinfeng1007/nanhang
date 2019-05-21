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
        public RoleService(IZhxyRepository r) : base(r)
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

        public void Delete(string id)
        {
            DelAndSave<Role>(id);
        }

        public List<User> GetRoleUsers(string roleId)
        {
            var users = Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.SecondKey.Equals(roleId)).Select(p => p.FirstKey).ToArrayAsync().Result;
            return Read<User>(p => users.Contains(p.Id)).ToListAsync().Result;
        }

        public string[] GetRoleUsersId(string roleId)
        {
            return Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.SecondKey.Equals(roleId)).Select(p => p.FirstKey).ToArrayAsync().Result;
        }

        public string[] GetRoleMenusId(string roleId)
        {
            return Read<Relevance>(p => p.Name.Equals(Relation.RolePower) && p.FirstKey.Equals(roleId)&&p.ThirdKey.Equals(SYS_CONSTS.DbNull)).Select(p => p.SecondKey).Distinct().ToArrayAsync().Result;
        }

        public dynamic GetRoleMenus(string roleId)
        {
            var menus = GetRoleMenusId(roleId);
            return Read<Menu>(p => menus.Contains(p.Id)).ToArrayAsync().Result;
        }



        public void AddRoleFunc(string roleId,  string[] funcs)
        {
            var functions = Read<Function>(p => funcs.Contains(p.Id)).Select(p => new { p.Id, p.MenuId }).ToList();
            functions.ForEach(item =>
            {
                Add(new Relevance { Name = Relation.RolePower, FirstKey = roleId, SecondKey = item.MenuId, ThirdKey = item.Id });
            });
            SaveChanges();
        }

        public void AddRoleMenu(string roleId, string[] menus)
        {
            var menuList = Read<Menu>(p => menus.Contains(p.Id)).Select(p =>  p.Id).ToList();
            menuList.ForEach(item =>
            {
                Add(new Relevance { Name = Relation.RolePower, FirstKey = roleId, SecondKey = item});
            });
            SaveChanges();
        }

        public void RemoveRoleFunc(string roleId, string[] funcs)
        {
            var removeList = Read<Relevance>(p => p.Name.Contains(Relation.RolePower) && p.FirstKey.Equals(roleId) && funcs.Contains(p.ThirdKey)).ToList();
            DelAndSave<Relevance>(removeList);
        }

        public void RemoveRoleMenu(string roleId, string[] menus)
        {
            var removeList = Read<Relevance>(p => p.Name.Contains(Relation.RolePower) && p.FirstKey.Equals(roleId) &&p.ThirdKey.Equals(SYS_CONSTS.DbNull)&& menus.Contains(p.SecondKey)).ToList();
            DelAndSave<Relevance>(removeList);
        }


        public string[] GetRoleFuncsId(string roleId)
        {
            return Read<Relevance>(p => p.Name.Equals(Relation.RolePower) && p.FirstKey.Equals(roleId)).Select(p => p.ThirdKey).ToArrayAsync().Result;
        }

        public dynamic GetRoleFuncs(string roleId)
        {
            var buttons = GetRoleFuncsId(roleId);
            return Read<Function>(p => buttons.Contains(p.Id)).ToArrayAsync().Result;
        }


        public dynamic GetMenuFuncsExcludeRole(string roleId, string menuId)
        {
            var roleFuncs = Read<Relevance>(p => p.Name.Equals(Relation.RolePower) && p.FirstKey.Equals(roleId) && p.SecondKey.Equals(menuId)).Select(p => p.ThirdKey).ToArrayAsync().Result;
            return Read<Function>(p => p.MenuId.Equals(menuId) && !roleFuncs.Contains(p.Id)).ToListAsync().Result;
        }

        public dynamic GetMenusExcludeRole(string roleId, string menuId)
        {
            var roleFuncs = Read<Relevance>(p => p.Name.Equals(Relation.RolePower) && p.FirstKey.Equals(roleId) ).Select(p => p.SecondKey).ToArrayAsync().Result;
            return Read<Menu>(p => p.ParentId.Equals(menuId) && !roleFuncs.Contains(p.Id)).ToListAsync().Result;
        }

        public void AddRoleUser(string roleId, string[] userIds)
        {
            var existingUsers = GetRoleUsersId(roleId);
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