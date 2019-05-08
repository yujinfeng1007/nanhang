using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ContactMap : EntityTypeConfiguration<MailContact>
    {
        public ContactMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("ID");
            Property(p => p.User).HasColumnName("User");
            Property(p => p.CreateOn).HasColumnName("CreateOn");
            Property(p => p.LastModify).HasColumnName("LastModify");
            Property(p => p.FimilyName).HasColumnName("FimilyName");
            Property(p => p.GivenName).HasColumnName("GivenName");
            Property(p => p.NickName).HasColumnName("NickName");
            Property(p => p.MemoName).HasColumnName("MemoName");
            Property(p => p.PhoneNumber).HasColumnName("PhoneNumber");
            Property(p => p.Email).HasColumnName("Email");
            Property(p => p.Company).HasColumnName("Company");
            Property(p => p.Position).HasColumnName("Position");
            Property(p => p.Tags).HasColumnName("Tags");
            Property(p => p.Remarks).HasColumnName("Remarks");
        }
    }
}