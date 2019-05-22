using ZHXY.Domain;

namespace ZHXY.Application
{
    public class AddFuncDto
    {
        public string MenuId { get; set; }
        public string EnCode { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int? SortCode { get; set; }
        public string Type { get; set; } = SYS_CONSTS.Func;
    }
}