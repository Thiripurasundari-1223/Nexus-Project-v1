using APIGateWay.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class PowerBiController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly HTTPClient _client;
        private readonly string _powerBiBaseURL = string.Empty;

        public PowerBiController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new();
            _powerBiBaseURL = _configuration.GetValue<string>("ApplicationURL:PowerBI:BaseURL");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetPowerBIAccessToken(string reportName)
        {
            var accessToken = await _client.GetAsync(_powerBiBaseURL, _configuration.GetValue<string>("ApplicationURL:PowerBI:GetEmbedInfo") + reportName);
            if (accessToken == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                StatusCode = "SUCCESS",
                StatusText = "Token Generated Successfully",
                Data = accessToken?.Data
            });
        }
    }
}
