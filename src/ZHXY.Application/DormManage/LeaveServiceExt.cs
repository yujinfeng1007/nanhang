using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Common;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    /// <summary>
    /// 请假服务私有成员
    /// </summary>
    public partial class LeaveService
    {
        /// <summary>
        /// 审批不通过时减去学生假期使用天数
        /// </summary>
        private void ResetLimit(string studentId, decimal? subtractedDays)
        {
            var currentSemesterId = GetCurrentSemesterId();
            var limit = Query<LeaveLimit>(p => p.StudentId.Equals(studentId) && p.SemesterId.Equals(currentSemesterId)).FirstOrDefault();
            limit.UsedDays -= subtractedDays.Value;
        }

        /// <summary>
        /// 检查可不可以申请
        /// </summary>
        private void CheckCanRequest(LeaveRequestDto input)
        {
            var leaveDays = Convert.ToDecimal(input.LeaveDays);
            if (leaveDays > 15) throw new Exception("请假天数不能大于15天!");
            if (!input.Approvers.Any()) throw new Exception("请先选择审批人!");
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

        private void SetViewStatus(ref List<LeaveListView> list)
        {

            foreach (var item in list)
            {
                if (Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result == -1).Any())
                {
                    item.ApprovalStatus = "3";
                    continue;
                }

                if (item.LeaveDays <= 3)
                {
                    var approve = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result != 0).FirstOrDefault();
                    item.Approver = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result == 0 && p.ApproveLevel == 1).Select(p => p.Approver.Name).ToArray();
                    if (null == approve)
                    {
                        item.ApprovalStatus = "1";
                        continue;
                    }
                    item.ApprovalStatus = approve.Result > 0 ? "2" : "3";
                    continue;
                }
                if (item.LeaveDays > 3)
                {
                    if (Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.ApproveLevel == 2).Any())
                    {
                        item.Approver = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result == 0 && p.ApproveLevel == 2).Select(p => p.Approver.Name).ToArray();
                        var approve = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.ApproveLevel == 2 && p.Result != 0).FirstOrDefault();
                        if (null == approve)
                        {
                            item.ApprovalStatus = "1";
                        }
                        else
                        {
                            item.ApprovalStatus = approve.Result > 0 ? "2" : "3";
                        }
                    }
                    else
                    {
                        item.Approver = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.Result == 0 && p.ApproveLevel == 1).Select(p => p.Approver.Name).ToArray();
                        var approve = Read<LeaveApprove>(p => p.OrderId.Equals(item.Id) && p.ApproveLevel == 1&& p.Result != 0).FirstOrDefault();
                        if (null == approve)
                        {
                            item.ApprovalStatus = "1";
                        }
                        else
                        {
                            item.ApprovalStatus = approve.Result > 0 ? "1" : "3";
                        }
                    }
                    continue;
                }
            }
        }

        /// <summary>
        /// 设置请假单状态
        /// </summary>
        /// <param name="leave"></param>
        private void SetOrderStatus(LeaveOrder order)
        {
            if (order.LeaveDays <= 3)
            {
                order.Status = "1";
                return;
            }
            var currentUserId = Operator.GetCurrent().Id;
            var approve = Read<LeaveApprove>(p => p.OrderId.Equals(order.Id) && p.ApproverId.Equals(currentUserId)).FirstOrDefaultAsync().Result;
            //if (approve.ApproveLevel == 2)  多级审批，只要有一个人审批通过，就算通过 
            order.Status = "1";
        }

    }


}