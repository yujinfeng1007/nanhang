using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 闸机
    /// </summary>
    public class Gate: IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNumber { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 软件版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public int Status { get; set; }
    }

}
