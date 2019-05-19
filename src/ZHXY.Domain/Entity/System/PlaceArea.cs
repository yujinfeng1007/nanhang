using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 地区
    /// </summary>
    public class PlaceArea : IEntity
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int? Level { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SimpleSpelling { get; set; }
        public int? SortCode { get; set; }
    }
}