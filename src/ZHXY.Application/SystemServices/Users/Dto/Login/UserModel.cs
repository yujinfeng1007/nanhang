namespace ZHXY.Application
{
    public class UserModel
    {
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string LoginId { get; set; }
        public string PassWord { get; set; }
        public string Telephone { get; set; }
        public string MobilePhone { get; set; }
        public string UserStatus { get; set; }
        public string CreatedTime { get; set; }
        public string LastUpdatedTime { get; set; }

        public string UserType { get; set; }

        public int Level { get; set; }
        public string post_Description { get; set; }

        public string OrgCode { get; set; }

        public string orgIds { get; set; }

        public string rootOrgId { get; set; }
    }
}