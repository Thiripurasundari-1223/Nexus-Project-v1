using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Timesheet;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Reports;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;
using System.Reflection;
using Timesheet.DAL.Services;

namespace Timesheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TimesheetServices _timesheetServices;
        private readonly string fromEmailId;
        private readonly int emailPort;
        private readonly string emailHost;
        private readonly string fromEmailPassword;
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Timesheet API - GET Method"
            });
        }

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cobServices"></param>
        /// <param name="iconfiguration"></param>
        /// <param name="cOBDBContext"></param>
        public TimesheetController(TimesheetServices timesheetServices, IConfiguration iconfiguration)
        {
            _configuration = iconfiguration;
            _timesheetServices = timesheetServices;
            fromEmailId = _configuration.GetValue<string>("EmailSettings:FromEmailId");
            fromEmailPassword = _configuration.GetValue<string>("EmailSettings:FromEmailPassword");
            emailPort = Convert.ToInt16(_configuration.GetValue<string>("EmailSettings:EmailPort"));
            emailHost = _configuration.GetValue<string>("EmailSettings:EmailHost");
        }
        #endregion

        #region Save or Update Timesheet
        /// <summary>
        /// Save or update the timesheet
        /// </summary>
        /// <param name="timesheetLog"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateTimesheetLog")]
        public IActionResult SaveOrUpdateTimesheetLog(List<TimesheetLogView> lstTimesheetLog)
        {
            List<TimesheetLogView> lstTimesheetLogResponse = new List<TimesheetLogView>();
            try
            {
                lstTimesheetLogResponse = _timesheetServices.SaveOrUpdateTimesheetLog(lstTimesheetLog).Result;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Timesheet(s) are saved successfully.",
                    Data = lstTimesheetLogResponse
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/SaveOrUpdateTimesheetLog", JsonConvert.SerializeObject(lstTimesheetLog));
                //logger.Error("Controller - TimesheetController, Action - InsertAndUpdateTimesheet, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = lstTimesheetLogResponse
                });
            }
        }
        #endregion

        #region Delete timesheet log
        /// <summary>
        /// Delete timesheet log
        /// </summary>
        /// <param name="TimesheetLogId"></param>
        /// <returns></returns>
        [HttpPost("DeleteTimesheetLog")]
        public IActionResult DeleteTimesheetLog(List<int> TimesheetLogId)
        {
            try
            {
                if (_timesheetServices.DeleteTimesheetLog(TimesheetLogId).Result)
                {
                    return Ok(new { StatusCode = "SUCCESS", StatusText = "Timesheet(s) are deleted successfully." });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/DeleteTimesheetLog", JsonConvert.SerializeObject(TimesheetLogId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }
        }
        #endregion

        #region Submit Timesheet 
        /// <summary>
        /// Submit Timesheet
        /// </summary>
        /// <param name="submitTimesheet"></param>
        /// <returns></returns>
        [HttpPost("SubmitTimesheet")]
        public IActionResult SubmitTimesheet(SubmitTimesheet submitTimesheet)
        {
            try
            {
                if (_timesheetServices.SubmitTimesheet(submitTimesheet).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Timesheet(s) are submitted successfully.",
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/SubmitTimesheet", JsonConvert.SerializeObject(submitTimesheet));
                //logger.Error("Controller - TimesheetController, Action - TimesheetSubmit, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }
        }
        #endregion 

        #region Get timesheet by timesheetId
        /// <summary>
        /// Get timesheet by timesheetId
        /// </summary>
        /// <param name="timesheetId"></param>
        /// <returns></returns>
        [HttpGet("GetTimesheetByTimesheetId")]
        public IActionResult GetTimesheetByTimesheetId(int timesheetId)
        {
            List<TimesheetLogView> lstTimesheet = new List<TimesheetLogView>();
            try
            {
                lstTimesheet = _timesheetServices.GetTimesheetByTimesheetId(timesheetId);
                return Ok(
                    new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = string.Empty,
                        Data = lstTimesheet == null?new List<TimesheetLogView>() : lstTimesheet
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetByTimesheetId", Convert.ToString(timesheetId));
                //logger.Error("Controller - TimesheetController, Action - GetTimesheetByTimesheetId, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = lstTimesheet
                });
            }

        }
        #endregion

        #region Approve or Reject timesheet by reporting manager
        /// <summary>
        /// Approve or Reject timesheet by reporting manager
        /// </summary>
        /// <param name="lstTimesheetLog"></param>
        /// <returns></returns>
        [HttpPost("ApproveOrRejectTimesheet")]
        public IActionResult ApproveOrRejectTimesheet(List<TimesheetStatusView> lstTimesheets)
        {
            try
            {
                string status = _timesheetServices.ApproveOrRejectTimesheet(lstTimesheets).Result;
                if (status != "")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Timesheet(s) are " + status + " successfully.",
                        Data = status
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/ApproveOrRejectTimesheet", JsonConvert.SerializeObject(lstTimesheets));
                //logger.Error("Controller - TimesheetController, Action - ApproveOrRejectTimesheet, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = ""
                });
            }

        }
        #endregion    

        #region Get resource timesheet by week
        /// <summary>
        /// Get resource timesheet by week
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpPost("GetResourceTimesheetByWeek")]
        public IActionResult GetResourceTimesheetByWeek(ProjectTimesheet projectTimesheet)
        {
            ResourceWeeklyTimesheet resourceWeeklyTimesheet = new ResourceWeeklyTimesheet();
            try
            {
                resourceWeeklyTimesheet = _timesheetServices.GetResourceTimesheetByWeek(projectTimesheet);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = resourceWeeklyTimesheet
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetResourceTimesheetByWeek", JsonConvert.SerializeObject(projectTimesheet));
                //logger.Error("Controller - TimesheetController, Action - GetResourceTimesheetByWeek, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = resourceWeeklyTimesheet
                });
            }
        }
        #endregion

        #region Add or Update comments
        /// <summary>
        /// Add or Update comments
        /// </summary>
        /// <param name="timesheetComments"></param>
        /// <returns></returns>
        [HttpPost("AddOrUpdateComments")]
        public IActionResult AddOrUpdateComments(TimesheetComments timesheetComments)
        {
            try
            {
                if (_timesheetServices.AddOrUpdateComments(timesheetComments).Result)
                {
                    return Ok(new { StatusCode = "SUCCESS" });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/AddOrUpdateComments", JsonConvert.SerializeObject(timesheetComments));
                //logger.Error("Controller - TimesheetController, Action - AddOrUpdateComments, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }
        }
        #endregion

        #region Delete comments
        /// <summary>
        /// Delete comments
        /// </summary>
        /// <param name="TimesheetCommentsId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteComments")]
        public IActionResult DeleteComments(int TimesheetCommentsId)
        {
            try
            {
                if (_timesheetServices.DeleteComments(TimesheetCommentsId).Result)
                {
                    return Ok(new { StatusCode = "SUCCESS" });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/DeleteComments", Convert.ToString(TimesheetCommentsId));
                //logger.Error("Controller - TimesheetController, Action - DeleteComments, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }
        }
        #endregion

        #region Delete Timesheet Logs
        /// <summary>
        /// Delete Timesheet Logs
        /// </summary>
        /// <param name="lstTimesheetLogIds"></param>
        /// <returns></returns>
        [HttpPost("DeleteTimesheetLogs")]
        public IActionResult DeleteTimesheetLogs(List<int> lstTimesheetLogIds)
        {
            try
            {
                if (_timesheetServices.DeleteTimesheetLogs(lstTimesheetLogIds).Result)
                {
                    return Ok(new { StatusCode = "SUCCESS" });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/DeleteTimesheetLogs", JsonConvert.SerializeObject(lstTimesheetLogIds));
                //logger.Error("Controller - TimesheetController, Action - DeleteComments, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message
                });
            }
        }
        #endregion

        #region Get Resource Team Timesheet
        [HttpPost("GetResourceTeamTimesheet")]
        public IActionResult GetResourceTeamTimesheet(TeamMember teamMember)
        {
            List<TeamTimesheet> teamTimesheets = new List<TeamTimesheet>();
            try
            {
                teamTimesheets = _timesheetServices.GetResourceTeamTimesheet(teamMember);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = teamTimesheets == null?new List<TeamTimesheet>() : teamTimesheets
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetResourceTeamTimesheet", JsonConvert.SerializeObject(teamMember));
                //logger.Error("Controller - TimesheetController, Action - GetResourceTeamTimesheet, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = teamTimesheets
                });
            }
        }
        #endregion

        #region Get Rejection Reason List
        /// <summary>
        /// Get Rejection Reason List
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <returns></returns>
        [HttpGet("GetRejectionReasonList")]
        public IActionResult GetRejectionReasonList()
        {
            List<RejectionReason> rejectionReasons = new List<RejectionReason>();
            try
            {
                rejectionReasons = _timesheetServices.GetRejectionReasonList();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = rejectionReasons ==  null ? new List<RejectionReason>() : rejectionReasons
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetRejectionReasonList");
                //logger.Error("Controller - TimesheetController, Action - GetRejectionReasonList, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = rejectionReasons
                });
            }
        }
        #endregion

        #region Get timesheet log by project id 
        /// <summary>
        /// Get timesheet log by project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTimesheetLogByProjectId")]
        public IActionResult GetTimesheetLogByProjectId(List<int?> lstProjectId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _timesheetServices.GetTimesheetLogByProjectId(lstProjectId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetLogByProjectId", JsonConvert.SerializeObject(lstProjectId));
                //logger.Error("Controller - TimesheetController, Action - GetTimesheetLogByProjectId, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<TimesheetLogView>()
                });
            }
        }
        #endregion

        #region Get Resource Id By Timesheet Id
        /// <summary>
        /// Get Resource Id By Timesheet Id
        /// </summary>
        /// <param name="pTimesheetId"></param>
        /// <returns></returns>
        [HttpGet("GetResourceIdByTimesheetId")]
        public IActionResult GetResourceIdByTimesheetId(int? pTimesheetId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _timesheetServices.GetReportingPersonIdByTimesheetId(pTimesheetId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetResourceIdByTimesheetId", Convert.ToString(pTimesheetId));
                //logger.Error("Controller - TimesheetController, Action - GetResourceIdByTimesheetId, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get timesheet report
        /// <summary>
        /// Get timesheet report
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTimesheetMasterData")]
        public IActionResult GetTimesheetMasterData()
        {
            TimesheetMasterData timesheetMasterData = new TimesheetMasterData();
            try
            {

                timesheetMasterData.Timesheet = _timesheetServices.GetAllTimesheet();
                timesheetMasterData.TimesheetLog = _timesheetServices.GetAllTimesheetLog();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timesheetMasterData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetMasterData");
                //logger.Error("Controller - TimesheetController, Action - GetTimesheetReport, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = timesheetMasterData
                });
            }
        }
        #endregion

        #region insert or update timesheetconfiguration        
        [HttpPost]
        [AllowAnonymous]
        [Route("InsertOrUpdateTimesheetConfiguration")]
        public IActionResult InsertOrUpdateTimesheetConfiguration(TimesheetConfigurationView timesheetConfigurationView)
        {
            try
            {
                int TimesheetConfigurationId = _timesheetServices.InsertOrUpdateTimesheetConfiguration(timesheetConfigurationView).Result;
                if (TimesheetConfigurationId > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Timesheet Configuration saved successfully.",
                        Data = TimesheetConfigurationId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/InsertOrUpdateTimesheetConfiguration", JsonConvert.SerializeObject(timesheetConfigurationView));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = 0 });
            }
        }
        #endregion

        #region Get Configuration Details
        [HttpGet]
        [Route("GetConfigurationDetails")]
        public IActionResult GetConfigurationDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _timesheetServices.GetConfigurationDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetConfigurationDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new TimesheetConfigurationView()
                });
            }
        }
        #endregion

        #region Get Timesheet Alert For Submission
        /// <summary>
        /// Get Timesheet Alert For Submission
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTimesheetAlertForSubmission")]
        public IActionResult GetTimesheetAlertForSubmission()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _timesheetServices.GetTimesheetAlertForSubmission()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetAlertForSubmission");
                //logger.Error("Controller - TimesheetController, Action - GetTimesheetAlertForSubmission, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new Tuple<List<int>, DateTime, bool>(new List<int>(), DateTime.Now, false)
                });
            }
        }
        #endregion

        #region Get Timesheet Alert For Approval
        /// <summary>
        /// Get Timesheet Alert For Approval
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTimesheetAlertForApproval")]
        public IActionResult GetTimesheetAlertForApproval()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _timesheetServices.GetTimesheetAlertForApproval()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetAlertForApproval");
                //logger.Error("Controller - TimesheetController, Action - GetTimesheetAlertForApproval, Error - " + ex.Message.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new Tuple<List<TimesheetAlertApproval>, DateTime,bool>(new List<TimesheetAlertApproval>(), DateTime.Now,false)
                });
            }
        }
        #endregion

        #region Get timesheet home report
        /// <summary>
        /// Get timesheet home report
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTimesheetHomeReport")]
        public IActionResult GetTimesheetHomeReport(ProjectTimesheet projectTimesheet)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _timesheetServices.GetTimesheetHomeReport(projectTimesheet)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetHomeReport");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new HomeReportData()
                });
            }
        }
        #endregion

        #region Get timesheet Utilization report By Resource Id
        [HttpGet("GetTimesheetUtilizationDetails")]
        public IActionResult GetTimesheetUtilizationDetails(int pResourceId)//, DateTime? pWeekStartDate
        {
            List<TimesheetUtilizationView> timesheetUtilizationData = new List<TimesheetUtilizationView>();
            try
            {
                timesheetUtilizationData = _timesheetServices.GetTimesheetUtilizationDetails(pResourceId);//, pWeekStartDate
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timesheetUtilizationData== null?new List<TimesheetUtilizationView>() : timesheetUtilizationData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetUtilizationDetails" + pResourceId);//+ pWeekStartDate
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = timesheetUtilizationData
                });
            }
        }
        #endregion

        #region Get timesheet Utilization report By Resource Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmployeeTimesheetUtilizationDetails")]
        public IActionResult GetEmployeeTimesheetUtilizationDetails(int pResourceId)
        {
            List<EmployeeTimeUtilizationView> timesheetUtilizationData = new List<EmployeeTimeUtilizationView>();
            try
            {
                timesheetUtilizationData = _timesheetServices.GetEmployeeTimesheetUtilizationDetails(pResourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timesheetUtilizationData == null ? new List<EmployeeTimeUtilizationView>() : timesheetUtilizationData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetEmployeeTimesheetUtilizationDetails" + pResourceId);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = timesheetUtilizationData
                });
            }
        }
        #endregion

        #region Get timesheet report By Resource Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmployeeTimesheetByEmployeeId")]
        public IActionResult GetEmployeeTimesheetByEmployeeId(int pResourceId)
        {
            List<EmployeeTimesheetHourView> timesheetData = new List<EmployeeTimesheetHourView>();
            try
            {
                timesheetData = _timesheetServices.GetEmployeeTimesheetByEmployeeId(pResourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timesheetData == null ? new List<EmployeeTimesheetHourView>() : timesheetData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetEmployeeTimesheetByEmployeeId" + pResourceId);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = timesheetData
                });
            }
        }
        #endregion

        #region Get timesheet report By Resource Id
        [HttpGet]
        [AllowAnonymous]
        [Route("GetTimesheetGridDetailByEmployeeId")]
        public IActionResult GetTimesheetGridDetailByEmployeeId(int pResourceId)
        {
            List<EmployeeTimesheetGridView> timesheetData = new List<EmployeeTimesheetGridView>();
            try
            {
                timesheetData = _timesheetServices.GetTimesheetGridDetailByEmployeeId(pResourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timesheetData == null ? new List<EmployeeTimesheetGridView>() : timesheetData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetGridDetailByEmployeeId" + pResourceId);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = timesheetData
                });
            }
        }
        #endregion
        #region Get timesheet by timesheet id
        [HttpPost]
        [Route("GetTimesheetLogByTimesheetId")]
        public IActionResult GetTimesheetLogByTimesheetId(List<int> timesheetId)
        {
            
            try
            {
                List<TimesheetLogView> timesheetData = _timesheetServices.GetTimesheetLogByTimesheetId(timesheetId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = timesheetData == null ? new List<TimesheetLogView>() : timesheetData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Timesheet", "Timesheet/GetTimesheetLogByTimesheetId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<TimesheetLogView>()
                });
            }
        }
        #endregion
    }
}