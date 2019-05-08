using System;
namespace ZHXY.Domain
{
    /// <summary>
    /// 楼栋信息
    /// </summary>   
    public class Building : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string Title { get; set; }
        public string Area { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNum { get; set; }
        public string UnitNum { get; set; }
        public string Address { get; set; }
        public string BuildingType { get; set; }//教学楼、办公楼、学生宿舍、教职工宿舍、食堂、其他
        public string BuildingStatus { get; set; }//正常使用、检修中、已停用
    }
}