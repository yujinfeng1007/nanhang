using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHXY.Domain;

namespace ZHXY.Application.DormServices.Face.Dto
{
    public class FaceService  :  AppService
    {
        public FaceService(IZhxyRepository r) : base(r) { }

        /// <summary>
        /// 头像更新申请
        /// </summary>
        public void Request(FaceRequestDto input)
        {            
            var face = new StuFaceOrder
            {
                ApplicantId = input.ApplicantId,
                //SubmitImg = input.SubmitImg,
                //ApproveImg = input.ApproveImg,               
                Status = "0"
            };
            //foreach (var item in input.Approvers)
            //{
            //    Add(new FaceApprove
            //    {
            //        ApproverId = item,
            //        OrderId = face.Id,
            //        ApproveLevel = 1
            //    });
            //}
            Add(face);
            SaveChanges();
        }



        /// <summary>
        /// 获取审批详情:学生姓名，审批前头像，提交头像，提交时间，审核状态
        /// </summary>
        public LeaveView GetApprovalDetail(string id, string currentUserId)
        {

            //var leave = Get<StuLeaveOrder>(id);
            //var leaveApprove = Read<LeaveApprove>(p => p.OrderId.Equals(id) && p.ApproverId.Equals(currentUserId)).FirstOrDefaultAsync().Result;
            //if (null == leaveApprove) throw new Exception("您不可以审批!");
            //var view = new LeaveView
            //{
            //    Id = leave.Id,
            //    LeaveerName = leave.Leaveer.Name,
            //    StartTime = leave.StartTime,
            //    EndOfTime = leave.EndOfTime,
            //    LeaveDays = leave.LeaveDays,
            //    LeaveType = leave.LeaveType,
            //    ReasonForLeave = leave.ReasonForLeave
            //};
            //if (Convert.ToDecimal(view.LeaveDays) <= 3)
            //{
            //    view.IsFinal = true;
            //}
            //else
            //{
            //    view.IsFinal = leaveApprove.ApproveLevel == 2;
            //}
            //return view;
            return null;
        }



    }
}
