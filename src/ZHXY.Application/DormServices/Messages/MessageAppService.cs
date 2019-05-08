using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 报表服务
    /// </summary>
    public class MessageAppService : AppService
    {
        public MessageAppService(IZhxyRepository r) => R = r;

        /// <summary>
        /// 设置机构消息接收人
        /// </summary>
        public void SetOrgMessageRecipient(SetOrgHeadDto input)
        {

        }

    }
}