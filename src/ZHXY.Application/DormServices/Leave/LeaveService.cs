using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 请假服务
    /// <author>yujinfeng</author>
    /// </summary>
    public class LeaveService : AppService
    {
        public LeaveService(IZhxyRepository r) : base(r) { }

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
                Add(new LeaveApprove
                {
                    ApproverId = item,
                    OrderId = leave.Id,
                    ApproveLevel = 1
                });
            }
            Add(leave);
            SaveChanges();
        }

        /// <summary>
        /// 审批不通过时减去学生假期使用天数
        /// </summary>
        private void MinusLimit(string studentId, decimal days)
        {
            var currentSemesterId = GetCurrentSemesterId();
            var limit = Query<LeaveLimit>(p => p.StudentId.Equals(studentId) && p.SemesterId.Equals(currentSemesterId)).FirstOrDefault();
            limit.UsedDays -= days;
        }

        /// <summary>
        /// 检查可不可以申请
        /// </summary>
        private void CheckCanRequest(LeaveRequestDto input)
        {
            var leaveDays = Convert.ToDecimal(input.LeaveDays);
            if (leaveDays > 15) throw new Exception("请假天数不能大于15天!");
            if (!input.Approvers.Any()) throw new Exception("请先选择审批人!");
            if (input.Approvers.Length < 2) throw new Exception("必须选择班主任和辅导员,缺一不可!");
            var currentSemesterId = GetCurrentSemesterId();
            if (string.IsNullOrEmpty(currentSemesterId)) throw new Exception("当前学期未设置,请联系管理员!");
            var limit = Query<LeaveLimit>(p => p.SemesterId.Equals(currentSemesterId) && p.StudentId.Equals(input.LeaveerId)).FirstOrDefaultAsync().Result;
            if (null == limit)
            {
                AddAndSave(new LeaveLimit
                {
                    SemesterId = currentSemesterId,
                    StudentId = input.LeaveerId,
                    UsedDays = leaveDays
                });
                return;
            }
            if ((limit.UsedDays + leaveDays) > 15)
            {
                throw new Exception($"您剩余的请假天数不足,请重新设置请假天数,当前剩余天数: {15 - limit.UsedDays}");
            }
            limit.UsedDays += leaveDays;
            SaveChanges();
        }

        /// <summary>
        /// 获取当前学期Id
        /// </summary>
        /// <returns></returns>
        private string GetCurrentSemesterId()
        {
            var now = DateTime.Now.Date;
            return Read<Semester>(p => p.StartTime <= now && p.EndOfTime >= now).Select(p => p.Id).FirstOrDefaultAsync().Result;
        }

        /// <summary>
        /// 获取审批详情
        /// </summary>
        public LeaveView GetApprovalDetail(string id, string currentUserId)
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
                AttachmentsPath = leave.AttachmentsPath
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
        public ApproveDetailView GetRequestDetail(string id)
        {
            var leave = Get<LeaveOrder>(id);
            var leaveApproves = Read<LeaveApprove>(p => p.OrderId.Equals(id) && p.Result != 0).Select(p => new LeaveApproveView
            {
                ApproverName = p.Approver.Name,
                Opinion = p.Opinion,
                Result = p.Result > 0 ? "同意" : "不同意"
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
                if (!input.IsAgreed) MinusLimit(item.LeaveerId, Convert.ToDecimal(item.LeaveDays));
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
            if (!input.IsAgreed) MinusLimit(leave.LeaveerId, Convert.ToDecimal(leave.LeaveDays));
            SaveChanges();


        }

        /// <summary>
        /// 获取请假列表
        /// </summary>
        /// <param name="status"> -1:所有的  1:已审批  0:未审批 </param>
        public dynamic GetLeaveHistory(GetLeaveHistoryDto input)
        {
            var query = Read<LeaveOrder>(p => p.LeaveerId.Equals(input.UserId));
            query = string.IsNullOrEmpty(input.Status) ? query : query.Where(p => p.Status.Equals(input.Status));
            query = string.IsNullOrEmpty(input.Keyword) ? query : query.Where(p => p.Reason.Contains(input.Keyword));
            query=query.Paging(input);
            var list = query.Join(Read<User>(),a=>a.LeaveerId,a=>a.Id,(a,b)=>new { a,b}).Select(j => new LeaveListView
            {
                Id = j.a.Id,
                LeaveerName = j.b.Name,
                StartTime = j.a.StartTime,
                EndOfTime = j.a.EndOfTime,
                LeaveDays = j.a.LeaveDays,
                LeaveType = j.a.LeaveType,
                ApprovalStatus = j.a.Status,
                ReasonForLeave = j.a.Reason,
                CreatedTime = j.a.CreatedTime
            }).ToListAsync().Result;

            foreach (var item in list)
            {
                if (Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result == -1).Any())
                {
                    item.ApprovalStatus = "已审批";
                    continue;
                }
                if (Convert.ToDecimal(item.LeaveDays) <= 3)
                {
                    item.ApprovalStatus = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result != 0).Any() ? "已审批" : "未审批";
                    continue;
                }
                if (Convert.ToDecimal(item.LeaveDays) > 3)
                {
                    if (Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.ApproveLevel == 2).Any())
                    {
                        item.ApprovalStatus = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result != 0 && p.ApproveLevel == 2).Any() ? "已审批" : "未审批";
                    }
                    else
                    {
                        item.ApprovalStatus = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result != 0).Any() ? "已审批" : "未审批";
                    }
                    continue;
                }
            }
            return list;
        }

        /// <summary>
        /// 获取审批列表
        /// </summary>
        public dynamic GetApprovalList(GetApprovalListDto input)
        {
            var leaveIds = Read<LeaveApprove>(p => p.ApproverId.Equals(input.CurrentUserId)).Select(p => p.OrderId).ToListAsync().Result;
            var query = Read<LeaveOrder>(p => leaveIds.Contains(p.Id));
            query = input.SearchPattern == 0
                ? query
                : query.Where(p => p.Status.Equals(input.SearchPattern));

            query = string.IsNullOrEmpty(input.Keyword)
                ? query
                : query.Where(p => p.Reason.Contains(input.Keyword));
            query=query.Paging(input);
            var list = query.Join(Read<User>(),a=>a.LeaveerId,b=>b.Id,(a,b)=> new{a,b }).Select(p => new LeaveListView
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
            }).ToListAsync().Result;
            SetViewStatus(input.CurrentUserId, ref list);
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
        /// 改变显示状态
        /// </summary>
        /// <param name="list"></param>
        private void SetViewStatus(string currentUserId, ref List<LeaveListView> list)
        {
            list.ForEach(item =>
            {
                var approve = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.ApproverId.Equals(currentUserId)).FirstOrDefault();
                var otherApprove = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.ApproveLevel.Equals(approve.ApproveLevel) && !p.ApproverId.Equals(currentUserId)).FirstOrDefault();
                var result = null == otherApprove ? approve.Result : approve.Result != 0 ? approve.Result : otherApprove.Result;
                switch (result)
                {
                    case 0:
                        {
                            item.ApprovalStatus = "1";
                            break;
                        }
                    case 1:
                        {
                            item.ApprovalStatus = "2";
                            break;
                        }
                    case -1:
                        {
                            item.ApprovalStatus = "3";
                            break;
                        }
                }

            });
        }

        /// <summary>
        /// 设置请假单状态
        /// </summary>
        /// <param name="leave"></param>
        private void SetOrderStatus(LeaveOrder order)
        {
            if (order.Days <= 3)
            {
                order.Status = "1";
                return;
            }
            var currentUserId = Operator.GetCurrent().Id;
            var approve = Read<LeaveApprove>(p => p.OrderId.Equals(order.Id) && p.ApproverId.Equals(currentUserId)).FirstOrDefaultAsync().Result;
            if (approve.ApproveLevel == 2) order.Status = "1";
        }

        #region 不计考勤请假

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
            Add<LeaveOrder>(list);
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
            var orgs = Read<Organ>(p => p.ParentId.Equals(orgId)).Select(p => p.Id).ToListAsync().Result;
            orgs?.Add(orgId);
            query = query.Where(p => orgs.Contains(p.ClassId));
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.Name.Contains(keyword));
            return query.Select(p => new { UserId = p.UserId, Name = p.Name }).ToListAsync().Result;
        }
        #endregion

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
        /// 获取用户可以取消的请假列表
        /// 1.一级审批人是当前用户
        /// 2.请假截至时间大于当前时间的
        /// 3.审批通过的
        /// </summary>
        public dynamic GetCanceList(GetCancelListDto input)
        {
            var passIds = Read<LeaveApprove>(p => p.Result == 1).Select(p => p.OrderId).Distinct().ToListAsync().Result;
            var rejectIds = Read<LeaveApprove>(p => p.Result == -1).Select(p => p.OrderId).Distinct().ToListAsync().Result;
            var canceledIds = Read<CancelHoliday>().Select(p => p.OrderId).Distinct().ToArrayAsync().Result;
            var ids = passIds.Except(rejectIds).Except(canceledIds);
            var orderIds = Read<LeaveApprove>(p => ids.Contains(p.OrderId) && p.ApproveLevel == 1 && p.ApproverId.Equals(input.CurrentUserId)).Select(p => p.OrderId).Distinct().ToArrayAsync().Result;
            var query = Read<LeaveOrder>(p => orderIds.Contains(p.Id))
             .Select(p => new CancellableLeaveView
             {
                 OrderId = p.Id,
                 ApplicantId = p.ApplicantId,
                 StartTime = p.StartTime,
                 EndOfTime = p.EndOfTime,
                 LeaveDays = p.LeaveDays,
                 ReasonForLeave = p.Reason,
                 LeaveType = p.LeaveType
             }).ToListAsync()
             .Result.Where(p => DateTime.Parse(p.EndOfTime).Date >= DateTime.Now.Date);
            var list= query.AsQueryable().Paging(input).ToListAsync().Result;
            list.ForEach(item => item.ApplicantName = Read<User>(p => p.Id.Equals(item.ApplicantId)).Select(p => p.Name).FirstOrDefault());
            return list;
        }

        /// <summary>
        /// 销假
        /// </summary>
        public void CancelHoliday(CancelHolidayDto input)
        {
            var cancelHoliday = input.MapTo<CancelHoliday>();
            var order = Read<LeaveOrder>(p => p.Id.Equals(input.OrderId)).FirstOrDefaultAsync().Result;
            if (null == order) throw new Exception($"未找到请假单! leaveId:{input.OrderId}");
            if (input.Days > decimal.Parse(order.LeaveDays)) throw new Exception("销假天数不能大于请假天数!");
            AddAndSave(cancelHoliday);
        }

    }
}