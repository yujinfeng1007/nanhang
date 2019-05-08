using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AttachmentMap : EntityTypeConfiguration<MailAttachment>
    {
        public AttachmentMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("ID");
            Property(p => p.Name).HasColumnName("Name");
            Property(p => p.Path).HasColumnName("Path");
            Property(p => p.ContentId).HasColumnName("ContentID");
            Property(p => p.MailId).HasColumnName("MailID");
        }
    }
}