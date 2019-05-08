using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class SysTableDefMap : EntityTypeConfiguration<TableDef>
    {
        public SysTableDefMap()
        {
            ToTable("Sys_TableDef");
            HasKey(t => t.F_Id);
        }
    }
}