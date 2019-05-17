using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string OrganId { get; set; }
        public int? Category { get; set; }
        public string EnCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public string DataType { get; set; }

        //自定义数据权限机构字段
        public string DataDeps { get; set; }

        public int? SortCode { get; set; }
        public string Description { get; set; }
    }

}