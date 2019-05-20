using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class InOutReceiveMap: EntityTypeConfiguration< InOutReceive>
    {
        public InOutReceiveMap()
        {
            ToTable("zhxy_in_out_receive");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Type).HasColumnName("type");
            Property(p => p.ReceiveUser).HasColumnName("receive_user");
        }
    }
}