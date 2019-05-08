using System.Collections.Generic;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ModuleButtonRepository : Data.Repository<SysButton>, IModuleButtonRepository
    {
        public void SubmitCloneButton(List<SysButton> entitys)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                foreach (var item in entitys) db.Insert(item);
                db.Commit();
            }
        }
    }
}