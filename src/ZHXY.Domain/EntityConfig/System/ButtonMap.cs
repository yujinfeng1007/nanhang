using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class ButtonMap : EntityTypeConfiguration<Button>
    {
        public ButtonMap()
        {
            ToTable("Sys_ModuleButton");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.MenuId).HasColumnName("F_ModuleId");
            Property(p => p.EnCode).HasColumnName("F_EnCode");
            Property(p => p.Name).HasColumnName("F_FullName");
            Property(p => p.Icon).HasColumnName("F_Icon");
            Property(p => p.SortCode).HasColumnName("F_SortCode");
        }
    }
}