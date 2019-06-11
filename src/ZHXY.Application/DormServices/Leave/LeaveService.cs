using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;

namespace ZHXY.Application
{
    /// <summary>
    /// 请假服务
    /// <author>yujinfeng</author>
    /// </summary>
    public partial class LeaveService : AppService, ILeaveService
    {
        public LeaveService(DbContext r) : base(r) { }

        /// <summary>
        /// 请假申请
        /// </summary>
        public void Request(LeaveRequestDto input)
        {
            CheckCanRequest(input);
            var leave = new LeaveOrder
            {
                ApplicantId = input.LeaveerId,
                LeaveerId = input.LeaveerId,
                StartTime = input.StartTime,
                EndOfTime = input.EndOfTime,
                LeaveDays = input.LeaveDays,
                LeaveType = input.LeaveType,
                Reason = input.ReasonForLeave,
                AttachmentsPath = input.AttachmentsPath,
                Status = "0"
            };
            foreach (var item in input.Approvers)
            {
                AddAndSave(new LeaveApprove
                {
                    ApproverId = item,
                    OrderId = leave.Id,
                    ApproveLevel = 1
                });
            }
            AddAndSave(leave);
            SaveChanges();
        }

        /// <summary>
        /// 获取审批详情
        /// </summary>
        public dynamic GetApprovalDetail(string id, string currentUserId)
        {
            var leave = Get<LeaveOrder>(id);
            var leaveApprove = Read<LeaveApprove>(p => p.OrderId.Equals(id) && p.ApproverId.Equals(currentUserId)).FirstOrDefaultAsync().Result;
            if (null == leaveApprove) throw new Exception("您不可以审批!");
            var view = new LeaveView
            {
                Id = leave.Id,
                LeaveerName = Get<User>(leave.LeaveerId)?.Name,
                StartTime = leave.StartTime,
                EndOfTime = leave.EndOfTime,
                LeaveDays = leave.LeaveDays,
                LeaveType = leave.LeaveType,
                ReasonForLeave = leave.Reason,
                AttachmentsPath = leave.AttachmentsPath,
                Approves = leaveApprove.ApproveLevel == 1 ? null : Read<LeaveApprove>(p => p.OrderId.Equals(leave.Id) && p.ApproveLevel == 1 && p.Result != 0)
                .Select(p => new
                {
                    ApproverName = p.Approver.Name,
                    p.Opinion,
                    p.Result,
                    Level = p.ApproveLevel
                }).ToList()
            };
            if (Convert.ToDecimal(view.LeaveDays) <= 3)
            {
                view.IsFinal = true;
            }
            else
            {
                view.IsFinal = leaveApprove.ApproveLevel == 2;
            }
            return view;
        }

        /// <summary>
        /// 添加审批人
        /// </summary>
        /// <param name="input"></param>
        public void AddApprover(AddApproverDto input) => AddAndSave(new LeaveApprove
        {
            OrderId = input.OrderId,
            ApproveLevel = 2,
            ApproverId = input.ApproverId
        });

        /// <summary>
        /// 获取上一个审批信息
        /// </summary>
        /// <param name="leaveId"></param>
        /// <returns></returns>
        public object GetPrevApprove(string leaveId)
        {
            return Read<LeaveApprove>(p => p.OrderId.Equals(leaveId) && p.ApproveLevel == 1 && p.Result != 0).Select(p => new { Name = p.Approver.Name, p.Opinion }).FirstOrDefaultAsync().Result;
        }

        /// <summary>
        /// 学生获取请假单详情
        /// </summary>
        public ApproveDetailView GetOrderDetail(string id)
        {
            var leave = Get<LeaveOrder>(id);
            var leaveApproves = Read<LeaveApprove>(p => p.OrderId.Equals(id)).OrderBy(p => p.ApproveLevel).Select(p =>
              new LeaveApproveView
              {
                  ApproverName = p.Approver.Name,
                  Opinion = p.Opinion,
                  Result = p.Result,
                  Level = p.ApproveLevel
              }).ToListAsync().Result;

            var obj = new ApproveDetailView
            {
                Id = leave.Id,
                LeaveerName = Get<User>(leave.LeaveerId)?.Name,
                StartTime = leave.StartTime,
                EndOfTime = leave.EndOfTime,
                LeaveDays = leave.LeaveDays,
                LeaveType = leave.LeaveType,
                ReasonForLeave = leave.Reason,
                AttachmentsPath = leave.AttachmentsPath,
                Approvers = Read<LeaveApprove>(p => p.OrderId.Equals(id)).Select(p => p.Approver.Name).ToArrayAsync().Result,
                Approves = leaveApproves
            };
            if (Read<LeaveApprove>(p => p.OrderId.Equals(id) && p.Result == -1).Any())
            {
                obj.Status = "已审批";
                return obj;
            }
            if (Convert.ToDecimal(obj.LeaveDays) <= 3)
            {
                obj.Status = Read<LeaveApprove>(p => p.OrderId.Equals(id) && p.Result != 0).Any() ? "已审批" : "未审批";
                return obj;
            }
            if (Convert.ToDecimal(obj.LeaveDays) > 3)
            {
                if (Read<LeaveApprove>(p => p.OrderId.Equals(obj.Id) && p.ApproveLevel == 2).Any())
                {
                    obj.Status = Read<LeaveApprove>(p => p.OrderId.Equals(id) && p.Result != 0 && p.ApproveLevel == 2).Any() ? "已审批" : "未审批";
                }
                else
                {
                    obj.Status = Read<LeaveApprove>(p => p.OrderId.Equals(id) && p.Result != 0).Any() ? "已审批" : "未审批";
                }
                return obj;
            }
            return obj;
        }

