using System;
using Topshelf;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace SocketApi
{

    /// <summary>
    /// 农行接口-使用SuperSocket框架
    /// </summary>
    class NewNongBankTask : ServiceControl, ServiceSuspend
    {
        //static string schoolCodeStatic = "   10319533100011779";//学校编码
        //static string schoolCodeStatic = "     103881999990082";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(NongBankTask));
        public NewNongBankTask()
        {
            
        }

        public bool Start(HostControl hostControl)
        {
            //Console.WriteLine("Press any key to start the server!");

            //Console.ReadKey();
            //Console.WriteLine();

            var appServer = new NhSocketServer();
            Console.WriteLine("The server started successfully!");
            //新连接处理方法
            appServer.NewSessionConnected += new SessionHandler<MySession>(appServer_NewSessionConnected);
            //处理请求
            appServer.NewRequestReceived += new RequestHandler<MySession, BinaryRequestInfo>(appServer_NewRequestReceived);

            //Setup the appServer
            if (!appServer.Setup(2333)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return false;
            }

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return false;
            }

            return true;

            //Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            //while (Console.ReadKey().KeyChar != 'q')
            //{
            //    Console.WriteLine();
            //    continue;
            //}

            ////Stop the appServer
            //appServer.Stop();

            //Console.WriteLine("The server was stopped!");
            //Console.ReadKey();


        }

        public bool Stop(HostControl hostControl)
        {
            
            Console.WriteLine("Socket Server Stopped......");
            log.Info(string.Format("Socket Server Stopped......"));
            //Thread.CurrentThread.Abort();
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            return true;
        }

        static void appServer_NewSessionConnected(MySession session)
        {
            session.Send("Welcome to NH Telnet Server");
        }

        static void appServer_NewRequestReceived(MySession session, BinaryRequestInfo requestInfo)
        {
            Console.WriteLine("appServer_NewRequestReceived......");
            session.Send("appServer_NewRequestReceived......");
            //switch (requestInfo.Key.ToUpper())
            //{
            //    case ("ECHO"):
            //        session.Send(requestInfo.Body);
            //        break;

            //    case ("ADD"):
            //        session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
            //        break;

            //    case ("MULT"):

            //        var result = 1;

            //        foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
            //        {
            //            result *= factor;
            //        }

            //        session.Send(result.ToString());
            //        break;
            //}
        }
    }
}
