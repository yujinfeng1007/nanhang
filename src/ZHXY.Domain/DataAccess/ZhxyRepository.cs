using System.Data.Entity;

namespace ZHXY.Domain
{
    public class ZhxyRepository : Repоsitory, IZhxyRepository
    {
        public ZhxyRepository():base()
        {
        }

        public ZhxyRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}