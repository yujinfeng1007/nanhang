using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class PublichRecordMap : EntityTypeConfiguration<OfficeAnnouncement>
    {
        public PublichRecordMap()
        {
            ToTable("PublichRecord");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.OfficeId).HasColumnName("F_OfficeId");
            Property(p => p.Content).HasColumnName("F_Content");
            Property(p => p.PublisherId).HasColumnName("F_PublisherId");
            Property(p => p.PublishTime).HasColumnName("F_PublishTime");
        }
    }
}