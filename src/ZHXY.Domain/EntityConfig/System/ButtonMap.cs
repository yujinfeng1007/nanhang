
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class ButtonMap : EntityTypeConfiguration<Button>
    {
        public ButtonMap()
        {
            ToTable("zhxy_button");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ModuleId).HasColumnName("module_id");
            Property(p => p.ParentId).HasColumnName("p_id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Icon).HasColumnName("icon");
            Property(p => p.Url).HasColumnName("url");
            Property(p => p.Sort).HasColumnName("sort");
        }
    }
}