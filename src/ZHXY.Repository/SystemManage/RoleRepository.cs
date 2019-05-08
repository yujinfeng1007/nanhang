using System.Collections.Generic;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class RoleRepository : Data.Repository<SysRole>, IRoleRepository
    {
        public new void Delete(string keyValue)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                db.Delete<SysRole>(t => t.F_Id == keyValue);
                db.Delete<SysRoleAuthorize>(t => t.F_ObjectId == keyValue);
                db.Commit();
            }
        }

        public void SubmitForm(SysRole roleEntity, List<SysRoleAuthorize> roleAuthorizeEntitys, string keyValue)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(roleEntity);
                }
                else
                {
                    roleEntity.F_Category = 1;
                    db.Insert(roleEntity);
                }

                db.Delete<SysRoleAuthorize>(t => t.F_ObjectId == roleEntity.F_Id);
                db.BatchInsert(roleAuthorizeEntitys);
                db.Commit();
            }
        }
    }
}