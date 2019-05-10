using System.Collections.Generic;

namespace ZHXY.Domain
{
    public interface ITeacherRepository : IRepositoryBase<Teacher>
    {
        void AddDatas(List<Teacher> datas);

        void AddDatasImport(List<Teacher> datas);

        void AddDatasImport(User userentity, UserLogin userlogentity, Teacher entity);
    }
}