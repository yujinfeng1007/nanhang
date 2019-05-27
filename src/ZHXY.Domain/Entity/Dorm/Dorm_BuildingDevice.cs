using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 大屏设备
    /// </summary>
    public partial class Device: IEntity
    {
        public string Id { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatorTime { get; set; }
    }
}
