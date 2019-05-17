using System;

namespace ZHXY.Dorm.Device.NH
{
    public class PushAppMessage
    {
        public void PushReportMessage(string userName, string body, string msgType = "1011")
        {
           var service= new PushService.数据中心接口服务SoapClient();
           var result= service.Portal_SendInstantMessage("49",userName,"JSON",body);
           Console.WriteLine("推送成功: "  + result);
        }
    }
}
