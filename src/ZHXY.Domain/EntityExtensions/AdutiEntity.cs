using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZHXY.Domain
{
    /// <summary>
    ///     审计实体基类(继承实体基类,实现实体接口,创建审计接口,修改审计接口)
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public abstract class AdutiEntity : BaseEntity, ICreateAuditable, IModifiedAuditable, IDeleteAuditable
    {
        /// <summary>
        ///     创建人(外键)
        /// </summary>
        [Column("F_CreatorUserId", TypeName = "varchar")]
        [StringLength(50)]
        public string CreatedByUserId { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Column("F_CreatorTime", TypeName = "datetime")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        ///     所属部门
        /// </summary>
        [Column("F_DepartmentId", TypeName = "varchar")]
        [StringLength(50)]
        public string OwnerDeptId { get; set; }

        /// <summary>
        ///     最后修改的用户(外键)
        /// </summary>
        [Column("F_LastModifyUserId", TypeName = "varchar")]
        [StringLength(50)]
        public string LastModifiedByUserId { get; set; }

        /// <summary>
        ///     最后修改的时间
        /// </summary>
        [Column("F_LastModifyTime", TypeName = "datetime")]
        public DateTime? LastModifiedTime { get; set; }

        /// <summary>
        ///     删除标记(逻辑删除)
        /// </summary>
        [Column("F_DeleteMark")]
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     删除的用户(外键)
        /// </summary>
        [Column("F_DeleteUserId", TypeName = "varchar")]
        [StringLength(50)]
        public string DeletedByUserId { get; set; }

        /// <summary>
        ///     删除的时间
        /// </summary>
        [Column("F_DeleteTime", TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }
    }
}