using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SchoolInfoMap : EntityTypeConfiguration<SchInfo>
    {
        public SchoolInfoMap()
        {
            ToTable("School_Info");
            HasKey(t => t.F_Id);
        }
    }
}