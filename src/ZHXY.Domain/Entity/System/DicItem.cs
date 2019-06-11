namespace ZHXY.Domain.Entity
{
    /// <summary>
    /// 字典明细
    /// </summary>
    public class DicItem: IEntity
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Sort { get; set; }

    }
}