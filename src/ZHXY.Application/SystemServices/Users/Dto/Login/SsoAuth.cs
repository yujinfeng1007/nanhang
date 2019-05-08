namespace ZHXY.Application
{
    public class SsoAuth
    {
        public string Callback { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenExpirationUtc { get; set; }
        public string AccessTokenIssueDateUtc { get; set; }
        public string[] Scope { get; set; }
        public bool IsDeleted { get; set; }
    }
}