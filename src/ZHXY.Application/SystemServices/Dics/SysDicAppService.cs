using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZHXY.Application
{
    /// <summary>
    /// 字典管理
    /// </summary>
    public class SysDicAppService : AppService
    {
        private IRepositoryBase<SysDic> Repository { get; }

        public SysDicAppService() => Repository = new Repository<SysDic>();

        public SysDicAppService(IRepositoryBase<SysDic> repos) => Repository = repos;

        public List<SysDic> GetList() => Repository.QueryAsNoTracking().ToList();

        public SysDic GetForm(string keyValue) => Repository.Find(keyValue);

        public void DeleteForm(string keyValue)
        {
            if (Repository.QueryAsNoTracking().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                Repository.BatchDelete(t => t.F_Id == keyValue);
            }
        }

        public void SubmitForm(SysDic itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.Modify(keyValue);
                Repository.Update(itemsEntity);
            }
            else
            {
                itemsEntity.Create();
                Repository.Insert(itemsEntity);
            }
        }
    }
}