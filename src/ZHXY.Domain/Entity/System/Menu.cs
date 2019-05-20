using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZHXY.Domain
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string ParentId { get; set; } = "null";
        public int Level { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public string IconForWeb { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public bool? IsMenu { get; set; }
        public bool? IsExpand { get; set; }
        public bool? IsPublic { get; set; }
        public int? SortCode { get; set; }
        public string BelongSys { get; set; }

        //public virtual List<Menu> ChildNodes { get; set; }
        //[JsonIgnore]public virtual Menu Parent { get; set; }
    }
}