using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZHXY.Application
{
    public class UserData
    {
        public dynamic Menus { get; set; }
        public dynamic Buttons { get; set; }
        public string[] Roles { get; set; }
        public string Duty { get; set; }
        public string Organ { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string HeadIcon { get;  set; }
    }

}