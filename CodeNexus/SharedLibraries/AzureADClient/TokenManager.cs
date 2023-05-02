using Microsoft.Extensions.Caching.Memory;
using SharedLibraries;
using System;
using System.Security.Claims;

namespace SharedLibraries.Authentication.Manager
{
    public interface ITokenManager
    {
        ClaimsPrincipal GetClaimsPrincipal(string accessToken, IMemoryCache cache);
    }
    public class TokenManager : ITokenManager
    {
        private readonly AppSettings appSettings;
        public TokenManager(AppSettings appSettings) { this.appSettings = appSettings; }
        public ClaimsPrincipal GetClaimsPrincipal(string accessToken, IMemoryCache cache)
        {
            throw new NotImplementedException();
        }
        //public string GenerateAccessToken(ClaimsIdentity claimsIdentity)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConstSecurityKey.ACCESS_KEY));
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = claimsIdentity,
        //        Expires = DateTime.UtcNow.AddMinutes(30),
        //        Issuer = tokenIssuer,
        //        Audience = appSettings.AppId,
        //        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
        //public string GenerateRefreshToken(ClaimsIdentity claimsIdentity)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConstSecurityKey.REFRESH_KEY));
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = claimsIdentity,
        //        Expires = DateTime.UtcNow.AddDays(1),
        //        Issuer = tokenIssuer,
        //        Audience = appSettings.AppId,
        //        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
        //public ClaimsPrincipal GetClaimsPrincipal(string accessToken, IMemoryCache cache)
        //{
        //    try
        //    {
        //        string ADAccessToken = string.Empty;
        //        var s = cache.TryGetValue("kannan.t@tvsnext.io", out ADAccessToken);
        //        string AZtoken = cache.Get("kannan.t@tvsnext.io").ToString();
        //        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AZtoken));
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        return tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidIssuer = tokenIssuer,
        //            ValidAudience = appSettings.AppId,
        //            IssuerSigningKey = securityKey
        //        }, out SecurityToken validatedToken);
        //    }
        //    catch (Exception ex )
        //    {
        //        throw;
        //    }
        //}
        //public JwtSecurityToken Validate(string token)
        //{
        //    string stsDiscoveryEndpoint = "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration";
        //    ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint);
        //    OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;
        //    TokenValidationParameters validationParameters = new TokenValidationParameters
        //    {
        //        ValidateAudience = false,
        //        ValidateIssuer = false,
        //        IssuerSigningTokens = config.SigningTokens,
        //        ValidateLifetime = false
        //    };
        //    JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();
        //    SecurityToken jwt;
        //    var result = tokendHandler.ValidateToken(token, validationParameters, out jwt);
        //    return jwt as JwtSecurityToken;
        //}
        //public async Task<ClaimsPrincipal> CreatePrincipleAsync()
        //{
        //    AzureActiveDirectoryToken azureToken = Token.FromJsonString<AzureActiveDirectoryToken>();
        //    var allParts = azureToken.IdToken.Split(".");
        //    var header = allParts[0];
        //    var payload = allParts[1];
        //    var idToken = payload.ToBytesFromBase64URLString().ToAscii().FromJsonString<AzureActiveDirectoryIdToken>();
        //    allParts = azureToken.AccessToken.Split(".");
        //    header = allParts[0];
        //    payload = allParts[1];
        //    var signature = allParts[2];
        //    var accessToken = payload.ToBytesFromBase64URLString().ToAscii().FromJsonString<C>();
        //    var accessTokenHeader = header.ToBytesFromBase64URLString().ToAscii().FromJsonString<AzureTokenHeader>();
        //    var isValid = await ValidateToken(accessTokenHeader.kid, header, payload, signature);
        //    if (!isValid)
        //    {
        //        throw new SecurityException("Token can not be validated");
        //    }
        //    var principal = await CreatePrincipalAsync(accessToken, idToken);
        //    return principal;
        //}
        //private async Task<bool> ValidateToken(string kid, string header, string payload, string signature)
        //{
        //    string keysAsString = null;
        //    const string microsoftKeysUrl = "https://login.microsoftonline.com/common/discovery/keys";
        //    using (var client = new HttpClient())
        //    {
        //        keysAsString = await client.GetStringAsync(microsoftKeysUrl);
        //    }
        //    var azureKeys = keysAsString.FromJsonString<MicrosoftConfigurationKeys>();
        //    var signatureKeyIdentifier = azureKeys.Keys.FirstOrDefault(key => key.kid.Equals(kid));
        //    if (signatureKeyIdentifier.IsNotNull())
        //    {
        //        var signatureKey = signatureKeyIdentifier.x5c.First();
        //        var certificate = new X509Certificate2(signatureKey.ToBytesFromBase64URLString());
        //        var rsa = certificate.GetRSAPublicKey();
        //        var data = string.Format("{0}.{1}", header, payload).ToBytes();

        //        var isValidSignature = rsa.VerifyData(data, signature.ToBytesFromBase64URLString(), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        //        return isValidSignature;
        //    }
        //    return false;
        //}
        //public string GenerateAccessToken(ClaimsIdentity claimsIdentity, string aud)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConstSecurityKey.ACCESS_KEY));
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = claimsIdentity,
        //        Expires = DateTime.UtcNow.AddMinutes(30),
        //        Issuer = tokenIssuer,
        //        Audience = aud,
        //        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
        //public string GenerateRefreshToken(ClaimsIdentity claimsIdentity, string aud)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConstSecurityKey.REFRESH_KEY));
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = claimsIdentity,
        //        Expires = DateTime.UtcNow.AddDays(1),
        //        Issuer = tokenIssuer,
        //        Audience = aud,
        //        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
    }
}