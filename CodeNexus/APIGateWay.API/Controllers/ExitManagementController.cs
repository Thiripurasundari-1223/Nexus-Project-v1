using APIGateWay.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.ExitManagement;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Accounts;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.ExitManagement;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    //[AllowAnonymous]
    [ApiController]
    public class ExitManagementController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HTTPClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _leavesBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _notificationBaseURL = string.Empty;
        private readonly string _attendanceBaseURL = string.Empty;
        private readonly string _exitManagementBaseURL = string.Empty;
        private readonly string _accountBaseURL = string.Empty;
        private readonly CommonFunction _commonFunction;
        private readonly string _accountsBaseURL = string.Empty;

        private readonly string strErrorMsg = "Something went wrong, please try again later";
        #region Constructor
        public ExitManagementController(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _configuration = configuration;
            _leavesBaseURL = _configuration.GetValue<string>("ApplicationURL:Leaves:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _notificationBaseURL = _configuration.GetValue<string>("ApplicationURL:Notifications");
            _attendanceBaseURL = _configuration.GetValue<string>("ApplicationURL:Attendance:BaseURL");
            _exitManagementBaseURL = _configuration.GetValue<string>("ApplicationURL:ExitManagement:BaseURL");
            _accountBaseURL = _configuration.GetValue<string>("ApplicationURL:Accounts:BaseURL");
            _accountsBaseURL = _configuration.GetValue<string>("ApplicationURL:Accounts:BaseURL");
            _commonFunction = new CommonFunction(configuration);
        }
        #endregion
        #region Insert/Update resignation       
        [HttpPost]
        [Route("InsertAndUpdateResignationDetails")]
        public async Task<IActionResult> InsertAndUpdateResignationDetails(EmployeeResignationDetailsView employeeResignationDetails)
        {
            EmployeeResignationDetailsView resignationDetails = new EmployeeResignationDetailsView();

            try
            {
                string hrDepartmentName = _configuration.GetValue<string>("GrantLeaveApproveHRDepartmentName");
                int? defaultNoticePeriod = _configuration.GetValue<int>("DefaultNoticePeriodDays");
                hrDepartmentName = hrDepartmentName == null ? "People Experience" : hrDepartmentName;
                /*
                var employeeResults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationApprover") + employeeResignationDetails.EmployeeId + "&hrDepartmentName=" + hrDepartmentName);
                employeeResignationDetails.ResignationApprover = JsonConvert.DeserializeObject<ResignationApproverView>(JsonConvert.SerializeObject(employeeResults?.Data));
                if(employeeResignationDetails?.ResignationApprover?.NoticePeriod ==null)
                {
                    if(employeeResignationDetails.ResignationApprover==null)
                    {
                        employeeResignationDetails.ResignationApprover = new ResignationApproverView();
                    }
                    employeeResignationDetails.ResignationApprover.NoticePeriod = defaultNoticePeriod;
                }
                */
                //var noticeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeByEmployeeId") + employeeResignationDetails.EmployeeId);
                //employee = JsonConvert.DeserializeObject<Employees>(JsonConvert.SerializeObject(noticeResult?.Data));
                //employeeResignationDetails.NoticePeriod = resignationDetails?.NoticePeriod == null ? 60 : (int)resignationDetails?.NoticePeriod;
                var result = await _client.PostAsJsonAsync(employeeResignationDetails, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:InsertAndUpdateResignationDetails"));
                resignationDetails = JsonConvert.DeserializeObject<EmployeeResignationDetailsView>(JsonConvert.SerializeObject(result?.Data));

                if (result != null && result?.StatusCode == "SUCCESS")
                {

                    if (resignationDetails.ResignationStatus != "Rejected" && resignationDetails.ResignationStatus != "Cancelled" && resignationDetails.ResignationStatus != "Withdrawal Approved")
                    {
                        UpdateEmployeeRelievingDate empDetails = new UpdateEmployeeRelievingDate();
                        empDetails.EmployeeId = employeeResignationDetails.EmployeeId;
                        empDetails.PersonalMobileNumber = employeeResignationDetails.MobileNumber;
                        empDetails.PersonalEmailId = employeeResignationDetails.PersonalEmailAddress;
                        empDetails.ModifiedBy = employeeResignationDetails.CreatedBy;
                        //empDetails.RelievingDate = resignationDetails.RelievingDate;
                        //empDetails.IsRevertRelievingDate = false;
                        //empDetails.ResignationDate = employeeResignationDetails.ResignationDate;
                        //empDetails.ResignationReason = resignationDetails.ResignationReason;
                        //empDetails.ResignationStatus = resignationDetails.ResignationStatus;
                        //empDetails.ExitType = employeeResignationDetails.CreatedBy == employeeResignationDetails.EmployeeId ? "Voluntary" : "Involuntary";
                        var resignationResult = _client.PostAsJsonAsync(empDetails, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeePersonalInfo"));
                        bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(resignationResult?.Result?.Data == null ? false : resignationResult?.Result?.Data));
                        //bool isActive = true;
                        //if (empDetails?.RelievingDate?.Date < DateTime.Now.Date)
                        //{
                        //    isActive = false;
                        //}
                        //EmployeeStatusView employeeStatus = new EmployeeStatusView();
                        //employeeStatus.EmployeeId = employeeResignationDetails.EmployeeId;
                        //employeeStatus.IsEnabled = isActive;
                        //employeeStatus.ModifiedBy = employeeResignationDetails.CreatedBy;
                        //var employeeResult = _client.PostAsJsonAsync(employeeStatus, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeeStatus"));
                        //bool? isUpdate = JsonConvert.DeserializeObject<bool?>(JsonConvert.SerializeObject(employeeResult?.Result?.Data));

                    }
                    //if (employeeResignationDetails.EmployeeResignationDetailsId == 0)
                    //{
                    //    SendResignationSubmissionMail(resignationDetails);
                    //}
                    if (employeeResignationDetails.EmployeeResignationDetailsId > 0 && employeeResignationDetails.IsWithdrawal == true)
                    {
                        SendWithdrawalResignationMail(resignationDetails);
                    }



                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        resignationDetails?.EmployeeResignationDetailsId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/InsertAndUpdateResignationDetails", JsonConvert.SerializeObject(employeeResignationDetails));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                resignationDetails?.EmployeeResignationDetailsId
            });
        }
        #endregion

        #region Insert/Update resignation reason       
        [HttpPost]
        [Route("InsertAndUpdateResignationReason")]
        public async Task<IActionResult> InsertAndUpdateResignationReason(EmployeeResignationDetailsView employeeResignationDetails)
        {
            EmployeeResignationDetailsView resignationDetails = new EmployeeResignationDetailsView();

            try
            {
                string hrDepartmentName = _configuration.GetValue<string>("GrantLeaveApproveHRDepartmentName");
                int? defaultNoticePeriod = _configuration.GetValue<int>("DefaultNoticePeriodDays");
                hrDepartmentName = hrDepartmentName == null ? "People Experience" : hrDepartmentName;
                var employeeResults = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationApprover") + employeeResignationDetails.EmployeeId + "&hrDepartmentName=" + hrDepartmentName);
                employeeResignationDetails.ResignationApprover = JsonConvert.DeserializeObject<ResignationApproverView>(JsonConvert.SerializeObject(employeeResults?.Data));
                if (employeeResignationDetails?.ResignationApprover?.NoticePeriod == null)
                {
                    if (employeeResignationDetails.ResignationApprover == null)
                    {
                        employeeResignationDetails.ResignationApprover = new ResignationApproverView();
                    }
                    employeeResignationDetails.ResignationApprover.NoticePeriod = defaultNoticePeriod;
                }
                var result = await _client.PostAsJsonAsync(employeeResignationDetails, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:InsertAndUpdateResignationReason"));
                resignationDetails = JsonConvert.DeserializeObject<EmployeeResignationDetailsView>(JsonConvert.SerializeObject(result?.Data));

                if (result != null && result?.StatusCode == "SUCCESS")
                {
                    if (resignationDetails.ResignationStatus != "Rejected" && resignationDetails.ResignationStatus != "Cancelled" && resignationDetails.ResignationStatus != "Withdrawal Approved")
                    {
                        UpdateEmployeeRelievingDate empDetails = new UpdateEmployeeRelievingDate();
                        empDetails.EmployeeId = employeeResignationDetails.EmployeeId;
                        empDetails.ModifiedBy = employeeResignationDetails.CreatedBy;
                        empDetails.RelievingDate = resignationDetails.RelievingDate;
                        empDetails.IsRevertRelievingDate = false;
                        empDetails.ResignationDate = employeeResignationDetails.ResignationDate;
                        empDetails.ResignationReason = resignationDetails.ResignationReason;
                        empDetails.ResignationStatus = resignationDetails.ResignationStatus;
                        empDetails.ExitType = employeeResignationDetails.CreatedBy == employeeResignationDetails.EmployeeId ? "Voluntary" : "Involuntary";
                        var resignationResult = _client.PostAsJsonAsync(empDetails, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeeRelievingDate"));
                        bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(resignationResult?.Result?.Data == null ? false : resignationResult?.Result?.Data));
                        bool isActive = true;
                        if (empDetails?.RelievingDate?.Date < DateTime.Now.Date)
                        {
                            isActive = false;
                        }
                        EmployeeStatusView employeeStatus = new EmployeeStatusView();
                        employeeStatus.EmployeeId = employeeResignationDetails.EmployeeId;
                        employeeStatus.IsEnabled = isActive;
                        employeeStatus.ModifiedBy = employeeResignationDetails.CreatedBy;
                        var employeeResult = _client.PostAsJsonAsync(employeeStatus, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeeStatus"));
                        bool? isUpdate = JsonConvert.DeserializeObject<bool?>(JsonConvert.SerializeObject(employeeResult?.Result?.Data));

                    }
                    //if (employeeResignationDetails.EmployeeResignationDetailsId == 0)
                    //{
                    SendResignationSubmissionMail(resignationDetails);
                    //}
                    //else if (employeeResignationDetails.EmployeeResignationDetailsId > 0 && employeeResignationDetails.IsWithdrawal == true)
                    //{
                    //    SendWithdrawalResignationMail(resignationDetails);
                    //}


                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        resignationDetails?.EmployeeResignationDetailsId
                    });
                }

                if (result != null && result?.StatusCode == "SUCCESS")
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        resignationDetails?.EmployeeResignationDetailsId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/InsertAndUpdateResignationReason", JsonConvert.SerializeObject(employeeResignationDetails));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                resignationDetails?.EmployeeResignationDetailsId
            });
        }
        #endregion

        #region Get All Resignation Details
        [HttpGet]
        [Route("GetAllResignationDetails")]
        public async Task<IActionResult> GetAllResignationDetails(int employeeID, bool isManager, bool isAdmin, bool isAllReportees)
        {

            EmployeeResignationDetailsList employeeResignationDetailsList = new EmployeeResignationDetailsList();
            AllResignationInputView allResignation = new AllResignationInputView();
            allResignation.IsMyData = false;
            allResignation.EmployeeId = employeeID;
            allResignation.IsAdmin = isAdmin;
            allResignation.IsAllData = isAllReportees;
            List<int> employeeList = new List<int>();
            try
            {
                employeeList.Add(employeeID);
                if (isManager == true)
                {
                    var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeListForManagerReport") + employeeID + "&isAll=" + isAllReportees);
                    List<EmployeeViewDetails> employeeDetailList = JsonConvert.DeserializeObject<List<EmployeeViewDetails>>(JsonConvert.SerializeObject(employeeListresult.Data));
                    employeeList = employeeDetailList.Select(ea => ea.EmployeeId).ToList();
                }
                else if (isManager == false)
                {
                    allResignation.IsMyData = true;
                }
                allResignation.ReporteesList = employeeList;
                List<EmployeeResignationDetailsView> employeeResignationDetails = new();
                var result = await _client.PostAsJsonAsync(allResignation, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetAllResignationDetails"));
                employeeResignationDetails = JsonConvert.DeserializeObject<List<EmployeeResignationDetailsView>>(JsonConvert.SerializeObject(result?.Data));

                var employeeResult = await _client.PostAsJsonAsync(employeeResignationDetails, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeResignationDetails"));
                employeeResignationDetails = JsonConvert.DeserializeObject<List<EmployeeResignationDetailsView>>(JsonConvert.SerializeObject(employeeResult?.Data));
                //var countryAndStateResult = await _client.GetAsync(_accountBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetCountryAndStateData"));
                //CountryAndState countryAndStateList = JsonConvert.DeserializeObject<CountryAndState>(JsonConvert.SerializeObject(countryAndStateResult?.Data));
                //employeeResignationDetails?.ForEach(x => x.Country = countryAndStateList?.CountryList?.Where(y => y.CountryId == x.CountryId).Select(z => z.CountryName).FirstOrDefault());
                //employeeResignationDetails?.ForEach(x => x.State = countryAndStateList?.StateList?.Where(y => y.StateId == x.StateId).Select(z => z.StateName).FirstOrDefault());
                employeeResignationDetailsList.EmployeeResignationDetailsView = employeeResignationDetails;
                employeeResignationDetailsList.AllReportees = employeeList;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    employeeResignationDetailsList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetAllResignationDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    employeeResignationDetailsList
                });
            }
        }
        #endregion
        //#region List All Resignation Reason
        //[HttpGet]
        //[Route("GetResignEmployeeDetail")]
        //public async Task<IActionResult> GetResignEmployeeDetail(int employeeId)
        //{
        //    List<ResignationReason> resinationReason = new();
        //    EmployeesViewModel employee = new EmployeesViewModel();
        //    try
        //    {
        //        var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDetailsByEmployeeId") + employeeId);
        //        employee = JsonConvert.DeserializeObject<EmployeesViewModel>(JsonConvert.SerializeObject(result?.Result?.Data));
        //        var reasonResult = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResinationReason"));
        //        resinationReason = JsonConvert.DeserializeObject<List<ResignationReason>>(JsonConvert.SerializeObject(reasonResult?.Data));
        //        if (employee != null)
        //        {
        //            employee.ResignationReason = resinationReason;
        //            return Ok(new
        //            {
        //                StatusCode = "SUCCESS",
        //                EmployeeDetails = employee
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignEmployeeDetail");
        //    }
        //    return Ok(new
        //    {
        //        StatusCode = "FAILURE",
        //        StatusText = strErrorMsg,
        //        EmployeeDetails = employee
        //    });
        //}
        //#endregion
        #region List All GetResignationMasterData
        [HttpGet]
        [Route("GetResignationMasterData")]
        public async Task<IActionResult> GetResignationMasterData(bool isAdmin, int employeeId)
        {
            List<ResignationReason> resinationReason = new();
            CountryAndState countryAndStateList = new();

            List<ResignationEmployeeMasterView> employeeList = new List<ResignationEmployeeMasterView>();
            ResignationMasterData employee = new ResignationMasterData();
            List<int> employeeIdList = new List<int>();
            try
            {
                if (isAdmin == false)
                {
                    employeeIdList.Add(employeeId);
                }
                var result = _client.PostAsJsonAsync(employeeIdList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeMasterData"));
                employeeList = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                var reasonResult = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResinationReason"));
                resinationReason = JsonConvert.DeserializeObject<List<ResignationReason>>(JsonConvert.SerializeObject(reasonResult?.Data));
                var countryAndStateResult = await _client.GetAsync(_accountBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetCountryAndStateData"));
                countryAndStateList = JsonConvert.DeserializeObject<CountryAndState>(JsonConvert.SerializeObject(countryAndStateResult?.Data));

                employee.employeeList = employeeList;
                employee.ResignationReasonList = resinationReason;
                employee.CountryAndStateList = countryAndStateList;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    EmployeeDetails = employee
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignationMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = employee
            });
        }
        #endregion
        #region Send mail for resignation
        [NonAction]
        public async Task<string> SendResignationSubmissionMail(EmployeeResignationDetailsView resignationDetails)
        {
            string mail = "";
            EmployeeManagerAndHeadDetailsView employeeManagerAndHeadDetails = new EmployeeManagerAndHeadDetailsView();
            var maildata = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeManagerAndHeadDetails") + resignationDetails.EmployeeId + "&approverId=" + resignationDetails.ApproverId);
            employeeManagerAndHeadDetails = JsonConvert.DeserializeObject<EmployeeManagerAndHeadDetailsView>(JsonConvert.SerializeObject(maildata?.Data));
            if (maildata != null)
            {
                resignationDetails.EmployeeName = employeeManagerAndHeadDetails.EmployeeFullName + '-' + employeeManagerAndHeadDetails.FormattedEmployeeId;
                resignationDetails.ApproverName = employeeManagerAndHeadDetails.ApproverName;
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                string MailSubject = resignationDetails?.ApprovalEmailTemplate?.Subject, MailBody = resignationDetails?.ApprovalEmailTemplate?.Body;

                MailBody = MailBody.Replace("@approverName", employeeManagerAndHeadDetails?.ApproverName);
                MailBody = MailBody.Replace("@formattedEmployeeId", employeeManagerAndHeadDetails?.FormattedEmployeeId);
                MailBody = MailBody.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                MailBody = MailBody.Replace("@managerName", employeeManagerAndHeadDetails?.MangerName + " " + employeeManagerAndHeadDetails?.ManagerFormattedId);
                MailBody = MailBody.Replace("@department", employeeManagerAndHeadDetails?.DepartmentName);
                MailBody = MailBody.Replace("@resignationDate", resignationDetails?.ResignationDate?.ToString("dd MMM yyyy"));
                MailBody = MailBody.Replace("@reason", resignationDetails?.ResignationReason);
                MailBody = MailBody.Replace("@baseURL", baseURL);
                MailBody = MailBody.Replace("@resignationId", resignationDetails?.EmployeeResignationDetailsId.ToString());
                MailSubject = MailSubject.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                var COOMail = appsetting.GetSection("COOEmialId").Value;
                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = employeeManagerAndHeadDetails?.ApproverEmail,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("ResignBUHeadNotification").Value == "true" ? employeeManagerAndHeadDetails?.BUHeadEmail + ',' + HRTeamMail + ',' + COOMail + ',' + employeeManagerAndHeadDetails?.ManagerEmail + ',' + employeeManagerAndHeadDetails?.EmployeeEmailAddress : employeeManagerAndHeadDetails?.EmployeeEmailAddress
                };
                mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
            }
            SendResignationApprovalNotification(resignationDetails, false);
            return mail;
        }
        #endregion
        #region Send mail for withdrawal resignation
        [NonAction]
        public async Task<string> SendWithdrawalResignationMail(EmployeeResignationDetailsView resignationDetails)
        {
            string mail = "";
            EmployeeManagerAndHeadDetailsView employeeManagerAndHeadDetails = new EmployeeManagerAndHeadDetailsView();
            var maildata = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeManagerAndHeadDetails") + resignationDetails.EmployeeId + "&approverId=" + resignationDetails.ApproverId);
            employeeManagerAndHeadDetails = JsonConvert.DeserializeObject<EmployeeManagerAndHeadDetailsView>(JsonConvert.SerializeObject(maildata?.Data));
            if (maildata != null)
            {
                resignationDetails.EmployeeName = employeeManagerAndHeadDetails.EmployeeFullName + "-" + employeeManagerAndHeadDetails.FormattedEmployeeId;
                resignationDetails.ApproverName = employeeManagerAndHeadDetails.ApproverName;
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                string MailSubject = resignationDetails?.WithdrawalEmailTemplate?.Subject, MailBody = resignationDetails?.WithdrawalEmailTemplate?.Body;
                MailBody = MailBody.Replace("@approverName", employeeManagerAndHeadDetails?.MangerName);
                MailBody = MailBody.Replace("@formattedEmployeeId", employeeManagerAndHeadDetails?.FormattedEmployeeId);
                MailBody = MailBody.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                MailBody = MailBody.Replace("@managerName", employeeManagerAndHeadDetails?.MangerName + " " + employeeManagerAndHeadDetails?.ManagerFormattedId);
                MailBody = MailBody.Replace("@department", employeeManagerAndHeadDetails?.DepartmentName);
                MailBody = MailBody.Replace("@resignationDate", resignationDetails?.ResignationDate?.ToString("dd MMM yyyy"));
                MailBody = MailBody.Replace("@reason", resignationDetails?.WithdrawalReason);
                MailBody = MailBody.Replace("@baseURL", baseURL);
                MailBody = MailBody.Replace("@resignationId", resignationDetails?.EmployeeResignationDetailsId.ToString());
                MailSubject = MailSubject.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);

                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                var COOMail = appsetting.GetSection("COOEmialId").Value;
                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = employeeManagerAndHeadDetails?.ApproverEmail,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("ResignBUHeadNotification").Value == "true" ? employeeManagerAndHeadDetails?.BUHeadEmail + ',' + HRTeamMail + ',' + COOMail + ',' + employeeManagerAndHeadDetails?.ManagerEmail + ',' + employeeManagerAndHeadDetails?.EmployeeEmailAddress : employeeManagerAndHeadDetails?.EmployeeEmailAddress
                };
                mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
            }
            SendResignationApprovalNotification(resignationDetails, true);

            return mail;

        }
        #endregion

        #region Get employee name by id
        [NonAction]
        public async Task<List<ResignedEmployeeView>> GetEmployeeDetailsById(List<int> lstEmployeeId)
        {
            List<ResignedEmployeeView> lstEmployeeName = new();
            try
            {
                var lstEmpId = lstEmployeeId?.Where(x => x != 0).Select(x => x).Distinct().ToList();
                var result = await _client.PostAsJsonAsync(lstEmpId, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDetailsById"));
                lstEmployeeName = JsonConvert.DeserializeObject<List<ResignedEmployeeView>>(JsonConvert.SerializeObject(result?.Data));
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetEmployeeDetailsById", JsonConvert.SerializeObject(lstEmployeeId));
            }
            return lstEmployeeName;
        }
        #endregion
        #region Get Resignation Details by id
        [HttpGet]
        [Route("GetResignationDetailsByResignationId")]
        public async Task<IActionResult> GetResignationDetailsByResignationId(int resignationId)
        {
            EmployeeResignationDetailsView employeeResignationDetails = new();
            EmployeesViewModel employee = new EmployeesViewModel();
            List<EmployeeResignationDetailsView> employeeResignationDetailsList = new();
            try
            {
                var result = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationDetailsByResignationId") + resignationId);
                employeeResignationDetails = JsonConvert.DeserializeObject<EmployeeResignationDetailsView>(JsonConvert.SerializeObject(result?.Data));
                if (employeeResignationDetails != null)
                {
                    employeeResignationDetailsList.Add(employeeResignationDetails);
                }
                var employeeResult = await _client.PostAsJsonAsync(employeeResignationDetailsList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeResignationDetails"));
                employeeResignationDetailsList = JsonConvert.DeserializeObject<List<EmployeeResignationDetailsView>>(JsonConvert.SerializeObject(employeeResult?.Data));
                employeeResignationDetails = employeeResignationDetailsList?.FirstOrDefault();

                //var countryAndStateResult = await _client.GetAsync(_accountBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetCountryAndStateById") + "?countryId=" + (employeeResignationDetails?.CountryId == null ? 0 : (int)employeeResignationDetails.CountryId).ToString() + "&stateId=" + (employeeResignationDetails?.StateId == null ? 0 : (int)employeeResignationDetails.StateId).ToString());
                //CountryNameAndStateName countryAndState = JsonConvert.DeserializeObject<CountryNameAndStateName>(JsonConvert.SerializeObject(countryAndStateResult?.Data));
                //employeeResignationDetails.Country = countryAndState?.Country?.CountryName;
                //employeeResignationDetails.State = countryAndState?.State?.StateName;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    employeeResignationDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignationDetailsByResignationId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    employeeResignationDetails
                });
            }
        }
        #endregion
        #region Insert or update approvel level
        [HttpPost]
        [Route("InsertOrUpdateResignationApproval")]
        public async Task<IActionResult> InsertOrUpdateResignationApproval(List<ResignationApproval> resignationApproval)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(resignationApproval, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:InsertOrUpdateResignationApproval"));
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                if (isSuccess)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = result.StatusText,
                        Data = true
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/InsertOrUpdateResignationApproval");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = false
            });
        }
        #endregion
        #region Get resign approvel level
        [HttpGet]
        [Route("GetResignationApproval")]
        public async Task<IActionResult> GetResignationApproval()
        {
            ApprovalConfiguration approvalConfiguration = new();
            try
            {
                var result = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationApproval"));
                approvalConfiguration = JsonConvert.DeserializeObject<ApprovalConfiguration>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = approvalConfiguration
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignationApproval");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = approvalConfiguration
            });
        }
        #endregion
        #region  Approve Or Reject Or Cancel Resignation
        [HttpPost]
        [Route("ApproveOrRejectOrCancelResignation")]
        public async Task<IActionResult> ApproveOrRejectOrCancelResignation(ApproveResignationView resignationDetails)
        {

            try
            {
                var result = await _client.PostAsJsonAsync(resignationDetails, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:ApproveOrRejectOrCancelResignation"));
                resignationDetails = JsonConvert.DeserializeObject<ApproveResignationView>(JsonConvert.SerializeObject(result?.Data));

                UpdateEmployeeRelievingDate empDetails = new UpdateEmployeeRelievingDate();
                empDetails.EmployeeId = resignationDetails.EmployeeId;
                empDetails.ModifiedBy = resignationDetails.CreatedBy;
                empDetails.RelievingDate = resignationDetails.RelievingDate;
                empDetails.ResignationReason = resignationDetails.ResignReason;
                empDetails.ResignationStatus = resignationDetails.OverAllStatus;
                empDetails.ResignationDate = resignationDetails.ResignDate;
                if (resignationDetails?.OverAllStatus == "Withdrawal Approved" || resignationDetails?.OverAllStatus == "Rejected" || resignationDetails?.OverAllStatus == "Cancelled")
                {
                    empDetails.IsRevertRelievingDate = true;
                }
                var resignationResult = _client.PostAsJsonAsync(empDetails, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeeRelievingDate"));
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(resignationResult?.Result?.Data == null ? false : resignationResult?.Result?.Data));
                bool isActive = true;
                if (empDetails?.RelievingDate?.Date < DateTime.Now.Date)
                {
                    isActive = false;
                }
                EmployeeStatusView employeeStatus = new EmployeeStatusView();
                employeeStatus.EmployeeId = resignationDetails.EmployeeId;
                employeeStatus.IsEnabled = isActive;
                employeeStatus.ModifiedBy = empDetails.ModifiedBy;
                var employeeResult = _client.PostAsJsonAsync(employeeStatus, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:UpdateEmployeeStatus"));
                bool? isUpdate = JsonConvert.DeserializeObject<bool?>(JsonConvert.SerializeObject(employeeResult?.Result?.Data));

                if (resignationDetails?.OverAllStatus == "Approved" || resignationDetails?.OverAllStatus == "Pending" || resignationDetails?.OverAllStatus == "Rejected" || resignationDetails?.OverAllStatus == "Cancelled")
                {
                    if (resignationDetails?.OverAllStatus != "Pending")
                    {
                        SendResignationApprovalMailToHR(resignationDetails);
                    }
                    if (resignationDetails?.NextLevelApproverId > 0)
                    {
                        SendResignationApprovalMailForNextLevel(resignationDetails);
                    }
                }
                if (resignationDetails?.OverAllStatus == "Withdrawal Approved" || resignationDetails?.OverAllStatus == "Withdrawal Pending" || resignationDetails?.OverAllStatus == "Withdrawal Rejected")
                {
                    if (resignationDetails?.OverAllStatus != "Withdrawal Pending")
                    {
                        SendWithdrawalResignationApprovalMailToHR(resignationDetails);
                    }
                    if (resignationDetails?.NextLevelApproverId > 0)
                    {
                        SendWithdrawalResignationApprovalMailForNextLevel(resignationDetails);
                    }
                }

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = result.StatusText,
                    Data = resignationDetails?.OverAllStatus == null ? "" : resignationDetails?.OverAllStatus
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/ApproveOrRejectOrCancelResignation");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = false
            });
        }
        #endregion
        #region Send approval notification email to HR group
        [NonAction]
        public async Task<string> SendResignationApprovalMailToHR(ApproveResignationView resignationDetails)
        {
            string mail = "";
            EmployeeManagerAndHeadDetailsView employeeManagerAndHeadDetails = new EmployeeManagerAndHeadDetailsView();
            var maildata = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeManagerAndHeadDetails") + resignationDetails.EmployeeId + "&approverId=" + resignationDetails.CreatedBy);
            employeeManagerAndHeadDetails = JsonConvert.DeserializeObject<EmployeeManagerAndHeadDetailsView>(JsonConvert.SerializeObject(maildata?.Data));
            if (maildata != null)
            {
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                ExitManagementEmailTemplate emailTemplate = resignationDetails?.EmailTemplateList?.Where(x => x.TemplateName == "ResignationApprovalHR").FirstOrDefault();
                string MailSubject = emailTemplate?.Subject, MailBody = emailTemplate?.Body;
                //MailBody = MailBody.Replace("@approverName", employeeManagerAndHeadDetails?.MangerName);
                MailBody = MailBody.Replace("@formattedEmployeeId", employeeManagerAndHeadDetails?.FormattedEmployeeId);
                MailBody = MailBody.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                MailBody = MailBody.Replace("@managerName", employeeManagerAndHeadDetails?.MangerName + " " + employeeManagerAndHeadDetails?.ManagerFormattedId);
                MailBody = MailBody.Replace("@department", employeeManagerAndHeadDetails?.DepartmentName);
                MailBody = MailBody.Replace("@resignationDate", resignationDetails?.ResignDate?.ToString("dd MMM yyyy"));
                MailBody = MailBody.Replace("@reason", resignationDetails?.ResignReason);
                MailBody = MailBody.Replace("@comments", resignationDetails?.Feedback);
                MailBody = MailBody.Replace("@baseURL", baseURL);
                MailBody = MailBody.Replace("@approverName", employeeManagerAndHeadDetails?.ApproverName);
                MailBody = MailBody.Replace("@resignationId", resignationDetails?.EmployeeResignationDetailsId.ToString());
                MailSubject = MailSubject.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                MailSubject = MailSubject.Replace("@status", resignationDetails?.Status);
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                var COOMail = appsetting.GetSection("COOEmialId").Value;
                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = employeeManagerAndHeadDetails?.EmployeeEmailAddress,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("ResignBUHeadNotification").Value == "true" ? employeeManagerAndHeadDetails?.BUHeadEmail + ',' + COOMail + ',' + employeeManagerAndHeadDetails?.ApproverEmail + ',' + employeeManagerAndHeadDetails?.ManagerEmail + ',' + HRTeamMail : employeeManagerAndHeadDetails?.EmployeeEmailAddress
                };
                mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                SendResignationStatusNotification(resignationDetails, employeeManagerAndHeadDetails?.ApproverName, false);
            }
            return mail;
        }
        #endregion
        #region Send next level approval notification
        [NonAction]
        public async Task<string> SendResignationApprovalMailForNextLevel(ApproveResignationView resignationDetails)
        {
            string mail = "";
            EmployeeManagerAndHeadDetailsView employeeManagerAndHeadDetails = new EmployeeManagerAndHeadDetailsView();
            var maildata = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeManagerAndHeadDetails") + resignationDetails.EmployeeId + "&approverId=" + resignationDetails.NextLevelApproverId);
            employeeManagerAndHeadDetails = JsonConvert.DeserializeObject<EmployeeManagerAndHeadDetailsView>(JsonConvert.SerializeObject(maildata?.Data));
            if (maildata != null)
            {
                resignationDetails.EmployeeName = employeeManagerAndHeadDetails?.EmployeeFullName + '-' + employeeManagerAndHeadDetails?.FormattedEmployeeId;
                resignationDetails.ApprovarName = employeeManagerAndHeadDetails?.ApproverName;
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                ExitManagementEmailTemplate emailTemplate = resignationDetails?.EmailTemplateList?.Where(x => x.TemplateName == "ResignationApprovalManager").FirstOrDefault();
                string MailSubject = emailTemplate?.Subject, MailBody = emailTemplate?.Body;
                MailBody = MailBody.Replace("@approverName", employeeManagerAndHeadDetails?.ApproverName);
                MailBody = MailBody.Replace("@formattedEmployeeId", employeeManagerAndHeadDetails?.FormattedEmployeeId);
                MailBody = MailBody.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                MailBody = MailBody.Replace("@managerName", employeeManagerAndHeadDetails?.MangerName + " " + employeeManagerAndHeadDetails?.ManagerFormattedId);
                MailBody = MailBody.Replace("@department", employeeManagerAndHeadDetails?.DepartmentName);
                MailBody = MailBody.Replace("@resignationDate", resignationDetails?.ResignDate?.ToString("dd MMM yyyy"));
                MailBody = MailBody.Replace("@reason", resignationDetails?.ResignReason);
                MailBody = MailBody.Replace("@baseURL", baseURL);
                MailBody = MailBody.Replace("@resignationId", resignationDetails?.EmployeeResignationDetailsId.ToString());
                MailSubject = MailSubject.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                //MailBody = MailBody.Replace("@comments", resignationDetails?.Feedback);
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                var COOMail = appsetting.GetSection("COOEmialId").Value;
                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = employeeManagerAndHeadDetails?.ApproverEmail,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("ResignBUHeadNotification").Value == "true" ? employeeManagerAndHeadDetails?.BUHeadEmail + ',' + HRTeamMail + ',' + employeeManagerAndHeadDetails?.ManagerEmail + ',' + employeeManagerAndHeadDetails?.EmployeeEmailAddress : employeeManagerAndHeadDetails?.EmployeeEmailAddress
                };
                mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                SendResignationNextLevelNotification(resignationDetails, false);
            }
            return mail;
        }
        #endregion
        #region Send withdrawal approval notification email to HR group
        [NonAction]
        public async Task<string> SendWithdrawalResignationApprovalMailToHR(ApproveResignationView resignationDetails)
        {
            string mail = "";
            EmployeeManagerAndHeadDetailsView employeeManagerAndHeadDetails = new EmployeeManagerAndHeadDetailsView();
            var maildata = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeManagerAndHeadDetails") + resignationDetails.EmployeeId + "&approverId=" + resignationDetails.CreatedBy);
            employeeManagerAndHeadDetails = JsonConvert.DeserializeObject<EmployeeManagerAndHeadDetailsView>(JsonConvert.SerializeObject(maildata?.Data));
            if (maildata != null)
            {
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                ExitManagementEmailTemplate emailTemplate = resignationDetails?.EmailTemplateList?.Where(x => x.TemplateName == "WithdrawalResignationApprovalHR").FirstOrDefault();
                string MailSubject = emailTemplate?.Subject, MailBody = emailTemplate?.Body;
                //MailBody = MailBody.Replace("@approverName", employeeManagerAndHeadDetails?.MangerName);
                MailBody = MailBody.Replace("@formattedEmployeeId", employeeManagerAndHeadDetails?.FormattedEmployeeId);
                MailBody = MailBody.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                MailBody = MailBody.Replace("@managerName", employeeManagerAndHeadDetails?.MangerName + " " + employeeManagerAndHeadDetails?.ManagerFormattedId);
                MailBody = MailBody.Replace("@department", employeeManagerAndHeadDetails?.DepartmentName);
                MailBody = MailBody.Replace("@resignationDate", resignationDetails?.ResignDate?.ToString("dd MMM yyyy"));
                MailBody = MailBody.Replace("@reason", resignationDetails?.WithdrawalReason);
                MailBody = MailBody.Replace("@approverName", employeeManagerAndHeadDetails?.ApproverName);
                MailBody = MailBody.Replace("@comments", resignationDetails?.Feedback);
                MailBody = MailBody.Replace("@baseURL", baseURL);
                MailBody = MailBody.Replace("@resignationId", resignationDetails?.EmployeeResignationDetailsId.ToString());
                MailSubject = MailSubject.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                MailSubject = MailSubject.Replace("@status", resignationDetails?.Status);
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                var COOMail = appsetting.GetSection("COOEmialId").Value;
                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = HRTeamMail,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("ResignBUHeadNotification").Value == "true" ? employeeManagerAndHeadDetails?.BUHeadEmail + ',' + COOMail + ',' + employeeManagerAndHeadDetails?.ApproverEmail + ',' + employeeManagerAndHeadDetails?.ManagerEmail + ',' + employeeManagerAndHeadDetails?.EmployeeEmailAddress : employeeManagerAndHeadDetails?.EmployeeEmailAddress
                };
                mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                SendResignationStatusNotification(resignationDetails, employeeManagerAndHeadDetails?.ApproverName, true);
            }
            return mail;
        }
        #endregion
        #region Send next level approval notification
        [NonAction]
        public async Task<string> SendWithdrawalResignationApprovalMailForNextLevel(ApproveResignationView resignationDetails)
        {
            string mail = "";
            EmployeeManagerAndHeadDetailsView employeeManagerAndHeadDetails = new EmployeeManagerAndHeadDetailsView();
            var maildata = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeManagerAndHeadDetails") + resignationDetails.EmployeeId + "&approverId=" + resignationDetails.NextLevelApproverId);
            employeeManagerAndHeadDetails = JsonConvert.DeserializeObject<EmployeeManagerAndHeadDetailsView>(JsonConvert.SerializeObject(maildata?.Data));
            if (maildata != null)
            {
                resignationDetails.EmployeeName = employeeManagerAndHeadDetails?.EmployeeFullName + '-' + employeeManagerAndHeadDetails?.FormattedEmployeeId;
                resignationDetails.ApprovarName = employeeManagerAndHeadDetails?.ApproverName;
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string baseURL = appsetting.GetSection("BaseURL").Value;
                ExitManagementEmailTemplate emailTemplate = resignationDetails?.EmailTemplateList?.Where(x => x.TemplateName == "WithdrawalResignationApprovalManager").FirstOrDefault();
                string MailSubject = emailTemplate?.Subject, MailBody = emailTemplate?.Body;
                MailBody = MailBody.Replace("@approverName", employeeManagerAndHeadDetails?.ApproverName);
                MailBody = MailBody.Replace("@formattedEmployeeId", employeeManagerAndHeadDetails?.FormattedEmployeeId);
                MailBody = MailBody.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                MailBody = MailBody.Replace("@managerName", employeeManagerAndHeadDetails?.MangerName + " " + employeeManagerAndHeadDetails?.ManagerFormattedId);
                MailBody = MailBody.Replace("@department", employeeManagerAndHeadDetails?.DepartmentName);
                MailBody = MailBody.Replace("@resignationDate", resignationDetails?.ResignDate?.ToString("dd MMM yyyy"));
                MailBody = MailBody.Replace("@reason", resignationDetails?.WithdrawalReason);
                MailBody = MailBody.Replace("@baseURL", baseURL);
                MailBody = MailBody.Replace("@resignationId", resignationDetails?.EmployeeResignationDetailsId.ToString());
                MailSubject = MailSubject.Replace("@employeeName", employeeManagerAndHeadDetails?.EmployeeFullName);
                //MailBody = MailBody.Replace("@comments", resignationDetails?.Feedback);
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                var COOMail = appsetting.GetSection("COOEmialId").Value;
                SendEmailView sendMailbyleaverequest = new();
                sendMailbyleaverequest = new()
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = employeeManagerAndHeadDetails?.ApproverEmail,
                    Subject = MailSubject,
                    MailBody = MailBody,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                    CC = appsetting.GetSection("ResignBUHeadNotification").Value == "true" ? employeeManagerAndHeadDetails?.BUHeadEmail + ',' + HRTeamMail + ',' + employeeManagerAndHeadDetails?.ManagerEmail + ',' + employeeManagerAndHeadDetails?.EmployeeEmailAddress : employeeManagerAndHeadDetails?.EmployeeEmailAddress
                };
                mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                SendResignationNextLevelNotification(resignationDetails, true);
            }
            return mail;
        }
        #endregion

        #region Get employee list
        [HttpGet]
        [Route("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList(int employeeId)
        {
            try
            {
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeList") + employeeId);
                List<EmployeeList> employeeListView = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeListView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetEmployeeList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion
        #region List All States By Country Id
        [HttpGet]
        [Route("ListAllStateByCountryId")]
        public async Task<IActionResult> ListAllStateByCountryId(int CountryId)
        {
            List<State> states = new();
            try
            {
                var result = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:ListAllStateByCountryId") + CountryId);
                states = JsonConvert.DeserializeObject<List<State>>(JsonConvert.SerializeObject(result.Data));
                if (states?.Count > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        ListOfStates = states
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/ListAllStateByCountryId", Convert.ToString(CountryId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfStates = states
            });
        }
        #endregion

        #region Send mail for  resignation notification 
        [NonAction]
        public async Task<string> SendResignationApprovalNotification(EmployeeResignationDetailsView resignationDetails, bool isWithdrawal)
        {
            string buttonName = null, requestType = null;
            List<int> employeeId = new List<int>();
            employeeId.Add(resignationDetails.EmployeeId);
            List<Notifications> notifications = new();
            Notifications notification = new();
            Notifications empNotification = new();
            if (isWithdrawal)
            {
                requestType = "Resignation Withdrawal";
                buttonName = "Approve Resignation Withdrawal";
            }
            else
            {
                requestType = "Resignation";
                buttonName = "Approve Resignation";
            }
            notification = new()
            {
                CreatedBy = resignationDetails?.CreatedBy == null ? 0 : (int)resignationDetails?.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                FromId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
                ToId = resignationDetails?.ApproverId == null ? 0 : (int)resignationDetails?.ApproverId,
                MarkAsRead = false,
                NotificationSubject = "New " + requestType + " request from " + resignationDetails?.EmployeeName,
                NotificationBody = "" + resignationDetails.EmployeeName + " has request for " + requestType + " Approval .",
                PrimaryKeyId = resignationDetails.EmployeeResignationDetailsId,
                ButtonName = buttonName,
                SourceType = "Resignation",
            };
            string regNotification = _commonFunction.Notification(notification).Result;
            empNotification = new Notifications
            {
                CreatedBy = resignationDetails?.CreatedBy == null ? 0 : (int)resignationDetails?.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                FromId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
                ToId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
                MarkAsRead = false,
                NotificationSubject = "" + requestType + " sent for approval.",
                NotificationBody = "Your " + requestType + " request has been sent to " + resignationDetails.ApproverName + "'s approval.",
                PrimaryKeyId = resignationDetails.EmployeeResignationDetailsId,
                ButtonName = "View " + requestType,
                SourceType = "Resignation",
            };
            string empnotification = _commonFunction.Notification(empNotification).Result;
            return regNotification;
        }
        #endregion

        #region Send mail for  resignation next Level notification 
        [NonAction]
        public async Task<string> SendResignationNextLevelNotification(ApproveResignationView resignationDetails, bool isWithdrawal)
        {
            string buttonName = null, requestType = null;
            List<int> employeeId = new List<int>();
            employeeId.Add(resignationDetails.EmployeeId);
            List<Notifications> notifications = new();
            Notifications notification = new();
            Notifications empNotification = new();

            if (isWithdrawal)
            {
                requestType = "Resignation Withdrawal";
                buttonName = "Approve Resignation Withdrawal";
            }
            else
            {
                requestType = "Resignation";
                buttonName = "Approve Resignation";
            }
            notification = new()
            {
                CreatedBy = resignationDetails?.CreatedBy == null ? 0 : (int)resignationDetails?.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                FromId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
                ToId = resignationDetails?.NextLevelApproverId == null ? 0 : (int)resignationDetails?.NextLevelApproverId,
                MarkAsRead = false,
                NotificationSubject = "New " + requestType + " request from " + resignationDetails?.EmployeeName,
                NotificationBody = "" + resignationDetails.EmployeeName + " has request for " + requestType + " Approval .",
                PrimaryKeyId = resignationDetails.EmployeeResignationDetailsId,
                ButtonName = buttonName,
                SourceType = "Resignation",
            };
            string regNotification = _commonFunction.Notification(notification).Result;

            empNotification = new Notifications
            {
                CreatedBy = resignationDetails?.CreatedBy == null ? 0 : (int)resignationDetails?.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                FromId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
                ToId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
                MarkAsRead = false,
                NotificationSubject = "" + requestType + " sent for approval.",
                NotificationBody = "Your " + requestType + " request has been sent to " + resignationDetails.ApprovarName + "'s approval.",
                PrimaryKeyId = resignationDetails.EmployeeResignationDetailsId,
                ButtonName = "View " + requestType,
                SourceType = "Resignation",
            };
            string empnotification = _commonFunction.Notification(empNotification).Result;

            return regNotification;
        }
        #endregion

        #region Send mail for  resignation status notification 
        [NonAction]
        public async Task<string> SendResignationStatusNotification(ApproveResignationView resignationDetails, string approverName, bool isWithdrawal)
        {
            string buttonName = null, requestType = null;
            List<int> employeeId = new List<int>();
            employeeId.Add(resignationDetails.EmployeeId);
            List<Notifications> notifications = new();
            Notifications notification = new();
            if (isWithdrawal)
            {
                requestType = "Resignation Withdrawal";
                buttonName = "View Resignation Withdrawal";
            }
            else
            {
                requestType = "Resignation";
                buttonName = "View Resignation";
            }
            notification = new()
            {
                CreatedBy = resignationDetails?.CreatedBy == null ? 0 : (int)resignationDetails?.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                FromId = resignationDetails?.NextLevelApproverId == null ? 0 : (int)resignationDetails?.NextLevelApproverId,
                ToId = resignationDetails?.EmployeeId == null ? 0 : (int)resignationDetails?.EmployeeId,
                MarkAsRead = false,
                NotificationSubject = "Your " + requestType + " request is  " + resignationDetails?.Status,
                NotificationBody = "" + approverName + " has " + resignationDetails.Status + " your " + requestType + " request.",
                PrimaryKeyId = resignationDetails.EmployeeResignationDetailsId,
                ButtonName = buttonName,
                SourceType = "Resignation",
            };
            string regNotification = _commonFunction.Notification(notification).Result;
            return regNotification;
        }
        #endregion

        #region Insert/Update resignation Interview      
        [HttpPost]
        [Route("InsertAndUpdateResignationInterviewDetails")]
        public async Task<IActionResult> InsertAndUpdateResignationInterviewDetails(ResignationInterviewDetailView resignationInterviewDetail)
        {
            ResignationInterviewDetailView resignationDetails = new ResignationInterviewDetailView();

            try
            {

                var result = await _client.PostAsJsonAsync(resignationInterviewDetail, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:InsertAndUpdateResignationInterviewDetails"));
                resignationDetails = JsonConvert.DeserializeObject<ResignationInterviewDetailView>(JsonConvert.SerializeObject(result?.Data));

                if (result != null && result?.StatusCode == "SUCCESS")
                {
                    SendExitInterviewMail(resignationDetails);

                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        resignationDetails?.ResignationInterviewId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/InsertAndUpdateResignationInterviewDetails", JsonConvert.SerializeObject(resignationInterviewDetail));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                resignationDetails?.ResignationInterviewId
            });
        }
        #endregion
        #region Send mail for exit interview
        [NonAction]
        public async Task<string> SendExitInterviewMail(ResignationInterviewDetailView exitDetails)
        {
            string mail = "";
            try
            {
                if (exitDetails.EmailTemplate != null)
                {
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    string baseURL = appsetting.GetSection("BaseURL").Value;
                    string MailSubject = exitDetails?.EmailTemplate?.Subject, MailBody = exitDetails?.EmailTemplate?.Body;
                    MailBody = MailBody.Replace("@employeeName", exitDetails?.FormattedEmployeeId + " " + exitDetails?.EmployeeName.Replace('-' + exitDetails?.FormattedEmployeeId, ""));
                    MailBody = MailBody.Replace("@exitInterviewId", exitDetails?.ResignationInterviewId.ToString());
                    MailBody = MailBody.Replace("@baseURL", baseURL);
                    var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                    SendEmailView sendMailbyleaverequest = new();
                    sendMailbyleaverequest = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = HRTeamMail,
                        Subject = MailSubject,
                        MailBody = MailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = exitDetails?.EmployeeEmailAddress
                    };
                    mail = _commonFunction.NotificationMail(sendMailbyleaverequest).Result;
                }
                string buttonName = null, requestType = null;
                List<Notifications> notifications = new();
                Notifications notification = new();
                requestType = "Resignation";
                buttonName = "View Exit Interview";
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListBySystemRole") + "Super Admin");
                List<int> employeeList = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(result?.Data));
                foreach (int employeeId in employeeList)
                {
                    notification = new()
                    {
                        CreatedBy = exitDetails?.CreatedBy == null ? 0 : (int)exitDetails?.CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        FromId = exitDetails?.EmployeeID == null ? 0 : (int)exitDetails?.EmployeeID,
                        ToId = employeeId == null ? 0 : (int)employeeId,
                        MarkAsRead = false,
                        NotificationSubject = "New Exit Interview Application from " + exitDetails.EmployeeName,
                        NotificationBody = "" + exitDetails.EmployeeName + " has submitted the Exit interview",
                        PrimaryKeyId = exitDetails.ResignationInterviewId,
                        ButtonName = buttonName,
                        SourceType = "Resignation",
                    };
                    string regNotification = _commonFunction.Notification(notification).Result;
                }

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/SendExitInterviewMail", JsonConvert.SerializeObject(exitDetails));
            }

            return mail;
        }
        #endregion
        #region Get resignation interview details by employee id    
        [HttpGet]
        [Route("GetResignationInterviewDetailByEmployeeId")]
        public async Task<IActionResult> GetResignationInterviewDetailByEmployeeId(int employeeId)
        {
            ResignationInterviewListView resignationDetails = new ResignationInterviewListView();
            //DateTime? resignationDate;
            List<ResignationEmployeeMasterView> employeeListView = new List<ResignationEmployeeMasterView>();
            try
            {
                var result = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationInterviewDetailByEmployeeId") + employeeId);
                resignationDetails = JsonConvert.DeserializeObject<ResignationInterviewListView>(JsonConvert.SerializeObject(result?.Data));
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForResignation") + employeeId);
                employeeListView = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(employeeResult?.Data));

                var employeeResignResult = await _client.PostAsJsonAsync(employeeListView.Select(x => x.EmployeeID).ToList(), _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetLastResignationIdByEmployeeList"));
                List<KeyWithIntValue> empResignDetails = JsonConvert.DeserializeObject<List<KeyWithIntValue>>(JsonConvert.SerializeObject(employeeResignResult?.Data));
                employeeListView?.ForEach(x => x.ResignationDetailsId = empResignDetails?.Where(y => y.Key == x.EmployeeID)?.Select(x => x.Value).FirstOrDefault());
                var employeeInterviewResult = await _client.PostAsJsonAsync(resignationDetails?.ResignationInterviewDetailList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeExitInterviewDetails"));
                resignationDetails.ResignationInterviewDetailList = JsonConvert.DeserializeObject<List<ResignationInterviewDetailView>>(JsonConvert.SerializeObject(employeeInterviewResult?.Data));
                resignationDetails.isEnableExitInterview = true;
                if (employeeId > 0)
                {
                    resignationDetails.isEnableExitInterview = false;
                    int? resignId = empResignDetails?.Where(x => x.Key == employeeId).Select(x => x.Value).FirstOrDefault();
                    if (resignId != null && resignId > 0)
                    {
                        ResignationInterviewDetailView detail = resignationDetails?.ResignationInterviewDetailList?.Where(x => x.ResignationDetailsId == resignId).FirstOrDefault();
                        if (detail == null)
                        {
                            resignationDetails.isEnableExitInterview = true;
                        }
                    }


                }
                if (result?.StatusCode == "SUCCESS")
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        Data = resignationDetails,
                        EmployeeList = employeeListView
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignationInterviewDetailByEmployeeId", JsonConvert.SerializeObject(employeeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = resignationDetails,
                EmployeeList = new List<Employees>()
            });
        }
        #endregion
        #region Get resignation interview details by interview id    
        [HttpGet]
        [Route("GetResignationInterviewDetailById")]
        public async Task<IActionResult> GetResignationInterviewDetailById(int resignationInterviewId)
        {
            ResignationInterviewDetailView resignationDetails = new ResignationInterviewDetailView();

            try
            {
                var result = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationInterviewDetailById") + resignationInterviewId);
                resignationDetails = JsonConvert.DeserializeObject<ResignationInterviewDetailView>(JsonConvert.SerializeObject(result?.Data));
                List<ResignationInterviewDetailView> interviewList = new List<ResignationInterviewDetailView>();
                interviewList.Add(resignationDetails);
                var employeeResult = await _client.PostAsJsonAsync(interviewList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeExitInterviewDetails"));
                interviewList = JsonConvert.DeserializeObject<List<ResignationInterviewDetailView>>(JsonConvert.SerializeObject(employeeResult?.Data));
                if (result?.StatusCode == "SUCCESS")
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText,
                        Data = interviewList.FirstOrDefault()
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignationInterviewDetailById", JsonConvert.SerializeObject(resignationInterviewId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = resignationDetails
            });
        }
        #endregion
        #region Get Resignation interview master data
        [HttpGet]
        [Route("GetResignationInterviewMasterData")]
        public async Task<IActionResult> GetResignationInterviewMasterData(int employeeId)
        {
            //ResignationInterviewMasterData ResignationInterviewMasterData = new ResignationInterviewMasterData();
            List<int> employeeIdList = new List<int>();
            try
            {
                if (employeeId > 0)
                {
                    employeeIdList.Add(employeeId);
                }
                var result = _client.PostAsJsonAsync(employeeIdList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetExitEmployeeMaster"));
                List<ResignationEmployeeMasterView> EmployeeList = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Result?.Data));

                var reasonResult = await _client.PostAsJsonAsync(EmployeeList, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationInterviewMasterData"));
                ResignationInterviewMasterData resignationReason = JsonConvert.DeserializeObject<ResignationInterviewMasterData>(JsonConvert.SerializeObject(reasonResult?.Data));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    EmployeeDetails = resignationReason
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignationInterviewMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = new ResignationInterviewMasterData()
            });
        }
        #endregion

        #region Delete Applied Exit Interview
        [HttpGet]
        [Route("DeleteExitInterviewByInterviewId")]
        public async Task<IActionResult> DeleteExitInterviewByInterviewId(int interviewId)
        {
            try
            {
                var Result = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:DeleteExitInterviewByInterviewId") + interviewId);
                if (Result != null && Result?.StatusCode?.ToLower() == "SUCCESS".ToLower())
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Exit Interview is deleted successfully.",
                        data = true
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/DeleteExitInterviewByInterviewId", JsonConvert.SerializeObject(interviewId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                data = false
            });
        }
        #endregion

        //#region Check enable exit interview
        //[NonAction]
        //public bool checkEnableExitInterview(ResignationInterviewDetailView exitInterviewDetail)
        //{

        //    if (exitInterviewDetail.ResignationDate != null)
        //    {
        //        var isEnable = DateTime.Compare((DateTime)exitInterviewDetail?.ResignationDate, (DateTime)exitInterviewDetail.CreatedOn);
        //        if (isEnable > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //#endregion


        //#region Check If the Add interview record enable
        //[HttpGet]
        //[Route("GetAddInterviewEnable")]
        //public async Task<IActionResult> GetAddInterviewEnable(int employeeId)
        //{
        //    DateTime? resignationDate;
        //    bool isEnable = false;
        //    try
        //    {
        //        var resignationResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeResignationDate") + employeeId);
        //        resignationDate = JsonConvert.DeserializeObject<DateTime?>(JsonConvert.SerializeObject(resignationResult?.Data));
        //        if (resignationDate != null)
        //        {
        //            ExitInterviewEnable exitInterview = new ExitInterviewEnable();
        //            exitInterview.ResignationDate = resignationDate;
        //            exitInterview.EmployeeId = employeeId;
        //            var result = await _client.PostAsJsonAsync(exitInterview,_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetCheckExitInterviewEnable"));
        //            isEnable = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
        //        }
        //        else
        //        {
        //            isEnable = false;
        //        } 

        //            return Ok(new
        //            {
        //                StatusCode = "SUCCESS",
        //                StatusText = "",
        //                data = isEnable
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetAddInterviewEnable", JsonConvert.SerializeObject(employeeId));
        //    }
        //    return Ok(new
        //    {
        //        StatusCode = "FAILURE",
        //        StatusText = strErrorMsg,
        //        data = false
        //    });
        //}
        //#endregion

        #region Get Resignation checklist master data
        [HttpGet]
        [Route("GetResignationChecklistMasterData")]
        public async Task<IActionResult> GetResignationChecklistMasterData(int employeeId)
        {
            List<int> employeeIdList = new List<int>();
            try
            {
                if (employeeId > 0)
                {
                    employeeIdList.Add(employeeId);
                }
                var result = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForResignation") + employeeId);
                List<ResignationEmployeeMasterView> EmployeeList = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Result?.Data));

                var reasonResult = await _client.PostAsJsonAsync(EmployeeList, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationChecklistMasterData"));
                ResignationChecklistMasterData resignationDetails = JsonConvert.DeserializeObject<ResignationChecklistMasterData>(JsonConvert.SerializeObject(reasonResult?.Data));
                if (resignationDetails == null)
                {
                    resignationDetails = new();
                }
                if (EmployeeList?.Count > 0)
                {
                    ResignationEmployeeMasterView employee = EmployeeList.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
                    resignationDetails.EmployeeID = employee == null ? 0 : (int)employee.EmployeeID;
                    resignationDetails.EmployeeName = employee?.EmployeeName;
                    resignationDetails.DepartmentName = employee?.DepartmentName;
                    resignationDetails.FormattedEmployeeID = employee?.FormattedEmployeeID;
                    resignationDetails.Designation = employee?.Designation;
                    resignationDetails.EmployeeType = employee?.EmployeeType;
                    resignationDetails.DateOfJoining = employee?.DateOfJoining;
                }
                List<StringKeyWithValue> roleEdit = new List<StringKeyWithValue>();
                roleEdit.Add(new StringKeyWithValue() { Key = "pmo", Value = _configuration.GetValue<string>("ChecklistRole:PMO") });
                roleEdit.Add(new StringKeyWithValue() { Key = "it", Value = _configuration.GetValue<string>("ChecklistRole:IT") });
                roleEdit.Add(new StringKeyWithValue() { Key = "admin", Value = _configuration.GetValue<string>("ChecklistRole:Admin") });
                roleEdit.Add(new StringKeyWithValue() { Key = "finance", Value = _configuration.GetValue<string>("ChecklistRole:Finance") });
                roleEdit.Add(new StringKeyWithValue() { Key = "hr", Value = _configuration.GetValue<string>("ChecklistRole:HR") });
                resignationDetails.CheckListEdit = roleEdit;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    EmployeeDetails = resignationDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignationChecklistMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EmployeeDetails = new ResignationChecklistMasterData()
            });
        }
        #endregion
        #region Insert/Update resignation checklist      
        [HttpPost]
        [Route("InsertOrUpdateResignationChecklist")]
        public async Task<IActionResult> InsertOrUpdateResignationChecklist(ResignationChecklistView resignationChecklistDetail)
        {
            try
            {
                ResignationChecklistView resignationDetails = new ResignationChecklistView();
                //var noticeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeByEmployeeId") + resignationChecklistDetail.EmployeeID);
                //Employees employee = JsonConvert.DeserializeObject<Employees>(JsonConvert.SerializeObject(noticeResult?.Data));
                //if(employee != null)
                //{
                //    resignationChecklistDetail.ResignDate = employee.ResignationDate;
                //}
                var result = await _client.PostAsJsonAsync(resignationChecklistDetail, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:InsertOrUpdateResignationChecklist"));
                resignationDetails = JsonConvert.DeserializeObject<ResignationChecklistView>(JsonConvert.SerializeObject(result?.Data));

                if (result != null && result?.StatusCode == "SUCCESS")
                {
                    if (resignationChecklistDetail?.ResignationChecklistId == 0 && resignationChecklistDetail?.ManagerCheckList == null)
                    {
                        SendManagerChecklistNotification(resignationDetails);
                    }
                    //else if (resignationChecklistDetail.IsSubmit == true && resignationChecklistDetail?.ManagerCheckList != null && resignationChecklistDetail?.PMOCheckList == null && resignationDetails?.IsManagerSubmited == false)
                    //{
                    //    SendManagerChecklistNotification(resignationDetails, _configuration.GetValue<string>("ChecklistRole:PMO"));
                    //}
                    else if (resignationChecklistDetail.IsSubmit == true && resignationChecklistDetail?.ManagerCheckList != null && resignationChecklistDetail?.ITCheckList == null && resignationDetails?.IsManagerSubmited == false)
                    {
                        SendManagerChecklistNotification(resignationDetails, _configuration.GetValue<string>("ChecklistRole:IT"));
                    }
                    else if (resignationChecklistDetail.IsSubmit == true && resignationChecklistDetail?.ITCheckList != null && resignationChecklistDetail?.AdminCheckList == null && resignationDetails?.IsITSubmited == false)
                    {
                        SendManagerChecklistNotification(resignationDetails, _configuration.GetValue<string>("ChecklistRole:Admin"));
                    }
                    else if (resignationChecklistDetail.IsSubmit == true && resignationChecklistDetail?.AdminCheckList != null && resignationChecklistDetail?.FinanceCheckList == null && resignationDetails?.IsAdminSubmited == false)
                    {
                        SendManagerChecklistNotification(resignationDetails, _configuration.GetValue<string>("ChecklistRole:Finance"));
                    }
                    else if (resignationChecklistDetail.IsSubmit == true && resignationChecklistDetail?.FinanceCheckList != null && resignationChecklistDetail?.HRCheckList == null && resignationDetails?.IsFinanceSubmited == false)
                    {
                        SendManagerChecklistNotification(resignationDetails, _configuration.GetValue<string>("ChecklistRole:HR"));
                    }

                }

                return Ok(new
                {
                    result.StatusCode,
                    result.StatusText,
                    Data = resignationDetails?.ResignationChecklistId
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/InsertOrUpdateResignationChecklist", JsonConvert.SerializeObject(resignationChecklistDetail));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = 0
            });
        }
        #endregion
        #region Get resignation checklist details by id
        [HttpGet]
        [Route("GetResignationChecklistById")]
        public async Task<IActionResult> GetResignationChecklistById(int employeeId, int resignationChecklistId, int loginUserId)
        {

            try
            {
                //var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetExitCheckListRole") + employeeId + "&loginUserId=" + loginUserId+ "&isAllReportees="+ isAllReportees);
                //List<string> checklistEdit = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(result?.Data));
                ResignationChecklistView details = new ResignationChecklistView();
                details.ResignationChecklistId = resignationChecklistId;
                var reasonResult = await _client.PostAsJsonAsync(details, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationChecklistById"));
                ResignationChecklistView resignationDetails = JsonConvert.DeserializeObject<ResignationChecklistView>(JsonConvert.SerializeObject(reasonResult?.Data));
                if (resignationDetails == null)
                {
                    resignationDetails = new ResignationChecklistView();
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = resignationDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetResignationChecklistById", JsonConvert.SerializeObject(employeeId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = new ResignationChecklistView()
            });
        }
        #endregion
        #region Get my Checklist Details
        [HttpGet]
        [Route("GetMyCheckListDetails")]
        public async Task<IActionResult> GetMyCheckListDetails(int employeeID, bool isAdmin)
        {
            List<ResignationChecklistDetails> checklistDetails = new();
            AllResignationInputView allResignation = new AllResignationInputView();
            allResignation.EmployeeId = employeeID;
            allResignation.IsAdmin = isAdmin;
            List<ResignationEmployeeMasterView> employeeListView = new List<ResignationEmployeeMasterView>();
            try
            {
                var result = await _client.PostAsJsonAsync(allResignation, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetMyCheckListDetails"));
                checklistDetails = JsonConvert.DeserializeObject<List<ResignationChecklistDetails>>(JsonConvert.SerializeObject(result?.Data));
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForResignation") + (isAdmin ? 0 : employeeID));
                employeeListView = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(employeeResult?.Data));

                var employeeResignResult = await _client.PostAsJsonAsync(employeeListView?.Select(x => x.EmployeeID).ToList(), _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetLastResignationIdByEmployeeList"));
                List<KeyWithIntValue> empResignDetails = JsonConvert.DeserializeObject<List<KeyWithIntValue>>(JsonConvert.SerializeObject(employeeResignResult?.Data));
                employeeListView?.ForEach(x => x.ResignationDetailsId = empResignDetails?.Where(y => y.Key == x.EmployeeID).Select(x => x.Value).FirstOrDefault());
                foreach (ResignationChecklistDetails item in checklistDetails)
                {
                    ResignationEmployeeMasterView emp = employeeListView.Where(x => x.EmployeeID == item.EmployeeID).FirstOrDefault();
                    item.EmployeeName = emp.EmployeeName;
                    item.FormattedEmployeeId = emp.FormattedEmployeeID;
                    item.IsActive = emp.IsActive == true ? "Active Employee" : "Ex Employee";
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = checklistDetails,
                    EmployeeList = employeeListView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetMyCheckListDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = checklistDetails,
                    EmployeeList = employeeListView
                });
            }
        }
        #endregion
        #region Get reportees Checklist Details
        [HttpGet]
        [Route("GetReporteesCheckListDetails")]
        public async Task<IActionResult> GetReporteesCheckListDetails(int employeeID, bool isAllReportees)
        {

            List<ChecklistEmployeeView> employeeChecklistDetails = new List<ChecklistEmployeeView>();
            AllResignationInputView allResignation = new AllResignationInputView();
            allResignation.EmployeeId = employeeID;
            allResignation.IsAllData = isAllReportees;
            try
            {
                //employeeList.Add(employeeID);
                var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetReporteesCheckListEmployee") + employeeID + "&isAll=" + isAllReportees);
                ReporteesChecklistEmployeeView employeeDetailList = JsonConvert.DeserializeObject<ReporteesChecklistEmployeeView>(JsonConvert.SerializeObject(employeeListresult.Data));
                List<int> employeeList = employeeDetailList?.EmployeeDetails?.Select(ea => ea.EmployeeId).ToList();
                allResignation.ReporteesList = employeeList;
                allResignation.EmployeeRole = employeeDetailList?.Role?.Distinct()?.ToList();
                allResignation.PMORole = _configuration.GetValue<string>("ChecklistRole:PMO");
                allResignation.ITRole = _configuration.GetValue<string>("ChecklistRole:IT");
                allResignation.AdminRole = _configuration.GetValue<string>("ChecklistRole:Admin");
                allResignation.FinanceRole = _configuration.GetValue<string>("ChecklistRole:Finance");
                allResignation.HRRole = _configuration.GetValue<string>("ChecklistRole:HR");
                var result = await _client.PostAsJsonAsync(allResignation, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetReporteesCheckListDetails"));
                employeeChecklistDetails = JsonConvert.DeserializeObject<List<ChecklistEmployeeView>>(JsonConvert.SerializeObject(result?.Data));

                var employeeResult = await _client.PostAsJsonAsync(employeeChecklistDetails, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetCheckListEmployeeDetails"));
                employeeChecklistDetails = JsonConvert.DeserializeObject<List<ChecklistEmployeeView>>(JsonConvert.SerializeObject(employeeResult?.Data));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = employeeChecklistDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetAllResignationDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = employeeChecklistDetails
                });
            }
        }
        #endregion
        #region delete Checklist Details
        [HttpGet]
        [Route("DeleteChecklistById")]
        public async Task<IActionResult> DeleteChecklistById(int checklistId)
        {
            try
            {
                var result = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:DeleteChecklistById") + checklistId);
                bool data = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));

                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/DeleteChecklistById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion

        #region Send mail for submit checklist
        [NonAction]
        public async Task<string> SendManagerChecklistNotification(ResignationChecklistView resignationDetails)
        {
            string mail = "";
            try
            {
                if (resignationDetails?.ExitCheckListTemplate != null)
                {
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    string baseURL = appsetting.GetSection("BaseURL").Value;
                    var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                    List<int> employeeIdList = new List<int>() { resignationDetails.EmployeeID };
                    var result = _client.PostAsJsonAsync(employeeIdList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeMasterData"));
                    List<ResignationEmployeeMasterView> empList = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                    ResignationEmployeeMasterView employee = empList?.FirstOrDefault();

                    //Notify to Manager
                    string managerMailSubject = resignationDetails?.ExitCheckListTemplate?.Subject, managerMailBody = resignationDetails?.ExitCheckListTemplate?.Body;
                    managerMailSubject = managerMailSubject.Replace("@employeeFullName", employee?.FormattedEmployeeID + " " + employee?.FirstName + " " + employee?.LastName);
                    managerMailBody = managerMailBody.Replace("@employeeName", employee?.FirstName + " " + employee?.LastName);
                    managerMailBody = managerMailBody.Replace("@formattedEmployeeId", employee?.FormattedEmployeeID);
                    managerMailBody = managerMailBody.Replace("@designation", employee?.Designation);
                    managerMailBody = managerMailBody.Replace("@dateOfJoining", employee?.DateOfJoining?.ToString("dd MMM yyyy"));
                    managerMailBody = managerMailBody.Replace("@resignationDate", employee?.ResignationDate?.ToString("dd MMM yyyy"));
                    managerMailBody = managerMailBody.Replace("@relievingDate", employee?.RelievingDate?.ToString("dd MMM yyyy"));
                    managerMailBody = managerMailBody.Replace("@resignationCheckListId", resignationDetails?.ResignationChecklistId.ToString());
                    managerMailBody = managerMailBody.Replace("@employeeId", resignationDetails?.EmployeeID.ToString());
                    managerMailBody = managerMailBody.Replace("@baseURL", baseURL);
                    managerMailBody = managerMailBody.Replace("@role", "manager");
                    SendEmailView managerChecklistRequest = new();
                    managerChecklistRequest = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = employee.ReportingManagerEmail,
                        Subject = managerMailSubject,
                        MailBody = managerMailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = HRTeamMail
                    };
                    mail = _commonFunction.NotificationMail(managerChecklistRequest).Result;


                    //Employee Notification
                    Notifications notification = new();
                    notification = new()
                    {
                        CreatedBy = resignationDetails.EmployeeID,
                        CreatedOn = DateTime.UtcNow,
                        FromId = resignationDetails.EmployeeID,
                        ToId = resignationDetails.EmployeeID,
                        MarkAsRead = false,
                        NotificationSubject = "View your Exit Checklist Status",
                        NotificationBody = "View your Exit Checklist Status",
                        PrimaryKeyId = resignationDetails?.ResignationChecklistId,
                        Data = resignationDetails?.EmployeeID.ToString(),
                        ButtonName = "View Exit Checklist",
                        SourceType = "Resignation",
                    };
                    string regNotification = _commonFunction.Notification(notification).Result;
                    //Manager Notification
                    Notifications managerNotification = new();

                    managerNotification = new()
                    {
                        CreatedBy = resignationDetails.EmployeeID,
                        CreatedOn = DateTime.UtcNow,
                        FromId = resignationDetails.EmployeeID,
                        ToId = employee.ReportingManagerId == null ? 0 : (int)employee.ReportingManagerId,
                        MarkAsRead = false,
                        NotificationSubject = "Exit Checklist Application from " + employee?.FormattedEmployeeID + " " + employee?.FirstName + " " + employee?.LastName,
                        NotificationBody = "Request to complete " + employee?.FormattedEmployeeID + " " + employee?.FirstName + " " + employee?.LastName + " Exit Checklist",
                        PrimaryKeyId = resignationDetails?.ResignationChecklistId,
                        Data = resignationDetails?.EmployeeID.ToString() + ",manager",
                        ButtonName = "View Exit Checklist",
                        SourceType = "Resignation",
                    };
                    string managerNotificationResponse = _commonFunction.Notification(managerNotification).Result;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/SendSubmitChecklistNotification", JsonConvert.SerializeObject(resignationDetails));
            }

            return mail;
        }
        #endregion

        #region Send mail for submit checklist
        [NonAction]
        public async Task<string> SendManagerChecklistNotification(ResignationChecklistView resignationDetails, string role)
        {
            string mail = "";
            try
            {
                if (resignationDetails?.ExitCheckListTemplate != null)
                {
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    string baseURL = appsetting.GetSection("BaseURL").Value;
                    var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                    List<int> employeeIdList = new List<int>() { resignationDetails.EmployeeID };
                    var result = _client.PostAsJsonAsync(employeeIdList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeMasterData"));
                    List<ResignationEmployeeMasterView> empList = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                    ResignationEmployeeMasterView employee = empList?.FirstOrDefault();
                    //Notify to next level role
                    var roleResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesDetailsBySystemRole") + role);
                    List<ResignationEmployeeMasterView> employeeList = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(roleResult?.Data));
                    if (employeeList?.Count > 0)
                    {
                        var receiverEmails = "";
                        List<string> receiverEmailList = new List<string>();
                        foreach (ResignationEmployeeMasterView employeeDetail in employeeList)
                        {
                            receiverEmailList.Add(employeeDetail.EmployeeEmail);
                            //Manager Notification
                            Notifications managerNotification = new();
                            managerNotification = new()
                            {
                                CreatedBy = resignationDetails.EmployeeID,
                                CreatedOn = DateTime.UtcNow,
                                FromId = resignationDetails.EmployeeID,
                                ToId = employeeDetail.EmployeeID,
                                MarkAsRead = false,
                                NotificationSubject = "Exit Checklist Application from " + employee?.FormattedEmployeeID + " " + employee?.FirstName + " " + employee?.LastName,
                                NotificationBody = "Request to complete " + employee?.FormattedEmployeeID + " " + employee?.FirstName + " " + employee?.LastName + " Exit Checklist",
                                PrimaryKeyId = resignationDetails?.ResignationChecklistId,
                                Data = resignationDetails?.EmployeeID.ToString() + "," + role,
                                ButtonName = "View Exit Checklist",
                                SourceType = "Resignation",
                            };
                            string managerNotificationResponse = _commonFunction.Notification(managerNotification).Result;
                        }
                        receiverEmails = string.Join(",", receiverEmailList);
                        string managerMailSubject = resignationDetails?.ExitCheckListTemplate?.Subject, managerMailBody = resignationDetails?.ExitCheckListTemplate?.Body;
                        managerMailSubject = managerMailSubject.Replace("@employeeFullName", employee?.FormattedEmployeeID + " " + employee?.FirstName + " " + employee?.LastName);
                        managerMailBody = managerMailBody.Replace("@employeeName", employee?.FirstName + " " + employee?.LastName);
                        managerMailBody = managerMailBody.Replace("@formattedEmployeeId", employee?.FormattedEmployeeID);
                        managerMailBody = managerMailBody.Replace("@designation", employee?.Designation);
                        managerMailBody = managerMailBody.Replace("@dateOfJoining", employee?.DateOfJoining?.ToString("dd MMM yyyy"));
                        managerMailBody = managerMailBody.Replace("@resignationDate", employee?.ResignationDate?.ToString("dd MMM yyyy"));
                        managerMailBody = managerMailBody.Replace("@relievingDate", employee?.RelievingDate?.ToString("dd MMM yyyy"));
                        managerMailBody = managerMailBody.Replace("@resignationCheckListId", resignationDetails?.ResignationChecklistId.ToString());
                        managerMailBody = managerMailBody.Replace("@employeeId", resignationDetails?.EmployeeID.ToString());
                        managerMailBody = managerMailBody.Replace("@baseURL", baseURL);
                        managerMailBody = managerMailBody.Replace("@role", role);
                        SendEmailView managerChecklistRequest = new();
                        managerChecklistRequest = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = receiverEmails,
                            Subject = managerMailSubject,
                            MailBody = managerMailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                            CC = HRTeamMail
                        };
                        mail = _commonFunction.NotificationMail(managerChecklistRequest).Result;
                    }


                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/SendSubmitChecklistNotification", JsonConvert.SerializeObject(resignationDetails));
            }

            return mail;
        }
        #endregion
        #region Get Employee exit interview  Remainder Notification 
        [HttpGet]
        [Route("ResignationInterviewNotification")]
        [AllowAnonymous]
        public async Task<IActionResult> ResignationInterviewNotification()
        {
            NotificationMasterData employeeInterviewDetails = new NotificationMasterData();
            List<ResignedEmployeeView> employeeDetails = new();
            try
            {
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                int days = _configuration.GetValue<int>("ExitNotificationConfiguration:ExitInterviewNotificationDays");
                var employeeResult = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationInterviewNotification") + (-days));
                employeeInterviewDetails = JsonConvert.DeserializeObject<NotificationMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                var result = await _client.PostAsJsonAsync(employeeInterviewDetails?.EmployeeList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDetailsById"));
                employeeDetails = JsonConvert.DeserializeObject<List<ResignedEmployeeView>>(JsonConvert.SerializeObject(result?.Data));
                foreach (ResignedEmployeeView employee in employeeDetails)
                {
                    SendEmailView sendMailDetails = new();
                    string baseURL = appsetting.GetSection("BaseURL").Value;
                    string mailSubject = employeeInterviewDetails?.EmailTemplate.Subject, mailBody = employeeInterviewDetails?.EmailTemplate.Body;
                    mailBody = mailBody.Replace("@employeeName", employee?.EmployeeName);
                    mailBody = mailBody.Replace("@baseURL", baseURL);
                    sendMailDetails = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = employee.EmployeeEmailId,
                        Subject = mailSubject,
                        MailBody = mailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = HRTeamMail
                    };
                    string mail = _commonFunction.NotificationMail(sendMailDetails).Result;
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = true
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetMyCheckListDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion        

        #region Get Checklist Completion Notification Employee
        [HttpGet]
        [Route("ChecklistCompleteEmployeeNotification")]
        [AllowAnonymous]
        public async Task<IActionResult> ChecklistCompleteEmployeeNotification()
        {
            try
            {
                ChecklistSubmissionEmployeeNotification();
                NotificationMasterData employeeChecklistDetails = new NotificationMasterData();
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                List<ResignedEmployeeView> employeeDetails = new();
                int days = _configuration.GetValue<int>("ExitNotificationConfiguration:EXitChecklistNotificationDays");
                var result = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetChecklistCompleteNotificationEmployee") + (-days));
                employeeChecklistDetails = JsonConvert.DeserializeObject<NotificationMasterData>(JsonConvert.SerializeObject(result?.Data));
                foreach (ResignationChecklist checkListDetails in employeeChecklistDetails?.ChecklistEmployeeList)
                {
                    if (checkListDetails.ManagerStatus == "Pending" || checkListDetails.ManagerStatus == "In-Progress")
                    {
                        this.SendManagerChecklistRemainderNotification(checkListDetails, employeeChecklistDetails?.EmailTemplate, "manager");
                    }
                    else if (checkListDetails.PMOStatus == "Pending" || checkListDetails.PMOStatus == "In-Progress")
                    {
                        this.SendManagerChecklistRemainderNotification(checkListDetails, employeeChecklistDetails?.EmailTemplate, _configuration.GetValue<string>("ChecklistRole:PMO"));
                    }
                    else if (checkListDetails.ITStatus == "Pending" || checkListDetails.ITStatus == "In-Progress")
                    {
                        this.SendManagerChecklistRemainderNotification(checkListDetails, employeeChecklistDetails?.EmailTemplate, _configuration.GetValue<string>("ChecklistRole:IT"));
                    }
                    else if (checkListDetails.AdminStatus == "Pending" || checkListDetails.AdminStatus == "In-Progress")
                    {
                        this.SendManagerChecklistRemainderNotification(checkListDetails, employeeChecklistDetails?.EmailTemplate, _configuration.GetValue<string>("ChecklistRole:Admin"));
                    }
                    else if (checkListDetails.FinanceStatus == "Pending" || checkListDetails.FinanceStatus == "In-Progress")
                    {
                        this.SendManagerChecklistRemainderNotification(checkListDetails, employeeChecklistDetails?.EmailTemplate, _configuration.GetValue<string>("ChecklistRole:Finance"));
                    }
                    else if (checkListDetails.HRStatus == "Pending" || checkListDetails.HRStatus == "In-Progress")
                    {
                        this.SendManagerChecklistRemainderNotification(checkListDetails, employeeChecklistDetails?.EmailTemplate, _configuration.GetValue<string>("ChecklistRole:HR"));
                    }


                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = true
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetChecklistSubmissionNotificationEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion
        #region Get Checklist Submission Notification Employee
        [NonAction]
        public async Task<IActionResult> ChecklistSubmissionEmployeeNotification()
        {
            try
            {
                NotificationMasterData employeeChecklistDetails = new NotificationMasterData();
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                List<ResignedEmployeeView> employeeDetails = new();
                int days = _configuration.GetValue<int>("ExitNotificationConfiguration:EXitChecklistNotificationDays");
                var result = await _client.GetAsync(_exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetChecklistSubmissionNotificationEmployee") + (-days));
                employeeChecklistDetails = JsonConvert.DeserializeObject<NotificationMasterData>(JsonConvert.SerializeObject(result?.Data));
                var employeeResult = await _client.PostAsJsonAsync(employeeChecklistDetails?.EmployeeList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDetailsById"));
                employeeDetails = JsonConvert.DeserializeObject<List<ResignedEmployeeView>>(JsonConvert.SerializeObject(employeeResult?.Data));
                foreach (ResignedEmployeeView employee in employeeDetails)
                {
                    SendEmailView sendMailDetails = new();
                    string baseURL = appsetting.GetSection("BaseURL").Value;
                    string mailSubject = employeeChecklistDetails?.EmailTemplate.Subject, mailBody = employeeChecklistDetails?.EmailTemplate.Body;
                    mailBody = mailBody.Replace("@employeeName", employee?.EmployeeName);
                    mailBody = mailBody.Replace("@baseURL", baseURL);
                    sendMailDetails = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = employee.EmployeeEmailId,
                        Subject = mailSubject,
                        MailBody = mailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = HRTeamMail
                    };
                    string mail = _commonFunction.NotificationMail(sendMailDetails).Result;
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = true
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/GetChecklistSubmissionNotificationEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = false
                });
            }
        }
        #endregion
        #region Send mail for submit checklist
        [NonAction]
        public async Task<string> SendManagerChecklistRemainderNotification(ResignationChecklist resignationDetails, ExitManagementEmailTemplate emailTemplate, string role)
        {
            string mail = "";
            try
            {
                List<ResignationEmployeeMasterView> employeeList = new List<ResignationEmployeeMasterView>();
                if (emailTemplate != null)
                {
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    string baseURL = appsetting.GetSection("BaseURL").Value;
                    var HRTeamMail = appsetting.GetSection("TVSN_HR").Value;
                    List<int> employeeIdList = new List<int>() { (int)resignationDetails.EmployeeID };
                    var result = _client.PostAsJsonAsync(employeeIdList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeMasterData"));
                    List<ResignationEmployeeMasterView> empList = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Result?.Data));
                    ResignationEmployeeMasterView employee = empList?.FirstOrDefault();
                    //Notify to next level role
                    if (role != "manager")
                    {
                        var roleResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesDetailsBySystemRole") + role);
                        employeeList = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(roleResult?.Data));
                    }
                    else
                    {
                        employeeList.Add(employee);
                    }
                    if (employeeList?.Count > 0)
                    {
                        string receiverEmails = string.Join(",", employeeList?.Select(x => x.EmployeeEmail).ToList());
                        string managerMailSubject = emailTemplate?.Subject, managerMailBody = emailTemplate?.Body;
                        managerMailSubject = managerMailSubject.Replace("@employeeFullName", employee?.FormattedEmployeeID + " " + employee?.FirstName + " " + employee?.LastName);
                        managerMailBody = managerMailBody.Replace("@employeeName", employee?.FirstName + " " + employee?.LastName);
                        managerMailBody = managerMailBody.Replace("@formattedEmployeeId", employee?.FormattedEmployeeID);
                        managerMailBody = managerMailBody.Replace("@designation", employee?.Designation);
                        managerMailBody = managerMailBody.Replace("@dateOfJoining", employee?.DateOfJoining?.ToString("dd MMM yyyy"));
                        managerMailBody = managerMailBody.Replace("@resignationDate", employee?.ResignationDate?.ToString("dd MMM yyyy"));
                        managerMailBody = managerMailBody.Replace("@relievingDate", employee?.RelievingDate?.ToString("dd MMM yyyy"));
                        managerMailBody = managerMailBody.Replace("@resignationCheckListId", resignationDetails?.ResignationChecklistId.ToString());
                        managerMailBody = managerMailBody.Replace("@employeeId", resignationDetails?.EmployeeID.ToString());
                        managerMailBody = managerMailBody.Replace("@baseURL", baseURL);
                        managerMailBody = managerMailBody.Replace("@role", role);
                        SendEmailView managerChecklistRequest = new();
                        managerChecklistRequest = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = role == "manager" ? employee.ReportingManagerEmail : receiverEmails,
                            Subject = managerMailSubject,
                            MailBody = managerMailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                            CC = HRTeamMail
                        };
                        mail = _commonFunction.NotificationMail(managerChecklistRequest).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/SendSubmitChecklistNotification", JsonConvert.SerializeObject(resignationDetails));
            }
            return mail;
        }
        #endregion


        #region Get Resignation employee filter list 
        [HttpPost]
        [Route("GetResignationEmployeeListByFilter")]
        public async Task<IActionResult> GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            try
            {
                EmployeeResignationDetailsList employeeResignationDetailsList = new EmployeeResignationDetailsList();
                List<ResignationEmployeeMasterView> employeeListView = new List<ResignationEmployeeMasterView>();
                //var result = await _client.PostAsJsonAsync(resignationEmployeeFilter, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeListByFilter"));
                //employeeListView = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Data));
                //OR
                List<int> employeeIdList = new List<int>();
                List<string> employeeRoleList = new List<string>();
                AllResignationInputView allResignation = new AllResignationInputView();

                if (resignationEmployeeFilter.IsManager && (resignationEmployeeFilter.IsAllReportees || resignationEmployeeFilter.IsCheckList))
                {
                    if (resignationEmployeeFilter.IsCheckList)
                    {
                        var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetReporteesCheckListEmployee") + resignationEmployeeFilter.EmployeeId + "&isAll=" + resignationEmployeeFilter.IsAllReportees);
                        ReporteesChecklistEmployeeView employeeDetailListCheckList = JsonConvert.DeserializeObject<ReporteesChecklistEmployeeView>(JsonConvert.SerializeObject(employeeListresult.Data));
                        employeeIdList = employeeDetailListCheckList?.EmployeeDetails?.Select(ea => ea.EmployeeId).ToList();
                        employeeRoleList = employeeDetailListCheckList.Role;
                    }
                    else
                    {
                        var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeListForManagerReport") + resignationEmployeeFilter.EmployeeId + "&isAll=" + resignationEmployeeFilter.IsAllReportees);
                        List<EmployeeViewDetails> employeeDetailList = JsonConvert.DeserializeObject<List<EmployeeViewDetails>>(JsonConvert.SerializeObject(employeeListresult.Data));
                        employeeIdList = employeeDetailList.Select(ea => ea.EmployeeId).ToList();
                    }

                    allResignation.IsAllData = resignationEmployeeFilter.IsAllReportees;
                }
                else
                    employeeRoleList.Add("manager");
                allResignation.EmployeeRole = employeeRoleList;
                if (resignationEmployeeFilter.EmployeeId > 0 && !resignationEmployeeFilter.IsCheckList)
                    employeeIdList.Add(resignationEmployeeFilter.EmployeeId);
                allResignation.EmployeeId = resignationEmployeeFilter.EmployeeId;
                allResignation.ReporteesList = employeeIdList;
                allResignation.PMORole = _configuration.GetValue<string>("ChecklistRole:PMO");
                allResignation.ITRole = _configuration.GetValue<string>("ChecklistRole:IT");
                allResignation.AdminRole = _configuration.GetValue<string>("ChecklistRole:Admin");
                allResignation.FinanceRole = _configuration.GetValue<string>("ChecklistRole:Finance");
                allResignation.HRRole = _configuration.GetValue<string>("ChecklistRole:HR");
                resignationEmployeeFilter.ResignationInputFilter = allResignation;
                resignationEmployeeFilter.ReporteesList = employeeIdList;

                var employeeResult = await _client.PostAsJsonAsync(resignationEmployeeFilter, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationEmployeeListByFilter"));
                employeeListView = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(employeeResult?.Data));

                //employeeIdList = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(employeeResult?.Data));
                //if (employeeIdList?.Count > 0)
                //{
                //    var result = await _client.PostAsJsonAsync(employeeIdList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeMasterData"));

                //    employeeListView = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Data));
                //}
                employeeResignationDetailsList.EmployeeListView = employeeListView;
                employeeResignationDetailsList.AllReportees = employeeIdList;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeResignationDetailsList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetResignationEmployeeListByFilter");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<ResignationEmployeeMasterView>()
                });
            }
        }
        #endregion
        #region Get Resignation employee filter list 
        [HttpPost]
        [Route("GetResignationEmployeeListByFilterCount")]
        public async Task<IActionResult> GetResignationEmployeeListByFilterCount(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            try
            {
                EmployeeResignationDetailsList employeeResignationDetailsList = new EmployeeResignationDetailsList();
                List<ResignationEmployeeMasterView> employeeListView = new List<ResignationEmployeeMasterView>();
                int count = 0;
                //var result = await _client.PostAsJsonAsync(resignationEmployeeFilter, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeListByFilter"));
                //employeeListView = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Data));
                //OR
                List<int> employeeIdList = new List<int>();
                List<string> employeeRoleList = new List<string>();
                AllResignationInputView allResignation = new AllResignationInputView();

                if (resignationEmployeeFilter.IsManager && (resignationEmployeeFilter.IsAllReportees || resignationEmployeeFilter.IsCheckList))
                {
                    if (resignationEmployeeFilter.IsCheckList)
                    {
                        var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetReporteesCheckListEmployee") + resignationEmployeeFilter.EmployeeId + "&isAll=" + resignationEmployeeFilter.IsAllReportees);
                        ReporteesChecklistEmployeeView employeeDetailListCheckList = JsonConvert.DeserializeObject<ReporteesChecklistEmployeeView>(JsonConvert.SerializeObject(employeeListresult.Data));
                        employeeIdList = employeeDetailListCheckList?.EmployeeDetails?.Select(ea => ea.EmployeeId).ToList();
                        employeeRoleList = employeeDetailListCheckList.Role;
                    }
                    else
                    {
                        var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeListForManagerReport") + resignationEmployeeFilter.EmployeeId + "&isAll=" + resignationEmployeeFilter.IsAllReportees);
                        List<EmployeeViewDetails> employeeDetailList = JsonConvert.DeserializeObject<List<EmployeeViewDetails>>(JsonConvert.SerializeObject(employeeListresult.Data));
                        employeeIdList = employeeDetailList.Select(ea => ea.EmployeeId).ToList();
                    }

                    allResignation.IsAllData = resignationEmployeeFilter.IsAllReportees;
                }
                else
                    employeeRoleList.Add("manager");
                allResignation.EmployeeRole = employeeRoleList;
                if (resignationEmployeeFilter.EmployeeId > 0 && !resignationEmployeeFilter.IsCheckList)
                    employeeIdList.Add(resignationEmployeeFilter.EmployeeId);
                allResignation.EmployeeId = resignationEmployeeFilter.EmployeeId;
                allResignation.ReporteesList = employeeIdList;
                allResignation.PMORole = _configuration.GetValue<string>("ChecklistRole:PMO");
                allResignation.ITRole = _configuration.GetValue<string>("ChecklistRole:IT");
                allResignation.AdminRole = _configuration.GetValue<string>("ChecklistRole:Admin");
                allResignation.FinanceRole = _configuration.GetValue<string>("ChecklistRole:Finance");
                allResignation.HRRole = _configuration.GetValue<string>("ChecklistRole:HR");
                resignationEmployeeFilter.ResignationInputFilter = allResignation;
                resignationEmployeeFilter.ReporteesList = employeeIdList;

                var employeeResult = await _client.PostAsJsonAsync(resignationEmployeeFilter, _exitManagementBaseURL, _configuration.GetValue<string>("ApplicationURL:ExitManagement:GetResignationEmployeeListByFilterCount"));
                count = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(employeeResult?.Data));
                //employeeIdList = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(employeeResult?.Data));
                //if (employeeIdList?.Count > 0)
                //{
                //    var result = await _client.PostAsJsonAsync(employeeIdList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResignationEmployeeMasterData"));

                //    employeeListView = JsonConvert.DeserializeObject<List<ResignationEmployeeMasterView>>(JsonConvert.SerializeObject(result?.Data));
                //}
                //employeeResignationDetailsList.EmployeeListView = employeeListView;
                //employeeResignationDetailsList.AllReportees = employeeIdList;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = count
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetResignationEmployeeListByFilterCount");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new List<ResignationEmployeeMasterView>()

                });
             }
        }
        #endregion
    }
}

