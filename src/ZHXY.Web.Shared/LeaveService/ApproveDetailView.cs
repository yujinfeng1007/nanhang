using System;
using System.Collections.Generic;

namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 请假详情Dto
    /// </summary>
    public class ApproveDetailView
    {
        public string Id { get; set; }
        public string LeaveerName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndOfTime { get; set; }
        public decimal? LeaveDays { get; set; }
        public bool Cancelable { get; set; }
        public string LeaveType { get; set; }
        public string ReasonForLeave { get; set; }
        public string Status { get; set; }
        public string[] Approvers { get; set; }
        public List<LeaveApproveView> Approves { get; set; }
        public string AttachmentsPath { get;  set; }
    }
}