using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class CourseRepository : Data.Repository<SchCourse>, ICourseRepository
    {
        public CourseRepository(string schoolCode) : base(schoolCode)
        {
        }

        public CourseRepository()
        {
        }

        public new void Delete(string keyValue)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                db.Delete<SchCourse>(t => t.F_Id == keyValue);
                db.Delete<SchCourse>(t => t.F_ParentId == keyValue);
                db.Delete<TeacherCourse>(t => t.F_CourseID == keyValue);
                db.Delete<SchGradeCourse>(t => t.F_CourseId == keyValue);
                db.Commit();
            }
        }

        public void AddCourseTeachers(SchCourse entity, string keyValue, string F_Teachers)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Modify(keyValue);
                    db.Update(entity);
                }
                else
                {
                    entity.Create();
                    db.Insert(entity);
                }

                db.Delete<TeacherCourse>(t => t.F_Code == entity.F_Code);
                var teachers = F_Teachers.Split(',');
                for (var i = 0; i < teachers.Length - 1; i++)
                {
                    var teacherentity = new TeacherCourse();
                    teacherentity.F_Code = entity.F_Code;
                    teacherentity.F_Teacher = teachers.ToString();
                    teacherentity.Create();
                    db.Insert(teacherentity);
                }

                db.Commit();
            }
        }
    }
}