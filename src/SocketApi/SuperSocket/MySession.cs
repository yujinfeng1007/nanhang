using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;

namespace SocketApi
{
    public class MySession : AppSession<MySession, BinaryRequestInfo>
    {
        protected override void HandleException(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
