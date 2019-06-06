using log4net;
using log4net.Config;
using System;
using System.IO;

namespace ZHXY.Common
{
    /// <summary>
    /// 日志类
    /// </summary>
    public static class LogHelper
    {
        public static ILog GetLogger(Type type) => LogManager.GetLogger(type);
        public static ILog GetLogger(string str) => LogManager.GetLogger(str);

        static LogHelper()
        {
            var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            if (file.Exists) XmlConfigurator.Configure(file);
        }
    }
}