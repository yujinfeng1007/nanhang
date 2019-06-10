using System.Collections.Generic;

namespace ZHXY.Application
{
    public interface ILeaveService: IAppService
    {
        void AddApprover(AddApproverDto input);
        void Approval(LeaveApprovalDto input);
        void SuspendLeave(string orderId);
        dynamic GetApprovalDetail(string id, string currentUserId);
        dynamic GetApprovalList(GetApprovalListDto input);
        object GetFinalJudgeList(string search);
        dynamic GetLeaveHistory(GetLeaveHistoryDto input);
        ApproveDetailView GetOrderDetail(string id);
        object GetPrevApprove(string leaveId);
        (List<LeaveListView> list, int recordCount, int pageCount) GetSpecialList(GetSpecialLeaveDto input);
        object GetStuByOrg(string orgId, string keyword);
        List<ContactGroupView> GetTeachers();
        void OneKeyApproval(OneKeyApprovalDto input);
        void Request(LeaveRequestDto input);
        void SpecialApply(BulkLeaveDto input, string currentUserId);
    }
}