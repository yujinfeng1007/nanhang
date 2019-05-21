using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHXY.Application.DormServices.Gates;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class FaceService  :  AppService
    {
        public FaceService(IZhxyRepository r) : base(r) { }
      

        /// <summary>
        /// 头像更新申请
        /// </summary>
        public void Request(FaceRequestDto input,string approveFilepath)
        {
            //生成更新后图像路径           

            var face = new StuFaceOrder
            {
                ApplicantId = input.ApplicantId,
                SubmitImg = input.SubmitImg,
                ApproveImg = approveFilepath,               
                Status = "0"
            };
            //获取学生所属楼栋的宿管
            var dormid = Read<DormStudent>(p => p.StudentId.Equals(input.ApplicantId)).Select(p => p.DormId).FirstOrDefault();
            var buildingno = Read<DormRoom>(p => p.Id.Equals(dormid)).Select(p => p.BuildingId).FirstOrDefault();
            var buildingIds = Read<Building>(p => p.BuildingNo.Equals(buildingno)).Select(p => p.Id).FirstOrDefault();
            var Approvers = Read<Relevance>(p => p.FirstKey.Equals(buildingIds)).Select(p => p.SecondKey).ToListAsync().Result;
            if (null == Approvers) throw new Exception("请先绑定宿管!");
            foreach (var item in Approvers)
            {
                Add(new FaceApprove
                {
                    ApproverId = item,
                    OrderId = face.Id,
                    ApproveLevel = 1
                });
            }
            Add(face);
            SaveChanges();
        }

      


        /// <summary>
        /// 获取头像审批列表
        /// </summary>
        public dynamic GetFaceApprovalList(GetFaceApprovalListDto input)
        {
            //获取当前用户所审批的审批单据
            var faceIds = Read<FaceApprove>(p => p.ApproverId.Equals(input.CurrentUserId)).Select(p => p.OrderId).ToListAsync().Result;
            //根据审批单据获取头像详细信息
            //var query1 = Read<StuFaceOrder>(p => faceIds.Contains(p.Id)).Select(p => p.Applicant).FirstOrDefault();
            //var test = query1.Name;
            var query = Read <StuFaceOrder>(p => faceIds.Contains(p.Id));
            query = string.IsNullOrEmpty(input.SearchPattern) ? query : query.Where(p => p.Status.Equals(input.SearchPattern));
            query = string.IsNullOrEmpty(input.Keyword) ? query : query.Where(p => p.Applicant.Name.Contains(input.Keyword));
           
                     
            query = query.Paging(input);
            var list = query.Select(p => new FaceListView
            {
                Id = p.Id,
                //tea?.JobNumber;
                ApplierName = p.Applicant.Name,
                SubmitImg = p.SubmitImg,
                ApproveImg = p.ApproveImg,                
                ApprovalStatus = p.Status,         
                CreatedTime = p.CreatedTime
            }).OrderByDescending(p =>p.CreatedTime).ToListAsync().Result;
            //SetViewStatus(input.CurrentUserId, ref list);
            return list;
            }

        /// <summary>
        /// 获取头像审批详情
        /// </summary>
        public FaceView GetFaceApprovalDetail(string appId, string currentUserId)
        {

            var face = Get<StuFaceOrder>(appId);
            //var leaveApprove = Read<FaceApprove>(p => p.OrderId.Equals(appId) && p.ApproverId.Equals(currentUserId)).FirstOrDefaultAsync().Result;
            //if (null == leaveApprove) throw new Exception("您不可以审批!");
            var view = new FaceView
            {
                Id = face.Id,
                ApplierName = face.Applicant.Name,
                SubmitImg = face.SubmitImg,
                ApproveImg = face.ApproveImg,
                ApprovalStatus = face.Status,
                CreatedTime = face.CreatedTime
            };
            //if (Convert.ToDecimal(view.LeaveDays) <= 3)
            //{
            //    view.IsFinal = true;
            //}
            //else
            //{
            //    view.IsFinal = leaveApprove.ApproveLevel == 2;
            //}
            return view;
        }


        /// <summary>
        /// 头像审批
        /// </summary>
        public void Approval(FaceApprovalDto input)
        {
            var face = Get<StuFaceOrder>(input.OrderId);
            if (null == face) throw new Exception("未找到头像申请信息!");
            //var faceApprove = Query<FaceApprove>(p => p.OrderId.Equals(face.Id) && p.ApproverId.Equals(input.CurrentUserId)).FirstOrDefaultAsync().Result;
            var faceApprovers = Query<FaceApprove>(p => p.OrderId.Equals(face.Id)).ToListAsync().Result;
            //if (null == faceApprove) throw new Exception("您不可以审批,头像申请单异常!");
            //if (faceApprove.Result != 0) throw new Exception("您已经审批过,不需要重复审批!");
            foreach ( var faceApprover in faceApprovers) {
                faceApprover.Result = input.IsAgreed ? 1 : -1;  
                faceApprover.Opinion = input.Opinion;
            }
            
            //SetOrderStatus(face);
            face.Status = "1";
            //if (!input.IsAgreed) MinusLimit(face.LeaveerId, Convert.ToDecimal(face.LeaveDays));
            //把多个审批人的状态同步更新为已审批

            SaveChanges();
            //审批同意则更新头像并下发
            if (input.IsAgreed) {

                new UserService(new ZhxyRepository()).UpdIco(face.ApplicantId, face.ApproveImg);
                new UserToGateService().SendUserHeadIco(new string[] { face.ApplicantId });
            }
           


        }



    }
}
