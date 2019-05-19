using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DbBackupMap : EntityTypeConfiguration<DbBackup>
    {
        public DbBackupMap()
        {
            ToTable("zhxy_db_backup");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.BackupType).HasColumnName("cackup_type");
            Property(p => p.DbName).HasColumnName("db_name");
            Property(p => p.FileName).HasColumnName("file_name");
            Property(p => p.FileSize).HasColumnName("file_size");
            Property(p => p.FilePath).HasColumnName("file_path");
            Property(p => p.BackupTime).HasColumnName("backup_time");



        }
    }
}