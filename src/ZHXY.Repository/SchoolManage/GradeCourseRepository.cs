using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class GradeCourseRepository : Data.Repository<SchGradeCourse>, IGradeCourseRepository
    {
      
        public void AddItem(SchGradeCourse entity, string keyValue)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                db.Delete<SchGradeCourse>(t => t.F_Grade == keyValue);
                if (!string.IsNullOrEmpty(entity.F_CourseId))
                {
                    var F_CourseId = entity.F_CourseId.Split(',');
                    for (var i = 0; i < F_CourseId.Length - 1; i++)
                    {
                        var ent = new SchGradeCourse();
                        ent.F_Divis = entity.F_Divis;
                        ent.F_Grade = entity.F_Grade;
                        ent.F_Grade_Code = entity.F_Grade_Code;
                        ent.F_Grade_Name = entity.F_Grade_Name;
                        ent.F_Memo = entity.F_Memo;
                        ent.F_School = entity.F_School;
                        ent.F_CourseId = F_CourseId[i];
                        ent.Create();
                        db.Insert(ent);
                    }
                }

                db.Commit();
            }
        }
    }
}