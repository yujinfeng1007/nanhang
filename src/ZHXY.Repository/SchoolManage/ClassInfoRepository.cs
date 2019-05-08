using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ClassInfoRepository : Data.Repository<ClassInfo>, IClassInfoRepository
    {
        public new void Delete(string keyValue)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                db.Delete<SysOrganize>(t => t.F_Id == keyValue);
                db.Delete<ClassInfo>(t => t.F_ClassID == keyValue);
                db.Delete<ClassTeacher>(t => t.F_ClassID == keyValue);
                db.Commit();
            }
        }
    }
}