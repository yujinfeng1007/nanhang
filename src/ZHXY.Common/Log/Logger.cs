using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Web;

namespace ZHXY.Common
{
    /// <summary>
    /// 日志类
    /// </summary>
    public static class Logger
    {
        public static ILog GetLogger(Type type) => LogManager.GetLogger(type);
        public static ILog GetLogger(string str) => LogManager.GetLogger(str);

        static Logger()
        {
            var configFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs/log4net.config"));
            if (!configFile.Exists)
                configFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            if (!configFile.Exists)
                configFile = new FileInfo(HttpContext.Current.Server.MapPath("/Configs/log4net.config"));
            if (!configFile.Exists)
                configFile = new FileInfo(HttpContext.Current.Server.MapPath("/log4net.config"));
            if (configFile.Exists)
                XmlConfigurator.Configure(configFile);
        }
    }
}