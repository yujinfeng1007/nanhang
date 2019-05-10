using System;

namespace ZHXY.Domain
{
    public class OriginalReport:Entity
    {
        public DateTime? Date { get; set; }
        /// <summary>
        /// 通道ID
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNum { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// 姓什么
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdNum { get; set; }
        /// <summary>
        /// 个人ID
        /// </summary>
        public string PersonId { get; set; }
        /// <summary>
        /// 人员类型
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 0-进门  1-出门
        /// </summary>
        public string InOut { get; set; }
        /// <summary>
        /// 报警类型
        /// </summary>
        public string EventType { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 门禁上报图片1
        /// </summary>
        public string Picture1 { get; set; }
        /// <summary>
        /// 门禁上报图片2
        /// </summary>
        public string Picture2 { get; set; }
        /// <summary>
        /// 门禁上报图片3
        /// </summary>
        public string Picture3 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string Code { get; set; }
    }
}
