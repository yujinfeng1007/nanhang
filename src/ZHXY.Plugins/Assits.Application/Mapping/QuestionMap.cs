using System.Data.Entity.ModelConfiguration;
using ZHXY.Assists.Entity;

namespace ZHXY.Assists.Mapping
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            ToTable("Assists_Question");
            HasKey(p => p.Id);

            Property(p => p.AskerId).HasColumnName("F_AskerId").HasColumnType("varchar").HasMaxLength(64).IsRequired();
            Property(p => p.CourseId).HasColumnName("F_CourseId").HasColumnType("varchar").HasMaxLength(64).IsRequired();
            Property(p => p.MarkId).HasColumnName("F_MarkId").HasColumnType("varchar").HasMaxLength(64);
            Property(p => p.Content).HasColumnName("F_Content").HasColumnType("varchar").HasMaxLength(500).IsRequired();

            HasRequired(p => p.Asker).WithMany().HasForeignKey(p => p.AskerId);
            HasRequired(p => p.Course).WithMany().HasForeignKey(p => p.CourseId);
            HasOptional(p => p.Mark).WithMany().HasForeignKey(p => p.MarkId);
        }
    }
}