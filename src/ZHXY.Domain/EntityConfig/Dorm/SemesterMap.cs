using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class SemesterMap : EntityTypeConfiguration<Semester>
    {
        public SemesterMap()
        {
            ToTable("School_Semester");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.Year).HasColumnName("F_Year");
            Property(p => p.Name).HasColumnName("F_Name");
            Property(p => p.Nickname).HasColumnName("F_Nickname");
            Property(p => p.StartTime).HasColumnName("F_Start_Time");
            Property(p => p.EndOfTime).HasColumnName("F_End_Time");
        }
    }
}