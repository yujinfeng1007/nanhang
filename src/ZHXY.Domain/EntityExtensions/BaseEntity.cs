using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZHXY.Domain
{
    /// <summary>
    ///     基础实体类
    ///     包含Id属性,且Id为主键
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        [Key]
        [Column("F_Id", TypeName = "varchar")]
        [MaxLength(50)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();

        [Column("F_Memo", TypeName = "varchar")]
        [MaxLength(500)]
        public string Remark { get; set; }
    }
}