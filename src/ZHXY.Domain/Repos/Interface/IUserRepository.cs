using System.Collections.Generic;

namespace ZHXY.Domain
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        void SubmitForm(User userEntity, UserLogin userLogOnEntity, string keyValue, List<UserRole> userRoles);

        void AddDatas(List<User> data);
    }
}