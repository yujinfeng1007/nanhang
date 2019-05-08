using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class CourseTeacherRepository : Data.Repository<TeacherCourse>, ICourseTeacherRepository
    {
       

        public void AddItem(string F_Teacher, string keyValue, string F_CourseID)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                if (!string.IsNullOrEmpty(F_Teacher))
                {
                    db.Delete<TeacherCourse>(t => t.F_Code == keyValue);
                    var F_Teachers = F_Teacher.Split(',');
                    for (var i = 0; i < F_Teachers.Length; i++)
                    {
                        var entity = new TeacherCourse();
                        entity.Create();
                        entity.F_Teacher = F_Teachers[i];
                        entity.F_Code = keyValue;
                        entity.F_CourseID = F_CourseID;
                        db.Insert(entity);
                    }
                }
                else
                {
                    db.Delete<TeacherCourse>(t => t.F_Code == keyValue);
                }

                db.Commit();
            }
        }
    }
}