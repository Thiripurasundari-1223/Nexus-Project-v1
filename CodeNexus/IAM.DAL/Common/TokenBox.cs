using System;

namespace IAM.DAL.Common
{
    public class TokenBox
    {
        public string AzureTokenKey { get; set; }
        public string EmailAddress { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public Double ExpiryOn { get; set; }
        public TokenBox(string azureTokenKey, string emailAddress, DateTime expiryDateTime, Double expiryOn)
        {
            this.AzureTokenKey = azureTokenKey;
            this.EmailAddress = emailAddress;
            this.ExpiryDateTime = expiryDateTime;
            this.ExpiryOn = expiryOn;
        }
    }
}