namespace ZHXY.Domain
{
    /// <summary>
    /// 字典
    /// </summary>
    public class Dic : IEntity
    {
        public string Code { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public int SortCode { get; set; }

    }
}