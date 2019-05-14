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
            Property(p => p.RoleId).HasColumnName("F_RoleId");
            Property(p => p.DutyId).HasColumnName("F_DutyId");
            Property(p => p.DeleteMark).HasColumnName("F_DeleteMark");
            Property(p => p.EnabledMark).HasColumnName("F_EnabledMark");
            Property(p => p.CreatorTime).HasColumnName("F_CreatorTime");
            Property(p => p.DataType).HasColumnName("F_Data_Type");
            Property(p => p.DataDeps).HasColumnName("F_Data_Deps");
            Property(p => p.UserSetUp).HasColumnName("F_User_SetUp");
            Property(p => p.Class).HasColumnName("F_Class");
            Property(p => p.UpdateTime).HasColumnName("F_UpdateTime");
            Property(p => p.File).HasColumnName("F_File");
        }
    }
}