using System.Collections.Generic;

namespace ZHXY.Application
{
    public class ContactGroupView
    {
        public string GroupName  { get; set; }
        public List<ContactView> Items { get; set; }
    }
}