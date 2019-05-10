using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    ///     author: zby
    ///     phone:  
    ///     email:  
    /// </summary>
    public class UpdateDormBuildingDto : CreateDormBuildingDto
    {
        public string Id { get; set; }

        public static implicit operator Building(UpdateDormBuildingDto dto)
        {
            return dto.MapTo<Building>();
        }
    }
}