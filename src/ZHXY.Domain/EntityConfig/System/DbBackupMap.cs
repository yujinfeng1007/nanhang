using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DbBackupMap : EntityTypeConfiguration<DbBackup>
    {
        public DbBackupMap()
        {
            ToTable("Sys_DbBackup");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.BackupType).HasColumnName("F_BackupType");
            Property(p => p.DbName).HasColumnName("F_DbName");
            Property(p => p.FileName).HasColumnName("F_FileName");
            Property(p => p.FileSize).HasColumnName("F_FileSize");
            Property(p => p.FilePath).HasColumnName("F_FilePath");
            Property(p => p.BackupTime).HasColumnName("F_BackupTime");



        }
    }
}