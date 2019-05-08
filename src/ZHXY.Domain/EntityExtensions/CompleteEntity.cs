using System.ComponentModel.DataAnnotations.Schema;

namespace ZHXY.Domain
{
    /// <summary>
    /// 方法过时，请使用Entity类
    ///     完整实体类(继承实体基类,实现创建审计接口,修改审计接口,删除审计接口,可排序接口,可禁用接口)
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public abstract class CompleteEntity : AdutiEntity, ISortable, IDisable
    {
        /// <summary>
        ///     启用标识
        /// </summary>
        [Column("F_EnabledMark")]
        public bool IsDisabled { get; set; }

        /// <summary>
        ///     排序码
        /// </summary>
        [Column("F_SortCode")]
        public int? SortCode { get; set; }
    }
}