using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class ModuleButtonRepository : Repository<SysButton>, IModuleButtonRepository
    {
        public void SubmitCloneButton(List<SysButton> entitys)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                foreach (var item in entitys) db.Insert(item);
                db.Commit();
            }
        }
    }
}