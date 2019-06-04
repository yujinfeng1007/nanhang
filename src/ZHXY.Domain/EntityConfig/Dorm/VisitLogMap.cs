using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class VisitApplyMap : EntityTypeConfiguration<VisitorApply>
    {
        public VisitApplyMap()
        {
            ToTable("zhxy_visit_apply");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.VisitorGender).HasColumnName("visitor_gender");
            Property(p => p.VisitorName).HasColumnName("visitor_name");
            Property(p => p.VisitorIDCard).HasColumnName("visitor_id_card");
            Property(p => p.VisitReason).HasColumnName("reason");
            Property(p => p.VisitType).HasColumnName("type");
            Property(p => p.ApplicantId).HasColumnName("applicant_id");
            Property(p => p.DormId).HasColumnName("dorm_id");
            Property(p => p.BuildingId).HasColumnName("building_id");
            Property(p => p.VisitStartTime).HasColumnName("start_time");
            Property(p => p.VisitEndTime).HasColumnName("end_time");
            Property(p => p.Relationship).HasColumnName("relation");
            Property(p => p.Status).HasColumnName("status");
            Property(p => p.ImgUri).HasColumnName("img_uri");
            Property(p => p.CreatedTime).HasColumnName("created_time");
            Property(p => p.ApprovedTime).HasColumnName("approved_time");
            Property(p => p.DhId).HasColumnName("dh_id");

            // 导航属性
            HasOptional(p => p.Student).WithMany().HasForeignKey(p => p.ApplicantId);
            HasOptional(p => p.DormRoom).WithMany().HasForeignKey(p => p.DormId);
        }
    }

}