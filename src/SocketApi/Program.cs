using System;
using System.IO;
using Topshelf;

namespace SocketApi
{
    internal class Program
    {
        /// <summary>
        //    1. 实例化并设置socket实例对象

        //    a.创建ip地址和端口

        //    b.绑定监听地址

        //    c.设置一下允许同时访问数

        //2. 监听连接

        //    a.通过启动一个新的线程执行，这样主线程不会假死(启动线程，所带的参数必须是object类型)

        //    b.利用循环等待连接并返回一个负责通信的socket实例

        //    c.返回的socket实例中可以获取到所连接客服的IP地址

        //3. 接收客服的发送过来的消息

        //    a.在监听方法中启动一个新的线程执行

        //    b.利用循环获取发送过来的消息，调用获取消息的方法需要传递一个字节变量参数，作为容器。方法返回值为int，表示获取到的有效字节数

        //    c.如果有效字节数为0则跳出循环

        //    d.接收到消息给客服的返回消息
        /// </summary>
        /// <param name="args"></param>

        private static void Main(string[] args)
        {
           
        }
    }
}