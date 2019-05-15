namespace ZHXY.Domain
{
    public static class Relation {

        public const char Separator = '_';
        /// <summary>
        /// 闸机和楼栋的关系
        /// </summary>
        public static string GateBuilding { get; } = $"{nameof(Gate)}{Separator}{nameof(Building)}";

        public static string BuildingUser { get; } = $"{nameof(Building)}{Separator}{nameof(User)}";


        /// <summary>
        /// 班级和班主任的关系
        /// </summary>
        public static string ClassLeader { get; } = $"{nameof(Organ)}{Separator}{nameof(Teacher)}";
    }
}