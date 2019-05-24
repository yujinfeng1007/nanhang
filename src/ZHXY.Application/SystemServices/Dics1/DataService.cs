using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class DataService : AppService
    {
        public DataService(IZhxyRepository r) : base(r) { }

        public async Task<string> GetMenuName(string menuId)
        {
            return await Read<Resource>(p => p.Id.Equals(menuId)).Select(p => p.Name).FirstOrDefaultAsync();
        }



    }
}