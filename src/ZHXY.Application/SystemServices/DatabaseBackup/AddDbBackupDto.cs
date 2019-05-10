namespace ZHXY.Application
{
    public class AddDbBackupDto 
    {
        public string BackupType { get; set; }
        public string DbName { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }


    }
}