namespace ZHXY.Common
{
    public static class Clients
    {
        public static readonly Client Client1 = new Client
        {
            Id = Configs.GetValue("clientId"),
            Secret = Configs.GetValue("clientSecret"),
            RedirectUrl = Paths.AuthorizeCodeCallBackPath
        };

        public static readonly Client Client2 = new Client
        {
            Id = "attendance_client",
            Secret = "attendance_client",
            RedirectUrl = Paths.ImplicitGrantCallBackPath
        };
    }

    public class Client
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string RedirectUrl { get; set; }
    }
}