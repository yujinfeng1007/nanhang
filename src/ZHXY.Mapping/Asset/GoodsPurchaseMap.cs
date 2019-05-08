using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsPurchaseMap : EntityTypeConfiguration<GoodsPurchase>
    {
        public GoodsPurchaseMap()
        {
            Property(p => p.InvoiceNumber).HasColumnName("F_InvoiceNumber");
            Property(p => p.Rate).HasColumnName("F_Rate");
            Property(p => p.SupplierName).HasColumnName("F_SupplierName");
            Property(p => p.SupplierContact).HasColumnName("F_SupplierContact");
            Property(p => p.SupplierPhone).HasColumnName("F_SupplierPhone");
            // 外键
            Property(p => p.SupplierId).HasColumnName("F_SupplierId");
            // 导航
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);
            HasOptional(p => p.Supplier).WithMany().HasForeignKey(p => p.SupplierId);
            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
        }
    }
}