using System.Collections.Generic;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ClassTeacherRepository : Data.Repository<ClassTeacher>, IClassTeacherRepository
    {
       
        public void AddItem(ClassTeacher entity, List<Dictionary<string, string>> dicd, string keyValue)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                if (dicd.Count > 0)
                {
                    db.Delete<ClassTeacher>(t => t.F_ClassID == keyValue);
                    for (var i = 0; i < dicd.Count; i++)
                    {
                        var ent = new ClassTeacher();
                        ent.Create();
                        ent.F_Teacher = dicd[i]["F_Teacher"];
                        ent.F_CourseID = dicd[i]["F_CourseID"];
                        ent.F_ClassID = keyValue;
                        ent.F_Leader_Tea = entity.F_Leader_Tea;
                        ent.F_Leader_Tea2 = entity.F_Leader_Tea2;
                        db.Insert(ent);
                    }
                }
                else
                {
                    db.Delete<ClassTeacher>(t => t.F_ClassID == keyValue);
                }

                db.Commit();
            }
        }
    }
}