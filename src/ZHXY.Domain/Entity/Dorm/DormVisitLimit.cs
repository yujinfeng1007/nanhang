using System;

namespace ZHXY.Domain
{
    public class DormVisitLimit : IEntity
    {

        public string StudentId { get; set; }

        public int TotalLimit { get; set; }

        public int UsableLimit { get; set; }

        public int IsAutoSet { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool Enabled { get; set; }

    }
}
