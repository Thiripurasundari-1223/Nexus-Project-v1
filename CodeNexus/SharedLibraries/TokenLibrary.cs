using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries
{
    

    public class TokenLibrary
    {
        public string _AzureTokenKey { get; set; }
        public string _EmailAddress { get; set; }
        public DateTime _ExpiryDateTime { get; set; }
        public Double _ExpiryOn { get; set; }

        public TokenLibrary(string azureTokenKey, string emailAddress, DateTime expiryDateTime, Double expiryOn)
        {
            this._AzureTokenKey = azureTokenKey;
            this._EmailAddress = emailAddress;
            this._ExpiryDateTime = expiryDateTime;
            this._ExpiryOn = expiryOn;

        }

        public static List<TokenLibrary> _TokenLibrary = new List<TokenLibrary>();
    }
}
