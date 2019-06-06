using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    /// <summary>
    /// 头像申请
    /// </summary>
    public class StuFaceOrderMap : EntityTypeConfiguration<StuFaceOrder>
    {
        public StuFaceOrderMap()
        {
            ToTable("zhxy_stu_face");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.CreatedTime).HasColumnName("created_time");
            Property(p => p.ApplicantId).HasColumnName("applicant");
            Property(p => p.SubmitImg).HasColumnName("submit_img");
            Property(p => p.ApproveImg).HasColumnName("approve_img");           
            Property(p => p.Status).HasColumnName("status");
            Property(p => p.ApprovalOpinion).HasColumnName("opinion");
            Property(p => p.ApproveTime).HasColumnName("approve_time");

            // 导航属性
            HasOptional(p => p.Applicant).WithMany().HasForeignKey(p => p.ApplicantId);           
        }
    }
}