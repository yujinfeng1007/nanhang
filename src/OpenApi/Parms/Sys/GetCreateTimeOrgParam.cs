using System;

namespace OpenApi.Parms.Sys.Parms
{
    public class GetCreateTimeOrgParam
    {
        public string F_APPKEY { get; set; }
        public string F_SESSIONKEY { get; set; }
        public string F_School_Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}