using System;
using System.ComponentModel.DataAnnotations;

namespace ZHXY.Domain
{
    /// <summary>
    /// 字段定义
    /// </summary>
    public class FieldDef : IEntity
    {
        public string F_Id { get; set; }

        public string F_TableDef_ID { get; set; }

        public string F_FieldName { get; set; }

        public string F_FieldTitle { get; set; }

        public string F_DataType { get; set; }

        public int? F_Length { get; set; }

        public int? F_DigitLen { get; set; }

        public int? F_ColWidth { get; set; }

        public int? F_SortCode { get; set; }

        /// <summary>
        /// 是否在列表中显示
        /// </summary>
        [Required]
        [Display(Name = "是否在列表中显示")]
        public bool F_IsListDispaly { get; set; }

        /// <summary>
        /// 是否在excel中显示
        /// </summary>
        [Required]
        [Display(Name = "是否在excel中显示")]
        public bool F_IsExcelDispaly { get; set; }

        [Required]
        [Display(Name = "字典")]
        public string F_Dic { get; set; }

        [Required]
        [Display(Name = "字典code")]
        public string F_Dic_Code { get; set; }

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

    }
}