using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class SysFieldDefAppService : AppService
    {
        private IRepositoryBase<FieldDef> Repository { get; }

        public SysFieldDefAppService()
        {
            Repository = new Repository<FieldDef>();
            R = new ZhxyRepository();
        }

        public SysFieldDefAppService(IRepositoryBase<FieldDef> repos, IZhxyRepository r)
        {
            Repository = repos;
            R = r;
        }
        public void DeleteByTableId(string keyValue) => Repository.BatchDelete(p => p.F_TableDef_ID == keyValue);

        public List<FieldDef> GetList(string fieldTitle, string tableId)
        {
            var query = Read<FieldDef>(t => t.F_TableDef_ID == tableId);
            query = string.IsNullOrEmpty(fieldTitle) ? query : query.Where(t => t.F_FieldTitle.Contains(fieldTitle));
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }

        public List<FieldDef> GetList4Export(string fieldTitle, string tableId)
        {
            var query = Read<FieldDef>(t => t.F_TableDef_ID == tableId && t.F_IsExcelDispaly == true);
            query = string.IsNullOrEmpty(fieldTitle) ? query : query.Where(t => t.F_FieldTitle.Contains(fieldTitle));
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }

        public List<FieldDef> GetList4ListShow(string fieldTitle, string tableId)
        {
            var expression = Read<FieldDef>(t => (t.F_IsListDispaly && t.F_TableDef_ID == tableId)|| t.F_FieldName.Equals("F_Id"));
            expression = string.IsNullOrEmpty(fieldTitle)? expression: expression.Where(t => t.F_FieldTitle.Contains(fieldTitle));
            return expression.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }


        public FieldDef Get(string id) => Get<FieldDef>(id);

        public void DeleteForm(string ids)
        {
            var idArr = ids.Split('|');
            var expression = ExtLinq.False<FieldDef>();
            for (var i = 0; i < idArr.Length - 1; i++)
            {
                var Id = idArr[i];
                expression = expression.Or(t => t.F_Id == Id);
            }
            Repository.BatchDelete(expression);
        }

        public void SubmitForm(FieldDef entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                Repository.Update(entity);
            }
            else
            {
                entity.Create();
                Repository.Insert(entity);
            }
        }

    }
}