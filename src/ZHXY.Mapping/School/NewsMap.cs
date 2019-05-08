using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class NewsMap : EntityTypeConfiguration<SchNews>
    {
        public NewsMap()
        {
            ToTable("School_News_Info");
            HasKey(t => t.F_Id);
        }
    }
}