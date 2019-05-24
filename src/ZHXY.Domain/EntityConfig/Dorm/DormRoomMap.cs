using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class DormRoomMap : EntityTypeConfiguration<DormRoom>
    {
        public DormRoomMap()
        {
            ToTable("zhxy_dorm");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Area).HasColumnName("area");
            Property(p => p.BuildingId).HasColumnName("building_id");
            Property(p => p.FloorNumber).HasColumnName("floor");
            Property(p => p.Capacity).HasColumnName("capacity");
            Property(p => p.DormType).HasColumnName("dorm_type");
            Property(p => p.RoomNumber).HasColumnName("room_number");
            Property(p => p.AllowGender).HasColumnName("allow_gender");
            Property(p => p.F_In).HasColumnName("f_in");
            Property(p => p.F_Free).HasColumnName("f_free");
            Property(p => p.Status).HasColumnName("status");
            Property(p => p.Title).HasColumnName("title");
            Property(p => p.ManagerId).HasColumnName("manager_id");
            Property(p => p.AdminstratorId).HasColumnName("adminstrator_id");

            HasRequired(p => p.Building).WithMany().HasForeignKey(p => p.BuildingId).WillCascadeOnDelete(false);
        }
    }
}