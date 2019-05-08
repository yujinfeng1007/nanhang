using System;
using System.ComponentModel.DataAnnotations;

namespace ZHXY.Domain
{
    /// <summary>
    /// 表定义
    /// </summary>
    public class TableDef : EntityBase, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        #region Declarations

        /// <summary>
        /// 表ID
        /// </summary>
        [Required]
        [Display(Name = "表ID")]
        public string F_Id { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        [Required]
        [Display(Name = "表名称")]
        public string F_TableName { get; set; }

        /// <summary>
        /// 表标题
        /// </summary>
        [Required]
        [Display(Name = "表标题")]
        public string F_TableTitle { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        [Display(Name = "序号")]
        public int? F_SortCode { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [Required]
        [Display(Name = "所属部门")]
        public string F_DepartmentId { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        [Required]
        [Display(Name = "删除标记")]
        public bool? F_DeleteMark { get; set; }

        /// <summary>
        /// 启用标记
        /// </summary>
        [Required]
        [Display(Name = "启用标记")]
        public bool? F_EnabledMark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Display(Name = "创建时间")]
        public DateTime? F_CreatorTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Required]
        [Display(Name = "创建者")]
        public string F_CreatorUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Required]
        [Display(Name = "修改时间")]
        public DateTime? F_LastModifyTime { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Required]
        [Display(Name = "修改者")]
        public string F_LastModifyUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [Required]
        [Display(Name = "删除时间")]
        public DateTime? F_DeleteTime { get; set; }

        /// <summary>
        /// 删除者
        /// </summary>
        [Required]
        [Display(Name = "删除者")]
        public string F_DeleteUserId { get; set; }

        #endregion Declarations
    }
}