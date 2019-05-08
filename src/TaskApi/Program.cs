using System;
using System.IO;
using Topshelf;

namespace TaskApi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
                HostFactory.Run(x =>               
                {
                    x.UseLog4Net();
                    x.Service<ServiceRunner>();
                    x.RunAsLocalSystem();          
                    x.SetDescription("循环任务");   
                    x.SetDisplayName("循环任务");   
                    x.SetServiceName("循环任务");   
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}