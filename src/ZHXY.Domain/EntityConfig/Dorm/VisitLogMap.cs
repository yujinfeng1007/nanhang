using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class VisitApplyMap : EntityTypeConfiguration<VisitApply>
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
            Property(p => p.ApplicantId).HasColumnName("applicant_id");
            Property(p => p.DormId).HasColumnName("dorm_id");
            Property(p => p.BuildingId).HasColumnName("building_id");
            Property(p => p.VisitStartTime).HasColumnName("start_time");
            Property(p => p.VisitEndOfTime).HasColumnName("end_time");
            Property(p => p.Relationship).HasColumnName("relation");
            Property(p => p.Status).HasColumnName("status");
            Property(p => p.ApplicationTime).HasColumnName("application_time");
            Property(p => p.ProcessingTime).HasColumnName("processing_time");

        }
    }

}