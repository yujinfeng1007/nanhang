namespace ZHXY.Application
{
    public class CreateParentDto
    {
        public string MobilePhone { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public bool EnabledMark { get; set; }
        public string Description { get; set; }
        public string HeadIcon { get; set; }
    }
}