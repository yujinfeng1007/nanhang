using ZHXY.Data;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class Schedule_Grade_Classroom_Repository : Repository<Schedule_Grade_Classroom_Entity>, ISchedule_Grade_Classroom_Repository
    {
      

        public void AddItem(Schedule_Grade_Classroom_Entity entity, string keyValue)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                db.Delete<Schedule_Grade_Classroom_Entity>(t => t.F_Grade == keyValue);
                if (!string.IsNullOrEmpty(entity.F_ClassroomId))
                {
                    var F_CourseId = entity.F_ClassroomId.Split(',');
                    for (var i = 0; i < F_CourseId.Length - 1; i++)
                    {
                        var ent = new Schedule_Grade_Classroom_Entity
                        {
                            F_Divis = entity.F_Divis,
                            F_Grade = entity.F_Grade,
                            F_Memo = entity.F_Memo,
                            F_ClassroomId = F_CourseId[i].ToString()
                        };
                        ent.Create();
                        db.Insert(ent);
                    }
                }
                db.Commit();
            }
        }
    }
}