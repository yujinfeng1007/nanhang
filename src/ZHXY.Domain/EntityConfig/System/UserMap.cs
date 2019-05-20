using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("zhxy_user");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Account).HasColumnName("account");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.NickName).HasColumnName("nickname");
            Property(p => p.HeadIcon).HasColumnName("head_icon");
            Property(p => p.Gender).HasColumnName("gender");
            Property(p => p.Birthday).HasColumnName("birthday");
            Property(p => p.MobilePhone).HasColumnName("mobile_phone");
            Property(p => p.Email).HasColumnName("email");
            Property(p => p.OrganId).HasColumnName("organ_id");
            Property(p => p.DutyId).HasColumnName("duty_id");
            Property(p => p.SetUp).HasColumnName("user_setup");
        }
    }
}