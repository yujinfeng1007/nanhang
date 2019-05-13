using System.Collections.Generic;

namespace ZHXY.Domain
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public new void Delete(string keyValue)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                db.Delete<Role>(t => t.Id == keyValue);
                db.Delete<RoleAuthorize>(t => t.F_ObjectId == keyValue);
                db.Commit();
            }
        }

        public void SubmitForm(Role roleEntity, List<RoleAuthorize> roleAuthorizeEntitys, string keyValue)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(roleEntity);
                }
                else
                {
                    roleEntity.Category = 1;
                    db.Insert(roleEntity);
                }

                db.Delete<RoleAuthorize>(t => t.F_ObjectId == roleEntity.Id);
                db.BatchInsert(roleAuthorizeEntitys);
                db.Commit();
            }
        }
    }
}