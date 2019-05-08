namespace TaskApi
{
    public class UserModel
    {
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string LoginId { get; set; }
        public string PassWord { get; set; }
        public string TELEPHONE { get; set; }
        public string MOBILE { get; set; }
        public string UserStatus { get; set; }
        public string CreatedTime { get; set; }
        public string LastUpdatedTime { get; set; }

        public string UserType { get; set; }

        public int Level { get; set; }

        public string post_Description { get; set; }

        public string user_Num { get; set; }
        public string sex { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string identity_num { get; set; }
        /// <summary>
        /// 科目
        /// </summary>
        public string major { get; set; }
        /// <summary>
        /// 民 族
        /// </summary>
        public string nationality { get; set; }
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string political { get; set; }
        /// <summary>
        /// 是否管理员，1是，0否
        /// </summary>
        public string has_school_admin { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string identity_type { get; set; }
        /// <summary>
        /// 入校时间
        /// </summary>
        public string join_school_time { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string picture{get;set;}
        public string isHeadmaster { get;  set; }
        public string isSubHeadmaster { get;  set; }

        public string IS_DELETED { get; set; }
        // 家长ID
        public string GuardianId { get; set; }

        public string GuardianName { get; set; }
    }
}