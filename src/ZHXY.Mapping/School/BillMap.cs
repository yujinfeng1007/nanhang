using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class BillMap : EntityTypeConfiguration<SchoolBill>
    {
        public BillMap()
        {
            ToTable("School_Bill");
            HasKey(t => t.F_Id);
            HasOptional(t => t.School_EntrySignUp_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_In)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.School_ExamSignUp_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_Exam)
                .WillCascadeOnDelete(false);
        }
    }
}