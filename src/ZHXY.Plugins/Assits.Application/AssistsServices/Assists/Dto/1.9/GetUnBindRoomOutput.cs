using System.Collections.Generic;

namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.9.获取未绑定辅助屏的教室信息
    /// </summary>
    public class GetUnBindRoomOutput
    {
        /// <summary>
        /// 是否已绑定
        /// </summary>
        public bool F_Is_Binded { get; set; }

        public List<RoomOutput> F_Rooms { get; set; }
        public BindRoomOutput F_RoomInfo { get; set; }
    }
}