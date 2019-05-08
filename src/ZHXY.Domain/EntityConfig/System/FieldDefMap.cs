using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class FieldDefMap : EntityTypeConfiguration<FieldDef>
    {
        public FieldDefMap()
        {
            ToTable("Sys_FieldDef");
            HasKey(t => t.F_Id);
        }
    }
}