namespace ZHXY.Application
{
    /// <summary>
    /// 请假审批Dto
    /// </summary>
    public class LeaveApproveView
    {
        public string ApproverName { get; set; }
        public string Opinion { get; set; }
        public int Result { get; set; }
        public int Level { get; set; }
    }
}