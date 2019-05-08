using ZHXY.Data;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ScheduleWishCourseTaskRepository : Repository<Schedule_WishCourseTask_Entity>,
        ISchedule_WishCourseTask_Repository
    {
        public new void Delete(string keyValue)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                db.Delete<Schedule_WishCourseGroup_Entity>(t => t.F_TaskId == keyValue);
                db.Delete<Schedule_WCTask_Group_Entity>(t => t.F_TaskId == keyValue);
                db.Delete<Schedule_WishCourseTask_Entity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
    }
}