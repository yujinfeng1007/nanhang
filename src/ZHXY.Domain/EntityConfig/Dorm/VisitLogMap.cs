using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class VisitApplyMap : EntityTypeConfiguration<VisitApply>
    {
        public VisitApplyMap()
        {
            ToTable("Dorm_VisitLog");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.VisitorGender).HasColumnName("F_Sex");
            Property(p => p.VisitorName).HasColumnName("F_Visitor_Name");
            Property(p => p.VisitorIDCard).HasColumnName("F_Visitor_Card");
            Property(p => p.VisitReason).HasColumnName("F_Visit_Reason");
            Property(p => p.ApplicantId).HasColumnName("F_CreatorUserId");
            Property(p => p.DormId).HasColumnName("F_Classroom_ID");
            Property(p => p.BuildingId).HasColumnName("F_Building_ID");
            Property(p => p.VisitStartTime).HasColumnName("F_Visit_startTime");
            Property(p => p.VisitEndOfTime).HasColumnName("F_Visit_endTime");
            Property(p => p.Relationship).HasColumnName("F_Rela");
            Property(p => p.Status).HasColumnName("status");
            Property(p => p.ApplicationTime).HasColumnName("F_CreatorTime");
            Property(p => p.ProcessingTime).HasColumnName("F_LastModifyTime");

        }
    }

}