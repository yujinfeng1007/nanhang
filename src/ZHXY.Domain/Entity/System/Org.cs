using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ZHXY.Domain
{
    /// <summary>
    /// 机构
    /// </summary>
    public class Org : IEntity
    {
        public string Id { get; set; }= Guid.NewGuid().ToString("N").ToUpper();
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Type { get; set; }
        public int? Sort { get; set; }
       

        #region 导航属性

        /// <summary>
        /// 下级机构
        /// </summary>
        [JsonIgnore] public virtual ICollection<Org> Children { get; set; } = new List<Org>();

        /// <summary>
        /// 上级机构
        /// </summary>
        [JsonIgnore] public virtual Org Parent { get; set; }

        #endregion 导航属性
    }
}