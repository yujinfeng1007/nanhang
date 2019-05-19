using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class SemesterMap : EntityTypeConfiguration<Semester>
    {
        public SemesterMap()
        {
            ToTable("zhxy_semester");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Year).HasColumnName("year");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Nickname).HasColumnName("nickname");
            Property(p => p.StartTime).HasColumnName("start_time");
            Property(p => p.EndOfTime).HasColumnName("end_time");
        }
    }
}