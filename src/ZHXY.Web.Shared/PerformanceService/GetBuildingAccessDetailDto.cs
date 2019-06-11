namespace ZHXY.Web.Shared
{
    public class GetBuildingAccessDetailDto: GetBuildingAccessStatisticsDto
    {
        public string UserId { get; set; }
        public string BuildingId { get; set; }
    }


}