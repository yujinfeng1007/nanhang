using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("Sys_Role");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.OrganId).HasColumnName("F_OrganizeId");
            Property(p => p.Category).HasColumnName("F_Category");
            Property(p => p.EnCode).HasColumnName("F_EnCode");
            Property(p => p.Name).HasColumnName("F_FullName");
            Property(p => p.Type).HasColumnName("F_Type");
            Property(p => p.DataType).HasColumnName("F_Data_Type");
            Property(p => p.DataDeps).HasColumnName("F_Data_Deps");
            Property(p => p.SortCode).HasColumnName("F_SortCode");
            Property(p => p.Description).HasColumnName("F_Description");
        }
    }
}