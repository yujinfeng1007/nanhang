using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class NoticeMap : EntityTypeConfiguration<SchNotice>
    {
        public NoticeMap()
        {
            ToTable("School_Notice");
            HasKey(t => t.F_Id);
        }
    }
}