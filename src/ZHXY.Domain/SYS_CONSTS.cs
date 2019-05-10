namespace ZHXY.Domain
{
    /// <summary>
    /// 智慧校园常量
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public static class SYS_CONSTS
    {

        //缓存key
        public static string DATAITEMS { get; } = "dataItems";
        public static string ORGANIZE { get; } = "organize";
        public static string ROLE { get; } = "role";
        public static string DUTY { get; } = "duty";
        public static string AUTHORIZEBUTTON { get; } = "authorizeButton";
        public static string AREA { get; } = "area";
        public static string AREACHILD { get; } = "areachild";
        public static string USERS { get; } = "users";

        public static string CLASSTEACHERS { get; } = "classTeachers";

        // 关系名称
        /// <summary>
        /// 闸机和楼栋的关系
        /// </summary>
        public static string REL_GATE_BUILDING { get; } = "GATE_BUILDING";

        public static string REL_BUILDING_USERS { get; } = "BUILDING_USER";

    }
}