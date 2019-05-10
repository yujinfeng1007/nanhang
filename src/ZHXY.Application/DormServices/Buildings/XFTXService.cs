using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 下发头像服务
    /// </summary>
    public class XFTXService : AppService
    {
        public XFTXService(IZhxyRepository r) : base(r) { }
    }
}