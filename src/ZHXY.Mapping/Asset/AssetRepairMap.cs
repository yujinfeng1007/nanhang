using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetRepairMap : EntityTypeConfiguration<AssetRepair>
    {
        public AssetRepairMap()
        {
            Property(p => p.AssetName).HasColumnName("F_AssetName").HasColumnType("varchar");
            Property(p => p.RepairDate).HasColumnName("F_RepairDate").HasColumnType("datetime");
            Property(p => p.Why).HasColumnName("F_Why").HasColumnType("varchar");
            Property(p => p.Company).HasColumnName("F_Company").HasColumnType("varchar");
            Property(p => p.Budget).HasColumnName("F_Budget").HasColumnType("decimal");
            Property(p => p.FinishDate).HasColumnName("F_FinishDate").HasColumnType("datetime");
            Property(p => p.Results).HasColumnName("F_Results").HasColumnType("varchar");
            Property(p => p.Situation).HasColumnName("F_Situation").HasColumnType("varchar");
            Property(p => p.ActualCost).HasColumnName("F_ActualCost").HasColumnType("decimal");
            Property(p => p.OtherCost).HasColumnName("F_OtherCost").HasColumnType("decimal");
            Property(p => p.AccOpinion).HasColumnName("F_AccOpinion").HasColumnType("varchar");
            Property(p => p.AccTime).HasColumnName("F_AccTime").HasColumnType("datetime");

            // 外键属性
            Property(p => p.AcceptorId).HasColumnName("F_AcceptorID").HasColumnType("varchar").HasMaxLength(50);

            // 导航属性
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);
            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
        }
    }
}