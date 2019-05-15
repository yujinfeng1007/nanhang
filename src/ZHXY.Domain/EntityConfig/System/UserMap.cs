using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("Sys_User");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.Account).HasColumnName("F_Account");
            Property(p => p.Name).HasColumnName("F_RealName");
            Property(p => p.NickName).HasColumnName("F_NickName");
            Property(p => p.HeadIcon).HasColumnName("F_HeadIcon");
            Property(p => p.Gender).HasColumnName("F_Gender");
            Property(p => p.Birthday).HasColumnName("F_Birthday");
            Property(p => p.MobilePhone).HasColumnName("F_MobilePhone");
            Property(p => p.Email).HasColumnName("F_Email");
            Property(p => p.WeChat).HasColumnName("F_WeChat");
            Property(p => p.OrganId).HasColumnName("F_DepartmentId");
            Property(p => p.DutyId).HasColumnName("F_DutyId");
            Property(p => p.UserSetUp).HasColumnName("F_User_SetUp");
        }
    }
}