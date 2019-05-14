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
        private IRoleRepository Repository { get; }
        private SysModuleAppService ModuleApp { get; }
        private SysButtonAppService ModuleButtonApp { get; }

        public RoleService()
        {
            R = new ZhxyRepository();
            Repository = new RoleRepository();
            ModuleApp = new SysModuleAppService();
            ModuleButtonApp = new SysButtonAppService();
        }

        public RoleService(IRoleRepository roleAuthorizeRepository, SysModuleAppService moduleApp, SysButtonAppService moduleButtonApp,IZhxyRepository  r)
        {
            Repository = roleAuthorizeRepository;
            ModuleApp = moduleApp;
            ModuleButtonApp = moduleButtonApp;
            R = r;
        }
      
        public List<Role> GetListById(string id)
        {
            var query = Read<Role>(p => p.Category == 1);
            return query.Where(p => p.Id.Contains(id)).OrderBy(p => p.SortCode).ToListAsync().Result;
        }

        public void Delete(string id)
        {
            Repository.Delete(id);
        }

        public void SubmitForm(Role roleEntity, string[] permissionIds2, string[] permissionIds3, string[] permissionIds4, string keyValue)
        {
            var count = Repository.QueryAsNoTracking().Count(t => t.EnCode == roleEntity.EnCode && t.Id != keyValue && t.Category != 2);
            if (count > 0)
                throw new Exception("编号重复");

            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Id = keyValue;
            }
            else
            {
                roleEntity.EnCode = roleEntity.Id;
            }

            var moduledata = ModuleApp.GetList();
            var buttondata = ModuleButtonApp.GetList();
            var roleAuthorizeEntitys = new List<RoleAuthorize>();
            foreach (var itemId in permissionIds2)
            {
                var roleAuthorizeEntity = new RoleAuthorize();
                roleAuthorizeEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.Id;
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
                var roleAuthorizeEntity = new RoleAuthorize();
                roleAuthorizeEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.Id;
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
                var roleAuthorizeEntity = new RoleAuthorize();
                roleAuthorizeEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.Id;
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
            Repository.SubmitForm(roleEntity, roleAuthorizeEntitys, keyValue);
        }

        public List<Role> GetListByRoleId(string roleId)
        {
            var strSql = new StringBuilder();
            if (!roleId.IsEmpty())
            {
                var F_RoleId = roleId.Split(',');
                for (var i = 0; i < F_RoleId.Length; i++)
                {
                    strSql.Append(" and F_Id != '" + F_RoleId[i] + "'");
                }
            }

            var Sql = "SELECT * FROM Sys_Role WHERE F_Category = 1 " +
              strSql.ToString();
            return Repository.FindList(Sql);
        }

        public dynamic GetList(string keyword = null)
        {
            var query = Read<Role>(p => p.Category == 1);
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword) || p.EnCode.Contains(keyword));
            return query.OrderBy(t => t.SortCode).ToListAsync().Result;
        }

        public dynamic Get(string id) => Get<Role>(id);
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
    }
}