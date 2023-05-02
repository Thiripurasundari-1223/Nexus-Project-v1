using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Services;

namespace Reports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportServices _reportServices;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ReportController(ReportServices reportServices)
        {
            _reportServices = reportServices;
        }
        #endregion

        #region Get Nexus Info
        /// <summary>
        /// Get Nexus Info
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNexusInfo")]
        public IActionResult GetNexusInfo()
        {
            try
            {
                var getReport = _reportServices.GetNexusInfo();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    ReportData = getReport
                });
            }
            catch (Exception ex)
            {
                logger.Error("Controller - ReportController, Action - GetProjectReport, Error - " + ex.Message.ToString());
                return BadRequest(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }

        }
        #endregion

        #region Get Account Report
        /// <summary>
        /// Get Account Report
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet("GetAccountReport")]
        public IActionResult GetAccountReport(int resourceId)
        {
            try
            {
                var getReport = _reportServices.GetAccountReport(resourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    ReportData = getReport
                });
            }
            catch (Exception ex)
            {
                logger.Error("Controller - ReportController, Action - GetProjectReport, Error - " + ex.Message.ToString());
                return BadRequest(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }

        }
        #endregion

        #region Get Project Report
        /// <summary>
        /// Get Project Report
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet("GetProjectReport")]
        public IActionResult GetProjectReport(int resourceId)
        {
            try
            {
                var getReport = _reportServices.GetProjectReport(resourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    ReportData = getReport
                });
            }
            catch (Exception ex)
            {
                logger.Error("Controller - ReportController, Action - GetProjectReport, Error - " + ex.Message.ToString());
                return BadRequest(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }

        }
        #endregion
         
        #region Get Resource Report
        /// <summary>
        /// Get Resource Report
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet("GetResourceReport")]
        public IActionResult GetResourceReport(int resourceId)
        {
            try
            {
                var getReport = _reportServices.GetResourceReport(resourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    ReportData = getReport
                });
            }
            catch (Exception ex)
            {
                logger.Error("Controller - ReportController, Action - GetResourceReport, Error - " + ex.Message.ToString());
                return BadRequest(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }
            
        }
        #endregion

        #region Get Timesheet Report
        /// <summary>
        /// Get Timesheet Report
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet("GetTimesheetReport")]
        public IActionResult GetTimesheetReport(int resourceId)
        {
            try
            {
                var getReport = _reportServices.GetTimesheetReport(resourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    ReportData = getReport
                });
            }
            catch (Exception ex)
            {
                logger.Error("Controller - ReportController, Action - GetTimesheetReport, Error - " + ex.Message.ToString());
                return BadRequest(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }

        }
        #endregion
    }
}
