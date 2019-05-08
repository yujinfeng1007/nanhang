using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetTransferDetailMap : EntityTypeConfiguration<AssetTransferDetail>
    {
        public AssetTransferDetailMap()
        {
            HasRequired(p => p.AssetInfo).WithMany().HasForeignKey(p => p.AssetId);
            HasRequired(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
        }
    }
}