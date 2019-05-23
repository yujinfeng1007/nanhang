﻿using ZHXY.Domain;
using System;
using ZHXY.Common;
using System.Linq;
using System.Collections.Generic;

namespace ZHXY.Application
{

    /// <summary>
    /// 报表服务
    /// </summary>
    public class ReportAppService : AppService
    {
        public ReportAppService(IZhxyRepository r) : base(r) { }

        /// <summary>
        /// 获取异常出入报表
        /// </summary>
        public void GetAbnormalAccess(GetAbnormalAccessDto dto)
        {
            




        }

        /// <summary>
        /// 首页-数据面板-图表
        /// </summary>
        public dynamic GetDefaultData()
        {
            // 考勤总人数
            var totalQty = Read<DormStudent>().Count();

            // 在寝人数
            var noOutQty = Read<NoOutReport>().Count();

            // 外出人数
            var noReturnQty = Read<NoReturnReport>().Count();

            // 请假人数
            var leaveQty = Read<LeaveOrder>().Count();

            // 已签到人数
            var signedQty = Read<DormStudent>().Count();

            // 晚归人数
            var laterReturnQty = Read<LateReturnReport>().Count();

            // 请假待审批记录
            var leaveList = Read<LeaveOrder>(t => t.Status == "0").ToList();


            return new
            {
                TotalQty = totalQty,
                NoOutQty = noOutQty,
                InQty = noOutQty,
                OutQty = noReturnQty,
                LeaveQty = leaveQty,
                LeftPieChart = new List<ChartsDataItemDto> {
                    new ChartsDataItemDto { Name="已签到",Value=signedQty},
                    new ChartsDataItemDto { Name="请假",Value=leaveQty},
                },
                RightPieChartData = new List<ChartsDataItemDto>
                { new ChartsDataItemDto { Name="其他异常",Value=signedQty},
                    new ChartsDataItemDto { Name="晚归",Value=laterReturnQty},
                    new ChartsDataItemDto { Name="未归",Value=noReturnQty},
                    new ChartsDataItemDto { Name="未出",Value=noOutQty},
                },
                LeavesData =leaveList
            };

        }
    }

    public class ChartsDataItemDto
    {
        public string Name { get; set; }

        public decimal Value { get; set; }
    }

    /// <summary>
    /// 获取异常出入统计
    /// </summary>
    public class GetAbnormalAccessDto 
    {
        /// <summary>
        /// 0:自定义时间  1:本周 2:本月  
        /// </summary>
        public int Pattern { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string OrgId { get; set; }
        public DateTime StartTime
        {
            get
            {
                var d = DateTime.MinValue;
                switch (Pattern)
                {
                    case -1: DateTime.TryParse(Start, out d); break;
                    case 0: d = DateHelper.GetStartTimeOfWeek(); break;
                    case 1: d = DateHelper.GetStartTimeOfMonth(); break;
                }
                return d;
            }
        }
        public DateTime EndOfTime
        {
            get
            {
                var d = DateTime.Now;
                switch (Pattern)
                {
                    case -1: d = DateTime.TryParse(End, out d) ? d.AddDays(1).AddSeconds(-1) : DateTime.Now.Date.AddDays(1).AddSeconds(-1); break;
                    case 0: d = DateHelper.GetEndTimeOfWeek(); break;
                    case 1: d = DateHelper.GetEndTimeOfMonth(); break;
                }
                return d;
            }

        }
    }


}