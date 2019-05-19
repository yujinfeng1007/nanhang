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

        public object GetLateReturnReport(string OrgId)
        {
            DateTime StartTime = DateTime.Now.Date.AddHours(8);
            List<string> OrgList = new List<string> { OrgId };
            this.GetChildOrg(OrgId, OrgList);
            var LateReturnList = Read<LateReturnReport>(p => p.CreatedTime > StartTime && OrgList.Contains(p.Class)).ToList();
            return LateReturnList.ToJson();
        }

        public object GetNotReturnReport(string OrgId)
        {
            DateTime StartTime = DateTime.Now.Date.AddHours(8);
            List<string> OrgList = new List<string> { OrgId };
            this.GetChildOrg(OrgId, OrgList);
            var NoReturnList = Read<NoReturnReport>(p => p.CreatedTime > StartTime && OrgList.Contains(p.ClassId)).ToList();
            return NoReturnList.ToJson();
        }

        public object GetNotOutReport(string OrgId)
        {
            DateTime StartTime = DateTime.Now.Date.AddHours(8);
            List<string> OrgList = new List<string> { OrgId };
            this.GetChildOrg(OrgId, OrgList);
            var NoOutList = Read<NoOutReport>(p => p.CreatedTime > StartTime && OrgList.Contains(p.ClassId)).ToList();
            return NoOutList.ToJson();
        }
    }
}