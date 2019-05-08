namespace ZHXY.Application
{
    /// <summary>
    ///     树模型
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public class TreeView
    {
        /// <summary>
        ///     节点ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     父节点ID
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        ///     层级
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        ///     是否已加载
        /// </summary>
        public bool Loaded { get; set; }

        /// <summary>
        ///     是否是叶子节点
        /// </summary>
        public bool IsLeaf { get; set; }

        /// <summary>
        ///     是否展开
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        ///     显示名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     父节点名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        ///     启用标记
        /// </summary>
        public bool IsDisabled { get; set; }

        public int? SortCode { get; set; }

    }
}