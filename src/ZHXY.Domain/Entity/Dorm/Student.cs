using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZHXY.Domain
{
    public class Student : EntityBase, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        #region Declarations

        /// <summary>
        /// 学生ID
        /// </summary>

        [Display(Name = "学生ID")]
        public string F_Id { get; set; }

        /// <summary>
        /// 年级编码
        /// </summary>

        [Display(Name = "年级编码")]
        public string F_Year { get; set; }

        /// <summary>
        /// 学部ID
        /// </summary>

        [Display(Name = "学部ID")]
        public string F_Divis_ID { get; set; }

        /// <summary>
        /// 年级ID
        /// </summary>

        [Display(Name = "年级ID")]
        public string F_Grade_ID { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        [Display(Name = "班级ID")]
        public string F_Class_ID { get; set; }

        /// <summary>
        /// 班级类型ID
        /// </summary>

        [Display(Name = "班级类型ID")]
        public string F_Subjects_ID { get; set; }

        public string F_Subjects { get; set; }

        /// <summary>
        /// 学号
        /// </summary>

        [Display(Name = "学号")]
        public string F_StudentNum { get; set; }

        /// <summary>
        /// 全国学籍号
        /// </summary>

        [Display(Name = "全国学籍号")]
        public string F_NationNum { get; set; }

        /// <summary>
        /// 入学时间
        /// </summary>

        [Display(Name = "入学时间")]
        public DateTime? F_InitDTM { get; set; }

        /// <summary>
        /// 学生系统用户
        /// </summary>

        [Display(Name = "学生系统用户")]
        public string F_Users_ID { get; set; }

        public virtual User studentSysUser { get; set; }

        /// <summary>
        /// 入学序号
        /// </summary>

        [Display(Name = "入学序号")]
        public string F_InitNum { get; set; }

        /// <summary>
        /// IC卡类别
        /// </summary>

        [Display(Name = "IC卡类别")]
        public string F_ICType { get; set; }

        /// <summary>
        /// 身份类别
        /// </summary>

        [Display(Name = "身份类别")]
        public string F_Type { get; set; }

        /// <summary>
        /// 翼校通号
        /// </summary>

        [Display(Name = "翼校通号")]
        public string F_YXT_Num { get; set; }

        /// <summary>
        /// 物理卡号
        /// </summary>

        [Display(Name = "物理卡号")]
        public string F_Mac_No { get; set; }

        /// <summary>
        /// 物理地址
        /// </summary>

        [Display(Name = "物理地址")]
        public string F_Mac_Addr { get; set; }

        /// <summary>
        /// 来源学校
        /// </summary>

        [Display(Name = "来源学校")]
        public string F_ComeFrom { get; set; }

        /// <summary>
        /// 注册用户ID
        /// </summary>

        [Display(Name = "注册用户ID")]
        public int? F_RegUsers_ID { get; set; }

        /// <summary>
        /// 注册用户姓名（招生老师）
        /// </summary>

        [Display(Name = "注册用户姓名（招生老师）")]
        public string F_RegisterName { get; set; }

        /// <summary>
        /// 注册用户账号（招生老师）
        /// </summary>

        [Display(Name = "注册用户账号（招生老师）")]
        public string F_RegisterNum { get; set; }

        /// <summary>
        /// 就读类别
        /// </summary>

        [Display(Name = "就读类别")]
        public string F_SchoolType { get; set; }

        /// <summary>
        /// 在校状态
        /// </summary>

        [Display(Name = "在校状态")]
        public string F_CurStatu { get; set; }

        /// <summary>
        /// 当前学年度缴费情况
        /// </summary>

        [Display(Name = "当前学年度缴费情况")]
        public string F_CurChargeDesc { get; set; }

        /// <summary>
        /// 父亲姓名
        /// </summary>

        [Display(Name = "父亲姓名")]
        public string F_Father { get; set; }

        /// <summary>
        /// 父亲联系电话
        /// </summary>

        [Display(Name = "父亲联系电话")]
        public string F_FatherTel { get; set; }

        /// <summary>
        /// 母亲姓名
        /// </summary>

        [Display(Name = "母亲姓名")]
        public string F_Mother { get; set; }

        /// <summary>
        /// 母亲联系电话
        /// </summary>

        [Display(Name = "母亲联系电话")]
        public string F_MotherTel { get; set; }

        /// <summary>
        /// 监护人姓名
        /// </summary>

        [Display(Name = "监护人姓名")]
        public string F_Guarder { get; set; }

        /// <summary>
        /// 监护关系
        /// </summary>

        [Display(Name = "监护关系")]
        public string F_Guarder_Relation { get; set; }

        /// <summary>
        /// 监护人联系电话
        /// </summary>

        [Display(Name = "监护人联系电话")]
        public string F_Guarder_Tel { get; set; }

        /// <summary>
        /// 监护人国籍
        /// </summary>

        [Display(Name = "监护人国籍")]
        public string F_Guarder_Nation { get; set; }

        /// <summary>
        /// 监护人证件号
        /// </summary>

        [Display(Name = "监护人证件号")]
        public string F_Guarder_CredNum { get; set; }

        /// <summary>
        /// 监护人证件类型
        /// </summary>

        [Display(Name = "监护人证件类型")]
        public string F_Guarder_CredType { get; set; }

        /// <summary>
        /// 监护人证件正面
        /// </summary>

        [Display(Name = "监护人证件正面")]
        public string F_Guarder_CredPhoto_Obve { get; set; }

        /// <summary>
        /// 监护人证件反面
        /// </summary>

        [Display(Name = "监护人证件反面")]
        public string F_Guarder_CredPhoto_Rever { get; set; }

        /// <summary>
        /// 插班标注
        /// </summary>

        [Display(Name = "插班标注")]
        public string F_InClass { get; set; }

        /// <summary>
        /// 生源类别
        /// </summary>

        [Display(Name = "生源类别")]
        public string F_Stu_Type { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>

        [Display(Name = "来源类型")]
        public string F_ComeFrom_Type { get; set; }

        /// <summary>
        /// 分数线
        /// </summary>

        [Display(Name = "分数线")]
        public string F_Sco_Line { get; set; }

        /// <summary>
        /// 分数差
        /// </summary>

        [Display(Name = "分数差")]
        public string F_Sco_Pir { get; set; }

        /// <summary>
        /// 随迁子女
        /// </summary>

        [Display(Name = "随迁子女")]
        public string F_Sqzn { get; set; }

        /// <summary>
        /// 多子女
        /// </summary>

        [Display(Name = "多子女")]
        public bool? F_Dzn { get; set; }

        /// <summary>
        /// 多子女凭证
        /// </summary>

        [Display(Name = "多子女凭证")]
        public string F_Dzn_File { get; set; }

        /// <summary>
        /// 多子女关联的学生姓名及身份证号
        /// </summary>

        [Display(Name = "多子女关联的学生姓名及身份证号")]
        public string F_Dzn_Memo { get; set; }

        /// <summary>
        /// 教师子女
        /// </summary>

        [Display(Name = "教师子女")]
        public bool? F_Teacherzn { get; set; }

        /// <summary>
        /// 教师子女凭证
        /// </summary>

        [Display(Name = "教师子女凭证")]
        public string F_Teacherzn_File { get; set; }

        /// <summary>
        /// 教师子女关联的学生姓名及身份证号
        /// </summary>

        [Display(Name = "教师子女关联的学生姓名及身份证号")]
        public string F_Teacherzn_Memo { get; set; }

        /// <summary>
        /// 复读生
        /// </summary>

        [Display(Name = "复读生")]
        public string F_Old { get; set; }

        /// <summary>
        /// 入学信息备注
        /// </summary>

        [Display(Name = "入学信息备注")]
        public string F_In_Memo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>

        [Display(Name = "姓名")]
        public string F_Name { get; set; }

        /// <summary>
        /// 曾用名
        /// </summary>

        [Display(Name = "曾用名")]
        public string F_Name_Old { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>

        [Display(Name = "英文名")]
        public string F_Name_En { get; set; }

        /// <summary>
        /// 性别
        /// </summary>

        [Display(Name = "性别")]
        public string F_Gender { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>

        [Display(Name = "出生日期")]
        public DateTime? F_Birthday { get; set; }

        /// <summary>
        /// 独生子女
        /// </summary>

        [Display(Name = "独生子女")]
        public string F_Only_One { get; set; }

        /// <summary>
        /// 照片
        /// </summary>

        [Display(Name = "照片")]
        public string F_FacePic_File { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>

        [Display(Name = "国籍")]
        public string F_Nation { get; set; }

        /// <summary>
        /// 宗教信仰
        /// </summary>

        [Display(Name = "宗教信仰")]
        public string F_ZJXY { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>

        [Display(Name = "证件类型")]
        public string F_CredType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>

        [Display(Name = "证件号码")]
        public string F_CredNum { get; set; }

        /// <summary>
        /// 证件正面照片
        /// </summary>

        [Display(Name = "证件正面照片")]
        public string F_CredPhoto_Obve { get; set; }

        /// <summary>
        /// 证件反面照片
        /// </summary>

        [Display(Name = "证件反面照片")]
        public string F_CredPhoto_Rever { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>

        [Display(Name = "籍贯")]
        public string F_Native { get; set; }

        /// <summary>
        /// 祖籍国
        /// </summary>

        [Display(Name = "祖籍国")]
        public string F_Native_Old { get; set; }

        /// <summary>
        /// 中文程度
        /// </summary>

        [Display(Name = "中文程度")]
        public string F_Cn_Level { get; set; }

        /// <summary>
        /// HSK-TEST
        /// </summary>

        [Display(Name = "HSK-TEST")]
        public string F_HSK_TEST { get; set; }

        /// <summary>
        /// 申请学习时间
        /// </summary>

        [Display(Name = "申请学习时间")]
        public DateTime? F_Stu_Time { get; set; }

        /// <summary>
        /// 民族
        /// </summary>

        [Display(Name = "民族")]
        public string F_Volk { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>

        [Display(Name = "政治面貌")]
        public string F_PolitStatu { get; set; }

        /// <summary>
        /// 家庭住址
        /// </summary>

        [Display(Name = "家庭住址")]
        public string F_Home_Addr { get; set; }

        /// <summary>
        /// 身高
        /// </summary>

        [Display(Name = "身高")]
        public string F_Height { get; set; }

        /// <summary>
        /// 体重
        /// </summary>

        [Display(Name = "体重")]
        public string F_Weight { get; set; }

        /// <summary>
        /// 血型
        /// </summary>

        [Display(Name = "血型")]
        public string F_Blood_Type { get; set; }

        /// <summary>
        /// 过敏药物
        /// </summary>

        [Display(Name = "过敏药物")]
        public string F_Allergy { get; set; }

        /// <summary>
        /// 过敏食物
        /// </summary>

        [Display(Name = "过敏食物")]
        public string F_Food { get; set; }

        /// <summary>
        /// 病史
        /// </summary>

        [Display(Name = "病史")]
        public string F_MedicalHis { get; set; }

        /// <summary>
        /// 病史补充
        /// </summary>

        [Display(Name = "病史补充")]
        public string F_MedicalHis_Memo { get; set; }

        /// <summary>
        /// 户口情况
        /// </summary>

        [Display(Name = "户口情况")]
        public string F_Reg_Status { get; set; }

        /// <summary>
        /// 户口所在地
        /// </summary>

        [Display(Name = "户口所在地")]
        public string F_RegAddr { get; set; }

        /// <summary>
        /// 与户主关系
        /// </summary>

        [Display(Name = "与户主关系")]
        public string F_RegRelat { get; set; }

        /// <summary>
        /// 户口本主页照片
        /// </summary>

        [Display(Name = "户口本主页照片")]
        public string F_RegPhoto_Obve { get; set; }

        /// <summary>
        /// 户口本学生页照片
        /// </summary>

        [Display(Name = "户口本学生页照片")]
        public string F_RegPhoto_Rever { get; set; }

        /// <summary>
        /// 户主姓名
        /// </summary>

        [Display(Name = "户主姓名")]
        public string F_RegMainName { get; set; }

        /// <summary>
        /// 户主身份证号
        /// </summary>

        [Display(Name = "户主身份证号")]
        public string F_RegMain_CredNum { get; set; }

        /// <summary>
        /// 户主身份证正面
        /// </summary>

        [Display(Name = "户主身份证正面")]
        public string F_RegMain_CredPhoto_Obve { get; set; }

        /// <summary>
        /// 户主身份证反面
        /// </summary>

        [Display(Name = "户主身份证反面")]
        public string F_RegMain_CredPhoto_Rever { get; set; }

        /// <summary>
        /// 家庭详细地址
        /// </summary>

        [Display(Name = "家庭详细地址")]
        public string F_FamilyAddr { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>

        [Display(Name = "联系电话")]
        public string F_Tel { get; set; }

        /// <summary>
        /// 源校地址
        /// </summary>

        [Display(Name = "源校地址")]
        public string F_ComeFromAddress { get; set; }

        /// <summary>
        /// 选拔成绩
        /// </summary>

        [Display(Name = "选拔成绩")]
        public string F_Score { get; set; }

        /// <summary>
        /// 选拔成绩单
        /// </summary>

        [Display(Name = "选拔成绩单")]
        public string F_Score_File { get; set; }

        [Display(Name = "接送站点")]
        public string F_ComeBackSite { get; set; }

        /// <summary>
        /// 接送方式
        /// </summary>

        [Display(Name = "接送方式")]
        public string F_ComeBackType { get; set; }

        /// <summary>
        /// 接送路线
        /// </summary>

        [Display(Name = "接送路线")]
        public string F_ComeBackRoute_ID { get; set; }

        /// <summary>
        /// 接送省
        /// </summary>

        [Display(Name = "接送省")]
        public string F_ComeBackPro { get; set; }

        /// <summary>
        /// 接送区县
        /// </summary>

        [Display(Name = "接送区县")]
        public string F_ComeBackArea { get; set; }

        /// <summary>
        /// 接送市
        /// </summary>

        [Display(Name = "接送市")]
        public string F_ComeBackCity { get; set; }

        /// <summary>
        /// 第二亲属亲子关系
        /// </summary>

        [Display(Name = "第二亲属亲子关系")]
        public string F_Relative2_Guarder_Relation { get; set; }

        /// <summary>
        /// 接送亲属亲子关系
        /// </summary>

        [Display(Name = "接送亲属亲子关系")]
        public string F_Relative3_Guarder_Relation { get; set; }

        /// <summary>
        /// 接送亲属姓名
        /// </summary>

        [Display(Name = "接送亲属姓名")]
        public string F_Relative3_Name { get; set; }

        /// <summary>
        /// 接送亲属电话
        /// </summary>

        [Display(Name = "接送亲属电话")]
        public string F_Relative3_Tel { get; set; }

        /// <summary>
        /// 第一亲属姓名
        /// </summary>

        [Display(Name = "第一亲属姓名")]
        public string F_Relative1_Name { get; set; }

        /// <summary>
        /// 第一亲属电话
        /// </summary>

        [Display(Name = "第一亲属电话")]
        public string F_Relative1_Tel { get; set; }

        /// <summary>
        /// 第一亲属文化程度
        /// </summary>

        [Display(Name = "第一亲属文化程度")]
        public string F_Relative1_Guarder { get; set; }

        /// <summary>
        /// 第一亲属工作单位
        /// </summary>

        [Display(Name = "第一亲属工作单位")]
        public string F_Relative1_Comp { get; set; }

        /// <summary>
        /// 第一亲属亲子关系
        /// </summary>

        [Display(Name = "第一亲属亲子关系")]
        public string F_Relative1_Guarder_Relation { get; set; }

        /// <summary>
        /// 第二亲属姓名
        /// </summary>

        [Display(Name = "第二亲属姓名")]
        public string F_Relative2_Name { get; set; }

        /// <summary>
        /// 第二亲属电话
        /// </summary>

        [Display(Name = "第二亲属电话")]
        public string F_Relative2_Tel { get; set; }

        /// <summary>
        /// 第二亲属文化程度
        /// </summary>

        [Display(Name = "第二亲属文化程度")]
        public string F_Relative2_Guarder { get; set; }

        /// <summary>
        /// 第二亲属工作单位
        /// </summary>

        [Display(Name = "第二亲属工作单位")]
        public string F_Relative2_Comp { get; set; }

        /// <summary>
        /// 接送亲属文化程度
        /// </summary>

        [Display(Name = "接送亲属文化程度")]
        public string F_Relative3_Guarder3 { get; set; }

        /// <summary>
        /// 接送亲属工作单位
        /// </summary>

        [Display(Name = "接送亲属工作单位")]
        public string F_Relative3_Comp3 { get; set; }

        /// <summary>
        /// 监护人工作单位
        /// </summary>

        [Display(Name = "监护人工作单位")]
        public string F_Guarder_Dw { get; set; }

        /// <summary>
        /// 监护人文化程度
        /// </summary>

        [Display(Name = "监护人文化程度")]
        public string F_Guarder_Wh { get; set; }

        /// <summary>
        /// 监护人其他联系方式
        /// </summary>

        [Display(Name = "监护人其他联系方式")]
        public string F_Guarder_LinkType { get; set; }

        /// <summary>
        /// 吃饭
        /// </summary>

        [Display(Name = "吃饭")]
        public string F_Eat { get; set; }

        /// <summary>
        /// 食欲
        /// </summary>

        [Display(Name = "食欲")]
        public string F_Relish { get; set; }

        /// <summary>
        /// 大小便
        /// </summary>

        [Display(Name = "大小便")]
        public string F_Incontinence { get; set; }

        /// <summary>
        /// 着装习惯
        /// </summary>

        [Display(Name = "着装习惯")]
        public string F_Dress { get; set; }

        /// <summary>
        /// 午睡
        /// </summary>

        [Display(Name = "午睡")]
        public string F_Sleep { get; set; }

        /// <summary>
        /// 穿脱衣
        /// </summary>

        [Display(Name = "穿脱衣")]
        public string F_Stripped { get; set; }

        /// <summary>
        /// 体质
        /// </summary>

        [Display(Name = "体质")]
        public string F_Physique { get; set; }

        /// <summary>
        /// 生活其他
        /// </summary>

        [Display(Name = "生活其他")]
        public string F_Life_Memo { get; set; }

        /// <summary>
        /// 序号
        /// </summary>

        [Display(Name = "序号")]
        public int? F_SortCode { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>

        [Display(Name = "所属部门")]
        public string F_DepartmentId { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>

        [Display(Name = "删除标记")]
        public bool? F_DeleteMark { get; set; }

        /// <summary>
        /// 启用标记
        /// </summary>

        [Display(Name = "启用标记")]
        public bool? F_EnabledMark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>

        [Display(Name = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>

        [Display(Name = "创建者")]
        public string F_CreatorUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>

        [Display(Name = "修改时间")]
        public DateTime? F_LastModifyTime { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>

        [Display(Name = "修改者")]
        public string F_LastModifyUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>

        [Display(Name = "删除时间")]
        public DateTime? F_DeleteTime { get; set; }

        [Display(Name = "删除者")]
        public string F_DeleteUserId { get; set; }

        /// <summary>
        /// 入学年度
        /// </summary>

        [Display(Name = "入学年度")]
        public string F_INYear { get; set; }

        /// <summary>
        /// 健康状况
        /// </summary>
        [Display(Name = "健康状况")]
        public string F_Health { get; set; }

        /// <summary>
        /// 源校省
        /// </summary>
        [Display(Name = "源校省")]
        public string F_ComeFrom_Province { get; set; }

        /// <summary>
        /// 源校市
        /// </summary>
        [Display(Name = "源校市")]
        public string F_ComeFrom_City { get; set; }

        /// <summary>
        /// 源校县
        /// </summary>
        [Display(Name = "源校县")]
        public string F_ComeFrom_Area { get; set; }

        /// <summary>
        /// 自我介绍
        /// </summary>
        [Display(Name = "自我介绍")]
        public string F_Introduction { get; set; }

        [ForeignKey("F_Divis_ID")]
        public Organ sysOrganize { get; set; }

        [ForeignKey("F_Id")]
        public DormStudent DormStudent { get; set; }

        public int F_InOut { get; set; }

        #endregion Declarations
    }
}