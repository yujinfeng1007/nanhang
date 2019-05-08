namespace TaskApi
{
    public class OrgModel
    {
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public string ParentOrgId { get; set; }

        //public string OrgCode { get; set; }
        public string OrgStatus { get; set; }

        public string CreatedTime { get; set; }
        public string LastUpdatedTime { get; set; }
        public int Level { get; set; }

        public string IS_DELETED { get; set; }
    }
}