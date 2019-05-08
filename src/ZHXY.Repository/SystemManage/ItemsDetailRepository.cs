using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ItemsDetailRepository : Data.Repository<SysDicItem>, IItemDetailRepository
    {
        public List<SysDicItem> GetItemList(string enCode)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    Sys_ItemsDetail d
                                    INNER  JOIN Sys_Items i ON i.F_Id = d.F_ItemId
                            WHERE   1 = 1
                                    AND i.F_EnCode = @enCode
                                    AND d.F_EnabledMark = 1
                                    AND d.F_DeleteMark = 0
                            ORDER BY d.F_SortCode ASC");
            DbParameter[] parameter =
            {
                new SqlParameter("@enCode", enCode)
            };
            return FindList(strSql.ToString(), parameter);
        }

        public List<SysDicItem> GetItemByEnCode(string enCode)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    Sys_ItemsDetail d
                                    INNER  JOIN Sys_Items i ON i.F_Id = d.F_ItemId
                            WHERE    i.F_EnCode = @enCode
                            ORDER BY d.F_SortCode ASC");
            DbParameter[] parameter =
            {
                new SqlParameter("@enCode", enCode)
            };
            return FindList(strSql.ToString(), parameter);
        }
    }
}