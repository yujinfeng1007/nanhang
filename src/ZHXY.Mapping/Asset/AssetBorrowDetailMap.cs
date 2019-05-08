using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetBorrowDetailMap : EntityTypeConfiguration<AssetBorrowDetail>
    {
        public AssetBorrowDetailMap()
        {
            Property(p => p.ToUserId).HasColumnName("F_ToUserID");
            Property(p => p.PlannedReturnDate).HasColumnName("F_PlannedReturnDate");
            Property(p => p.ActualReturnDate).HasColumnName("F_ActualReturnDate");

            HasRequired(p => p.AssetInfo).WithMany().HasForeignKey(p => p.AssetId);
            HasRequired(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);

            HasOptional(p => p.ToUser).WithMany().HasForeignKey(p => p.ToUserId);
        }
    }
}