        /// <summary>
        /// 批量审批
        /// </summary>
        public void OneKeyApproval(OneKeyApprovalDto input)
        {
            if (!input.Orders.Any()) throw new Exception("批量审批,请假id不能为空!");
            var approves = Query<LeaveApprove>(p => input.Orders.Contains(p.OrderId) && p.ApproverId.Equals(input.CurrentUserId) && p.Result == 0).ToListAsync().Result;
            approves.ForEach(item =>
            {
                item.Result = input.IsAgreed ? 1 : -1;
                item.Opinion = input.Opinion;
            });
            SaveChanges();

            Query<LeaveOrder>(p => input.Orders.Contains(p.Id)).ToListAsync().Result
            .ForEach(item =>
            {
                SetOrderStatus(item);
                if (!input.IsAgreed) ResetLimit(item.LeaveerId, item.LeaveDays);
            });
            SaveChanges();
        }

        /// <summary>
        /// 请假审批
        /// </summary>
        public void Approval(LeaveApprovalDto input)
        {
            var leave = Get<LeaveOrder>(input.OrderId);
            if (null == leave) throw new Exception("未找到请假申请信息!");
            var leaveApprove = Query<LeaveApprove>(p => p.OrderId.Equals(leave.Id) && p.ApproverId.Equals(input.CurrentUserId)).FirstOrDefaultAsync().Result;
            if (null == leaveApprove) throw new Exception("您不可以审批,请假单异常!");
            if (leaveApprove.Result != 0) throw new Exception("您已经审批过,不需要重复审批!");
            leaveApprove.Result = input.IsAgreed ? 1 : -1;
            leaveApprove.Opinion = input.Opinion;
            SetOrderStatus(leave);
            if (!input.IsAgreed) ResetLimit(leave.LeaveerId, leave.LeaveDays);
            SaveChanges();


        }

        /// <summary>
        /// 学生获取请假列表
        /// </summary>
        /// <param name="status"> -1:所有的  1:已审批  0:未审批 </param>
        public dynamic GetLeaveHistory(GetLeaveHistoryDto input)
        {
            var query = Read<LeaveOrder>(p => p.LeaveerId.Equals(input.UserId));
            query = string.IsNullOrEmpty(input.Status) || input.Status.Equals("-1") ? query : query.Where(p => p.Status.Equals(input.Status));
            query = string.IsNullOrEmpty(input.Keyword) ? query : query.Where(p => p.Reason.Contains(input.Keyword));
            query = query.Paging(input);
            var list = query.Join(Read<User>(), a => a.LeaveerId, a => a.Id, (a, b) => new LeaveListView
            {
                Id = a.Id,
                LeaveerName = b.Name,
                StartTime = a.StartTime,
                EndOfTime = a.EndOfTime,
                LeaveDays = a.LeaveDays,
                LeaveType = a.LeaveType,
                ApprovalStatus = a.Status,
                ReasonForLeave = a.Reason,
                CreatedTime = a.CreatedTime
            }).OrderByDescending(p => p.CreatedTime).ToListAsync().Result;
            var now = DateTime.Now.Date;
            SetViewStatus(ref list);
            list.ForEach(item =>
            {
                var canceled = Read<LeaveSuspend>(p => p.OrderId.Equals(item.Id)).Any();
                item.Canceled = canceled;
                item.Cancelable = item.ApprovalStatus == "2" && item.EndOfTime.Date > now && !canceled;
            });
            return list;
        }

        /// <summary>
        /// 获取审批列表
        /// </summary>
        public dynamic GetApprovalList(GetApprovalListDto input)
        {
            var leaveIds = Read<LeaveApprove>(p => p.ApproverId.Equals(input.CurrentUserId)).Select(p => p.OrderId).ToListAsync().Result;
            var joinQuery = Read<LeaveOrder>().Join(Read<LeaveApprove>(), o => o.Id, a => a.OrderId, (o, a) => new { o, a })
                .Where(p => leaveIds.Contains(p.o.Id) && p.a.ApproverId.Contains(input.CurrentUserId));
            joinQuery = input.Status.Equals("0")
                ? joinQuery.Where(p => p.a.Result == 0)
                : joinQuery.Where(p => p.a.Result != 0);
            var query = joinQuery.Select(p => p.o);
            query = string.IsNullOrEmpty(input.Keyword)
                 ? query
                 : query.Where(p => p.Reason.Contains(input.Keyword));
            query = query.Paging(input);
            var list = query.Join(Read<User>(), a => a.LeaveerId, b => b.Id, (a, b) => new { a, b }).Select(p => new LeaveListView
            {
                Id = p.a.Id,
                LeaveerName = p.b.Name,
                StartTime = p.a.StartTime,
                EndOfTime = p.a.EndOfTime,
                LeaveDays = p.a.LeaveDays,
                LeaveType = p.a.LeaveType,
                ApprovalStatus = p.a.Status,
                ReasonForLeave = p.a.Reason,
                CreatedTime = p.a.CreatedTime
            }).OrderByDescending(p => p.CreatedTime).ToListAsync().Result;
            SetViewStatus(input.CurrentUserId, ref list);
            list.ForEach(item => item.Canceled = Read<LeaveSuspend>(p => p.OrderId.Equals(item.Id)).Any());
            return list;
        }

