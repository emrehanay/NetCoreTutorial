namespace NetCoreTutorial.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string UploadDirectory { get; set; }
        public int FileSizeLimit { get; set; }
        public int CacheExpiryMinute { get; set; }
        public int TokenExpiryDay { get; set; }
    }
}