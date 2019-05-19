using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class FunctionMap : EntityTypeConfiguration<Function>
    {
        public FunctionMap()
        {
            ToTable("zhxy_func");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.MenuId).HasColumnName("menu_id");
            Property(p => p.EnCode).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Icon).HasColumnName("icon");
            Property(p => p.SortCode).HasColumnName("sort_code");
        }
    }
}