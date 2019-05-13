using System;
using System.ComponentModel.DataAnnotations;

namespace ZHXY.Domain
{
    /// <summary>
    /// 老师信息
    /// </summary>
    public class Teacher : IEntity
    {

        /// <summary>
        /// 教师ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        public virtual User teacherSysUser { get; set; }

        /// <summary>
        /// 隶属学部
        /// </summary>
        public string F_Divis_ID { get; set; }

        /// <summary>
        /// 教师姓名
        /// </summary>
        public string F_Name { get; set; }

        /// <summary>
        /// 曾用名
        /// </summary>
        [Required]
        [Display(Name = "曾用名")]
        public string F_Name_Old { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        [Display(Name = "性别")]
        public string F_Gender { get; set; }

        /// <summary>
        /// 教师工号
        /// </summary>
        [Required]
        [Display(Name = "教师工号")]
        public string F_Num { get; set; }

        /// <summary>
        /// 国籍/地区
        /// </summary>
        [Required]
        [Display(Name = "国籍/地区")]
        public string F_Nation { get; set; }

        /// <summary>
        /// 电子照片
        /// </summary>
        [Required]
        [Display(Name = "电子照片")]
        public string F_FacePhoto { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        [Required]
        [Display(Name = "证件类型")]
        public string F_CredType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        [Required]
        [Display(Name = "证件号码")]
        public string F_CredNum { get; set; }

        /// <summary>
        /// 证件正面照片
        /// </summary>
        [Required]
        [Display(Name = "证件正面照片")]
        public string F_CredPhoto_Obve { get; set; }

        /// <summary>
        /// 证件反面照片
        /// </summary>
        [Required]
        [Display(Name = "证件反面照片")]
        public string F_CredPhoto_Rever { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Required]
        [Display(Name = "出生日期")]
        public DateTime? F_Birthday { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        [Required]
        [Display(Name = "籍贯")]
        public string F_Native { get; set; }

        /// <summary>
        /// 出生地
        /// </summary>
        [Required]
        [Display(Name = "出生地")]
        public string F_RegAddr { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        [Required]
        [Display(Name = "民族")]
        public string F_Volk { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        [Required]
        [Display(Name = "政治面貌")]
        public string F_PolitStatu { get; set; }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        [Required]
        [Display(Name = "婚姻状况")]
        public string F_Marrige { get; set; }

        /// <summary>
        /// 健康状况
        /// </summary>
        [Required]
        [Display(Name = "健康状况")]
        public string F_Health { get; set; }

        /// <summary>
        /// 参加工作时间
        /// </summary>
        [Required]
        [Display(Name = "参加工作时间")]
        public string F_InWork_Date { get; set; }

        /// <summary>
        /// 进校时间
        /// </summary>
        [Required]
        [Display(Name = "进校时间")]
        public DateTime? F_EntryTime { get; set; }

        /// <summary>
        /// 教职工来源
        /// </summary>
        [Required]
        [Display(Name = "教职工来源")]
        public string F_Source_Teacher { get; set; }

        /// <summary>
        /// 教职工类别
        /// </summary>
        [Required]
        [Display(Name = "教职工类别")]
        public string F_Type_Teacher { get; set; }

        /// <summary>
        /// 是否在编
        /// </summary>
        [Required]
        [Display(Name = "是否在编")]
        public string F_Payroll { get; set; }

        /// <summary>
        /// 用人形式
        /// </summary>
        [Required]
        [Display(Name = "用人形式")]
        public string F_YRXS { get; set; }

        /// <summary>
        /// 签订合同情况
        /// </summary>
        [Required]
        [Display(Name = "签订合同情况")]
        public string F_If_Contract { get; set; }

        /// <summary>
        /// 科别
        /// </summary>
        [Required]
        [Display(Name = "科别")]
        public string F_Profession { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        [Required]
        [Display(Name = "毕业院校")]
        public string F_Academy { get; set; }

        /// <summary>
        /// 最高学历
        /// </summary>
        [Required]
        [Display(Name = "最高学历")]
        public string F_Education { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        [Required]
        [Display(Name = "职务")]
        public string F_Duties { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [Required]
        [Display(Name = "职称")]
        public string F_Titles { get; set; }

        /// <summary>
        /// 有特殊教育从业证书
        /// </summary>
        [Required]
        [Display(Name = "有特殊教育从业证书")]
        public string F_TSJYCYZS { get; set; }

        /// <summary>
        /// 信息技术应用能力
        /// </summary>
        [Required]
        [Display(Name = "信息技术应用能力")]
        public string F_XXJSYY { get; set; }

        /// <summary>
        /// 属于公费师范生
        /// </summary>
        [Required]
        [Display(Name = "属于公费师范生")]
        public string F_GFSFS { get; set; }

        /// <summary>
        /// 参加基础服务项目
        /// </summary>
        [Required]
        [Display(Name = "参加基础服务项目")]
        public string F_JCFW { get; set; }

        /// <summary>
        /// 参加基础服务项目起始时间
        /// </summary>
        [Required]
        [Display(Name = "参加基础服务项目起始时间")]
        public string F_JCFW_Start { get; set; }

        /// <summary>
        /// 参加基础服务项目截至时间
        /// </summary>
        [Required]
        [Display(Name = "参加基础服务项目截至时间")]
        public string F_JCFW_End { get; set; }

        /// <summary>
        /// 是否特级教师
        /// </summary>
        public string F_If_Sp { get; set; }

        /// <summary>
        /// 是否县级以上骨干教师
        /// </summary>
        public string F_If_Gg { get; set; }

        /// <summary>
        /// 人员状态
        /// </summary>
        public string F_Status { get; set; }


        /// <summary>
        ///
        /// </summary>
        public string F_Duty { get; set; }

        public string F_MobilePhone { get; set; }


        public string F_RoleId { get; set; }

        public string F_Email { get; set; }

        /// <summary>
        /// F_Duties_RMSJ
        /// </summary>
        public DateTime? F_Duties_RMSJ { get; set; }

        /// <summary>
        /// 工龄
        /// </summary>
        public string F_GL { get; set; }

        /// <summary>
        /// 校龄
        /// </summary>
        public string F_XL { get; set; }

        /// <summary>
        /// 第一学历
        /// </summary>
        public string F_DYXL { get; set; }

        /// <summary>
        /// 第一学历学位
        /// </summary>
        public string F_DYXLXW { get; set; }

        /// <summary>
        /// 第一学历毕业院校
        /// </summary>
        public string F_DYBYYX { get; set; }

        /// <summary>
        /// 第一学历专业
        /// </summary>
        public string F_DYZY { get; set; }

        /// <summary>
        /// 最高学历学位
        /// </summary>
        public string F_ZGXW { get; set; }

        /// <summary>
        /// 最高学历取得方式
        /// </summary>
        public string F_ZGSJ { get; set; }

        /// <summary>
        /// 职称取得时间
        /// </summary>
        public DateTime? F_ZCSJ { get; set; }

        /// <summary>
        /// 校内职称及档级
        /// </summary>
        public string F_XNDJ { get; set; }

        /// <summary>
        /// 校内职称及档级取得时间
        /// </summary>
        public DateTime? F_XNDJSJ { get; set; }

        /// <summary>
        /// 是否有教师资格证
        /// </summary>
        public string F_SFZGZ { get; set; }

        /// <summary>
        /// 资格证编号
        /// </summary>
        public string F_ZGZBH { get; set; }

        /// <summary>
        /// 资格证名称
        /// </summary>
        public string F_ZGZMC { get; set; }

        /// <summary>
        /// 星级
        /// </summary>
        public string F_XJ { get; set; }

        /// <summary>
        /// 首签日期
        /// </summary>
        public DateTime? F_SQSJ { get; set; }

        /// <summary>
        /// 合同签订日
        /// </summary>
        public DateTime? F_HTQDR { get; set; }

        /// <summary>
        /// 合同到期日
        /// </summary>
        public DateTime? F_HTDQR { get; set; }

        /// <summary>
        /// 合同期限（年）
        /// </summary>
        public string F_HTN { get; set; }

        /// <summary>
        /// 是否转正
        /// </summary>
        public string F_SFZZ { get; set; }

        /// <summary>
        /// 社保缴纳情况
        /// </summary>
        public string F_SBQK { get; set; }

        /// <summary>
        /// 工伤保险缴纳情况
        /// </summary>
        public string F_GSQK { get; set; }

        /// <summary>
        /// 公积金缴纳情况
        /// </summary>
        public string F_GJJQK { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string F_JJUser { get; set; }

        /// <summary>
        /// 紧急联系人联系号码
        /// </summary>
        public string F_JJPhone { get; set; }

        /// <summary>
        /// 离职时间
        /// </summary>
        public string F_LZSJ { get; set; }

        /// <summary>
        /// F_School
        /// </summary>
        public string F_School { get; set; }

        public string F_NL { get; set; }

        public string F_ZGZY { get; set; }

        /// <summary>
        /// 卡类别
        /// </summary>
        public string F_Introduction { get; set; }

        /// <summary>
        /// 卡类别
        /// </summary>
        public string F_Ktype { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        ///
        public string F_Kstatu { get; set; }

        /// <summary>
        /// 客户身份
        /// </summary>
        ///
        public string F_Identity { get; set; }

        /// <summary>
        /// 教职工
        /// </summary>
        ///
        public string F_Type_Tea { get; set; }

        /// <summary>
        /// '物理卡号
        /// </summary>
        ///
        public string F_Mac_No { get; set; }

        /// <summary>
        /// 网盘帐号
        /// </summary>
        public string F_YPNo { get; set; }

        /// <summary>
        /// 网盘密码
        /// </summary>
        public string F_YPPwd { get; set; }
    }
}