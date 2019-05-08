using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SchoolAttachmentMap : EntityTypeConfiguration<SchAttachment>
    {
        public SchoolAttachmentMap()
        {
            ToTable("School_Attachments");
            HasKey(t => t.F_Id);
        }
    }
}