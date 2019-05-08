using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class LeaveApproveMap : EntityTypeConfiguration<LeaveApprove>
    {
        public LeaveApproveMap()
        {
            ToTable("School_LeaveApprove");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.OrderId).HasColumnName("F_LeaveId");
            Property(p => p.ApproverId).HasColumnName("F_ApproverId");
            Property(p => p.ApproveLevel).HasColumnName("F_ApproveLevel");
            Property(p => p.Result).HasColumnName("F_Status");
            Property(p => p.Opinion).HasColumnName("F_Opinion");

            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
        }
    }
}