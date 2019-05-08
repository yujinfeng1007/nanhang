namespace ZHXY.Domain
{
    /// <summary>
    /// 关联
    /// </summary>
    public class Relevance : IEntity
    {
        /// <summary>
        /// 关系名称
        /// </summary>
        public string Name { get; set; }
        public string FirstKey { get; set; }
        public string SecondKey { get; set; }
        public string ThirdKey { get; set; }

    }
}