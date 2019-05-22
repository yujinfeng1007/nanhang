using ZHXY.Domain;

namespace ZHXY.Application
{
    public class AddMenuDto
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string IconForWeb { get; set; }
        public string Url { get; set; }
        public string Type { get; set; } = SYS_CONSTS.Menu;
        public string BelongSys { get; set; }
    }
}