using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class SysTableDefAppService : AppService
    {
        private IRepositoryBase<TableDef> Repository { get; }

        public SysTableDefAppService()
        {
            Repository = new Repository<TableDef>();
            R = new ZhxyRepository();
        }

        public SysTableDefAppService(IRepositoryBase<TableDef> repos, IZhxyRepository r)
        {
            Repository = repos;
            R = r;
        }

        public List<TableDef> GetList() => Read<TableDef>().OrderBy(t => t.F_SortCode).ToListAsync().Result;

        public List<TableDef> GetList(string tableName)
        {
            var query = Read<TableDef>(t => t.F_DeleteMark == false);
            query = string.IsNullOrEmpty(tableName) ? query : query.Where(t => t.F_TableName.Contains(tableName));
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }

        public List<TableDef> GetByTName(string F_TableName)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    Sys_TableDef d
                            WHERE   F_TableName = @F_TableName
                                    And F_DeleteMark != 'true'");
            DbParameter[] parameter =
            {
                 new SqlParameter("@F_TableName",F_TableName)
            };
            return Repository.FindList(strSql.ToString(), parameter);
        }

        public TableDef Get(string id) => Get<TableDef>(id);

        public void Delete(string id) => DelAndSave<TableDef>(id);

        public void SubmitForm(TableDef entity, string keyValue)
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