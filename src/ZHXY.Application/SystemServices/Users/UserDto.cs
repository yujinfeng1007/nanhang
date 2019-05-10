using System;

namespace ZHXY.Application
{
    public class UserDto
    {
        public string F_Id { get; set; }

        public string F_Account { get; set; }

        public string F_RealName { get; set; }

        public string F_NickName { get; set; }

        public string F_HeadIcon { get; set; }

        public bool? F_Gender { get; set; }

        public DateTime? F_Birthday { get; set; }

        public string F_MobilePhone { get; set; }


        public string EmailPassword { get; set; }


        public string F_DepartmentId { get; set; }

        public string F_RoleId { get; set; }
        public string F_DutyId { get; set; }
        public bool? F_IsAdministrator { get; set; }
        public string F_OrganizeId { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }


        public DateTime? F_UpdateTime { get; set; }

        public string F_File { get; set; }
    }
}