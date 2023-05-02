using APIGateWay.API.Common;
using APIGateWay.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Leaves;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using SharedLibraries.ViewModels.Notifications;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HTTPClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _timesheetBaseURL = string.Empty;
        private readonly string _projectBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _attendanceBaseURL = string.Empty;
        private readonly string _leavesBaseURL = string.Empty;
        Stopwatch stopWatch = new();
        private readonly string strErrorMsg = "Something went wrong, please try again later";
        private readonly CommonFunction _commonFunction;

        #region Constructor
        public TimesheetController(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _configuration = configuration;
            _timesheetBaseURL = _configuration.GetValue<string>("ApplicationURL:Timesheet:BaseURL");
            _projectBaseURL = _configuration.GetValue<string>("ApplicationURL:Projects:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _attendanceBaseURL = _configuration.GetValue<string>("ApplicationURL:Attendance:BaseURL");
            _leavesBaseURL = _configuration.GetValue<string>("ApplicationURL:Leaves:BaseURL");
            _commonFunction = new CommonFunction(configuration);
        }
        #endregion

        #region Save or Update Timesheet
        [HttpPost]
        [Route("SaveOrUpdateTimesheetLog")]
        public IActionResult SaveOrUpdateTimesheetLog(List<TimesheetLogView> lstTimesheetLog)
        {
            List<TimesheetLogView> lstTimesheet = new();
            try
            {
                var result = _client.PostAsJsonAsync(lstTimesheetLog, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:SaveOrUpdateTimesheetLog"));
                lstTimesheet = JsonConvert.DeserializeObject<List<TimesheetLogView>>(JsonConvert.SerializeObject(result.Result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        lstTimesheet
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/SaveOrUpdateTimesheetLog", JsonConvert.SerializeObject(lstTimesheetLog));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                lstTimesheet
            });
        }
        #endregion

        #region Delete timesheet log
        [HttpPost]
        [Route("DeleteTimesheetLog")]
        public IActionResult DeleteTimesheetLog(List<int> TimesheetLogId)
        {
            try
            {
                var result = _client.PostAsJsonAsync(TimesheetLogId, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:DeleteTimesheetLog"));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/DeleteTimesheetLog", JsonConvert.SerializeObject(TimesheetLogId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Submit Timesheet 
        [HttpPost]
        [Route("SubmitTimesheet")]
        public async Task<IActionResult> SubmitTimesheet(SubmitTimesheet submitTimesheet)
        {
            string statusText = "", statusCode = "";
            try
            {
                var projectresult = await _client.PostAsJsonAsync(submitTimesheet.TimesheetLog.Select(x => x.ProjectId).Distinct(), _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetProjectSPOCByProjectId"));
                submitTimesheet.ListOfProjectSPOC = JsonConvert.DeserializeObject<List<ProjectSPOC>>(JsonConvert.SerializeObject(projectresult?.Data));
                var result = await _client.PostAsJsonAsync(submitTimesheet, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:SubmitTimesheet"));
                if (result != null)
                {
                    statusCode = result.StatusCode;
                    statusText = result.StatusText;
                    await SendSubmitNotification(submitTimesheet);
                    return Ok(new
                    {
                        statusCode,
                        statusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/SubmitTimesheet", JsonConvert.SerializeObject(submitTimesheet));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText
            });
        }
        #endregion
        #region send Notification
        [NonAction]        
        public async Task<bool> SendSubmitNotification(SubmitTimesheet submitTimesheet)
        {
            
            try
            {
                    List<TeamTimesheet> teamTimesheets = new();
                    List<TeamTimesheet> finalTeamTimesheets = new();
                    DateTime firstDayOfWeek = DateTime.UtcNow;
                    string userName = "", reportingPersonName = "", weekInfo = "";
                    List<int?> empIds = new();
                    List<EmployeeName> lstEmployeeName = new();
                    int? resourceId = submitTimesheet?.TimesheetLog?[0].ResourceId;
                    if (submitTimesheet.TimesheetLog.Count > 0)
                    {
                        TimesheetLogView firstElement = submitTimesheet.TimesheetLog.FirstOrDefault();
                        if (firstElement != null)
                        {
                            firstDayOfWeek = firstElement.PeriodSelection.AddDays(DayOfWeek.Sunday - firstElement.PeriodSelection.DayOfWeek); // FirstDayOfWeek(firstElement.PeriodSelection);                        
                            weekInfo = firstDayOfWeek.ToString("MMM dd") + " - " + firstDayOfWeek.AddDays(6).ToString("MMM dd");
                        }
                    }
                    empIds.Add(resourceId);
                    foreach (ProjectSPOC projectSPOC in submitTimesheet?.ListOfProjectSPOC)
                    {
                        empIds.Add(projectSPOC.SPOCId);
                        teamTimesheets = GetTeamTimesheet((int)projectSPOC.SPOCId, firstDayOfWeek);
                        foreach (TeamTimesheet teamTimesheet in teamTimesheets)
                        {
                            finalTeamTimesheets.Add(teamTimesheet);
                        }
                    }
                    var employeeResult = await _client.PostAsJsonAsync(empIds, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetEmployeeNameById"));
                    lstEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeResult?.Data));
                    if (lstEmployeeName?.Count > 0)
                    {
                        userName = lstEmployeeName.Where(x => x.EmployeeId == resourceId).Select(x => x.EmployeeFullName).FirstOrDefault();
                    }
                    List<Notifications> notifications = new();
                    int? notificationTimesheetId = finalTeamTimesheets.Where(x => x.TimesheetId != null).Select(x => x.TimesheetId).FirstOrDefault();
                    NotificationDataView notificationData = new()
                    {
                        PrimaryKeyId = notificationTimesheetId == null ? 0 : (int)notificationTimesheetId,
                        WeekStartDate = firstDayOfWeek
                    };
                    Notifications notification = new()
                    {
                        CreatedBy = (int)resourceId,
                        CreatedOn = DateTime.UtcNow,
                        FromId = (int)resourceId,
                        ToId = (int)resourceId,
                        MarkAsRead = false,
                        NotificationSubject = "Timesheet sent for approval.",
                        NotificationBody = "Your timesheet for " + weekInfo + " has been sent for approval.",
                        PrimaryKeyId = notificationTimesheetId,
                        ButtonName = "View Timesheet",
                        SourceType = "Timesheet",
                        Data = JsonConvert.SerializeObject(notificationData)
                    };
                    notifications.Add(notification);
                    foreach (ProjectSPOC projectSPOC in submitTimesheet?.ListOfProjectSPOC)
                    {
                        notificationTimesheetId = finalTeamTimesheets.Where(x => x.TimesheetId != null).Select(x => x.TimesheetId).FirstOrDefault();
                        NotificationDataView notificationJsonData = new()
                        {
                            PrimaryKeyId = notificationTimesheetId == null ? 0 : (int)notificationTimesheetId,
                            WeekStartDate = firstDayOfWeek
                        };
                        reportingPersonName = lstEmployeeName.Where(x => x.EmployeeId == projectSPOC.SPOCId).Select(x => x.EmployeeFullName).FirstOrDefault();
                        notification = new()
                        {
                            CreatedBy = (int)resourceId,
                            CreatedOn = DateTime.UtcNow,
                            FromId = (int)resourceId,
                            ToId = (int)projectSPOC.SPOCId,
                            MarkAsRead = false,
                            NotificationSubject = "New timesheet request from " + userName,
                            NotificationBody = userName + "'s " + weekInfo + " timesheet request is waiting for your approval.",
                            PrimaryKeyId = notificationTimesheetId,
                            ButtonName = "Approve Timesheet",
                            SourceType = "Timesheet",
                            Data = JsonConvert.SerializeObject(notificationJsonData)
                        };
                        notifications.Add(notification);
                    }
                using var notificationClient = new HttpClient
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                };
                HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();

                int? employeeId = submitTimesheet?.TimesheetLog?.Select(x => x.ResourceId).FirstOrDefault();
                    
                    List<int> ManagerList = submitTimesheet?.ListOfProjectSPOC?.Select(x => x.SPOCId==null?0:(int)x.SPOCId).Distinct().ToList();
                    foreach (int managerId in ManagerList)
                    {
                        var empresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAndApproverDetails") + employeeId + "&approverId=" + managerId);
                        EmployeeandManagerView employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(empresults?.Data));
                        
                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                        string MailSubject = null, MailBody = null;

                    string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 600 + ">" +
                        "<tr bgcolor='#FFA93E'  style='text-align:center';>" +
                        "<td><b>Project Name</b></td>"+
                        "<td><b> " + firstDayOfWeek.ToString("dd MMM yyyy") + "</b></td >" +
                        "<td><b>" + firstDayOfWeek.AddDays(1).ToString("dd MMM yyyy") + "</b></td >" +
                        "<td><b>" + firstDayOfWeek.AddDays(2).ToString("dd MMM yyyy") + "</b></td >" +
                        "<td><b>" + firstDayOfWeek.AddDays(3).ToString("dd MMM yyyy") + "</b></td >" +
                        "<td><b>" + firstDayOfWeek.AddDays(4).ToString("dd MMM yyyy") + "</b></td >" +
                        "<td><b>" + firstDayOfWeek.AddDays(5).ToString("dd MMM yyyy") + "</b></td >" +
                        "<td><b>" + firstDayOfWeek.AddDays(6).ToString("dd MMM yyyy") + "</b></td >" + 
                        "<td><b>Total Hours</b></td></tr>";                    
                    List<int> projectList = submitTimesheet?.ListOfProjectSPOC?.Where(x => x.SPOCId == managerId).Select(x => x.projectId==null?0:(int)x.projectId).Distinct().OrderByDescending(x=>x).ToList();

                    foreach(int projectId in projectList)
                    {
                        
                        List<TimesheetLogView> totalClockedHours = submitTimesheet?.TimesheetLog?.Where(x => x.ProjectId == projectId).Select(x=>x).ToList();
                        long totalProjectHours = 0;
                        foreach(TimesheetLogView clocked in totalClockedHours)
                        {
                            if(!string.IsNullOrEmpty(clocked?.ClockedHours))
                            {
                                string[] hour = clocked?.ClockedHours.Split(":");
                                TimeSpan totalHours = new TimeSpan(hour?.Length > 0 ? Convert.ToInt32(hour[0]) : 0, hour?.Length > 1 ? Convert.ToInt32(hour[1]) : 0, 0);
                                totalProjectHours += totalHours.Ticks;
                                string hours = "",minutes = "";
                                if(hour?.Length > 0)
                                {
                                    hours = hour[0].Length > 1 ? hour[0] : ("0"+ hour[0]) ;
                                }
                                if (hour?.Length > 1)
                                {
                                    minutes = hour[1].Length > 1 ? hour[1] :("0"+ hour[1]) ;
                                }
                                clocked.ClockedHours = hours + ":" + minutes;
                            }
                        }
                        var dayOneClockedHours = submitTimesheet?.TimesheetLog?.Where(x => x.ProjectId == projectId && x.PeriodSelection.Date == firstDayOfWeek.Date).Select(x => x.ClockedHours).FirstOrDefault();
                        var dayTwoClockedHours = submitTimesheet?.TimesheetLog?.Where(x => x.ProjectId == projectId && x.PeriodSelection.Date == firstDayOfWeek.Date.AddDays(1).Date).Select(x => x.ClockedHours).FirstOrDefault();
                        var dayThreeClockedHours = submitTimesheet?.TimesheetLog?.Where(x => x.ProjectId == projectId && x.PeriodSelection.Date == firstDayOfWeek.Date.AddDays(2).Date).Select(x => x.ClockedHours).FirstOrDefault();
                        var dayfourClockedHours = submitTimesheet?.TimesheetLog?.Where(x => x.ProjectId == projectId && x.PeriodSelection.Date == firstDayOfWeek.Date.AddDays(3).Date).Select(x => x.ClockedHours).FirstOrDefault();
                        var dayfiveClockedHours = submitTimesheet?.TimesheetLog?.Where(x => x.ProjectId == projectId && x.PeriodSelection.Date == firstDayOfWeek.Date.AddDays(4).Date).Select(x => x.ClockedHours).FirstOrDefault();
                        var daysixClockedHours = submitTimesheet?.TimesheetLog?.Where(x => x.ProjectId == projectId && x.PeriodSelection.Date == firstDayOfWeek.Date.AddDays(5).Date).Select(x => x.ClockedHours).FirstOrDefault();
                        var daysevenClockedHours = submitTimesheet?.TimesheetLog?.Where(x => x.ProjectId == projectId && x.PeriodSelection.Date == firstDayOfWeek.Date.AddDays(6).Date).Select(x => x.ClockedHours).FirstOrDefault();

                        TimeSpan sumOfdaysevenClockedHours = new TimeSpan(totalProjectHours);
                        string projectName = submitTimesheet?.ListOfProjectSPOC?.Where(x => x.projectId == projectId).Select(x => x.projectName).FirstOrDefault();
                        textBody += "<tr style='text-align:center';> <td>" + (projectName == null ? "" : projectName) + "</td>";
                        textBody += "<td>" + (dayOneClockedHours == null ? "00:00" : dayOneClockedHours) + "</td>";

                        textBody += "<td>" + (dayTwoClockedHours == null ? "00:00" : dayTwoClockedHours) + "</td>";

                        textBody += "<td>" + (dayThreeClockedHours == null ? "00:00" : dayThreeClockedHours) + "</td>";

                        textBody += "<td>" + (dayfourClockedHours == null ? "00:00" : dayfourClockedHours) + "</td>";

                        textBody += "<td>" + (dayfiveClockedHours == null ? "00:00" : dayfiveClockedHours) + "</td>";

                        textBody += "<td>" + (daysixClockedHours == null ? "00:00" : daysixClockedHours) + "</td>";

                        textBody += "<td>" + (daysevenClockedHours == null ? "00:00" : daysevenClockedHours) + "</td>";

                       string totalProHours= ((sumOfdaysevenClockedHours.Days * 24) + sumOfdaysevenClockedHours.Hours).ToString();
                       string totalProMinutes = sumOfdaysevenClockedHours.Minutes.ToString();
                        textBody += "<td>" + (totalProHours.Length > 1 ? totalProHours : ("0" + totalProHours)) +":"+ (totalProMinutes.Length > 1 ? totalProMinutes : ("0" + totalProMinutes)) + "</td></tr >";
                    }

                    
                    textBody += "</table>";

                    string baseURL = appsetting.GetSection("BaseURL").Value;
                        MailSubject = @"{EmployeeName} has sent a timesheet request";
                        MailBody = @"<html>
                                    <body>
                                    <p>Dear {ManagerName},</p>   
                                    <p>{EmployeeName} timesheet has been submitted and is waiting for your approval. Please click <a href='{link}/#/pmsnexus/timesheets/all-timesheets/team-timesheets'>here</a> to Approve/Reject.</p>  
                                    <div>{timelogBody}</div>
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>
                                    </html>";
                        MailSubject = MailSubject.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                        MailBody = MailBody.Replace("{ManagerName}", employeeandManager?.ManagerName);
                        MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                        MailBody = MailBody.Replace("{timelogBody}", textBody);
                        MailBody = MailBody.Replace("{Feedback}", submitTimesheet?.TimesheetLog?.Select(x => x.Comments).FirstOrDefault());
                        MailBody = MailBody.Replace("{link}", baseURL);
                        SendEmailView sendMailbyleaverequest = new();
                        sendMailbyleaverequest = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = employeeandManager?.ManagerEmailID,
                            Subject = MailSubject,
                            MailBody = MailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                            CC = employeeandManager?.EmployeeEmailID
                        };
                        string mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                    }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/SendSubmitNotification", "");
                return false;
            }
            return true;
        }
        #endregion
        #region Get timesheet by timesheetId 
        [HttpGet]
        [Route("GetTimesheetByTimesheetId")]
        public IActionResult GetTimesheetByTimesheetId(int timesheetId)
        {
            List<TimesheetLogView> ListOfTimesheet = new();
            try
            {
                var result = _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:SubmitTimesheet") + timesheetId);
                ListOfTimesheet = JsonConvert.DeserializeObject<List<TimesheetLogView>>(JsonConvert.SerializeObject(result.Result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        ListOfTimesheet
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetTimesheetByTimesheetId", Convert.ToString(timesheetId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfTimesheet
            });
        }
        #endregion

        #region Approve or Reject timesheet by reporting manager
        [HttpPost]
        [Route("ApproveOrRejectTimesheet")]
        public async Task<IActionResult> ApproveOrRejectTimesheet(List<TimesheetStatusView> lstTimesheets)
        {
            string statusText = "", statusCode = "";
            try
            {
                var result = await _client.PostAsJsonAsync(lstTimesheets, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:ApproveOrRejectTimesheet"));
                if (result != null)
                {
                    statusCode = result.StatusCode;
                    statusText = result.StatusText;
                    SendApproveNotification(lstTimesheets,result);
                    return Ok(new
                    {
                        statusCode,
                        statusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/ApproveOrRejectTimesheet", JsonConvert.SerializeObject(lstTimesheets));
                statusText = strErrorMsg;
                statusCode = "FAILURE";
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText
            });
        }
        #endregion

        #region send Notification
        [NonAction]
        public async Task<bool> SendApproveNotification(List<TimesheetStatusView> lstTimesheets, SuccessData result)
        
        {

            try
            {
                List<int?> empIds = new();
                string reportingPersonName = "";
                int reportingPersonId = (int)(lstTimesheets?[0].ResourceId), resourceId = 0;
                List<EmployeeName> lstEmployeeName = new();
                var resultResourceInfo = await _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetResourceIdByTimesheetId") + lstTimesheets[0].TimesheetId);
                if (resultResourceInfo?.Data > 0)
                {
                    resourceId = (int)resultResourceInfo?.Data;
                }
                empIds.Add(reportingPersonId);
                var employeeResult = await _client.PostAsJsonAsync(empIds, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetEmployeeNameById"));
                lstEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeResult?.Data));
                if (lstEmployeeName?.Count > 0) reportingPersonName = lstEmployeeName[0].EmployeeFullName;
                string notificationSubject;
                string notificationBody;
                DateTime weekStartDate = (DateTime)lstTimesheets?[0].WeekStartDate;
                string weekInfo = lstTimesheets?[0].WeekStartDate.Value.ToString("MMM dd") + " - " + lstTimesheets?[0].WeekStartDate.Value.AddDays(6).ToString("MMM dd");
                if (result.Data == "rejected")
                {
                    notificationSubject = "Timesheet request is rejected";
                    notificationBody = reportingPersonName + " has rejected your request for the Timesheet " + weekInfo + ".";
                }
                else
                {
                    notificationSubject = "Timesheet request is approved";
                    notificationBody = reportingPersonName + " has approved your request for the Timesheet " + weekInfo + ".";
                }
                List<Notifications> notifications = new();
                NotificationDataView notificationData = new()
                {
                    PrimaryKeyId = lstTimesheets[0].TimesheetId,
                    WeekStartDate = lstTimesheets?[0].WeekStartDate
                };
                Notifications notification = new()
                {
                    CreatedBy = reportingPersonId,
                    CreatedOn = DateTime.UtcNow,
                    FromId = reportingPersonId,
                    ToId = resourceId,
                    MarkAsRead = false,
                    NotificationSubject = notificationSubject,
                    NotificationBody = notificationBody,
                    PrimaryKeyId = lstTimesheets[0].TimesheetId,
                    ButtonName = "View Timesheet",
                    SourceType = "Timesheet",
                    Data = JsonConvert.SerializeObject(notificationData)
                };
                notifications.Add(notification);
                using var notificationClient = new HttpClient
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                };
                HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();

                var empresults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAndApproverDetails") + resourceId + "&approverId=" + reportingPersonId);
                EmployeeandManagerView employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(empresults?.Data));

                //Get Timelog details
                List<int> timesheetId = lstTimesheets?.Select(x => x.TimesheetId).Distinct().OrderByDescending(x=>x).ToList();
                var timelogDetails =await _client.PostAsJsonAsync(timesheetId, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetLogByTimesheetId"));
                //await notificationClient.PostAsJsonAsync("Timesheet/GetTimesheetLogByTimesheetId", timesheetId);
                List<TimesheetLogView> timesheetData = JsonConvert.DeserializeObject<List<TimesheetLogView>>(JsonConvert.SerializeObject(timelogDetails?.Data));

                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string MailSubject = null, MailBody = null;
                string baseURL = appsetting.GetSection("BaseURL").Value;
                MailSubject = "Your request for timesheet is {Status}";
                string textBody = " <table border=" + 1 + " style='border-collapse:collapse' cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 600 + ">" +
                       "<tr bgcolor='#FFA93E'  style='text-align:center';>" +
                       "<td><b>Project Name</b></td>" +
                       "<td><b> " + weekStartDate.ToString("dd MMM yyyy") + "</b></td >" +
                       "<td><b>" + weekStartDate.AddDays(1).ToString("dd MMM yyyy") + "</b></td >" +
                       "<td><b>" + weekStartDate.AddDays(2).ToString("dd MMM yyyy") + "</b></td >" +
                       "<td><b>" + weekStartDate.AddDays(3).ToString("dd MMM yyyy") + "</b></td >" +
                       "<td><b>" + weekStartDate.AddDays(4).ToString("dd MMM yyyy") + "</b></td >" +
                       "<td><b>" + weekStartDate.AddDays(5).ToString("dd MMM yyyy") + "</b></td >" +
                       "<td><b>" + weekStartDate.AddDays(6).ToString("dd MMM yyyy") + "</b></td >" +
                       "<td><b>Total Hours</b></td>" +
                       "<td><b>Status</b></td></tr>";

                var projectresult = await _client.PostAsJsonAsync(timesheetData?.Select(x => x.ProjectId).Distinct(), _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetProjectSPOCByProjectId"));
                List < ProjectSPOC > projectDetails = JsonConvert.DeserializeObject<List<ProjectSPOC>>(JsonConvert.SerializeObject(projectresult?.Data));
                string projectOverallStatus = "Approved";
                string comments = "";
                foreach (int timesheet in timesheetId)
                {
                    List<TimesheetLogView> timelogList = timesheetData.Where(x => x.TimesheetId == timesheet).Select(x => x).ToList();
                    long totalProjectHours = 0;
                    foreach (TimesheetLogView clocked in timelogList)
                    {
                        if (!string.IsNullOrEmpty(clocked?.ClockedHours))
                        {
                            string[] hour = clocked?.ClockedHours.Split(":");
                            TimeSpan totalHours = new TimeSpan(hour?.Length > 0 ? Convert.ToInt32(hour[0]) : 0, hour?.Length > 1 ? Convert.ToInt32(hour[1]) : 0, 0);
                            totalProjectHours += totalHours.Ticks;
                            string hours = "", minutes = "";
                            if (hour?.Length > 0)
                            {
                                hours = hour[0].Length > 1 ? hour[0] : ("0" + hour[0]);
                            }
                            if (hour?.Length > 1)
                            {
                                minutes = hour[1].Length > 1 ? hour[1] : ("0" + hour[1]);
                            }
                            clocked.ClockedHours = hours + ":" + minutes;
                        }
                    }
                    
                    var dayOneClockedHours = timelogList?.Where(x => x.TimesheetId == timesheet && x.PeriodSelection.Date == weekStartDate.Date).Select(x => x.ClockedHours).FirstOrDefault();
                    var dayTwoClockedHours = timelogList?.Where(x => x.TimesheetId == timesheet && x.PeriodSelection.Date == weekStartDate.Date.AddDays(1).Date).Select(x => x.ClockedHours).FirstOrDefault();
                    var dayThreeClockedHours = timelogList?.Where(x => x.TimesheetId == timesheet && x.PeriodSelection.Date == weekStartDate.Date.AddDays(2).Date).Select(x => x.ClockedHours).FirstOrDefault();
                    var dayfourClockedHours = timelogList?.Where(x => x.TimesheetId == timesheet && x.PeriodSelection.Date == weekStartDate.Date.AddDays(3).Date).Select(x => x.ClockedHours).FirstOrDefault();
                    var dayfiveClockedHours = timelogList?.Where(x => x.TimesheetId == timesheet && x.PeriodSelection.Date == weekStartDate.Date.AddDays(4).Date).Select(x => x.ClockedHours).FirstOrDefault();
                    var daysixClockedHours = timelogList?.Where(x => x.TimesheetId == timesheet && x.PeriodSelection.Date == weekStartDate.Date.AddDays(5).Date).Select(x => x.ClockedHours).FirstOrDefault();
                    var daysevenClockedHours = timelogList?.Where(x => x.TimesheetId == timesheet && x.PeriodSelection.Date == weekStartDate.Date.AddDays(6).Date).Select(x => x.ClockedHours).FirstOrDefault();

                    TimeSpan sumOfdaysevenClockedHours = new TimeSpan(totalProjectHours);
                    int projectId = timelogList.Where(x => x.TimesheetId == timesheet).Select(x => x.ProjectId==null?0:(int)x.ProjectId).FirstOrDefault();
                    string projectName = projectDetails?.Where(x => x.projectId == projectId).Select(x => x.projectName).FirstOrDefault();
                    bool projectStatus = lstTimesheets.Where(x => x.TimesheetId == timesheet).Select(x => x.IsApproved).FirstOrDefault();
                    if(projectStatus == false)
                    {
                        projectOverallStatus = "Rejected";
                    }
                    string timesheetComments= lstTimesheets.Where(x => x.TimesheetId == timesheet).Select(x => x.Comments).FirstOrDefault();
                    if(!string.IsNullOrEmpty(timesheetComments))
                    {
                        comments += timesheetComments + "<br/>" ;
                    }                    
                    textBody += "<tr style='text-align:center';> <td>" + (projectName == null ? "" : projectName) + "</td>";
                    textBody += "<td>" + (dayOneClockedHours == null ? "00:00" : dayOneClockedHours) + "</td>";

                    textBody += "<td>" + (dayTwoClockedHours == null ? "00:00" : dayTwoClockedHours) + "</td>";

                    textBody += "<td>" + (dayThreeClockedHours == null ? "00:00" : dayThreeClockedHours) + "</td>";

                    textBody += "<td>" + (dayfourClockedHours == null ? "00:00" : dayfourClockedHours) + "</td>";

                    textBody += "<td>" + (dayfiveClockedHours == null ? "00:00" : dayfiveClockedHours) + "</td>";

                    textBody += "<td>" + (daysixClockedHours == null ? "00:00" : daysixClockedHours) + "</td>";

                    textBody += "<td>" + (daysevenClockedHours == null ? "00:00" : daysevenClockedHours) + "</td>";

                    string totalProHours = ((sumOfdaysevenClockedHours.Days * 24) + sumOfdaysevenClockedHours.Hours).ToString();
                    string totalProMinutes = sumOfdaysevenClockedHours.Minutes.ToString();
                    textBody += "<td>" + (totalProHours.Length > 1 ? totalProHours : ("0" + totalProHours)) + ":" + (totalProMinutes.Length > 1 ? totalProMinutes : ("0" + totalProMinutes)) + "</td>";
                    
                    textBody += "<td>" + (projectStatus?"Approved":"Rejected") + "</td></tr >";
                }

                MailBody = @"<html>
                                    <body>                                  
                                    <p>Dear {EmployeeName},</p>                                    
                                    <p>Your timesheet request has been {Status}. Please click <a href='{link}/#/pmsnexus/timesheets/all-timesheets/my-timesheets'>here</a> to view.</p>                                   
                                    <div>{timelogBody}</div>
                                    <table><tbody><tr><td><p><b>Comments : </b>{Feedback}</p></td></tr></tbody></table>
                                    </body>                                   
                                    </html>";

                MailSubject = MailSubject.Replace("{Status}", projectOverallStatus?.ToLower());
                MailBody = MailBody.Replace("{EmployeeName}", employeeandManager?.EmployeeName);
                MailBody = MailBody.Replace("{Feedback}", comments);
                MailBody = MailBody.Replace("{timelogBody}", textBody);
                MailBody = MailBody.Replace("{Status}", projectOverallStatus?.ToLower());
                MailBody = MailBody.Replace("{link}", baseURL);

                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = employeeandManager?.EmployeeEmailID,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = employeeandManager?.ManagerEmailID
                };
                string leaveMail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/SendApproveNotification", "");
                return false;
            }
            return true;
        }
        #endregion

        #region Get resource timesheet by week
        [HttpGet]
        [Route("GetResourceTimesheetByWeek")]
        public IActionResult GetResourceTimesheetByWeek(int resourceId, DateTime weekStartDay, int SPOCId = 0, int timesheetId = 0)
        {
            string statusText = "";
            DateTime startDate = weekStartDay;
            DateTime endDate = startDate.AddDays(6);
            ResourceWeeklyTimesheet resourceWeeklyTimesheet = new()
            {
                LstTimesheetSet = new List<TimesheetSet>(),
                IsSubmited = false,
                IsApproved = false,
                IsRejected = false,
                ListOfProject = new List<ResourceProjectList>(),
                WeeklyTimesheetComments = new List<WeeklyTimesheetComments>(),
                EmployeeAttendanceHours = new List<EmployeeAttendanceHours>(),
                EmployeeHolidayList = new List<Holiday>(),
                EmployeeLeaveList = new List<EmployeeLeavesForTimeSheetView>(),
                EmployeeShiftTiming = new TimeandWeekendDefinitionView()
            };
            List<EmployeeName> lstEmployeeName = new();
            try
            {
                var projectResult = _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetProjectTimesheet") + resourceId);
                ProjectTimesheet projectTimesheet = JsonConvert.DeserializeObject<ProjectTimesheet>(JsonConvert.SerializeObject(projectResult.Result.Data));
                projectTimesheet.ResourceId = resourceId;
                projectTimesheet.WeekStartDate = weekStartDay;
                projectTimesheet.SPOCId = SPOCId;
                projectTimesheet.TimesheetId = timesheetId;
                var result = _client.PostAsJsonAsync(projectTimesheet, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetResourceTimesheetByWeek"));
                resourceWeeklyTimesheet = JsonConvert.DeserializeObject<ResourceWeeklyTimesheet>(JsonConvert.SerializeObject(result.Result.Data));
                var empList = resourceWeeklyTimesheet?.WeeklyTimesheetComments?.Select(x => x.CreatedById).ToList();
                if (empList?.Count > 0)
                {
                    var employeeResult = _client.PostAsJsonAsync(resourceWeeklyTimesheet?.WeeklyTimesheetComments?.Select(x => x.CreatedById).ToList(), _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetEmployeeNameById"));
                    lstEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeResult.Result.Data));
                    //Assign comments created by
                    resourceWeeklyTimesheet.WeeklyTimesheetComments.ForEach(x => x.CreatedBy = lstEmployeeName.Where(y => y.EmployeeId == x.CreatedById).Select(x => x.EmployeeFullName).FirstOrDefault());
                }
                AttendanceWeekView attendanceWeekView = new();
                attendanceWeekView.EmployeeId = resourceId;
                attendanceWeekView.DateTime = weekStartDay;
                attendanceWeekView.FromDate = startDate;
                attendanceWeekView.ToDate = endDate;
                var attendanceResult = _client.PostAsJsonAsync(attendanceWeekView, _attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAttendanceDetailByEmployeeId"));
                List<DetailsView> attendanceDetails = JsonConvert.DeserializeObject<List<DetailsView>>(JsonConvert.SerializeObject(attendanceResult.Result.Data));
                List<EmployeeAttendanceHours> employeeAttendanceHours = new();
                if (attendanceDetails?.Count > 0)
                {
                    employeeAttendanceHours = new();
                    employeeAttendanceHours = (from attendance in attendanceDetails
                                               where attendance.EmployeeId == resourceId && attendance.Date >= startDate && attendance.Date <= endDate
                                               select new EmployeeAttendanceHours
                                               { EmployeeId = attendance.EmployeeId, TotalHours = attendance.TotalHours, Date = attendance.Date }).ToList();
                    resourceWeeklyTimesheet.EmployeeAttendanceHours = employeeAttendanceHours;
                }
                var employeedeptandlocation = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDepartmentAndLocation") + resourceId);
                EmployeeDepartmentAndLocationView employeeDepartmentAndLocationdetails = JsonConvert.DeserializeObject<EmployeeDepartmentAndLocationView>(JsonConvert.SerializeObject(employeedeptandlocation.Result.Data));

                int? shiftId = 0;
                TimeandWeekendDefinitionView shifttotaltime = new TimeandWeekendDefinitionView();
                var shiftDetais = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetEmployeeTimeandWeekendbyShiftID"));
                List<ShiftTimeandWeekendView> WeekendList = JsonConvert.DeserializeObject<List<ShiftTimeandWeekendView>>(JsonConvert.SerializeObject(shiftDetais?.Result.Data));
                shifttotaltime.ShiftTime = new ShiftTimeDefinition();
                shifttotaltime.WeekEndNameList = new List<WeekendShiftName>();
                List<WeekendShiftName> weekEndList = new List<WeekendShiftName>();
                if (employeeDepartmentAndLocationdetails?.EmployeeShiftDetails?.Count > 0)
                {
                    foreach (EmployeeShiftDetailsView item in employeeDepartmentAndLocationdetails?.EmployeeShiftDetails)
                    {
                        for(DateTime day= weekStartDay.Date; day<= weekStartDay.AddDays(6); day=day.AddDays(1))
                        {
                            if (weekStartDay.Date >= item.ShiftFromDate && (item.ShiftToDate == null || weekStartDay.Date <= item.ShiftToDate))
                            {
                                weekEndList = weekEndList.Concat(WeekendList?.Where(x => x.ShiftDetailsId == item.ShiftDetailsId).Select(x => x.WeekEndNameList).FirstOrDefault()).ToList();
                            }
                        }
                        if (weekStartDay.Date >= item.ShiftFromDate && (item.ShiftToDate == null || weekStartDay.Date <= item.ShiftToDate))
                        {
                            shifttotaltime.ShiftTime.TotalHours = WeekendList?.Where(x => x.ShiftDetailsId == item.ShiftDetailsId).Select(x => x.TotalHours).FirstOrDefault();
                            shifttotaltime.ShiftTime.PresentHour = WeekendList?.Where(x => x.ShiftDetailsId == item.ShiftDetailsId).Select(x => x.TotalHours).FirstOrDefault();
                            //shifttotaltime.WeekEndNameList = WeekendList?.Where(x => x.ShiftDetailsId == item.ShiftDetailsId).Select(x => x.WeekEndNameList).FirstOrDefault();

                            shiftId = WeekendList?.Where(x => x.ShiftDetailsId == item.ShiftDetailsId).Select(x => x.ShiftDetailsId).FirstOrDefault();
                        }
                    }
                    if(weekEndList?.Count>0)
                    {
                        shifttotaltime.WeekEndNameList = weekEndList.GroupBy(x=>x.WeekEndID).Select(x => x.FirstOrDefault()).ToList();
                    }
                    
                }

                    if (shifttotaltime?.ShiftTime?.TotalHours == null)
                    {
                        shifttotaltime.ShiftTime.TotalHours = WeekendList?.Where(x => x.IsGenralShift == true).Select(x => x.TotalHours).FirstOrDefault();
                        shifttotaltime.ShiftTime.PresentHour = WeekendList?.Where(x => x.IsGenralShift == true).Select(x => x.TotalHours).FirstOrDefault();
                        shifttotaltime.WeekEndNameList = WeekendList?.Where(x => x.IsGenralShift == true).Select(x => x.WeekEndNameList).FirstOrDefault();
                        shiftId = WeekendList?.Where(x => x.IsGenralShift == true).Select(x => x.ShiftDetailsId).FirstOrDefault();
                        if (shifttotaltime?.ShiftTime?.TotalHours == null)
                        {
                            shifttotaltime.ShiftTime.TotalHours = WeekendList?.Select(x => x.TotalHours).FirstOrDefault();
                            shifttotaltime.ShiftTime.PresentHour = WeekendList?.Select(x => x.TotalHours).FirstOrDefault();
                            shifttotaltime.WeekEndNameList = WeekendList?.Select(x => x.WeekEndNameList).FirstOrDefault();
                            shiftId = WeekendList?.Select(x => x.ShiftDetailsId).FirstOrDefault();
                        }
                    }
              
                

                HolidayInput holidayInput = new()
                {
                    DepartmentId = employeeDepartmentAndLocationdetails.DepartmentId,
                    FromDate = startDate,
                    ToDate = endDate,
                    LocationId = employeeDepartmentAndLocationdetails.LocationId,
                    ShiftDetailsId = shiftId == null ? 0 : (int)shiftId
                };
                var holidaybydept = _client.PostAsJsonAsync(holidayInput, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetHolidaybyDeptandLocandShifandDate"));
                List<Holiday> Holidaylist = JsonConvert.DeserializeObject<List<Holiday>>(JsonConvert.SerializeObject(holidaybydept.Result.Data));
                if (Holidaylist?.Count > 0)
                {
                    resourceWeeklyTimesheet.EmployeeHolidayList = Holidaylist.Where(x=>x.IsRestrictHoliday == false).Select(x=>x).ToList();

                }

                if (shifttotaltime != null && shifttotaltime.ShiftTime?.TotalHours != null)
                {
                    string[] Hours = shifttotaltime?.ShiftTime?.TotalHours.Split(":");
                    TimeSpan? TotalHours = new TimeSpan(Hours?.Length > 0 ? Convert.ToInt32(Hours[0]) : 0, Hours?.Length > 1 ? Convert.ToInt32(Hours[1]) : 0, Hours?.Length > 2 ? Convert.ToInt32(Hours[2]) : 0);
                    int workingDays = 7;
                    if (shifttotaltime?.WeekEndNameList?.Count > 0)
                    {
                        workingDays = workingDays - (shifttotaltime.WeekEndNameList.Select(x => x.WeekEndID).Distinct().Count());
                    }
                    if(resourceWeeklyTimesheet?.EmployeeHolidayList?.Count>0)
                    {
                        workingDays = workingDays - (resourceWeeklyTimesheet.EmployeeHolidayList.Select(x => x.HolidayID).Distinct().Count());
                    }
                    TimeSpan totalHour = new TimeSpan(TotalHours.Value.Ticks * workingDays);
                    shifttotaltime.ShiftTime.TotalHours = (((24*totalHour.Days)+ totalHour.Hours).ToString().Length > 1 ? ((24 * totalHour.Days) + totalHour.Hours).ToString() : "0" + ((24 * totalHour.Days) + totalHour.Hours).ToString()) + "h" + " " + (totalHour.Minutes.ToString().Length > 1 ? totalHour.Minutes.ToString() : "0" + totalHour.Minutes.ToString()) + "m";
                    resourceWeeklyTimesheet.EmployeeShiftTiming = shifttotaltime;
                }
                else resourceWeeklyTimesheet.EmployeeShiftTiming = new();

                if (!string.IsNullOrEmpty(resourceWeeklyTimesheet?.EmployeeShiftTiming?.ShiftTime?.PresentHour))
                {
                    string[] Hours = shifttotaltime?.ShiftTime?.PresentHour.Split(":");
                    TimeSpan TotalHours = new TimeSpan(Hours?.Length > 0 ? Convert.ToInt32(Hours[0]) : 0, Hours?.Length > 1 ? Convert.ToInt32(Hours[1]) : 0, Hours?.Length > 2 ? Convert.ToInt32(Hours[2]) : 0);

                    //Find preload hours
                    if (resourceWeeklyTimesheet?.ResourceTimeList?.Count > 0)
                    {
                        foreach (ResourceTimeList data in resourceWeeklyTimesheet.ResourceTimeList)
                        {
                            if (data.AllocationPercent != null)
                            {
                                TimeSpan preLoad = new TimeSpan((long)(data.AllocationPercent * TotalHours.Ticks) / 100);
                                data.PreLoadedHours = (preLoad.Hours.ToString().Length > 1 ? preLoad.Hours.ToString() : "0" + preLoad.Hours.ToString()) + ":" + (preLoad.Minutes.ToString().Length > 1 ? preLoad.Minutes.ToString() : "0" + preLoad.Minutes.ToString());
                            }

                        }
                    }
                }

                EmployeeLeavesForTimeSheetViewInput employeeLeavesForTimeSheetViewInput = new()
                {
                    resourceId = resourceId,
                    fromDate = startDate,
                    toDate = endDate
                };
                var employeeleave = _client.PostAsJsonAsync(employeeLeavesForTimeSheetViewInput, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeLeavesForTimesheet"));
                List<EmployeeLeavesForTimeSheetView> EmployeeLeavelist = JsonConvert.DeserializeObject<List<EmployeeLeavesForTimeSheetView>>(JsonConvert.SerializeObject(employeeleave.Result.Data));
                if (EmployeeLeavelist?.Count > 0)
                {
                    resourceWeeklyTimesheet.EmployeeLeaveList = EmployeeLeavelist;
                }
                
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        ListOfTimesheet = resourceWeeklyTimesheet
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetResourceTimesheetByWeek", " ResourceId- " + resourceId.ToString() + " WeekStartDay- " + weekStartDay.ToString() + " SPOCId- " + SPOCId.ToString() + " TimesheetId- " + timesheetId.ToString());
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                ListOfTimesheet = resourceWeeklyTimesheet
            });
        }
        #endregion

        #region Add or Update comments
        [HttpPost]
        [Route("AddOrUpdateComments")]
        public IActionResult AddOrUpdateComments(TimesheetComments timesheetComments)
        {
            try
            {
                var result = _client.PostAsJsonAsync(timesheetComments, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:AddOrUpdateComments"));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/AddOrUpdateComments", JsonConvert.SerializeObject(timesheetComments));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Delete comments
        [HttpDelete]
        [Route("DeleteComments")]
        public IActionResult DeleteComments(int TimesheetCommentsId)
        {
            try
            {
                var result = _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:DeleteComments") + TimesheetCommentsId);
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/DeleteComments", Convert.ToString(TimesheetCommentsId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Delete Timesheet Logs
        [HttpPost]
        [Route("DeleteTimesheetLogs")]
        public IActionResult DeleteTimesheetLogs(List<int> lstTimesheetLogIds)
        {
            try
            {
                var result = _client.PostAsJsonAsync(lstTimesheetLogIds, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:DeleteTimesheetLogs"));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/DeleteTimesheetLogs", JsonConvert.SerializeObject(lstTimesheetLogIds));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get resource project list
        [HttpGet]
        [Route("GetResourceProjectList")]
        public IActionResult GetResourceProjectList(int ResourceId = 0)
        {
            List<ResourceProjectList> resourceProjectList = new();
            try
            {
                var result = _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetResourceProjectList") + ResourceId);
                resourceProjectList = JsonConvert.DeserializeObject<List<ResourceProjectList>>(JsonConvert.SerializeObject(result.Result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        ListOfTimesheet = resourceProjectList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetResourceProjectList", Convert.ToString(ResourceId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfTimesheet = resourceProjectList
            });
        }
        #endregion

        #region Get Resource Team Timesheet
        private List<TeamTimesheet> GetTeamTimesheet(int resourceId, DateTime? weekStartDay)
        {
            List<TeamTimesheet> teamTimesheets = new();
            try
            {
                var resourceTeamId = _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetReportingPersonTeamMember") + resourceId + "&weekStartDay=" + weekStartDay?.ToString("MM/dd/yyyy"));
                List<TeamMemberDetails> lstResources = JsonConvert.DeserializeObject<List<TeamMemberDetails>>(JsonConvert.SerializeObject(resourceTeamId.Result.Data));
                if (lstResources?.Count > 0)
                {
                    var resourceTeamMemberResult = _client.PostAsJsonAsync(lstResources, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTeamMemberDetails"));
                    List<TeamMemberDetails> lstResourceTeam = JsonConvert.DeserializeObject<List<TeamMemberDetails>>(JsonConvert.SerializeObject(resourceTeamMemberResult.Result.Data));
                    TeamMember teamMember = new()
                    {
                        ListOfTeamMember = lstResourceTeam,
                        ResourceId = resourceId,
                        WeekStartDate = weekStartDay
                    };
                    var result = _client.PostAsJsonAsync(teamMember, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetResourceTeamTimesheet"));
                    teamTimesheets = JsonConvert.DeserializeObject<List<TeamTimesheet>>(JsonConvert.SerializeObject(result.Result.Data));

                    var attendanceResult = _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAllActiveShift"));
                    List<ShiftView> shiftDetails = JsonConvert.DeserializeObject<List<ShiftView>>(JsonConvert.SerializeObject(attendanceResult.Result.Data));
                    ShiftView defaultShift = shiftDetails?.Where(x => x.ShiftName?.ToLower() == "general shift").Select(x=>x).FirstOrDefault();
                    if (defaultShift == null)
                    {
                        defaultShift= shiftDetails?.Select(x => x).FirstOrDefault();
                    }
                    foreach (TeamTimesheet item in teamTimesheets)
                    {
                        int? userShiftId = lstResourceTeam?.Where(x => x.UserId == item.UserId).Select(x=>x.ShiftId).FirstOrDefault();
                        ShiftView userShift = defaultShift;
                        if (userShiftId != null)
                        {
                            userShift = shiftDetails?.Where(x => x.ShiftDetailsId== userShiftId).Select(x => x).FirstOrDefault();
                        }
                        if(userShift != null && lstResourceTeam !=null)
                        {
                            long totalpercentage = lstResourceTeam.Where(x => x.UserId == item.UserId).Select(x => (long)x.Allocation).Sum();
                            var shiftHours = userShift.TotalHours.Split(":");
                            if(shiftHours.Length>0)
                            {
                                TimeSpan hours = new TimeSpan(Convert.ToInt32(shiftHours[0]), Convert.ToInt32(shiftHours[1]),0);
                                long ticks = (hours.Ticks * totalpercentage) / 100;
                                TimeSpan requiredHours = new TimeSpan(ticks * (7 - userShift.WeekEndDays));
                                item.RequiredHours = (int)requiredHours.TotalHours + ":" + requiredHours.Minutes;
                            }                            
                        }
                    }
                    //if (teamTimesheets?.Count > 0)
                    //{
                    //    foreach (TeamTimesheet item in teamTimesheets)
                    //    {
                    //        EmployeeLeavesForTimeSheetViewInput employeeLeavesForTimeSheetViewInput = new()
                    //        {
                    //            resourceId = item.UserId,
                    //            fromDate = weekStartDay.Value.Date,
                    //            toDate = weekStartDay.Value.AddDays(6).Date
                    //        };
                    //        var employeeleave = _client.PostAsJsonAsync(employeeLeavesForTimeSheetViewInput, _leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetEmployeeLeavesForTimesheet"));
                    //        List<EmployeeLeavesForTimeSheetView> EmployeeLeavelist = JsonConvert.DeserializeObject<List<EmployeeLeavesForTimeSheetView>>(JsonConvert.SerializeObject(employeeleave.Result.Data));
                    //        if (EmployeeLeavelist?.Count > 0)
                    //        {
                    //            EmployeeLeavelist = EmployeeLeavelist.Where(x => x.IsAllowTimesheet != true && x.IsFullDay == true).Select(x => x).ToList();
                    //        }

                    //        if (EmployeeLeavelist?.Count > 0)
                    //        {
                    //            string[] shift = item?.shiftHours?.Split(":");
                    //            TimeSpan shiftHours = new TimeSpan(shift?.Length > 0 ? Convert.ToInt32(shift[0]) : 0, shift?.Length > 1 ? Convert.ToInt32(shift[1]) : 0, 0);
                    //            int? fulldayLeave = EmployeeLeavelist?.Where(x => x.IsFullDay == true).Select(x => x).Count();
                    //            if (fulldayLeave != null && fulldayLeave > 0)
                    //            {
                    //                TimeSpan leaveHours = new TimeSpan(shiftHours.Ticks * (long)fulldayLeave);
                    //                string[] required = item?.RequiredHours?.Split(":");
                    //                TimeSpan previousRequiredHour = new TimeSpan(required?.Length > 0 ? Convert.ToInt32(required[0]) : 0, required?.Length > 1 ? Convert.ToInt32(required[1]) : 0, 0);
                    //                TimeSpan sumHour = new TimeSpan(leaveHours.Ticks + previousRequiredHour.Ticks);
                    //                item.RequiredHours = (sumHour.TotalHours.ToString().Length > 1 ? sumHour.TotalHours.ToString() : "0" + sumHour.TotalHours.ToString()) + ":" + (sumHour.Minutes.ToString().Length > 1 ? sumHour.Minutes.ToString() : "0" + sumHour.Minutes.ToString());
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetTeamTimesheet", " ResourceId- " + resourceId.ToString() + " WeekStartDay- " + weekStartDay.ToString());
            }
            return teamTimesheets;
        }

        [HttpGet]
        [Route("GetResourceTeamTimesheet")]
        public IActionResult GetResourceTeamTimesheet(int resourceId, DateTime? weekStartDay)
        {
            List<TeamTimesheet> teamTimesheets = new();
            try
            {
                teamTimesheets = GetTeamTimesheet(resourceId, weekStartDay);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    ListOfTimesheet = teamTimesheets
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetResourceTeamTimesheet", " ResourceId- " + resourceId.ToString() + " WeekStartDay- " + weekStartDay.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfTimesheet = teamTimesheets
            });
        }
        #endregion

        #region Get Rejection Reason List
        [HttpGet]
        [Route("GetRejectionReasonList")]
        public IActionResult GetRejectionReasonList()
        {
            List<RejectionReason> rejectionReasons = new();
            try
            {
                var result = _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetRejectionReasonList"));
                rejectionReasons = JsonConvert.DeserializeObject<List<RejectionReason>>(JsonConvert.SerializeObject(result.Result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        ListOfRejectionReason = rejectionReasons
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetRejectionReasonList");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfRejectionReason = rejectionReasons
            });
        }
        #endregion

        #region Get timesheetlog by project id
        [HttpPost]
        [Route("GetTimesheetLogByProjectId")]
        public IActionResult GetTimesheetLogByProjectId(List<int?> lstProjectId)
        {
            List<TimesheetLogView> lstTimesheetLog = new();
            try
            {
                var result = _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetLogByProjectId") + lstProjectId);
                lstTimesheetLog = JsonConvert.DeserializeObject<List<TimesheetLogView>>(JsonConvert.SerializeObject(result.Result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.Result.StatusCode,
                        result.Result.StatusText,
                        Data = lstTimesheetLog
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetTimesheetLogByProjectId", JsonConvert.SerializeObject(lstProjectId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = lstTimesheetLog
            });
        }
        #endregion

        #region Insert Or Update Timesheet Configuration
        [HttpPost]
        [Route("InsertOrUpdateTimesheetConfiguration")]
        public async Task<IActionResult> InsertOrUpdateTimesheetConfiguration(TimesheetConfigurationView timesheetConfigurationView)
        {
            int timesheetConfigurationId = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(timesheetConfigurationView, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:InsertOrUpdateTimesheetConfiguration"));
                timesheetConfigurationId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        timesheetConfigurationId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/InsertOrUpdateTimesheetConfiguration", JsonConvert.SerializeObject(timesheetConfigurationView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                timesheetConfigurationId
            });
        }
        #endregion

        #region Get Configuration Details
        [HttpGet]
        [Route("GetConfigurationDetails")]
        public async Task<IActionResult> GetConfigurationDetails()
        {
            TimesheetConfigurationView timesheetConfigurationView = new();
            try
            {
                var result = await _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetConfigurationDetails"));
                timesheetConfigurationView = JsonConvert.DeserializeObject<TimesheetConfigurationView>(JsonConvert.SerializeObject(result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        TimesheetCongifugurationDetails = timesheetConfigurationView
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetConfigurationDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                TimesheetCongifugurationDetails = timesheetConfigurationView
            });
        }
        #endregion

        #region Get Timesheet Configuration Master Data
        [HttpGet]
        [Route("GetTimesheetConfigurationMasterData")]
        public async Task<IActionResult> GetTimesheetConfigurationMasterData()
        {
            List<TimesheetConfigurationWeekDay> listOfDay = new();
            try
            {
                var listOfDayResult = await _client.GetAsync(_configuration.GetValue<string>("ApplicationURL:Notification:BaseURL"), _configuration.GetValue<string>("ApplicationURL:Notification:GetAllWeekDayList"));
                listOfDay = JsonConvert.DeserializeObject<List<TimesheetConfigurationWeekDay>>(JsonConvert.SerializeObject(listOfDayResult.Data));
                if (listOfDay?.Count > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        weekDayList = listOfDay
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetTimesheetConfigurationMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                weekDayList = listOfDay
            });
        }
        #endregion

        #region Get Timesheet Alert For Submission
        [HttpGet]
        [AllowAnonymous]
        [Route("GetTimesheetAlertForSubmission")]
        public async Task<IActionResult> GetTimesheetAlertForSubmission()
        {
            string statusText = "", statusCode = "SUCCESS";
            try
            {
                var resourceResult =await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectAllocatedResourceList"));
                List<int> timesheetResourceId = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(resourceResult?.Data));
                if (timesheetResourceId?.Count > 0)
                {
                    var timesheetResult = await _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetAlertForSubmission"));
                    Tuple<List<int>, DateTime,bool> submittedResourceList = JsonConvert.DeserializeObject<Tuple<List<int>, DateTime, bool>>(JsonConvert.SerializeObject(timesheetResult?.Data));
                    if(submittedResourceList !=null && submittedResourceList.Item3==true)
                    {
                        List<int> notificationResource = timesheetResourceId?.Except(submittedResourceList?.Item1).ToList();
                        var adminResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAdminEmployeeId") + _configuration.GetValue<string>("AdminRoleName"));
                        int adminEmployeeId = adminResult?.Data == null ? 1 : JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(adminResult?.Data));
                        if (notificationResource?.Count > 0)
                        {
                            List<Notifications> notifications = new();
                            string weekInfo = submittedResourceList?.Item2.Date.ToString("MMM dd") + " - " + submittedResourceList?.Item2.Date.AddDays(6).ToString("MMM dd");
                            foreach (var resourceId in notificationResource)
                            {
                                NotificationDataView notificationData = new()
                                {
                                    PrimaryKeyId = resourceId,
                                    WeekStartDate = submittedResourceList?.Item2.Date
                                };
                                Notifications notification = new()
                                {
                                    CreatedBy = adminEmployeeId,
                                    CreatedOn = DateTime.UtcNow,
                                    FromId = resourceId,
                                    ToId = resourceId,
                                    MarkAsRead = false,
                                    NotificationSubject = "Please submit timesheet",
                                    NotificationBody = "Please submit your timesheet for the week " + weekInfo + ".",
                                    PrimaryKeyId = resourceId,
                                    ButtonName = "Submit Timesheet",
                                    SourceType = "Timesheet",
                                    Data = JsonConvert.SerializeObject(notificationData)
                                };
                                notifications.Add(notification);
                            }
                            if (notifications?.Count > 0)
                            {
                                using var notificationClient = new HttpClient
                                {
                                    BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                                };
                                HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                                var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();
                                if (notificationResponse?.IsSuccessStatusCode == false)
                                {
                                    statusCode = "FAILURE";
                                    statusText = notificationResult?.Result?.StatusText;
                                }
                            }
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetTimesheetAlertForSubmission");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText
            });
        }
        #endregion

        #region Get Timesheet Alert For Approval
        [HttpGet]
        [AllowAnonymous]
        [Route("GetTimesheetAlertForApproval")]
        public async Task<IActionResult> GetTimesheetAlertForApproval()
        {
            string statusText = "", statusCode = "SUCCESS";
            try
            {
                var timesheetResult =await _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetAlertForApproval"));
                Tuple<List<TimesheetAlertApproval>, DateTime,bool> timesheetApproval = JsonConvert.DeserializeObject<Tuple<List<TimesheetAlertApproval>, DateTime,bool>>(JsonConvert.SerializeObject(timesheetResult?.Data));
                var employeeNameResult = await _client.PostAsJsonAsync(timesheetApproval?.Item1.Select(x => x.ResourceId).ToList(), _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeNameById"));
                List<EmployeeName> nameResult = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeNameResult?.Data));
                var projectSPOCResult = _client.PostAsJsonAsync(timesheetApproval?.Item1.Select(x => x.ProjectId).ToList(), _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectSPOCByProjectId"));
                List<ProjectSPOC> projectSPOC = JsonConvert.DeserializeObject<List<ProjectSPOC>>(JsonConvert.SerializeObject(projectSPOCResult.Result.Data));
                var adminResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAdminEmployeeId") + _configuration.GetValue<string>("AdminRoleName"));
                int adminEmployeeId = adminResult?.Data==null?1: JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(adminResult?.Data));
                if (timesheetApproval?.Item1?.Count > 0 && timesheetApproval?.Item3==true)
                {
                    List<Notifications> notifications = new();
                    foreach (var spocId in timesheetApproval?.Item1)
                    {
                        NotificationDataView notificationData = new()
                        {
                            PrimaryKeyId = spocId.ResourceId,
                            WeekStartDate = timesheetApproval.Item2.Date
                        };
                        string resourceName = nameResult.Where(x => x.EmployeeId == spocId.ResourceId).Select(x => x.EmployeeFullName).FirstOrDefault();
                        string weekInfo = timesheetApproval.Item2.Date.ToString("MMM dd") + " - " + timesheetApproval.Item2.Date.AddDays(6).ToString("MMM dd");
                        Notifications notification = new()
                        {
                            CreatedBy = adminEmployeeId,
                            CreatedOn = DateTime.UtcNow,
                            FromId = spocId.ResourceId,
                            ToId = projectSPOC.Where(x => x.projectId == spocId.ProjectId).Select(x => x.SPOCId == null ? 0 : (int)x.SPOCId).FirstOrDefault(),
                            MarkAsRead = false,
                            NotificationSubject = resourceName + " timesheet request is waiting for your approval",
                            NotificationBody = resourceName + " timesheet request " + weekInfo + " is waiting for your approval.",
                            PrimaryKeyId = spocId.ResourceId,
                            ButtonName = "Approve Timesheet",
                            SourceType = "Timesheet",
                            Data = JsonConvert.SerializeObject(notificationData)
                        };
                        notifications.Add(notification);
                    }
                    if (notifications?.Count > 0)
                    {
                        using var notificationClient = new HttpClient
                        {
                            BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                        };
                        HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                        var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();
                        if (notificationResponse?.IsSuccessStatusCode == false)
                        {
                            statusCode = "FAILURE";
                            statusText = notificationResult?.Result?.StatusText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Timesheet/GetTimesheetAlertForApproval");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText
            });
        }
        #endregion

        #region Timer Start
        [HttpGet]
        [Route("TimerStart")]
        public IActionResult TimerStart()
        {
            stopWatch.Start();
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "",
                data = stopWatch
            });
        }
        #endregion

        #region Timer Stop
        [HttpGet]
        [Route("TimerStop")]
        public IActionResult TimerStop()
        {
            stopWatch.Stop();
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "",
                data = stopWatch
            });
        }
        #endregion

        #region First Day Of Week
        public static DateTime FirstDayOfWeek(DateTime date)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var dateDiff = date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (dateDiff < 0)
                dateDiff += 7;
            return date.AddDays(-dateDiff).Date;
        }
        #endregion
    }
}