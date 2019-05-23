using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class VisitorApproveMap : EntityTypeConfiguration<VisitorApprove>
    {
        public VisitorApproveMap()
        {
            ToTable("zhxy_visit_approve");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.VisitId).HasColumnName("visit_id");
            Property(p => p.ApproverId).HasColumnName("approver_id");
            Property(p => p.ApproveResult).HasColumnName("approve_result");
            Property(p => p.Opinion).HasColumnName("opinion");
            Property(p => p.ApproveLevel).HasColumnName("approve_level");

            // 导航属性
            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);            
        }
    }

}