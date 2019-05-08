using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    public class SysDutyAppService : AppService
    {
        private IRoleRepository Repository { get; }

        public SysDutyAppService()
        {
            Repository = new RoleRepository();
            R = new ZhxyRepository();
        }

        public SysDutyAppService(IRoleRepository repos)
        {
            Repository = repos;
            R = new ZhxyRepository();
        }

        public List<Role> GetList(string keyword = "")
        {
            var query = Read<Role>(t => t.F_Category == 2);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.F_FullName.Contains(keyword)|| t.F_EnCode.Contains(keyword));
            }
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }

        public Role Get(string id) => Get<Role>(id);

        public void Delete(string id)
        {
            var entity=Get<Role>(id);
            DelAndSave(entity);
        }

        public void SubmitForm(Role roleEntity, string keyValue)
        {
            var count = Repository.QueryAsNoTracking().Count(t => t.F_EnCode == roleEntity.F_EnCode && t.F_Id != keyValue && t.F_Category == 2);
            if (count > 0)
                throw new Exception("编号重复");
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Modify(keyValue);
                Repository.Update(roleEntity);
            }
            else
            {
                //若代码不唯空，主键同代码
                roleEntity.Create();
                if (!roleEntity.F_EnCode.IsEmpty())
                {
                    roleEntity.F_Id = roleEntity.F_EnCode;
                }
                roleEntity.F_Category = 2;
                Repository.Insert(roleEntity);
            }
        }

    }
}