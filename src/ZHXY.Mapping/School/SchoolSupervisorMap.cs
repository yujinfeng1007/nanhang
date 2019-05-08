using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity.School;

namespace ZHXY.Mapping.School
{
    public class SchoolSupervisorMap : EntityTypeConfiguration<school_supervisor>
    {
        public SchoolSupervisorMap()
        {
            ToTable("school_supervisor");
            HasKey(t => t.id);
        }
    }
}
