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
            ToTable("School_Stu_Face");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.CreatedTime).HasColumnName("F_CreatorTime");
            Property(p => p.ApplicantId).HasColumnName("F_Applicant").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.SubmitImg).HasColumnName("F_SubmitImg").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.ApproveImg).HasColumnName("F_ApproveImg").HasColumnType("varchar").HasMaxLength(50);           
            Property(p => p.Status).HasColumnName("F_Status").HasColumnType("varchar");
            Property(p => p.ApprovalOpinion).HasColumnName("F_ApprovalOpinion").HasColumnType("varchar");


            // 导航属性
            HasOptional(p => p.Applicant).WithMany().HasForeignKey(p => p.ApplicantId);           
        }
    }
}