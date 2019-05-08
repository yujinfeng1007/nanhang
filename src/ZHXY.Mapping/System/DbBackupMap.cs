using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class DbBackupMap : EntityTypeConfiguration<SysDbBackup>
    {
        public DbBackupMap()
        {
            ToTable("Sys_DbBackup");
            HasKey(t => t.F_Id);
        }
    }
}