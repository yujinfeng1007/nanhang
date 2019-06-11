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
    public class SysRoleAppService : AppService
    {
        public SysRoleAppService(DbContext r) : base(r)
        {
        }

        public List<SysRole> GetList(Pagination pagination, string keyword = "")
        {
            var query = Read<SysRole>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.F_FullName.Contains(keyword) || p.F_EnCode.Contains(keyword));
            pagination.Records = query.CountAsync().Result;
            return query.OrderBy(t => t.F_SortCode).Skip(pagination.Skip).Take(pagination.Rows).ToListAsync().Result;
        }

        public List<SysRole> GetList(string keyword = "")
        {
            var query = Read<SysRole>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.F_FullName.Contains(keyword) || p.F_EnCode.Contains(keyword));
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }

        public List<SysRole> GetListById(string id) => Read<SysRole>(p => p.F_Id.Contains(id) ).OrderBy(p => p.F_SortCode).ToListAsync().Result;

        public SysRole Get(string id) => Get<SysRole>(id);

        public void DeleteForm(string keyValue) => DelAndSave<SysRole>(keyValue);

        public void SubmitForm(SysRole roleEntity, string[] permissionIds2, string[] permissionIds3, string[] permissionIds4, string keyValue)
        {
            var count = Read<SysRole>().Count(t => t.F_EnCode == roleEntity.F_EnCode && t.F_Id != keyValue);
            if (count > 0)
                throw new Exception("编号重复");

            if (!string.IsNullOrEmpty(keyValue))
            {
                var old = Get(keyValue);
                roleEntity.MapTo(old);
                old.F_Id = keyValue;
                UpdroleAuthorizeEntitys(permissionIds2, permissionIds3, permissionIds4, old.F_Id);
            }
            else
            {
                roleEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                roleEntity.F_EnCode = roleEntity.F_Id;
                AddAndSave(roleEntity);
                UpdroleAuthorizeEntitys(permissionIds2,permissionIds3,permissionIds4, roleEntity.F_Id);
            }
          
            SaveChanges();
        }

        private void UpdroleAuthorizeEntitys(string[] permissionIds2, string[] permissionIds3, string[] permissionIds4, string keyValue)
        {
            var moduledata = Read<SysModule>().ToList();
            var buttondata = Read<SysButton>().ToList();
            var roleAuthorizeEntitys = new List<SysRoleAuthorize>();
            foreach (var itemId in permissionIds2)
            {
                var roleAuthorizeEntity = new SysRoleAuthorize();
                roleAuthorizeEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = keyValue;
                roleAuthorizeEntity.F_ItemId = itemId;
                if (moduledata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 1;
                }
                if (buttondata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 2;
                }
                roleAuthorizeEntitys.Add(roleAuthorizeEntity);
            }
            foreach (var itemId in permissionIds3)
            {
                var roleAuthorizeEntity = new SysRoleAuthorize();
                roleAuthorizeEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = keyValue;
                roleAuthorizeEntity.F_ItemId = itemId;
                if (moduledata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 1;
                }
                if (buttondata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 2;
                }
                roleAuthorizeEntitys.Add(roleAuthorizeEntity);
            }
            foreach (var itemId in permissionIds4)
            {
                var roleAuthorizeEntity = new SysRoleAuthorize();
                roleAuthorizeEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = keyValue;
                roleAuthorizeEntity.F_ItemId = itemId;
                if (moduledata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 1;
                }
                if (buttondata.Find(t => t.F_Id == itemId) != null)
                {
                    roleAuthorizeEntity.F_ItemType = 2;
                }
                roleAuthorizeEntitys.Add(roleAuthorizeEntity);
            }

            Del<SysRoleAuthorize>(t => t.F_ObjectId == keyValue);
            AddRange<SysRoleAuthorize>(roleAuthorizeEntitys);
        }

        public List<SysRole> GetListByRoleId(string roleId)
        {
            var F_RoleId = roleId.Split(',');
            var datas = Read<SysRole>(t => !F_RoleId.Contains(t.F_Id)).ToList();
            return datas;
        }
    }
}