using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetMap : EntityTypeConfiguration<Asset>
    {
        public AssetMap()
        {
            Property(p => p.AssetNumber).HasColumnName("F_AssetNumber");
            Property(p => p.Name).HasColumnName("F_FullName");
            Property(p => p.BarCode).HasColumnName("F_BarCode");
            Property(p => p.Brand).HasColumnName("F_Brand");
            Property(p => p.Size).HasColumnName("F_Size");
            Property(p => p.Model).HasColumnName("F_Model");
            Property(p => p.Origin).HasColumnName("F_Origin");
            Property(p => p.UnitPrice).HasColumnName("F_UnitPrice");
            Property(p => p.Salvage).HasColumnName("F_Salvage");
            Property(p => p.TimeLimit).HasColumnName("F_TimeLimit");
            Property(p => p.MLoss).HasColumnName("F_MLoss");
            Property(p => p.RegistrationDate).HasColumnName("F_RegistrationDate");
            Property(p => p.PurchaseDate).HasColumnName("F_PurchaseDate");
            Property(p => p.InvoiceNumber).HasColumnName("F_InvoiceNumber");
            Property(p => p.RegisterNumber).HasColumnName("F_RegisterNumber");
            Property(p => p.WarrantyPeriod).HasColumnName("F_WarrantyPeriod");
            Property(p => p.UseTo).HasColumnName("F_UseTo");
            Property(p => p.PurchaseNumber).HasColumnName("F_PurchaseNumber");
            Property(p => p.RepairCycle).HasColumnName("F_RepairCycle");
            Property(p => p.SearchIndex).HasColumnName("F_SearchIndex");

            // 字典
            Property(p => p.BuyOfWay).HasColumnName("F_BuyOfWay").HasColumnType("smallint");
            Property(p => p.Source).HasColumnName("F_Source").HasColumnType("smallint");
            Property(p => p.Status).HasColumnName("F_Status").HasColumnType("smallint");
            // 外键
            Property(p => p.StoreId).HasColumnName("F_StoreID");
            Property(p => p.OwnerId).HasColumnName("F_OwnerID");
            Property(p => p.UserId).HasColumnName("F_UserID");
            Property(p => p.CustodianId).HasColumnName("F_CustodianID");
            Property(p => p.CategoryId).HasColumnName("F_Type");
            Property(p => p.UseDepartmentId).HasColumnName("F_UseDepartmentID");
            Property(p => p.SupplierId).HasColumnName("F_SupplierID");


            //导航
            HasOptional(p => p.StorePlace).WithMany().HasForeignKey(p => p.StoreId);
            HasOptional(p => p.Owner).WithMany().HasForeignKey(p => p.OwnerId);
            HasOptional(p => p.Supplier).WithMany().HasForeignKey(p => p.SupplierId);
            HasOptional(p => p.User).WithMany().HasForeignKey(p => p.UserId);
            HasOptional(p => p.Custodian).WithMany().HasForeignKey(p => p.CustodianId);
            HasOptional(p => p.UseDepartment).WithMany().HasForeignKey(p => p.UseDepartmentId);
            HasOptional(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId);
        }
    }
}