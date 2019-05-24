using System;

namespace ZHXY.Application
{
    public class DicItemView: CreateDicItemDto
    {
        public string Id { get; set; }
        
        public bool IsDefault { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}