using System;

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
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 隶属机构
        /// </summary>
        public string OrganId { get; set; }

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
        public string CredType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string CredNumber { get; set; }

        /// <summary>
        /// 进校时间
        /// </summary>
        public DateTime? EntryTime { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhone { get; set; }
    }
}