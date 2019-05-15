using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            ToTable("Sys_Module");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.ParentId).HasColumnName("F_ParentId");
            Property(p => p.Level).HasColumnName("level");
            Property(p => p.EnCode).HasColumnName("F_EnCode");
            Property(p => p.Name).HasColumnName("F_FullName");
            Property(p => p.Icon).HasColumnName("F_Icon");
            Property(p => p.IconForWeb).HasColumnName("F_Ico");
            Property(p => p.Url).HasColumnName("F_UrlAddress");
            Property(p => p.Target).HasColumnName("F_Target");
            Property(p => p.IsMenu).HasColumnName("F_IsMenu");
            Property(p => p.IsExpand).HasColumnName("F_IsExpand");
            Property(p => p.IsPublic).HasColumnName("F_IsPublic");
            Property(p => p.SortCode).HasColumnName("F_SortCode");
            Property(p => p.BelongSys).HasColumnName("F_BelongSys");

            HasMany(p => p.ChildNodes).WithOptional(p => p.Parent).HasForeignKey(p => p.ParentId);

        }
    }
}