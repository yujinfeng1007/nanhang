using System;

namespace ZHXY.Domain
{
    public class Student : IEntity
    {

        /// <summary>
        /// 学生ID
        /// </summary>

        public string Id { get; set; }

        /// <summary>
        /// 年级编码
        /// </summary>

        public string YearNumber { get; set; }

        /// <summary>
        /// 学部ID
        /// </summary>

        public string DivisId { get; set; }

        /// <summary>
        /// 年级ID
        /// </summary>

        public string GradeId { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 班级类型ID
        /// </summary>

        public string SubjectId { get; set; }


        /// <summary>
        /// 学号
        /// </summary>

        public string StudentNumber { get; set; }


        /// <summary>
        /// 入学时间
        /// </summary>

        public DateTime? InitDTM { get; set; }

        /// <summary>
        /// 学生系统用户
        /// </summary>

        public string UserId { get; set; }



        /// <summary>
        /// 物理卡号
        /// </summary>

        public string CardNumber { get; set; }

   
      
        /// <summary>
        /// 在校状态
        /// </summary>

        public string CurStatu { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>

        public string Gender { get; set; }

     

        /// <summary>
        /// 证件类型
        /// </summary>

        public string CredType { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>

        public string CredNumber { get; set; }

      


        /// <summary>
        /// 政治面貌
        /// </summary>

        public string PolitStatu { get; set; }

       

      

        /// <summary>
        /// 联系电话
        /// </summary>

        public string MobilePhone { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>

        public string OrganId { get; set; }

        /// <summary>
        /// 在寝情况 0=进 1=出
        /// </summary>
        public string InOut { get; set; }

        /// <summary>
        /// 脸部图像
        /// </summary>
        public string FacePic { get; set; }

    }
}