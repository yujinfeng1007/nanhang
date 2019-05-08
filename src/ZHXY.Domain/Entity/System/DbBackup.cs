using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 数据库备份
    /// </summary>
    public class DbBackup :IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string BackupType { get; set; }
        public string DbName { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public DateTime? BackupTime { get; set; } = DateTime.Now;


    }
}