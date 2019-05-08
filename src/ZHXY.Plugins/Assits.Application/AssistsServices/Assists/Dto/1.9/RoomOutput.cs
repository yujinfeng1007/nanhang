namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.9.获取未绑定辅助屏的教室信息
    /// </summary>
    public class RoomOutput
    {
        /// <summary>
        /// 教室ID
        /// </summary>
        public string F_Id { get; set; }

        /// <summary>
        /// 楼编号
        /// </summary>
        public string F_Building_No { get; set; }

        /// <summary>
        /// 楼层号
        /// </summary>
        public string F_Floor_No { get; set; }

        /// <summary>
        /// 教室编号
        /// </summary>
        public string F_Classroom_No { get; set; }

        /// <summary>
        /// 教室名称
        /// </summary>
        public string F_Name { get; set; }
    }
}