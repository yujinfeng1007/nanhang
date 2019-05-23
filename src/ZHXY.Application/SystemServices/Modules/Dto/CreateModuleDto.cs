namespace ZHXY.Application
{
    public class CreateModuleDto
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string AffiliatedSystem { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string Icon { get; set; }
        public string ClassisIcon { get; set; }
        public int SortCode { get; set; }
        public string[] Setting { get; set; }
        public string Profile { get; set; }

    }
    }