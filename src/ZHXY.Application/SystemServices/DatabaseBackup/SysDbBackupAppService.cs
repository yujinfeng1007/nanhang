using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 数据库备份管理
    /// </summary>
    public class SysDbBackupAppService : AppService
    {
        private IRepositoryBase<DbBackup> Repository { get; }


        public SysDbBackupAppService() => Repository = new Repository<DbBackup>();

        public SysDbBackupAppService(IRepositoryBase<DbBackup> repos) => Repository = repos;

        public List<DbBackup> GetList(string queryJson)
        {
            var expression = ExtLinq.True<DbBackup>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                var condition = queryParam["condition"].ToString();
                var keyword = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "DbName":
                        expression = expression.And(t => t.F_DbName.Contains(keyword));
                        break;

                    case "FileName":
                        expression = expression.And(t => t.F_FileName.Contains(keyword));
                        break;
                }
            }
            return Repository.QueryAsNoTracking(expression).OrderByDescending(t => t.F_BackupTime).ToList();
        }

        public DbBackup GetForm(string keyValue) => Repository.Find(keyValue);

        public void DeleteForm(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            var expression = ExtLinq.False<DbBackup>();
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                var Id = F_Id[i];
                expression = expression.Or(t => t.F_Id == Id);
            }
            Repository.BatchDelete(expression);
        }

        public void SubmitForm(DbBackup dbBackupEntity)
        {
            dbBackupEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
            dbBackupEntity.F_EnabledMark = true;
            dbBackupEntity.F_BackupTime = DateTime.Now;
            //Repository.ExecuteDbBackup(dbBackupEntity);
            Backup(dbBackupEntity);
        }

        private void Backup(DbBackup dbBackupEntity)
        {
            DbHelper.ExecuteSqlCommand($"backup database {dbBackupEntity.F_DbName} to disk ='{dbBackupEntity.F_FilePath}'");
            dbBackupEntity.F_FileSize = new FileInfo(dbBackupEntity.F_FilePath).Length.ToFileSizeString();
            dbBackupEntity.F_FilePath = "/Resource/DbBackup/" + dbBackupEntity.F_FileName;
            Repository.Insert(dbBackupEntity);
        }
    }
}