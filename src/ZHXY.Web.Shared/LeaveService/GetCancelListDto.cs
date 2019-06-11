namespace ZHXY.Web.Shared
{
    public class GetCancelListDto: Pagination
    {
        public string CurrentUserId { get; set; }
        public string keyword { get; set; }

    }
}