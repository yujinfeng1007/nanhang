using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class OrganizeRepository : Data.Repository<SysOrganize>, IOrganizeRepository
    {
        public OrganizeRepository(string schoolCode) : base(schoolCode)
        {
        }

        public OrganizeRepository()
        {
        }
    }
}