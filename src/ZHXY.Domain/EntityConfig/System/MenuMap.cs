using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            ToTable("zhxy_menu");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ParentId).HasColumnName("p_id");
            Property(p => p.Level).HasColumnName("level");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Icon).HasColumnName("icon");
            Property(p => p.IconForWeb).HasColumnName("icon_web");
            Property(p => p.Url).HasColumnName("url");
            Property(p => p.Target).HasColumnName("target");
            Property(p => p.IsMenu).HasColumnName("is_menu");
            Property(p => p.IsExpand).HasColumnName("is_expand");
            Property(p => p.IsPublic).HasColumnName("is_public");
            Property(p => p.SortCode).HasColumnName("sort_code");
            Property(p => p.BelongSys).HasColumnName("belong_sys");

            HasMany(p => p.ChildNodes).WithOptional(p => p.Parent).HasForeignKey(p => p.ParentId);

        }
    }
}