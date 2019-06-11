using System;

namespace ZHXY.Web.Shared
{
    public class CancellableLeaveView
    {
        public string OrderId { get; set; }
        public string ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndOfTime { get; set; }
        public decimal? LeaveDays { get; set; }
        public string ReasonForLeave { get; set; }
        public string LeaveType { get; set; }
    }
}