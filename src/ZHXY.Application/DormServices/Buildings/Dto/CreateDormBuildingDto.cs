using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 创建楼栋DTO
    ///     author: zby
    ///     
    /// </summary>
    public class CreateDormBuildingDto
    {
        public string Title { get; set; }
        public string Area { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNum { get; set; }
        public string UnitNum { get; set; }
        public string Address { get; set; }
        public string BuildingType { get; set; }
        public string BuildingStatus { get; set; }

        public static implicit operator Building(CreateDormBuildingDto dto)
        {
            return dto.MapTo<Building>();
        }
    }
}