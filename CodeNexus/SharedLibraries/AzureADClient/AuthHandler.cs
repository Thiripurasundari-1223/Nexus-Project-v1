using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using SharedLibraries.AzureADClient;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLibraries.Authentication.Handler
{
    public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        private readonly string AzureADURL;
        private readonly string AzureADURLForEndpoint;
        private readonly string AzureADClientId;
        private readonly string AzureADTenentId;
        private readonly string AzureADClientSecret;
        public AuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, IConfiguration iconfiguration) : base(options, logger, encoder, clock)
        {
            _configuration = iconfiguration;
            AzureADURL = _configuration.GetValue<string>("AzureADSecrets:URL");
            AzureADURLForEndpoint = _configuration.GetValue<string>("AzureADSecrets:URLForEndpoint");
            AzureADClientId = _configuration.GetValue<string>("AzureADSecrets:client_id");
            AzureADTenentId = _configuration.GetValue<string>("AzureADSecrets:tenent_id");
            AzureADClientSecret = _configuration.GetValue<string>("AzureADSecrets:client_secret");
        }
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                    return AuthenticateResult.Fail(AuthentcationConstant.UNAUTHORIZED);
                return await await Task.FromResult(ValidateAsync(Context));
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
        public async Task<AuthenticateResult> ValidateAsync(HttpContext httpContext)
        {
            try
            {
                if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                    return AuthenticateResult.Fail(AuthentcationConstant.UNAUTHORIZED);
                string accessToken = httpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;
                accessToken = accessToken.Replace("Bearer ", string.Empty);
                AuthenticationTicket userTicket;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken jsonToken = handler.ReadToken(accessToken);
                JwtSecurityToken tokenData = handler.ReadToken(accessToken) as JwtSecurityToken;
                string tokenEmailAddress = tokenData.Payload["preferred_username"].ToString();
                UserValidateTokenCache userValidateToken = UserTokenValidateCacheManager.GetUserValidateTokenCache(accessToken);
                //var userValidateToken = UserTokenCacheManager.GetUserIDTokenCache(accessToken);
                // await ValidateAZureADTokenAsync(accessToken);
                if (accessToken != null)
                {
                    string token = accessToken;
                    string myIssuer = String.Format(CultureInfo.InvariantCulture, AzureADURL, AzureADTenentId);
                    SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AzureADClientSecret));
                    string stsDiscoveryEndpoint = String.Format(CultureInfo.InvariantCulture, AzureADURLForEndpoint, AzureADTenentId);
                    ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());
                    OpenIdConnectConfiguration config = await configManager.GetConfigurationAsync();
                    IdentityModelEventSource.ShowPII = true;
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    TokenValidationParameters validationParameters = new TokenValidationParameters
                    {
                        ValidIssuers = new[] { myIssuer },
                        ValidAudiences = new[] { AzureADClientId },
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        IssuerSigningKeys = config.SigningKeys,
                        ValidateLifetime = true
                    };
                    JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();
                    System.Security.Claims.ClaimsPrincipal claimsPrincipal = tokendHandler.ValidateToken(accessToken, validationParameters, out SecurityToken jwt);
                    UserTokenValidateCacheManager.AddUserValidateTokenCache(accessToken, claimsPrincipal, jwt);
                    userTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);
                    httpContext.User = claimsPrincipal;
                    Thread.CurrentPrincipal = claimsPrincipal;
                }
                else
                {
                    httpContext.User = userValidateToken.ClaimsPrincipal;
                    Thread.CurrentPrincipal = userValidateToken.ClaimsPrincipal;
                    userTicket = new AuthenticationTicket(userValidateToken.ClaimsPrincipal, Scheme.Name);
                }
                return AuthenticateResult.Success(userTicket);
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail(AuthentcationConstant.UNAUTHORIZED);
            }
        }
        public async Task<SecurityToken> ValidateAZureADTokenAsync(string pToken)
        {
            try
            {
                string myIssuer = String.Format(CultureInfo.InvariantCulture, AzureADURL, AzureADTenentId);
                SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AzureADClientSecret));
                string stsDiscoveryEndpoint = String.Format(CultureInfo.InvariantCulture, AzureADURLForEndpoint, AzureADTenentId);
                ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());
                OpenIdConnectConfiguration config = await configManager.GetConfigurationAsync();
                IdentityModelEventSource.ShowPII = true;
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidIssuers = new[] { myIssuer },
                    ValidAudiences = new[] { AzureADClientId },
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    IssuerSigningKeys = config.SigningKeys,
                    ValidateLifetime = true
                };
                SecurityToken validatedToken = (SecurityToken)new JwtSecurityToken();
                System.Security.Claims.ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(pToken, validationParameters, out validatedToken);
                return validatedToken;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}