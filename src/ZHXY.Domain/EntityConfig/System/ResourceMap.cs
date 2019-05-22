using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class ResourceMap : EntityTypeConfiguration<Resource>
    {
        public ResourceMap()
        {
            ToTable("zhxy_resource");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ParentId).HasColumnName("p_id");
            Property(p => p.Level).HasColumnName("level");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.FullName).HasColumnName("full_name");
            Property(p => p.Icon).HasColumnName("icon");
            Property(p => p.IconForWeb).HasColumnName("icon_web");
            Property(p => p.Url).HasColumnName("url");
            Property(p => p.BelongSys).HasColumnName("belong_sys");
            Property(p => p.Type).HasColumnName("type");
            Property(p => p.Description).HasColumnName("description");
            Property(p => p.SortCode).HasColumnName("sort_code");

        }
    }
}