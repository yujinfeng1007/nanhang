using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class FaceApproveMap : EntityTypeConfiguration<FaceApprove>
    {
        public FaceApproveMap()
        {
            ToTable("zhxy_face_approve");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("id");
            Property(p => p.OrderId).HasColumnName("face_id");
            Property(p => p.ApproverId).HasColumnName("approver_id");
            Property(p => p.ApproveLevel).HasColumnName("approve_level");
            Property(p => p.Result).HasColumnName("status");
            Property(p => p.Opinion).HasColumnName("opinion");

            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
        }
    }
}