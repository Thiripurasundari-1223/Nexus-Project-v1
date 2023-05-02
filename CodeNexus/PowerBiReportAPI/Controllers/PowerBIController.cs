using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerBiReportAPI.Extensions;
using PowerBiReportAPI.Models;
using PowerBiReportAPI.Services;
using System;
using System.Text.Json;

namespace PowerBiReportAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PowerBIController : ControllerBase
    {
        private readonly PbiEmbedService _pbiEmbedService;
        private readonly IOptions<AzureAd> _azureAd;
        private readonly IOptions<PowerBI> _powerBI;

        public PowerBIController(PbiEmbedService pbiEmbedService, IOptions<AzureAd> azureAd, IOptions<PowerBI> powerBI)
        {
            _pbiEmbedService = pbiEmbedService;
            _azureAd = azureAd;
            _powerBI = powerBI;
        }

        [HttpGet]
        public IActionResult GetEmbedInfo(string reportName)
        {
            try
            {
                // Validate whether all the required configurations are provided in appsettings.json
                string configValidationResult = ConfigValidatorService.ValidateConfig(_azureAd, _powerBI);
                if (!string.IsNullOrEmpty(configValidationResult))
                {
                    HttpContext.Response.StatusCode = 400;
                    return Ok();
                }

                reportName = reportName.GetReportId();

                if (reportName == string.Empty)
                {
                    return Ok(new
                    {
                        StatusCode = "Failed",
                        StatusText = "Unable to fetch the Report Id",
                        Data = reportName
                    });
                }

                EmbedParams embedParams = _pbiEmbedService.GetEmbedParams(new Guid(_powerBI.Value.WorkspaceId), new Guid(reportName));
                var token = JsonSerializer.Serialize<EmbedParams>(embedParams);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Token Generated Successfully Successfully",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;
                return Ok(new
                {
                    StatusCode = "FAILED",
                    StatusText = "Token Generation Failed",
                    Data = ""
                });
            }
        }
    }
}
