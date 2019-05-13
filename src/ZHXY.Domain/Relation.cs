namespace ZHXY.Domain
{
    public static class Relation {

        public const char Separator = '_';
        /// <summary>
        /// 闸机和楼栋的关系
        /// </summary>
        public static string GateBuilding { get; } = $"{nameof(Gate)}{Separator}{nameof(Building)}";

        public static string BuildingUser { get; } = $"{nameof(Building)}{Separator}{nameof(User)}";
    }
}