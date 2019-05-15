using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class UserLogOnMap : EntityTypeConfiguration<UserLogin>
    {
        public UserLogOnMap()
        {
            ToTable("Sys_UserLogOn");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.UserId).HasColumnName("F_UserId");
            Property(p => p.UserPassword).HasColumnName("F_UserPassword");
            Property(p => p.UserSecretkey).HasColumnName("F_UserSecretkey");
            Property(p => p.PreviousVisitTime).HasColumnName("F_PreviousVisitTime");
            Property(p => p.LastVisitTime).HasColumnName("F_LastVisitTime");
            Property(p => p.LogOnCount).HasColumnName("F_LogOnCount");
        }
    }
}