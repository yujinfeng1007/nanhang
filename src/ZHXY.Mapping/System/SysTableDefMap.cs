using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SysTableDefMap : EntityTypeConfiguration<SysTableDef>
    {
        public SysTableDefMap()
        {
            ToTable("Sys_TableDef");
            HasKey(t => t.F_Id);
        }
    }
}