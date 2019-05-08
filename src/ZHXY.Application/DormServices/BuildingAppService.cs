using ZHXY.Application;
using ZHXY.Domain;

namespace ZHXY.ApplicationServices
{
    public class BuildingAppService : AppService
    {
        public BuildingAppService(IZhxyRepository r) => R = r;
    }
}
