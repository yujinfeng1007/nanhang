namespace ZHXY.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dorm_FlowInAndOutOfBuilding:IEntity
    {
        [Key]
        [StringLength(64)]
        public string flow_id { get; set; }

        [StringLength(64)]
        public string building_no { get; set; }

        [StringLength(64)]
        public string user_id { get; set; }

        public bool? direction { get; set; }

        public DateTime? occurrence_time { get; set; }

        public bool? is_abnormal { get; set; }
    }
}
