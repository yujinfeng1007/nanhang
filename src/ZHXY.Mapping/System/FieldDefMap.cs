using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class FieldDefMap : EntityTypeConfiguration<SysFieldDef>
    {
        public FieldDefMap()
        {
            ToTable("Sys_FieldDef");
            HasKey(t => t.F_Id);
        }
    }
}