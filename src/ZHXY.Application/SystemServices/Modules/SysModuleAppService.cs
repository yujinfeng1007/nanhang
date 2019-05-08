using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;

namespace ZHXY.Application
{

    /// <summary>
    /// 模块管理
    /// </summary>
    public class SysModuleAppService:AppService
    {
        private IRepositoryBase<SysModule> Repository { get; }

        public SysModuleAppService() => Repository = new Repository<SysModule>();
        public SysModuleAppService(IRepositoryBase<SysModule> repos) => Repository = repos;


        public List<SysModule> GetList() => Repository.QueryAsNoTracking().OrderBy(t => t.F_SortCode).ToList();

        public List<SysModule> GetEnableList() => Repository.QueryAsNoTracking(t => t.F_EnabledMark == true).OrderBy(t => t.F_SortCode).ToList();

        public SysModule GetForm(string keyValue) => !keyValue.IsEmpty() ? Repository.Find(keyValue) : null;

        public void DeleteForm(string keyValue)
        {
            if (Repository.QueryAsNoTracking().Count(t => t.F_ParentId == keyValue) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }

            Repository.BatchDelete(t => t.F_Id == keyValue);
        }

        public void SubmitForm(SysModule moduleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleEntity.Modify(keyValue);
                Repository.Update(moduleEntity);
            }
            else
            {
                moduleEntity.Create();
                Repository.Insert(moduleEntity);
            }
        }
    }
}