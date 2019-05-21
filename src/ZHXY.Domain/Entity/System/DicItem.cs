using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 字典明细
    /// </summary>
    public class DicItem: IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string Code { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}