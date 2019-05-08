using System.Collections.Generic;

namespace ZHXY.Domain
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        void SubmitForm(Role roleEntity, List<RoleAuthorize> roleAuthorizeEntitys, string keyValue);
    }
}