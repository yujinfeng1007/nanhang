using ZHXY.Data;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ScheduleMoveClassRepository : Repository<Schedule_MoveClass_Entity>, ISchedule_MoveClass_Repository
    {
        public new void Delete(string keyValue)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                db.Delete<Schedule_MoveClassStudent_Entity>(t => t.F_MoveClassId == keyValue);
                db.Delete<Schedule_MoveClass_Entity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
    }
}