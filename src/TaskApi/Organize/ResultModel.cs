namespace TaskApi
{
    public class ResultModel
    {
        public string Error { get; set; }
        public string Result { get; set; }
        public bool Success { get; set; }
        public int Total { get; set; }
    }
}