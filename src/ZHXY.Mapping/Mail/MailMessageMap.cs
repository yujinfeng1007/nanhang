using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class MailMessageMap : EntityTypeConfiguration<MailMessage>
    {
        public MailMessageMap()
        {
            //ToTable("School_MailMessage");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("ID");
            Property(p => p.Uid).HasColumnName("UID");
            Property(p => p.UserId).HasColumnName("UserID");
            Property(p => p.MessageId).HasColumnName("MessageID");
            Property(p => p.Date).HasColumnName("Date");
            Property(p => p.From).HasColumnName("From");
            Property(p => p.Sender).HasColumnName("Sender");
            Property(p => p.ReplyTo).HasColumnName("ReplyTo");
            Property(p => p.To).HasColumnName("To");
            Property(p => p.Cc).HasColumnName("Cc");
            Property(p => p.Bcc).HasColumnName("Bcc");
            Property(p => p.Subject).HasColumnName("Subject");
            Property(p => p.Attachments).HasColumnName("Attachments");
            Property(p => p.BodyTextEncoding).HasColumnName("BodyTextEncoding");
            Property(p => p.BodyText).HasColumnName("BodyText");
            Property(p => p.BodyHtmlTextEncoding).HasColumnName("BodyHtmlTextEncoding");
            Property(p => p.BodyHtmlText).HasColumnName("BodyHtmlText");
            Property(p => p.Header).HasColumnName("Header");
            Property(p => p.ContentId).HasColumnName("ContentID");
            Property(p => p.ContentTransferEncoding).HasColumnName("ContentTransferEncoding");
            Property(p => p.ContentType).HasColumnName("ContentType");
            Property(p => p.ContentBase).HasColumnName("ContentBase");
            Property(p => p.IsMarkedForDeletion).HasColumnName("IsMarkedForDeletion");
            Property(p => p.Box).HasColumnName("Box");
            Property(p => p.IsRead).HasColumnName("IsRead");
            Property(p => p.CreateOn).HasColumnName("CreateOn");
            Property(p => p.LastModify).HasColumnName("LastModify");
        }
    }
}