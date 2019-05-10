using ZHXY.Domain;
using System;

namespace ZHXY.Application
{
    public class TeacherDto
    {
        /// <summary>
        /// 教师ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        public virtual User User { get; set; }

        /// <summary>
        /// 隶属学部
        /// </summary>
        public string DivisId { get; set; }

        /// <summary>
        /// 教师姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 教师工号
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// 电子照片
        /// </summary>
        public string FacePhoto { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string ZJLX { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZJHM { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        public string ZZMM { get; set; }

        /// <summary>
        /// 进校时间
        /// </summary>
        public DateTime? EntryTime { get; set; }


        /// <summary>
        /// 科别
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public bool? DeleteMark { get; set; }



        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatorTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string LastModifyUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }

        /// <summary>
        /// 删除者
        /// </summary>
        public string DeleteUserId { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string SJHM { get; set; }

        public string RoleId { get; set; }

        /// <summary>
        /// 工龄
        /// </summary>
        public string GL { get; set; }

        /// <summary>
        /// 校龄
        /// </summary>
        public string XL { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public string NL { get; set; }

        /// <summary>
        /// 卡类别
        /// </summary>
        public string Introduction { get; set; }



    }
}