using ZHXY.Common;

namespace ZHXY.Application
{
    public class GetCancelListDto: Pagination
    {
        public string CurrentUserId { get; set; }
        public string keyword { get; set; }

    }
}