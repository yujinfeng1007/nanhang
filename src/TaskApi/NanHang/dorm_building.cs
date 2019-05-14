namespace TaskApi.NanHang
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dorm_building
    {
        [StringLength(50)]
        public string id { get; set; }

        [StringLength(50)]
        public string title { get; set; }

        [StringLength(50)]
        public string area { get; set; }

        [StringLength(50)]
        public string building_no { get; set; }

        [StringLength(50)]
        public string floor_num { get; set; }

        [StringLength(50)]
        public string unit_num { get; set; }

        [StringLength(500)]
        public string address { get; set; }

        [StringLength(50)]
        public string classroom_type { get; set; }

        [StringLength(50)]
        public string classroom_status { get; set; }
    }
}
