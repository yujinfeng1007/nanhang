using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{

    public class DormMap: EntityTypeConfiguration<DormRoom>
    {
        public DormMap()
        {
            ToTable("dorm_dorm");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.Area).HasColumnName("F_Area");
            Property(p => p.BuildingId).HasColumnName("F_Building_No");
            Property(p => p.FloorNumber).HasColumnName("F_Floor_No");
            Property(p => p.UnitNumber).HasColumnName("F_Unit_No");
            Property(p => p.Capacity).HasColumnName("F_Accommodate_No");
            Property(p => p.DormType).HasColumnName("F_Classroom_Type");
            Property(p => p.RoomNumber).HasColumnName("F_Classroom_No");
            Property(p => p.AllowGender).HasColumnName("F_Sex");
            Property(p => p.F_In).HasColumnName("F_In");
            Property(p => p.F_Free).HasColumnName("F_Free");
            Property(p => p.Status).HasColumnName("F_Classroom_Status");
            Property(p => p.Title).HasColumnName("F_Title");
            Property(p => p.ManagerId).HasColumnName("F_Leader_ID");
            Property(p => p.AdminstratorId).HasColumnName("F_Manager_ID");
        }
    }
}