using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class ModuleButtonRepository : Repository<Button>, IModuleButtonRepository
    {
        public void SubmitCloneButton(List<Button> entitys)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                foreach (var item in entitys) db.Insert(item);
                db.Commit();
            }
        }
    }
}