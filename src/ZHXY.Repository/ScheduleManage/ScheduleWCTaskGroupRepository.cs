using ZHXY.Data;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ScheduleWCTaskGroupRepository : Repository<Schedule_WCTask_Group_Entity>,
        ISchedule_WCTask_Group_Repository
    {
        public new void Delete(string keyValue)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                db.Delete<Schedule_WishCourseGroup_Entity>(t => t.F_TaskCourseGroupID == keyValue);
                db.Delete<Schedule_WCTask_Group_Entity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
    }
}