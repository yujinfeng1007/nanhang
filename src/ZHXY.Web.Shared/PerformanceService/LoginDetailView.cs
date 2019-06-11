using System;

namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 登录记录view
    /// </summary>
    public class LoginDetailView
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime LoginTime { get; set; }
        public string WayOfLogin { get; set; }
    }
    

  
}