        /// <summary>
        /// 获取终审人列表
        /// </summary>
        public object GetFinalJudgeList(string search)
        {
            var query = Read<User>(p => p.DutyId.Contains("teacher"));
            query = string.IsNullOrEmpty(search) ? query : query.Where(p => p.Name.Contains(search));
            return query.Select(p => new { Id = p.Id, Text = p.Name }).ToListAsync().Result;
        }

        /// <summary>
        /// 不计考勤请假申请
        /// </summary>
        public void SpecialApply(BulkLeaveDto input, string currentUserId)
        {
            var list = new List<LeaveOrder>();
            var leaveers = input.Leaveers.ToList().Distinct().ToArray();
            foreach (var item in leaveers)
            {
                list.Add(new LeaveOrder
                {
                    StartTime = input.StartTime,
                    EndOfTime = input.EndOfTime,
                    LeaveerId = item,
                    LeaveType = "9",
                    Status = "99",
                    Reason = input.ReasonForLeave,
                    HeadTeacherId = currentUserId,
                    CreatedTime = DateTime.Now
                });
            }
            AddRange<LeaveOrder>(list);
            SaveChanges();
        }

        /// <summary>
        /// 获取不计考勤请假列表
        /// </summary>
        public (List<LeaveListView> list, int recordCount, int pageCount) GetSpecialList(GetSpecialLeaveDto input)
        {
            var query = Read<LeaveOrder>(p => p.HeadTeacherId.Equals(input.CurrentUserId) && p.LeaveType == "9" && p.Status == "99");
            query = string.IsNullOrEmpty(input.Keyword) ? query : query.Where(p => p.Reason.Contains(input.Keyword));
            var recordCount = query.CountAsync().Result;
            var pageCount = recordCount % input.Rows == 0 ? recordCount / input.Rows : (recordCount / input.Rows) + 1;
            query = query.OrderByDescending(p => p.CreatedTime).Skip(input.Page - 1).Take(input.Rows);
            var list = query.Join(Read<User>(), a => a.LeaveerId, b => b.Id, (a, b) => new { a, b }).Select(p => new LeaveListView
            {
                Id = p.a.Id,
                LeaveerName = p.b.Name,
                StartTime = p.a.StartTime,
                EndOfTime = p.a.EndOfTime,
                LeaveDays = p.a.LeaveDays,
                LeaveType = p.a.LeaveType,
                ReasonForLeave = p.a.Reason,
            }).ToListAsync().Result;
            return (list, recordCount, pageCount);
        }


        /// <summary>
        /// 获取机构学生,此处获取的是当前机构和下级机构的学生
        /// </summary>
        /// <param name="orgId">机构Id</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        public object GetStuByOrg(string orgId, string keyword)
        {
            var query = Read<Student>();
            if (string.IsNullOrEmpty(orgId)) return null;
            var orgs = Read<Org>(p => p.ParentId.Equals(orgId)).Select(p => p.Id).ToListAsync().Result;
            orgs?.Add(orgId);
            query = query.Where(p => orgs.Contains(p.ClassId));
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.Name.Contains(keyword));
            return query.Select(p => new { UserId = p.UserId, Name = p.Name }).ToListAsync().Result;
        }

        /// <summary>
        /// 获取老师信息
        /// </summary>
        /// <returns></returns>
        public List<ContactGroupView> GetTeachers()
        {
            return Read<Teacher>().Select(p => new ContactView { Id = p.UserId, Name = p.Name }).ToListAsync().Result.GroupBy(p => p.GroupName).Select(g => new ContactGroupView
            {
                GroupName = g.Key,
                Items = g.Where(s => s.GroupName.Equals(g.Key)).ToList()
            }).OrderBy(p => p.GroupName).ToList();
        }

        /// <summary>
        /// 销假
        /// </summary>
        /// <param name="orderId"></param>
        public void SuspendLeave(string orderId)
        {
            var currentDate = DateTime.Now.Date;
            var order = Read<LeaveOrder>(p => p.Id.Equals(orderId)).FirstOrDefault();
            if (order.EndOfTime.Date <= currentDate) throw new Exception("Holiday deadline must be after today!");
            var suspendLeave = new LeaveSuspend
            {
                OrderId = order.Id,
                Days = (order.EndOfTime.Date - currentDate).Days
            };
            Add(suspendLeave);
            ResetLimit(order.LeaveerId, suspendLeave.Days);
            SaveChanges();
        }
    }


}