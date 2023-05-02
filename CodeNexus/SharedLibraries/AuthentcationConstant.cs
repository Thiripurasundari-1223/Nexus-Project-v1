namespace SharedLibraries
{
    public class AuthentcationConstant
    {
        public const string UNAUTHORIZED = "Unauthorized access";
    }
    public class ConstAuthMode
    {
        public const string DYNAMIC_AUTH = "DynamicAuth";
    }
    public class AppSettings
    {
        public string AppId { get; set; }
        public string AppUrl { get; set; }
    }
    public class ConstSecurityKey
    {
        public const string ACCESS_KEY = "tvs12345^&*()NXT";
        public const string REFRESH_KEY = "tvs09876%$#@!NXT";
    }
    public class TokenKey
    {
        public string AuthTokenKey { get; set; }
        public string UserEmailID { get; set; }
    }
}