using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ExamTitleMap : EntityTypeConfiguration<ExamTitle>
    {
        public ExamTitleMap()
        {
            ToTable("School_ExamTitle");
            HasKey(t => t.F_Id);
        }
    }
}