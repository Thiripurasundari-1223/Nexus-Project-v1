using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace IAM.API.Handlers
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                     ILoggerFactory logger,
                                     UrlEncoder encoder,
                                     ISystemClock clock) : base(options, logger, encoder, clock)
        { }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
        }
    }
}