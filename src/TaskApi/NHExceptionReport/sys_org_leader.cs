namespace TaskApi.NHExceptionReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_org_leader
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(64)]
        public string org_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(64)]
        public string user_id { get; set; }
    }
}
