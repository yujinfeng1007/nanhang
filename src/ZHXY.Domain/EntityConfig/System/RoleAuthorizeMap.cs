
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class RoleAuthorizeMap : EntityTypeConfiguration<RoleAuthorize>
    {
        public RoleAuthorizeMap()
        {
            ToTable("zhxy_role_authorize");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ItemType).HasColumnName("item_type");
            Property(p => p.ItemId).HasColumnName("item_id");
            Property(p => p.ObjectType).HasColumnName("obj_type");
            Property(p => p.ObjectId).HasColumnName("obj_id");
        }
    }
}