using System;
using ZHXY.Domain;

namespace ZHXY.Assists.Entity
{
    /// <summary>
    /// 电子班牌
    /// </summary>
    public class DevicesEntity : EntityBase, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        /// <summary>
        /// ID
        /// </summary>

        public string F_Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>

        public int? F_SortCode { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>

        public string F_DepartmentId { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>

        public bool? F_DeleteMark { get; set; }

        /// <summary>
        /// 启用标记
        /// </summary>

        public bool? F_EnabledMark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime? F_CreatorTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>

        public string F_CreatorUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>

        public DateTime? F_LastModifyTime { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>

        public string F_LastModifyUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>

        public DateTime? F_DeleteTime { get; set; }

        /// <summary>
        /// 删除者
        /// </summary>

        public string F_DeleteUserId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>

        public string F_Memo { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>

        public string F_Device_Code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>

        public string F_Device_Name { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>

        public string F_Brand { get; set; }

        /// <summary>
        /// 类型
        /// </summary>

        public string F_Type { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>

        public string F_Size { get; set; }

        /// <summary>
        /// 状态（运行中、已关机、检修、停用）
        /// </summary>

        public string F_Device_Status { get; set; }

        /// <summary>
        /// 设备IP
        /// </summary>

        public string F_IP { get; set; }

        /// <summary>
        /// 设备序列号
        /// </summary>

        public string F_Sn { get; set; }

        /// <summary>
        /// 教室ID
        /// </summary>

        public string F_Classroom { get; set; }

        public virtual Classroom Classroom { get; set; }

        /// <summary>
        /// 是否启动倒计时
        /// </summary>

        public string F_IfCountdown { get; set; }

        /// <summary>
        /// 倒计时标题
        /// </summary>

        public string F_Countdown_Title { get; set; }

        /// <summary>
        /// 倒计时开始时间
        /// </summary>

        public DateTime? F_Countdown_StartTime { get; set; }

        /// <summary>
        /// 倒计时结束时间
        /// </summary>

        public DateTime? F_Countdown_EndTime { get; set; }

        /// <summary>
        /// 显示模式（横屏/竖屏）
        /// </summary>

        public string F_Display_Style { get; set; }

        /// <summary>
        /// 风格模式
        /// </summary>

        public string F_Style { get; set; }

        /// <summary>
        /// 安装地点
        /// </summary>

        public string F_Address { get; set; }

        /// <summary>
        /// 班级id
        /// </summary>

        public string F_Class { get; set; }

        ///// <summary>
        ///// 班级信息
        ///// </summary>

        //public string F_Class_Info { get; set; }

        /// <summary>
        /// 定时开关机规则（周一、周二。。。）
        /// </summary>

        public string F_Switch_Rules { get; set; }

        /// <summary>
        /// 首页栏目（风采图片 视频 新闻 活动）
        /// </summary>

        public string F_HomePage_Channel { get; set; }

        /// <summary>
        /// 系统版本号
        /// </summary>

        public string F_SystemNo { get; set; }

        /// <summary>
        /// apk版本号
        /// </summary>

        public string F_ApkNo { get; set; }

        /// <summary>
        /// 皮肤
        /// </summary>
        public string F_Skin_ID { get; set; }

        /// <summary>
        /// 开机时间
        /// </summary>
        public TimeSpan F_Start { get; set; }

        /// <summary>
        /// 关机时间
        /// </summary>
        public TimeSpan F_End { get; set; }

        /// <summary>
        /// 开关机设置周期
        /// </summary>
        public string F_Week { get; set; }

        /// <summary>
        /// 宣传模式开始时间
        /// </summary>
        public DateTime? F_StartTime { get; set; }

        /// <summary>
        /// 宣传模式截止时间
        /// </summary>
        public DateTime? F_EndTime { get; set; }

        /// <summary>
        /// 宣传标题
        /// </summary>
        public string F_Title { get; set; }

        /// <summary>
        /// 宣传模式显示方式(图片/视频）
        /// </summary>
        public string F_Display_Type { get; set; }

        /// <summary>
        /// 宣传模式文件集合
        /// </summary>
        public string F_Files { get; set; }

        /// <summary>
        /// 班牌类型
        /// </summary>
        public string F_BrandType { get; set; }

        /// <summary>
        /// Mac
        /// </summary>
        public string F_TeaMac { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        public string F_TeaIp { get; set; }
    }
}