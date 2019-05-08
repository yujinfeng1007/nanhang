using System.Data.Entity;
using ZHXY.Data;
using ZHXY.Domain;

namespace ZHXY.Repository
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