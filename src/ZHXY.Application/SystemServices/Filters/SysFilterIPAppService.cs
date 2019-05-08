using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class SysFilterIPAppService : AppService
    {
        private IRepositoryBase<FilterIP> Repository { get; }

        public SysFilterIPAppService() => Repository = new Repository<FilterIP>();
        public SysFilterIPAppService(IRepositoryBase<FilterIP> repos) => Repository = repos;

        public List<FilterIP> GetList(string keyword)
        {
            var expression = ExtLinq.True<FilterIP>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_StartIP.Contains(keyword));
            }
            return Repository.QueryAsNoTracking(expression).OrderByDescending(t => t.F_DeleteTime).ToList();
        }

        public FilterIP GetForm(string keyValue) => Repository.Find(keyValue);

        public void DeleteForm(string keyValue) => Repository.BatchDelete(t => t.F_Id == keyValue);

        public void SubmitForm(FilterIP filterIPEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                filterIPEntity.Modify(keyValue);
                Repository.Update(filterIPEntity);
            }
            else
            {
                filterIPEntity.Create();
                Repository.Insert(filterIPEntity);
            }
        }
    }
}