namespace ZHXY.Domain
{
    public static class Relation
    {

        public const char Separator = '_';
        /// <summary>
        /// 闸机和楼栋的关系
        /// </summary>
        public static string GateBuilding { get; } = $"{nameof(Gate)}{Separator}{nameof(Building)}";

        
        /// <summary>
        /// 大屏和楼栋的关系
        /// </summary>
        public static string DeviceBuilding { get;} = $"{nameof(Device)}{Separator}{nameof(Building)}";


        /// <summary>
        /// 楼栋和用户的关系
        /// </summary>
        public static string BuildingUser { get; } = $"{nameof(Building)}{Separator}{nameof(User)}";


        /// <summary>
        /// 班级和班主任的关系
        /// </summary>
        public static string ClassLeader { get; } = $"{nameof(Organ)}{Separator}{nameof(Teacher)}";


        /// <summary>
        /// 用户和角色
        /// </summary>
        public static string UserRole { get; } = $"{nameof(User)}{Separator}{nameof(Role)}";


        /// <summary>
        /// 用户资源
        /// </summary>
        public static string RoleResource { get; } = $"{nameof(Role).ToLower()}{Separator}{nameof(Resource).ToLower()}";

    }
}