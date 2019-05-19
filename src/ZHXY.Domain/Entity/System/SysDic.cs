using System;
namespace ZHXY.Domain
{
    /// <summary>
    /// 字典
    /// </summary>
    public class SysDic : IEntity
    {
        public string Id { get; set; }
        public int Category { get; set; }
       
        public string Name { get; set; }
        public int SortCode { get; set; }


    }
}