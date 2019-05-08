using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class WechatConfigMap : EntityTypeConfiguration<SchWeChat>
    {
        public WechatConfigMap()
        {
            ToTable("School_Wechat_Config");
            HasKey(t => t.F_Id);
        }
    }
}