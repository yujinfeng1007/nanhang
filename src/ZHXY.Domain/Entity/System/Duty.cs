using System;

namespace ZHXY.Domain
{
    public class Duty : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string Code { get; set; }
        public string Name { get; set; }
        public int? SortCode { get; set; }
        public string Description { get; set; }
    }

}