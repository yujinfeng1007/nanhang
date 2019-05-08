using ZHXY.Data;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ScheduleClassRepository : Repository<Schedule_Class_Schedule_Entity>, IScheduleClassRepository
    {
        public new void Delete(string keyValue)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                db.Delete<Schedule_MoveClassStudent_Entity>(t =>
                    t.Schedule_MoveClass_Entity.F_ClassIds.Contains(keyValue));
                db.Delete<Schedule_MoveClass_Entity>(t => t.F_ClassIds.Contains(keyValue));

                db.Delete<Schedule_ClassStudent_Entity>(t => t.F_ClassId == keyValue);
                db.Delete<Schedule_Class_Schedule_Entity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
    }
}