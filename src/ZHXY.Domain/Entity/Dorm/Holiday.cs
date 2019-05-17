using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHXY.Domain
{
    public class Holiday: IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
