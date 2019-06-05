using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 销假服务
    /// </summary>
    public class CloseLeaveService:AppService
    {
        public CloseLeaveService(IZhxyRepository r) : base(r) { }

        /// <summary>
        /// 销假
        /// </summary>
        /// <param name="orderId"></param>
        public void CloseLeave(string orderId)
        {

        }







    }


}