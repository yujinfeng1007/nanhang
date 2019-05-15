using System;

namespace ZHXY.Dorm.Device.NH
{
    public class PushAppMessage
    {
        public void PushReportMessage(string userName, string body, string msgType = "1011")
        {
            var a = new NHMessage.Portal_SendInstantMessageRequest(new NHMessage.Portal_SendInstantMessageRequestBody
            {
                SiteID = "",
                UID = userName,
                MsgType =msgType,
                MsgContent = body
            });
            Console.WriteLine("推送成功: "  + a );
        }
    }
}
