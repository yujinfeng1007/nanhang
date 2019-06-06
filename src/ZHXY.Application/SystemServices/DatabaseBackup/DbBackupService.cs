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
    public class DbBackupService : AppService
    {
        public DbBackupService(IZhxyRepository r) : base(r)
        {
        }

        public List<DbBackup> GetList(string queryJson)
        {
            var expression = ExtLinq.True<DbBackup>();
            var queryParam = queryJson.Parse2JObject();
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                var condition = queryParam["condition"].ToString();
                var keyword = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "DbName":
                        expression = expression.And(t => t.DbName.Contains(keyword));
                        break;

                    case "FileName":
                        expression = expression.And(t => t.FileName.Contains(keyword));
                        break;
                }
            }
            return Read(expression).OrderByDescending(t => t.BackupTime).ToList();
        }

        public DbBackup GetById(string id) => Get<DbBackup>(id);

        public void Delete(string id)
        {
            var obj = Get<DbBackup>(id);
            DelAndSave(obj);
        }

        public void Add(DbBackup dbBackupEntity)
        {
            dbBackupEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
            dbBackupEntity.BackupTime = DateTime.Now;
            Backup(dbBackupEntity);
        }

        private void Backup(DbBackup dbBackupEntity)
        {
            DbHelper.ExecuteSqlCommand($"backup database {dbBackupEntity.DbName} to disk ='{dbBackupEntity.FilePath}'");
            dbBackupEntity.FileSize = new FileInfo(dbBackupEntity.FilePath).Length.ToFileSizeString();
            dbBackupEntity.FilePath = "/Resource/DbBackup/" + dbBackupEntity.FileName;
            AddAndSave(dbBackupEntity);
        }
    }
}