using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 大屏设备app版本管理
    /// </summary>
    public class AppVersion:IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 更新说明
        /// </summary>
        public string Description { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

    }
}
