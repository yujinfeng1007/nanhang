using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 字典明细
    /// </summary>
    public class SysDicItem: IEntity
    {
        public string DicId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}