namespace ZHXY.Web.Shared
{
    public class AddFilterIpDto
    {
        public bool? Type { get; set; }
        public string StartWithIp { get; set; }
        public string EndWithIp { get; set; }
        public string Description { get; set; }
    }
}