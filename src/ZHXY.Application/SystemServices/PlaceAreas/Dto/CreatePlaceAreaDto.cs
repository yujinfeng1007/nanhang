namespace ZHXY.Application
{
    public class CreatePlaceAreaDto
    {
        public string ParentId { get; set; }
        public string Name { get; set; }
        public int SortCode { get; set; }
        public bool Effective { get; set; }
        public string Remark { get; set; }
    }
}