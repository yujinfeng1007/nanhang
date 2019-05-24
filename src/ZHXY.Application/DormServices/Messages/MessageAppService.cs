using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 报表服务
    /// </summary>
    public class MessageAppService : AppService
    {
        public MessageAppService(IZhxyRepository r) : base(r) { }

        /// <summary>
        /// 设置机构消息接收人
        /// </summary>
        public void SetOrgMessageRecipient(SetOrgHeadDto input)
        {

        }

        /// <summary>
        /// 查询晚归报表接口
        /// </summary>
        /// <param name="OrgId"></param>
        /// <param name="ReportDate"></param>
        /// <returns></returns>
        public object GetLateReturnReport(string OrgId, string ReportDate)
        {
            var ReportTime = Convert.ToDateTime(ReportDate);
            List<string> OrgList = new List<string> { OrgId };
            this.GetChildOrg(OrgId, OrgList);
            var LateReturnList = Read<LateReturnReport>(p => ReportTime == p.CreatedTime && OrgList.Contains(p.Class)).ToList();
            return LateReturnList.ToJson();
        }

        /// <summary>
        /// 查询未归报表接口
        /// </summary>
        /// <param name="OrgId"></param>
        /// <param name="ReportDate"></param>
        /// <returns></returns>
        public object GetNotReturnReport(string OrgId, string ReportDate)
        {
            var ReportTime = Convert.ToDateTime(ReportDate);
            List<string> OrgList = new List<string> { OrgId };
            this.GetChildOrg(OrgId, OrgList);
            var NoReturnList = Read<NoReturnReport>(p => p.CreatedTime == ReportTime && OrgList.Contains(p.ClassId)).ToList();
            return NoReturnList.ToJson();
        }

        /// <summary>
        /// 查询长时间未出的报表接口
        /// </summary>
        /// <param name="OrgId"></param>
        /// <param name="ReportDate"></param>
        /// <returns></returns>
        public object GetNotOutReport(string OrgId, string ReportDate)
        {
            var ReportTime = Convert.ToDateTime(ReportDate);
            List<string> OrgList = new List<string> { OrgId };
            this.GetChildOrg(OrgId, OrgList);
            var NoOutList = Read<NoOutReport>(p => p.CreatedTime == ReportTime && OrgList.Contains(p.ClassId)).ToList();
            return NoOutList.ToJson();
        }
    }
}