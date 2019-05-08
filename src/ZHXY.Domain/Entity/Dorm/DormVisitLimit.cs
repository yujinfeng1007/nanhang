using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZHXY.Domain
{
    public class DormVisitLimit : Entity
    {
        public new string F_Id { get; set; }

        public string Student_Id { get; set; }

        public int TotalLimit { get; set; }

        public int UsableLimit { get; set; }

        public int IsAutoSet { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdaetTime { get; set; }

        public new bool F_EnabledMark { get; set; }

        [ForeignKey("Student_Id")]
        public virtual DormStudent DormStudent { get; set; }

        [ForeignKey("Student_Id")]
        public virtual Student student { get; set; }
    }
}
