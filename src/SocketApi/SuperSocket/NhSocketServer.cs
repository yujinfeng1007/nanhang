using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SocketApi
{
    public class NhSocketServer : AppServer<MySession,BinaryRequestInfo>
    {
        public NhSocketServer()
            : base(new DefaultReceiveFilterFactory<MyReceiveFilter, BinaryRequestInfo>())
        {

        }
    }
}