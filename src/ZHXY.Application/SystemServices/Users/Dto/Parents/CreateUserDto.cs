using System;

namespace ZHXY.Application
{
    public class CreateUserDto
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Duty { get; set; }
        public string Dept { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public string WeChat { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public bool AllowedToLogin { get; set; }
        public string UserIdentity { get; set; }
        public string Remark { get; set; }

        public string DataPermissionType { get; set; }
        public string[] Roles { get; set; }
    }

}