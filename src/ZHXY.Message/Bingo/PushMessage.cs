using Link.Message.Client;
using Link.Message.Client.content;
using Link.Message.Client.messager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHXY.Message.Bingo
{
    public static class PushMessage
    {
        public static void SendMessage(string message, string loginId, string name)
        {
            try
            {
                MessageClient messageClient = new MessageClient(
        "http://www.jxgqedu.com:10082/",
        "81cfd7ff-d646-41ee-acce-10b4e294729f",
        "2a6703b5c096407aa488ebfe1abcaf56");
                SendMessageResult result = messageClient.SendSingleMessage(
           new TextMessageContent(message),
           new PersonMessageReceiver(loginId, name));
            }
            catch
            {

            }
        }
    }
}
