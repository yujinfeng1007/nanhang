using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class FaceService  :  AppService
    {
        public FaceService(DbContext r) : base(r) { }
      

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
            //获取学生所在的dormid
            var dormid = Read<DormStudent>(p => p.StudentId.Equals(input.ApplicantId)).Select(p => p.DormId).FirstOrDefault();
            //获取dorm对应的楼栋Id
            var buildingId = Read<DormRoom>(p => p.Id.Equals(dormid)).Select(p => p.BuildingId).FirstOrDefault();
            //获取楼栋对应的宿管
            var Approvers = Read<Relevance>(p => p.FirstKey.Equals(buildingId)).Select(p => p.SecondKey).ToListAsync().Result;
            if (null == Approvers) throw new Exception("请先绑定宿管!");
            foreach (var item in Approvers)
            {
                AddAndSave(new FaceApprove
                {
                    ApproverId = item,
                    OrderId = face.Id,
                    ApproveLevel = 1
                });
            }
            AddAndSave(face);
            SaveChanges();
        }

        /// <summary>
        /// 学生和宿管获取头像审批列表
        /// </summary>
        public dynamic GetFaceApprovalList(GetFaceApprovalListDto input)
        {
            IQueryable<StuFaceOrder> query = null;
            var faceListViews = new List<FaceListView>();
            //判断登陆用户是学生，还是宿管   若是学生获取所有提交的申请，若是老师则查看所有审批的申请
            var dutyId = Read<User>(p => p.Id.Equals(input.CurrentUserId)).Select(p => p.DutyId).FirstOrDefaultAsync().Result;
            if (dutyId.Equals("teacherDuty") || dutyId.Equals("suguanDuty"))
            {
                //获取当前用户所审批的审批单据
                var faceIds = Read<FaceApprove>(p => p.ApproverId.Equals(input.CurrentUserId)).Select(p => p.OrderId).ToListAsync().Result;
                //根据审批单据获取头像详细信息
                query = Read<StuFaceOrder>(p => faceIds.Contains(p.Id));
            }
            else if (dutyId.Equals("studentDuty"))
            {
                query = Read<StuFaceOrder>(p => p.ApplicantId.Equals(input.CurrentUserId));
            }
            else
            {
                return faceListViews;
            }
            query = string.IsNullOrEmpty(input.SearchPattern) ? query : query.Where(p => p.Status.Equals(input.SearchPattern));
            query = string.IsNullOrEmpty(input.Keyword) ? query : query.Where(p => p.Applicant.Name.Contains(input.Keyword));
            faceListViews = query.OrderByDescending(p => p.CreatedTime).PagingNoSort(input).Select(p => new FaceListView
            {
                Id = p.Id,
                ApplierName = p.Applicant.Name,
                SubmitImg = p.SubmitImg,
                ApproveImg = p.ApproveImg,
                ApprovalStatus = p.Status,
                CreatedTime = p.CreatedTime
            }).ToListAsync().Result;
            return faceListViews;
        }

        /// <summary>
        /// 学生、宿管获取头像审批详情
        /// </summary>
        public FaceListView GetFaceApprovalDetail(string appId, string currentUserId)
        {
            //获取审批结果和意见（随机取一条即可）
            var approveInfo = Read<FaceApprove>(p => p.OrderId.Equals(appId)).FirstOrDefaultAsync().Result;
            var face = Get<StuFaceOrder>(appId);
            var view = new FaceListView
            {
                Id = face.Id,
                ApplierName = face.Applicant.Name,
                SubmitImg = face.SubmitImg,
                ApproveImg = face.ApproveImg,
                ApprovalStatus = face.Status,
                Result = approveInfo.Result,
                Opinion = approveInfo.Opinion,
                CreatedTime = face.CreatedTime
            };
            return view;
        }


        /// <summary>
        /// 头像审批
        /// </summary>
        public void Approval(FaceApprovalDto input)
        {
            var face = Get<StuFaceOrder>(input.OrderId);
            if (null == face) throw new Exception("未找到头像申请信息!");
            var faceApprovers = Query<FaceApprove>(p => p.OrderId.Equals(face.Id)).ToListAsync().Result;
            foreach ( var faceApprover in faceApprovers) {
                faceApprover.Result = input.IsAgreed ? "1" :"-1" ;  
                faceApprover.Opinion = input.Opinion;
            }
            face.ApproveTime = DateTime.Now;
            face.Status = "1";                        
            SaveChanges();
            //审批同意则更新头像并下发
            if (input.IsAgreed) {
                new UserService(new ZhxyDbContext()).UpdIco(face.ApplicantId, face.ApproveImg);
                new StudentService(new ZhxyDbContext()).UpdIco(face.ApplicantId, face.ApproveImg);                
                new UserToGateService().SendUserHeadIco(new string[] { face.ApplicantId });
                Console.WriteLine();
            }
        }
    }
}
