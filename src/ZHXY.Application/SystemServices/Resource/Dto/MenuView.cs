namespace ZHXY.Application
{
    public class MenuView
    {
        public string Id { get; set; }
        public bool IsLeaf { get; set; }
        public string ParentId { get; set; }
        public string EnCode { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string IconForWeb { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public int? SortCode { get; set; }
        public string BelongSys { get; set; }
        public int  Level { get; set; }
    }
}