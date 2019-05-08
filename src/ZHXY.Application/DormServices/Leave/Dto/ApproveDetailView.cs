using System.Collections.Generic;

namespace ZHXY.Application
{
    /// <summary>
    /// 请假详情Dto
    /// </summary>
    public class ApproveDetailView
    {
        public string Id { get; set; }
        public string LeaveerName { get; set; }
        public string StartTime { get; set; }
        public string EndOfTime { get; set; }
        public string LeaveDays { get; set; }
        public string LeaveType { get; set; }
        public string ReasonForLeave { get; set; }
        public string Status { get; set; }
        public string[] Approvers { get; set; }
        public List<LeaveApproveView> Approves { get; set; }


    }
}