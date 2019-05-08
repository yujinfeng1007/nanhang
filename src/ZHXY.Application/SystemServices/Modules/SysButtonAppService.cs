using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 模块按钮管理
    /// </summary>
    public class SysButtonAppService : AppService
    {
        private IModuleButtonRepository Repository { get; }

        public SysButtonAppService() => Repository = new ModuleButtonRepository();

        public SysButtonAppService(IModuleButtonRepository repos) => Repository = repos;

        public List<SysButton> GetList(string moduleId = "")
        {
            var expression = ExtLinq.True<SysButton>();
            if (!string.IsNullOrEmpty(moduleId))
            {
                expression = expression.And(t => t.F_ModuleId == moduleId);
            }
            return Repository.QueryAsNoTracking(expression).OrderBy(t => t.F_SortCode).ToList();
        }

        public SysButton GetForm(string keyValue) => Repository.Find(keyValue);

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

        public void SubmitForm(SysButton moduleButtonEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                moduleButtonEntity.Modify(keyValue);
                Repository.Update(moduleButtonEntity);
            }
            else
            {
                moduleButtonEntity.Create();
                Repository.Insert(moduleButtonEntity);
            }
        }

        public void SubmitCloneButton(string moduleId, string Ids)
        {
            var ArrayId = Ids.Split(',');
            var data = GetList();
            var entitys = new List<SysButton>();
            foreach (var item in ArrayId)
            {
                var moduleButtonEntity = data.Find(t => t.F_Id == item);
                moduleButtonEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                moduleButtonEntity.F_ModuleId = moduleId;
                entitys.Add(moduleButtonEntity);
            }
            Repository.SubmitCloneButton(entitys);
        }
    }
}