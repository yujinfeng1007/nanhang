using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DbBackupMap : EntityTypeConfiguration<DbBackup>
    {
        public DbBackupMap()
        {
            ToTable("Sys_DbBackup");
            HasKey(t => t.F_Id);
        }
    }
}