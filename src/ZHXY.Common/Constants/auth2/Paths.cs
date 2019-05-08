namespace ZHXY.Common
{
    public static class Paths
    {
        public static string AuthorizationServerBaseAddress = Configs.GetValue("baseUrl");
        public static string ResourceServerBaseAddress = Configs.GetValue("resBaseUrl");
        public static string ImplicitGrantCallBackPath = "http://localhost:38515/Home/SignIn";
        public static string AuthorizeCodeCallBackPath = Configs.GetValue("redirectUrl");
        public static string AuthorizePath = Configs.GetValue("authorizePath");
        public static string TokenPath = Configs.GetValue("tokenPath");
        public static string LoginPath = Configs.GetValue("loginPath");
        public static string LogoutPath = Configs.GetValue("logoutPath");
        public static string MePath = "/api/Me";
        public static string UserInfoPath = Configs.GetValue("userInfoPath");
    }
}