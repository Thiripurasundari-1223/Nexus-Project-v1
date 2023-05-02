namespace PowerBiReportAPI.Models
{
    public class AzureAd
    {
        public string AuthenticationMode { get; set; }

        public string AuthorityUri { get; set; }

        public string ClientId { get; set; }

        public string TenantId { get; set; }

        public string[] Scope { get; set; }

        public string PbiUsername { get; set; }

        public string PbiPassword { get; set; }

        public string ClientSecret { get; set; }
    }
}
