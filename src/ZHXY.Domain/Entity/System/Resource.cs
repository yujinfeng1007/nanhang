using System;

namespace ZHXY.Domain
{
    public class Resource : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string ParentId { get; set; } 
        public int? Level { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public string IconForWeb { get; set; }
        public string Url { get; set; }
        public string BelongSys { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public int? SortCode { get; set; }

    }
}