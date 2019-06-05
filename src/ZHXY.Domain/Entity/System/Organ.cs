using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ZHXY.Domain
{
    /// <summary>
    /// 机构
    /// </summary>
    public class Organ : IEntity
    {
        public string Id { get; set; }= Guid.NewGuid().ToString("N").ToUpper();
        public string ParentId { get; set; }
        public string EnCode { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string CategoryId { get; set; }
        public int? SortCode { get; set; }
       

        #region 导航属性

        /// <summary>i
        /// 下级机构
        /// </summary>
        [JsonIgnore] public virtual ICollection<Organ> Children { get; set; } = new List<Organ>();

        /// <summary>
        /// 上级机构
        /// </summary>
        [JsonIgnore] public virtual Organ Parent { get; set; }

        #endregion 导航属性
    }
}