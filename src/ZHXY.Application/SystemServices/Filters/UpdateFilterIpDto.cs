namespace ZHXY.Application
{
    public class UpdateFilterIpDto
    {
        public string Id { get; set; }
        public bool? Type { get; set; }
        public string StartWithIp { get; set; }
        public string EndWithIp { get; set; }
        public string Description { get; set; }
    }
}