using System;
using System.ComponentModel.DataAnnotations;

namespace TaskApi.DH
{

    public partial class LineRecordMoudle
    {
        [StringLength(50)]
        public string id { get; set; } = null;

        public DateTime? date { get; set; } = null;

        [StringLength(50)]
        public string channelCode { get; set; } = null;

        [StringLength(50)]
        public string channelName { get; set; } = null;

        [StringLength(50)]
        public string departmentCode { get; set; } = null;

        [StringLength(50)]
        public string departmentName { get; set; } = null;

        [StringLength(50)]
        public string cardNum { get; set; } = null;

        [StringLength(50)]
        public string firstName { get; set; } = null;

        [StringLength(50)]
        public string lastName { get; set; } = null;

        [StringLength(50)]
        public string tel { get; set; } = null;

        [StringLength(1)]
        public string gender { get; set; } = null;

        [StringLength(50)]
        public string idNum { get; set; } = null;

        [StringLength(50)]
        public string personId { get; set; } = null;

        [StringLength(1)]
        public string cardType { get; set; } = null;

        [StringLength(1)]
        public string inOut { get; set; } = null;

        [StringLength(50)]
        public string eventType { get; set; } = null;

        [StringLength(1)]
        public string deviceType { get; set; } = null;

        [StringLength(50)]
        public string swipDate { get; set; } = null;

        [StringLength(50)]
        public string picture1 { get; set; } = null;

        public string picture2 { get; set; } = null;

        public string picture3 { get; set; } = null;

        public string picture4 { get; set; } = null;

        public string memo { get; set; } = null;

        public string alarmCode { get; set; } = null;

        public string pictureUrl { get; set; } = null;

        [StringLength(50)]
        public string code { get; set; } = null;
    }
}
