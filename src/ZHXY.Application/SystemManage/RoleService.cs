using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;
using System.Data.Entity;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleService : AppService
    {
        public RoleService(DbContext r) : base(r)
        {
        }

        public List<Role> GetList(Pagination pag, string keyword = "")
        {
            var query = Read<Role>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            query=query.Paging(pag);
            return query.ToListAsync().Result;
        }

        public List<Role> GetList(string keyword = "")
        {
            var query = Read<Role>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.Name.Contains(keyword) || p.Code.Contains(keyword));
            return query.OrderBy(t => t.Sort).ToListAsync().Result;
        }

        public List<Role> GetListById(string id) => Read<Role>(p => p.Id.Contains(id) ).OrderBy(p => p.Sort).ToListAsync().Result;

        public Role Get(string id) => Get<Role>(id);

        public void DeleteForm(string keyValue) => DelAndSave<Role>(keyValue);

        public void SubmitForm(Role roleEntity, string[] permissionIds2, string[] permissionIds3, string[] permissionIds4, string keyValue)
        {
            var count = Read<Role>().Count(t => t.Code == roleEntity.Code && t.Id != keyValue);
            if (count > 0)
                throw new Exception("编号重复");

            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = Get(keyValue);
                roleEntity.MapTo(old);
                old.Id = keyValue;
                UpdroleAuthorizeEntitys(permissionIds2, permissionIds3, permissionIds4, old.Id);
            }
            else
            {
                roleEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                roleEntity.Code = roleEntity.Id;
                AddAndSave(roleEntity);
                UpdroleAuthorizeEntitys(permissionIds2,permissionIds3,permissionIds4, roleEntity.Id);
            }
          
            SaveChanges();
        }

        private void UpdroleAuthorizeEntitys(string[] permissionIds2, string[] permissionIds3, string[] permissionIds4, string keyValue)
        {
            var moduledata = Read<Module>().ToList();
            var buttondata = Read<Button>().ToList();
            var roleAuthorizeEntitys = new List<RoleAuthorize>();
            foreach (var itemId in permissionIds2)
            {
                var roleAuthorizeEntity = new RoleAuthorize();
                roleAuthorizeEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.ObjectType = 1;
                roleAuthorizeEntity.ObjectId = keyValue;
                roleAuthorizeEntity.ItemId = itemId;
                if (moduledata.Find(t => t.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 1;
                }
                if (buttondata.Find(t => t.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 2;
                }
                roleAuthorizeEntitys.Add(roleAuthorizeEntity);
            }
            foreach (var itemId in permissionIds3)
            {
                var roleAuthorizeEntity = new RoleAuthorize();
                roleAuthorizeEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.ObjectType = 1;
                roleAuthorizeEntity.ObjectId = keyValue;
                roleAuthorizeEntity.ItemId = itemId;
                if (moduledata.Find(t => t.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 1;
                }
                if (buttondata.Find(t => t.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 2;
                }
                roleAuthorizeEntitys.Add(roleAuthorizeEntity);
            }
            foreach (var itemId in permissionIds4)
            {
                var roleAuthorizeEntity = new RoleAuthorize();
                roleAuthorizeEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.ObjectType = 1;
                roleAuthorizeEntity.ObjectId = keyValue;
                roleAuthorizeEntity.ItemId = itemId;
                if (moduledata.Find(t => t.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 1;
                }
                if (buttondata.Find(t => t.Id == itemId) != null)
                {
                    roleAuthorizeEntity.ItemType = 2;
                }
                roleAuthorizeEntitys.Add(roleAuthorizeEntity);
            }

            Del<RoleAuthorize>(t => t.ObjectId == keyValue);
            AddRange(roleAuthorizeEntitys);
        }

        public List<Role> GetListByRoleId(string roleId)
        {
            var F_RoleId = roleId.Split(',');
            var datas = Read<Role>(t => !F_RoleId.Contains(t.Id)).ToList();
            return datas;
        }

        /// <summary>
        /// 获取用户的角色id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string[] GetUserRoles(string userId) => Read<UserRole>(t => t.UserId == userId).Select(p => p.RoleId).ToArray();


        public Dictionary<string, object> GetRoleListByCache()
        {
            if (!RedisCache.KeyExists(SysConsts.ROLE))
            {
                var data = GetList();
                var dictionary = new Dictionary<string, object>();
                foreach (var item in data)
                {
                    var fieldItem = new { encode = item.Code, fullname = item.Name };
                    dictionary.Add(item.Id, fieldItem);
                }
                RedisCache.Set(SysConsts.ROLE, dictionary);
            }
            return RedisCache.Get<Dictionary<string, object>>(SysConsts.ROLE);
        }


    }
}