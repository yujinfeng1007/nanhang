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
    public class SysRoleAppService : AppService
    {
        private IRoleRepository Repository { get; }
        private SysModuleAppService ModuleApp { get; }
        private SysButtonAppService ModuleButtonApp { get; }

        public SysRoleAppService()
        {
            R = new ZhxyRepository();
            Repository = new RoleRepository();
            ModuleApp = new SysModuleAppService();
            ModuleButtonApp = new SysButtonAppService();
        }

        public SysRoleAppService(IRoleRepository roleAuthorizeRepository, SysModuleAppService moduleApp, SysButtonAppService moduleButtonApp,IZhxyRepository  r)
        {
            Repository = roleAuthorizeRepository;
            ModuleApp = moduleApp;
            ModuleButtonApp = moduleButtonApp;
            R = r;
        }

        public List<Role> GetList(Pagination pagination, string keyword = "")
        {
            var query = Read<Role>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p =>  p.F_Category == 1&&(p.F_FullName.Contains(keyword) || p.F_EnCode.Contains(keyword)));
            pagination.Records = query.CountAsync().Result;
            return query.OrderBy(t => t.F_SortCode).Skip(pagination.Skip).Take(pagination.Rows).ToListAsync().Result;
        }

        public List<Role> GetList(string keyword = "")
        {
            var query = Read<Role>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.F_FullName.Contains(keyword) || p.F_EnCode.Contains(keyword));
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }

        public List<Role> GetListById(string id) => Read<Role>(p => p.F_Id.Contains(id) && p.F_Category == 1).OrderBy(p => p.F_SortCode).ToListAsync().Result;

        public Role Get(string id) => Get<Role>(id);

        public void DeleteForm(string keyValue) => Repository.Delete(keyValue);

        public void SubmitForm(Role roleEntity, string[] permissionIds2, string[] permissionIds3, string[] permissionIds4, string keyValue)
        {
            var count = Repository.QueryAsNoTracking().Count(t => t.F_EnCode == roleEntity.F_EnCode && t.F_Id != keyValue && t.F_Category != 2);
            if (count > 0)
                throw new Exception("编号重复");

            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.F_Id = keyValue;
            }
            else
            {
                roleEntity.Create();
                roleEntity.F_EnCode = roleEntity.F_Id;
            }

            var moduledata = ModuleApp.GetList();
            var buttondata = ModuleButtonApp.GetList();
            var roleAuthorizeEntitys = new List<RoleAuthorize>();
            foreach (var itemId in permissionIds2)
            {
                var roleAuthorizeEntity = new RoleAuthorize();
                roleAuthorizeEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                roleAuthorizeEntity.F_ObjectType = 1;
                roleAuthorizeEntity.F_ObjectId = roleEntity.F_Id;
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
                roleAuthorizeEntity.F_ObjectId = roleEntity.F_Id;
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
                roleAuthorizeEntity.F_ObjectId = roleEntity.F_Id;
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
    }
}