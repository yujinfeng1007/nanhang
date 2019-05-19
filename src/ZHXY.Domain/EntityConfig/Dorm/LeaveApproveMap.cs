using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class LeaveApproveMap : EntityTypeConfiguration<LeaveApprove>
    {
        public LeaveApproveMap()
        {
            ToTable("zhxy_leave_approve");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("id");
            Property(p => p.OrderId).HasColumnName("order_id");
            Property(p => p.ApproverId).HasColumnName("approver_id");
            Property(p => p.ApproveLevel).HasColumnName("approve_level");
            Property(p => p.Result).HasColumnName("result");
            Property(p => p.Opinion).HasColumnName("opinion");

            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
        }
    }
}