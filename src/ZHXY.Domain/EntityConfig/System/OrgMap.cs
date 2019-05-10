using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class OrgMap : EntityTypeConfiguration<Organ>
    {
        public OrgMap()
        {
            ToTable("Sys_Organize");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.ParentId).HasColumnName("F_ParentId");
            Property(p => p.EnCode).HasColumnName("F_EnCode");
            Property(p => p.Name).HasColumnName("F_FullName");
            Property(p => p.ShortName).HasColumnName("F_ShortName");
            Property(p => p.CategoryId).HasColumnName("F_CategoryId");
            Property(p => p.SortCode).HasColumnName("F_SortCode");
            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.ParentId);
        }
    }
}