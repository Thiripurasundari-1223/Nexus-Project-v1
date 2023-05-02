using APIGateWay.API.Common;
using APIGateWay.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;
using SharedLibraries.ViewModels.PolicyManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class PolicyMgmtController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly HTTPClient _client;
        private readonly CommonFunction _commonFunction;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _policyMgmtBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";

        #region Constructor
        public PolicyMgmtController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new();
            _commonFunction = new CommonFunction(configuration);
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _policyMgmtBaseURL = _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:BaseURL");
        }
        #endregion

        #region Get PolicyMgmt's Master Data
        [HttpGet]
        [Route("GetPolicyMgmtMasterData")]
        public async Task<IActionResult> GetPolicyMgmtMasterData()
        {
            PolicyMgmtMasterData masterData = new();
            try
            {
                List<Department> departments = new();
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetDepartmentDropDownList"));
                departments = JsonConvert.DeserializeObject<List<Department>>(JsonConvert.SerializeObject(result?.Data));
                masterData.Departments = departments;
                List<EmployeeLocation> employeeLocations = new();
                result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetLocationDropDownList"));
                employeeLocations = JsonConvert.DeserializeObject<List<EmployeeLocation>>(JsonConvert.SerializeObject(result?.Data));
                masterData.Locations = employeeLocations;
                List<Roles> Roles = new();
                result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetRolesList"));
                Roles = JsonConvert.DeserializeObject<List<Roles>>(JsonConvert.SerializeObject(result?.Data));
                masterData.Roles = Roles;
                List<FolderView> folders = new();
                result = await _client.GetAsync(_policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetFoldersForPolicy"));
                folders = JsonConvert.DeserializeObject<List<FolderView>>(JsonConvert.SerializeObject(result?.Data));
                masterData.FolderView = folders;
                if (result != null)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        PolicyMgmtMasterData = masterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetPolicyMgmtMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                PolicyMgmtMasterData = masterData
            });
        }
        #endregion

        #region Save Policy Document
        [HttpPost]
        [Route("SavePolicyDocument")]
        public async Task<IActionResult> SavePolicyDocument(PolicyDocumentView policyDocumentView)
        {
            string statusText = "";
            try
            {
                if (policyDocumentView?.ToShareWithAll == null && (policyDocumentView.EmployeeId == null || policyDocumentView.EmployeeId == 0))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "You have to choose Share with all or Employee Id.",
                        PolicyDocumentId = 0,
                        CreatedByName = "",
                        ModifiedByName = ""
                    });
                }
                string PolicyDocumentsDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                if (!string.IsNullOrEmpty(PolicyDocumentsDirectory))
                {
                    //Create base directory
                    if (!Directory.Exists(PolicyDocumentsDirectory))
                    {
                        Directory.CreateDirectory(PolicyDocumentsDirectory);
                    }
                    //Create Employee, EmployeeId directories
                    if (policyDocumentView.EmployeeId > 0)
                    {
                        if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Employee")))
                        {
                            Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Employee"));
                        }
                        if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocumentView.EmployeeId.ToString())))
                        {
                            Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocumentView.EmployeeId.ToString()));
                        }
                        if (!string.IsNullOrEmpty(policyDocumentView.FolderName))
                        {
                            if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocumentView.EmployeeId.ToString(), policyDocumentView.FolderName)))
                            {
                                Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocumentView.EmployeeId.ToString(), policyDocumentView.FolderName));
                            }
                        }
                    }
                    else
                    {
                        //Create Organization directory
                        if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Organization")))
                        {
                            Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Organization"));
                        }
                    }
                }
                string docPath = ""; bool isAddPolicy = policyDocumentView.PolicyDocumentId <= 0;
                if (policyDocumentView.EmployeeId > 0)
                {
                    if (!string.IsNullOrEmpty(policyDocumentView.FolderName))
                        docPath = Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocumentView.EmployeeId.ToString(), policyDocumentView.FolderName);
                    else
                        docPath = Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocumentView.EmployeeId.ToString());
                }
                else
                {
                    docPath = Path.Combine(PolicyDocumentsDirectory, "Organization");
                }
                if (policyDocumentView.DocumentToUpload != null && policyDocumentView.DocumentToUpload.DocumentName != null && !System.IO.File.Exists(policyDocumentView.DocumentToUpload?.DocumentName))
                {
                    policyDocumentView.FilePath = Path.Combine(docPath, policyDocumentView.FileName);
                    if (policyDocumentView.ExistingFilePath != null && policyDocumentView.PolicyDocumentId > 0 && policyDocumentView.FilePath != policyDocumentView.ExistingFilePath)
                    {
                        if (System.IO.File.Exists(policyDocumentView.ExistingFilePath))
                            System.IO.File.Delete(policyDocumentView.ExistingFilePath);
                    }
                    if (System.IO.File.Exists(policyDocumentView.FilePath) && policyDocumentView.DocumentToUpload?.DocumentSize > 0 && !(policyDocumentView.PolicyDocumentId > 0))
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "File name is already exists.",
                            PolicyDocumentId = 0,
                            CreatedByName = "",
                            ModifiedByName = ""
                        });
                    }
                    if (policyDocumentView.DocumentToUpload.DocumentAsBase64.Contains(','))
                        policyDocumentView.DocumentToUpload.DocumentAsBase64 = policyDocumentView.DocumentToUpload.DocumentAsBase64[(policyDocumentView.DocumentToUpload.DocumentAsBase64.IndexOf(",") + 1)..];
                    policyDocumentView.DocumentToUpload.DocumentAsByteArray = Convert.FromBase64String(policyDocumentView.DocumentToUpload.DocumentAsBase64);
                    using Stream fileStream = new FileStream(policyDocumentView.FilePath, FileMode.Create);
                    fileStream.Write(policyDocumentView.DocumentToUpload.DocumentAsByteArray, 0, policyDocumentView.DocumentToUpload.DocumentAsByteArray.Length);
                }
                SuccessData data = await _client.PostAsJsonAsync(policyDocumentView, _policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:SavePolicyDocument"));
                policyDocumentView.PolicyDocumentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(data?.Data));
                if (data != null && data.StatusCode == "SUCCESS")
                {
                    string EmployeeFullName = "", EmployeeEmailAddess = "";
                    List<PolicyAcknowledgementView> policyAcknowledgementView = new();
                    if (policyDocumentView.ToShareWithAll == true || policyDocumentView.EmployeeId == null || policyDocumentView.EmployeeId == 0)
                    {
                        policyAcknowledgementView = await GetEmployeeList(policyDocumentView.PolicyDocumentId, "policy");
                    }
                    else if (policyDocumentView.EmployeeId > 0)
                    {
                        List<int> userId = new();
                        List<EmployeeName> lstEmployeeName = new();
                        userId.Add((int)policyDocumentView.EmployeeId);
                        lstEmployeeName = await GetEmployeeNamesAsync(userId);
                        if (lstEmployeeName.Count > 0)
                        {
                            if (policyDocumentView.CreatedBy > 0)
                            {
                                EmployeeFullName = lstEmployeeName.Where(x => x.EmployeeId == policyDocumentView.EmployeeId).
                                                    Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                                EmployeeEmailAddess = lstEmployeeName.Where(x => x.EmployeeId == policyDocumentView.EmployeeId).
                                                    Select(y => y.EmployeeEmailId).FirstOrDefault().ToString();
                            }
                        }
                    }
                    if (policyDocumentView.NotifyThrough?.Where(x => x.Equals("notification")).Any() == true && policyDocumentView.CreatedBy > 0)
                    {
                        string sStstus = "added";
                        if (isAddPolicy == false) sStstus = "updated";
                        List<Notifications> notifications = new();
                        Notifications notification = new()
                        {
                            CreatedBy = policyDocumentView.CreatedBy ?? 0,
                            CreatedOn = DateTime.UtcNow,
                            FromId = policyDocumentView.CreatedBy ?? 0,
                            ToId = policyDocumentView.CreatedBy ?? 0,
                            MarkAsRead = false,
                            NotificationSubject = "Policy Management Document (" + policyDocumentView.FileName + ") has been " + sStstus + " successfully.",
                            NotificationBody = "Policy Management Document (" + policyDocumentView.FileName + ") has been " + sStstus + " successfully.",
                            PrimaryKeyId = policyDocumentView.PolicyDocumentId,
                            ButtonName = "View Policy Document",
                            SourceType = "Policy Management"
                        };
                        notifications.Add(notification);
                        if (policyDocumentView.EmployeeId > 0)
                        {
                            notification = new()
                            {
                                CreatedBy = policyDocumentView.CreatedBy ?? 0,
                                CreatedOn = DateTime.UtcNow,
                                FromId = policyDocumentView.CreatedBy ?? 0,
                                ToId = policyDocumentView.EmployeeId ?? 0,
                                MarkAsRead = false,
                                NotificationSubject = "Employee Document (" + policyDocumentView.FileName + ") has been " + sStstus + " successfully.",
                                NotificationBody = "Employee Document (" + policyDocumentView.FileName + ") has been " + sStstus + " successfully.",
                                PrimaryKeyId = policyDocumentView.PolicyDocumentId,
                                ButtonName = "View Employee Document",
                                SourceType = "Policy Management"
                            };
                            notifications.Add(notification);
                        }
                        if (policyAcknowledgementView?.Count > 0)
                        {
                            policyAcknowledgementView = policyAcknowledgementView.OrderBy(x => x.EmployeeId).ToList();
                            //sending a notification to all or specific role(s)/department(s)/location(s).
                            foreach (PolicyAcknowledgementView view in policyAcknowledgementView)
                            {
                                if (policyDocumentView.CreatedBy != view.EmployeeId)
                                {
                                    notification = new()
                                    {
                                        CreatedBy = policyDocumentView.CreatedBy ?? 0,
                                        CreatedOn = DateTime.UtcNow,
                                        FromId = policyDocumentView.CreatedBy ?? 0,
                                        ToId = view.EmployeeId ?? 0,
                                        MarkAsRead = false,
                                        NotificationSubject = "Policy Management Document (" + policyDocumentView.FileName + ") has been " + sStstus + " successfully.",
                                        NotificationBody = "Policy Management Document (" + policyDocumentView.FileName + ") has been " + sStstus + " successfully.",
                                        PrimaryKeyId = policyDocumentView.PolicyDocumentId,
                                        ButtonName = "View Policy Document",
                                        SourceType = "Policy Management"
                                    };
                                    notifications.Add(notification);
                                }
                            }
                        }
                        Uri uri = new(_configuration.GetValue<string>("ApplicationURL:Notifications"));
                        using var notificationClient = new HttpClient
                        {
                            BaseAddress = uri
                        };
                        HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                        SuccessData successData = notificationResponse.Content.ReadAsAsync<SuccessData>().Result;
                    }
                    if (policyDocumentView.NotifyThrough?.Where(x => x.Equals("email")).Any() == true)
                    {
                        //sending an email to all or specific role(s)/department(s)/location(s).
                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                        string MainContent = "A new organization policy has been added and shared with you.";
                        string MailSubject = "New Policy Added";
                        string baseURL = appsetting.GetSection("BaseURL").Value;
                        if (isAddPolicy == false)
                        {
                            MailSubject = "Policy Updated";
                            MainContent = "An organization policy has been updated and shared with you.";
                        }
                        List<SendEmailView> sendEmails = new();
                        if (policyDocumentView.ToShareWithAll == true)
                        {
                            string MailBody = @"<html>
<body>
    <p>Dear {EmployeeName},</p>
    <p>{MainContent} </p>
    <p>File Name: {Filename} </p>
    <p>Please click <a href='{link}/#/pmsnexus/policiesdocuments/policies?isAdmin=false&policyid={PolicyDocumentId}'>here</a> to view.</p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                            MailBody = MailBody.Replace("{MainContent}", MainContent);
                            MailBody = MailBody.Replace("{Filename}", policyDocumentView.FileName ?? "");
                            MailBody = MailBody.Replace("{link}", baseURL);
                            MailBody = MailBody.Replace("{mailLink}", baseURL);
                            MailBody = MailBody.Replace("{PolicyDocumentId}", policyDocumentView.PolicyDocumentId!.ToString());
                            MailBody = MailBody.Replace("{EmployeeName}", "All");
                            SendEmailView sendEmail = new()
                            {
                                FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                ToEmailID = appsetting.GetSection("ToShareWithAll").Value,
                                Subject = MailSubject,
                                MailBody = MailBody,
                                ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                Host = appsetting.GetSection("EmailHost").Value,
                                FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                            };
                            sendEmails.Add(sendEmail);
                        }
                        else if (policyDocumentView.EmployeeId > 0 && policyDocumentView.IsGenerated != true)
                        {
                            string MailBody = @"<html>
<body>
    <p>Dear {EmployeeName},</p>
    <p>An employee document has been added and shared with you.</p>
    <p>File Name: {Filename}</p>
    <p>Please click <a href='{link}/#/pmsnexus/policiesdocuments/my-documents?isAdmin=false&documentid={PolicyDocumentId}'>here</a> to view.</p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                            MailBody = MailBody.Replace("{Filename}", policyDocumentView.FileName ?? "");
                            MailBody = MailBody.Replace("{link}", baseURL);
                            MailBody = MailBody.Replace("{mailLink}", baseURL);
                            MailBody = MailBody.Replace("{PolicyDocumentId}", policyDocumentView.PolicyDocumentId!.ToString());
                            MailBody = MailBody.Replace("{EmployeeName}", EmployeeFullName);
                            MailSubject = "Document shared with you";
                            SendEmailView sendEmail = new()
                            {
                                FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                ToEmailID = EmployeeEmailAddess,
                                Subject = MailSubject,
                                MailBody = MailBody,
                                ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                Host = appsetting.GetSection("EmailHost").Value,
                                FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                            };
                            sendEmails.Add(sendEmail);
                        }
                        else
                        {
                            foreach (PolicyAcknowledgementView view in policyAcknowledgementView)
                            {
                                if (policyDocumentView.CreatedBy != view.EmployeeId)
                                {
                                    string MailBody = @"<html>
<body>
    <p>Dear {EmployeeName},</p>
    <p>{MainContent}</p>
    <p>File Name: {Filename}</p>
    <p>Please click <a href='{link}/#/pmsnexus/policiesdocuments/policies?isAdmin=false&policyid={PolicyDocumentId}'>here</a> to view.</p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                                    MailBody = MailBody.Replace("{MainContent}", MainContent);
                                    MailBody = MailBody.Replace("{Filename}", policyDocumentView.FileName ?? "");
                                    MailBody = MailBody.Replace("{link}", baseURL);
                                    MailBody = MailBody.Replace("{mailLink}", baseURL);
                                    MailBody = MailBody.Replace("{PolicyDocumentId}", policyDocumentView.PolicyDocumentId!.ToString());
                                    MailBody = MailBody.Replace("{EmployeeName}", view.EmployeeName + " - " + view.FormattedEmployeeId);
                                    SendEmailView sendEmail = new()
                                    {
                                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                        ToEmailID = view.EmployeeEmail,
                                        Subject = MailSubject,
                                        MailBody = MailBody,
                                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                        Host = appsetting.GetSection("EmailHost").Value,
                                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                                    };
                                    sendEmails.Add(sendEmail);
                                }
                            }
                        }
                        if (sendEmails.Count > 0)
                        {
                            var mail = _commonFunction.NotificationMailForPolicyMgmt(sendEmails);
                        }
                    }
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    if (policyDocumentView.CreatedBy > 0) userIds.Add((int)policyDocumentView.CreatedBy);
                    if (policyDocumentView.ModifiedBy > 0) userIds.Add((int)policyDocumentView.ModifiedBy);
                    if (policyDocumentView.EmployeeId > 0) userIds.Add((int)policyDocumentView.EmployeeId);
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            if (policyDocumentView.CreatedBy > 0)
                                policyDocumentView.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == policyDocumentView.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            if (policyDocumentView.ModifiedBy > 0)
                                policyDocumentView.ModifiedByName = listEmployeeName.Where(x => x.EmployeeId == policyDocumentView.ModifiedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            if (policyDocumentView.EmployeeId > 0)
                                policyDocumentView.EmployeeName = listEmployeeName.Where(x => x.EmployeeId == policyDocumentView.EmployeeId).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                        }
                    }
                    return Ok(new
                    {
                        data.StatusCode,
                        data.StatusText,
                        policyDocumentView.PolicyDocumentId,
                        policyDocumentView.CreatedByName,
                        policyDocumentView.ModifiedByName
                    });
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                if (policyDocumentView.DocumentToUpload != null)
                {
                    policyDocumentView.DocumentToUpload.DocumentAsBase64 = null;
                    policyDocumentView.DocumentToUpload.DocumentAsByteArray = null;
                }
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/SavePolicyDocument", JsonConvert.SerializeObject(policyDocumentView));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                PolicyDocumentId = 0,
                CreatedByName = "",
                ModifiedByName = ""
            });
        }
        #endregion

        #region Save Folder
        [HttpPost]
        [Route("SaveFolder")]
        public async Task<IActionResult> SaveFolder(FolderView folderView)
        {
            string statusText;
            try
            {
                SuccessData data = await _client.PostAsJsonAsync(folderView, _policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:SaveFolder"));
                folderView.FolderId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(data?.Data));
                if (data != null && data.StatusCode == "SUCCESS")
                {
                    return Ok(new
                    {
                        data.StatusCode,
                        data.StatusText,
                        folderView.FolderId
                    });
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/SaveFolder", JsonConvert.SerializeObject(folderView));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                FolderId = 0
            });
        }
        #endregion

        #region Get Policy Documents By Id
        [HttpGet]
        [Route("GetPolicyDocumentById")]
        public async Task<IActionResult> GetPolicyDocumentById(int PolicyDocumentId, int UserId)
        {
            PolicyDocumentView policyDocument = new();
            try
            {
                var result = await _client.GetAsync(_policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetPolicyDocumentById") +
                                        PolicyDocumentId + "&UserId=" + UserId);
                policyDocument = JsonConvert.DeserializeObject<PolicyDocumentView>(JsonConvert.SerializeObject(result?.Data));
                if (policyDocument != null)
                {
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    if (policyDocument.CreatedBy > 0) userIds.Add((int)policyDocument.CreatedBy);
                    if (policyDocument.ModifiedBy > 0) userIds.Add((int)policyDocument.ModifiedBy);
                    if (policyDocument.EmployeeId > 0) userIds.Add((int)policyDocument.EmployeeId);
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            if (policyDocument.CreatedBy > 0)
                                policyDocument.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == policyDocument.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            if (policyDocument.ModifiedBy > 0)
                                policyDocument.ModifiedByName = listEmployeeName.Where(x => x.EmployeeId == policyDocument.ModifiedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            if (policyDocument.EmployeeId > 0)
                                policyDocument.EmployeeName = listEmployeeName.Where(x => x.EmployeeId == policyDocument.EmployeeId).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    PolicyDocument = policyDocument,
                    DocumentAsBase64 = string.IsNullOrEmpty(policyDocument.FilePath) ? null : System.IO.File.Exists(policyDocument.FilePath) ? Convert.ToBase64String(System.IO.File.ReadAllBytes(policyDocument.FilePath)) : ""
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetPolicyDocumentById", Convert.ToString(PolicyDocumentId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                PolicyDocument = policyDocument
            });
        }
        #endregion

        #region Get Policy Documents By User Id or Get All
        [HttpGet]
        [Route("GetPolicyDocumentByUserId")]
        public async Task<IActionResult> GetPolicyDocumentByUserId(int? UserId = 0, string DocType = "")
        {
            List<PolicyDocumentView> policyDocuments = new();
            string StatusCode = "SUCCESS";
            try
            {
                int? LocationId = 0, RoleId = 0, DepartmentId = 0, CurrentWorkPlaceId = 0, CurrentWorkLocationId = 0;
                UserId ??= 0;
                if (UserId > 0 && DocType.ToLower() == "policy")
                {
                    EmployeeBasicInfoView employee = new();
                    var employeeInfo = await _client.GetAsync(_employeeBaseURL,
                                    _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeBasicInfoByEmployeeID") + UserId);
                    employee = JsonConvert.DeserializeObject<EmployeeBasicInfoView>(JsonConvert.SerializeObject(employeeInfo?.Data));
                    if (employee != null)
                    {
                        LocationId = employee.Employee.LocationId ?? 0;
                        RoleId = employee.Employee.RoleId ?? 0;
                        DepartmentId = employee.Employee.DepartmentId ?? 0;
                        CurrentWorkLocationId = employee.Employee.CurrentWorkLocationId ?? 0;
                        CurrentWorkPlaceId = employee.Employee.CurrentWorkPlaceId ?? 0;
                    }
                }
                var result = await _client.GetAsync(_policyMgmtBaseURL,
                                    _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetPolicyDocumentByUserId") + UserId +
                                    "&DocType=" + DocType + "&LocationId=" + LocationId + "&RoleId=" + RoleId + "&DepartmentId=" + DepartmentId +
                                    "&CurrentWorkLocationId=" + CurrentWorkLocationId + "&CurrentWorkPlaceId=" + CurrentWorkPlaceId);
                policyDocuments = JsonConvert.DeserializeObject<List<PolicyDocumentView>>(JsonConvert.SerializeObject(result?.Data));
                if (UserId > 0 && DocType.ToLower() != "policy")
                {
                    EmployeesViewModel employees = new();
                    result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDetailsByEmployeeId") + UserId);
                    employees = JsonConvert.DeserializeObject<EmployeesViewModel>(JsonConvert.SerializeObject(result?.Data));
                    policyDocuments ??= new();
                    PolicyDocumentView policyDocument;
                    if (employees != null)
                    {
                        if (employees.ProfilePicture != null)
                        {
                            policyDocument = new()
                            {
                                EmployeeId = employees.Employee.EmployeeID,
                                CreatedOn = employees.ProfilePicture.CreatedOn,
                                FileName = employees.ProfilePicture.DocumentName,
                                CreatedBy = employees.ProfilePicture.EmployeeId,
                                DocumentToUpload = employees.ProfilePicture
                            };
                            policyDocuments.Add(policyDocument);
                        }
                        if (employees.AddressProof != null)
                        {
                            foreach (DocumentsToUpload addressProof in employees.AddressProof)
                            {
                                policyDocument = new()
                                {
                                    EmployeeId = employees.Employee.EmployeeID,
                                    CreatedOn = addressProof.CreatedOn,
                                    FileName = addressProof.DocumentName,
                                    CreatedBy = addressProof.EmployeeId,
                                    DocumentToUpload = addressProof
                                };
                                policyDocuments.Add(policyDocument);
                            }
                        }
                        if (employees.EmployeeDependent != null)
                        {
                            foreach (EmployeeDependentView dependentProof in employees.EmployeeDependent)
                            {
                                if (dependentProof.DependentDetailsProof != null)
                                {
                                    policyDocument = new()
                                    {
                                        EmployeeId = employees.Employee.EmployeeID,
                                        CreatedOn = dependentProof.DependentDetailsProof.CreatedOn,
                                        FileName = dependentProof.DependentDetailsProof.DocumentName,
                                        CreatedBy = dependentProof.DependentDetailsProof.EmployeeId,
                                        DocumentToUpload = dependentProof.DependentDetailsProof
                                    };
                                    policyDocuments.Add(policyDocument);
                                }
                            }
                        }
                    }
                    EmployeeWorkAndEducationDetailView workAndEducationDetailView = new();
                    result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeWorkHistory") + UserId);
                    workAndEducationDetailView = JsonConvert.DeserializeObject<EmployeeWorkAndEducationDetailView>(JsonConvert.SerializeObject(result?.Data));
                    if (workAndEducationDetailView != null && workAndEducationDetailView?.WorkHistoriesList?.Count > 0)
                    {
                        foreach (WorkHistoryView workHistory in workAndEducationDetailView.WorkHistoriesList)
                        {
                            if (workHistory.paySlip != null)
                            {
                                policyDocument = new()
                                {
                                    EmployeeId = employees.Employee.EmployeeID,
                                    CreatedOn = workHistory.paySlip.CreatedOn,
                                    FileName = workHistory.paySlip.DocumentName,
                                    CreatedBy = workHistory.paySlip.EmployeeId,
                                    DocumentToUpload = workHistory.paySlip
                                };
                                policyDocuments.Add(policyDocument);
                            }
                            if (workHistory.serviceLetter != null)
                            {
                                policyDocument = new()
                                {
                                    EmployeeId = employees.Employee.EmployeeID,
                                    CreatedOn = workHistory.serviceLetter.CreatedOn,
                                    FileName = workHistory.serviceLetter.DocumentName,
                                    CreatedBy = workHistory.serviceLetter.EmployeeId,
                                    DocumentToUpload = workHistory.paySlip
                                };
                                policyDocuments.Add(policyDocument);
                            }
                            if (workHistory.OfferLetter != null)
                            {
                                policyDocument = new()
                                {
                                    EmployeeId = employees.Employee.EmployeeID,
                                    CreatedOn = workHistory.OfferLetter.CreatedOn,
                                    FileName = workHistory.OfferLetter.DocumentName,
                                    CreatedBy = workHistory.OfferLetter.EmployeeId,
                                    DocumentToUpload = workHistory.paySlip
                                };
                                policyDocuments.Add(policyDocument);
                            }
                        }
                    }
                    workAndEducationDetailView = new();
                    result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeEducationDetail") + UserId);
                    workAndEducationDetailView = JsonConvert.DeserializeObject<EmployeeWorkAndEducationDetailView>(JsonConvert.SerializeObject(result?.Data));
                    if (workAndEducationDetailView != null && workAndEducationDetailView?.EducationDetailsList?.Count > 0)
                    {
                        foreach (EducationDetailView education in workAndEducationDetailView.EducationDetailsList)
                        {
                            if (education.Marksheet != null)
                            {
                                policyDocument = new()
                                {
                                    EmployeeId = employees.Employee.EmployeeID,
                                    CreatedOn = education.Marksheet.CreatedOn,
                                    FileName = education.Marksheet.DocumentName,
                                    CreatedBy = education.Marksheet.EmployeeId,
                                    DocumentToUpload = education.Marksheet
                                };
                                policyDocuments.Add(policyDocument);
                            }
                            if (education.Certificate != null)
                            {
                                policyDocument = new()
                                {
                                    EmployeeId = employees.Employee.EmployeeID,
                                    CreatedOn = education.Certificate.CreatedOn,
                                    FileName = education.Certificate.DocumentName,
                                    CreatedBy = education.Certificate.EmployeeId,
                                    DocumentToUpload = education.Certificate
                                };
                                policyDocuments.Add(policyDocument);
                            }
                        }
                    }
                }
                if (policyDocuments != null && policyDocuments.Count > 0)
                {
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    foreach (PolicyDocumentView policyDocument in policyDocuments)
                    {
                        if (policyDocument.CreatedBy > 0) userIds.Add((int)policyDocument.CreatedBy);
                        if (policyDocument.ModifiedBy > 0) userIds.Add((int)policyDocument.ModifiedBy);
                        if (policyDocument.EmployeeId > 0) userIds.Add((int)policyDocument.EmployeeId);
                    }
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            foreach (PolicyDocumentView policyDocument in policyDocuments)
                            {
                                if (policyDocument.CreatedBy > 0)
                                    policyDocument.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == policyDocument.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                                if (policyDocument.ModifiedBy > 0)
                                    policyDocument.ModifiedByName = listEmployeeName.Where(x => x.EmployeeId == policyDocument.ModifiedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                                if (policyDocument.EmployeeId > 0)
                                    policyDocument.EmployeeName = listEmployeeName.Where(x => x.EmployeeId == policyDocument.EmployeeId).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusCode = "FAILURE";
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetPolicyDocumentByUserId", Convert.ToString(UserId));
            }
            return Ok(new
            {
                StatusCode,
                StatusText = StatusCode == "FAILURE" ? strErrorMsg : "",
                PolicyDocuments = policyDocuments
            });
        }
        #endregion

        #region Get Employee Names List As Async
        private async Task<List<EmployeeName>> GetEmployeeNamesAsync(List<int> userIds)
        {
            var employeeResult = await _client.PostAsJsonAsync(userIds, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeNameById"));
            return JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeResult.Data));
        }
        #endregion

        #region Delete Policy Documents By Id
        [HttpPost]
        [Route("DeletePolicyDocumentById")]
        public async Task<IActionResult> DeletePolicyDocumentById(int PolicyDocumentId, int ModifiedBy)
        {
            bool archiveDocument = false;
            try
            {
                SuccessData result = await _client.GetAsync(_policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetPolicyDocumentById") + PolicyDocumentId);
                PolicyDocumentView policyDocument = JsonConvert.DeserializeObject<PolicyDocumentView>(JsonConvert.SerializeObject(result?.Data));
                if (policyDocument != null)
                {
                    string PolicyDocumentsDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                    if (!string.IsNullOrEmpty(PolicyDocumentsDirectory))
                    {
                        //Create base directory
                        if (!Directory.Exists(PolicyDocumentsDirectory))
                        {
                            Directory.CreateDirectory(PolicyDocumentsDirectory);
                        }
                        //Create Archive directory
                        if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Archive")))
                        {
                            Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Archive"));
                        }
                        PolicyDocumentsDirectory = Path.Combine(PolicyDocumentsDirectory, "Archive");
                        //Create Employee, EmployeeId directories
                        if (policyDocument.EmployeeId > 0)
                        {
                            if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Employee")))
                            {
                                Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Employee"));
                            }
                            if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocument.EmployeeId.ToString())))
                            {
                                Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocument.EmployeeId.ToString()));
                            }
                            if (!string.IsNullOrEmpty(policyDocument.FolderName))
                            {
                                if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocument.EmployeeId.ToString(), policyDocument.FolderName)))
                                {
                                    Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocument.EmployeeId.ToString(), policyDocument.FolderName));
                                }
                            }
                        }
                        else
                        {
                            if (!Directory.Exists(Path.Combine(PolicyDocumentsDirectory, "Organization")))
                            {
                                Directory.CreateDirectory(Path.Combine(PolicyDocumentsDirectory, "Organization"));
                            }
                        }
                    }
                    string ArchivePath = "";
                    if (policyDocument.EmployeeId > 0)
                    {
                        if (!string.IsNullOrEmpty(policyDocument.FolderName))
                            ArchivePath = Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocument.EmployeeId.ToString(), policyDocument.FolderName);
                        else
                            ArchivePath = Path.Combine(PolicyDocumentsDirectory, "Employee", policyDocument.EmployeeId.ToString());
                    }
                    else
                    {
                        ArchivePath = Path.Combine(PolicyDocumentsDirectory, "Organization");
                    }
                    if (!string.IsNullOrEmpty(policyDocument.FilePath))
                    {
                        string fileName = policyDocument.FilePath.Split("\\")?[^1];
                        ArchivePath = Path.Combine(ArchivePath, fileName);
                        if (System.IO.File.Exists(policyDocument.FilePath))
                        {
                            byte[] DocumentAsByteArray = System.IO.File.ReadAllBytes(policyDocument.FilePath);
                            using Stream fileStream = new FileStream(ArchivePath, FileMode.Create);
                            fileStream.Write(DocumentAsByteArray, 0, DocumentAsByteArray.Length);
                            System.IO.File.Delete(policyDocument.FilePath);
                        }
                    }
                    result = await _client.PostAsJsonAsync("", _policyMgmtBaseURL,
                        _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:DeletePolicyDocumentById") +
                        "?PolicyDocumentId=" + PolicyDocumentId.ToString() + "&ModifiedBy=" + ModifiedBy.ToString() +
                        "&ArchivePath=" + ArchivePath);
                    archiveDocument = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    ArchiveDocument = archiveDocument
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/DeletePolicyDocumentById", Convert.ToString(PolicyDocumentId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ArchiveDocument = archiveDocument
            });
        }
        #endregion

        #region Get Document Types
        [HttpGet]
        [Route("GetDocumentTypes")]
        public async Task<IActionResult> GetDocumentTypes(int documentTypeId = 0)
        {
            List<DocumentTypeView> documentTypeViews = new();
            List<DocumentTagView> documentTagViews = new();
            try
            {
                var result = await _client.GetAsync(_policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetDocumentTypes")
                    + "?documentTypeId=" + documentTypeId);
                documentTypeViews = JsonConvert.DeserializeObject<List<DocumentTypeView>>(JsonConvert.SerializeObject(result?.Data));
                if (documentTypeViews != null)
                {
                    if (documentTypeId == 0)
                    {
                        documentTagViews = documentTypeViews[0].DocumentTag;
                        documentTypeViews[0].DocumentTag = null;
                        if (documentTagViews.Count == 1 && documentTypeViews[0].DocumentTypeId == 0)
                            documentTypeViews = new();
                    }
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    foreach (DocumentTypeView documentType in documentTypeViews)
                    {
                        if (documentType.CreatedBy != null) userIds.Add((int)documentType.CreatedBy);
                        if (documentType.ModifiedBy != null) userIds.Add((int)documentType.ModifiedBy);
                    }
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            foreach (DocumentTypeView documentType in documentTypeViews)
                            {
                                if (documentType.CreatedBy != null)
                                    documentType.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == documentType.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                                if (documentType.ModifiedBy != null)
                                    documentType.ModifiedByName = listEmployeeName.Where(x => x.EmployeeId == documentType.ModifiedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            }
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    DocumentTypes = documentTypeViews,
                    DocumentAsBase64 = documentTypeId > 0 ? string.IsNullOrEmpty(documentTypeViews[0].TemplateFile) ? null :
                                                            System.IO.File.Exists(documentTypeViews[0].TemplateFile) ?
                                                            Convert.ToBase64String(System.IO.File.ReadAllBytes(documentTypeViews[0].TemplateFile))
                                                            : "" : "",
                    DocumentAsBase64ForSignature = documentTypeId > 0 ? string.IsNullOrEmpty(documentTypeViews[0].SignatureFile) ? "" :
                                                            documentTypeViews[0].SignatureFile : "",
                    DocumentTags = documentTagViews
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetDocumentTypes");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                DocumentTypes = documentTypeViews,
                DocumentAsBase64 = "",
                DocumentAsBase64ForSignature = "",
                DocumentTags = documentTagViews
            });
        }
        #endregion

        #region Get Employee List For Requested Documents
        [HttpGet]
        [Route("GetEmployeeListForRequestedDocuments")]
        public async Task<IActionResult> GetEmployeeListForRequestedDocuments(string status)
        {
            List<EmployeeListForRequestedDocuments> employees = new();
            try
            {
                var result = await _client.GetAsync(_policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetEmployeeListForRequestedDocuments") +
                                                            "?status=" + status);
                employees = JsonConvert.DeserializeObject<List<EmployeeListForRequestedDocuments>>(JsonConvert.SerializeObject(result?.Data));
                if (employees != null)
                {
                    List<int> userIds = new();
                    List<EmployeeDetailListView> listEmployeeName = new();
                    foreach (EmployeeListForRequestedDocuments employee in employees)
                    {
                        userIds.Add((int)employee.EmployeeId);
                    }
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        var employeeResult = await _client.PostAsJsonAsync(userIds, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForRequestedDocumentGrid"));
                        listEmployeeName = JsonConvert.DeserializeObject<List<EmployeeDetailListView>>(JsonConvert.SerializeObject(employeeResult.Data));
                        if (listEmployeeName.Count > 0)
                        {
                            foreach (EmployeeListForRequestedDocuments employee in employees)
                            {
                                employee.EmployeeName = listEmployeeName.Where(x => x.EmployeeId == employee.EmployeeId).
                                                                    Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).
                                                                    FirstOrDefault()?.ToString();
                                employee.EmployeePhoto = listEmployeeName.Where(x => x.EmployeeId == employee.EmployeeId).
                                                                    Select(y => y.ProfilePic).
                                                                    FirstOrDefault()?.ToString();
                                employee.EmployeeStatus = listEmployeeName.Where(x => x.EmployeeId == employee.EmployeeId).
                                                                    Select(y => y.EmployeementType).
                                                                    FirstOrDefault()?.ToString();
                            }
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    employees
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetEmployeeListForRequestedDocuments");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                employees
            });
        }
        #endregion

        #region Get Requested Documents
        [HttpGet]
        [Route("GetRequestedDocuments")]
        public async Task<IActionResult> GetRequestedDocuments(int UserId = 0)
        {
            List<RequestedDocumentViewGroupByDocumentType> documentTypes = new();
            try
            {
                List<RequestedDocumentView> requestedDocuments = new();
                var result = await _client.GetAsync(_policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetRequestedDocuments") + UserId);
                requestedDocuments = JsonConvert.DeserializeObject<List<RequestedDocumentView>>(JsonConvert.SerializeObject(result?.Data));
                if (requestedDocuments != null && requestedDocuments.Count > 0)
                {
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    foreach (RequestedDocumentView documentView in requestedDocuments)
                    {
                        if (documentView.CreatedBy != null) userIds.Add((int)documentView.CreatedBy);
                        if (documentView.ModifiedBy != null) userIds.Add((int)documentView.ModifiedBy);
                        if (documentView.ApprovedOrRejectedBy != null) userIds.Add((int)documentView.ApprovedOrRejectedBy);
                    }
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            foreach (RequestedDocumentView documentView in requestedDocuments)
                            {
                                if (documentView.CreatedBy != null)
                                    documentView.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == documentView.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                                if (documentView.ModifiedBy != null)
                                    documentView.ModifiedByName = listEmployeeName.Where(x => x.EmployeeId == documentView.ModifiedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                                if (documentView.ApprovedOrRejectedBy != null)
                                    documentView.ApprovedOrRejectedByName = listEmployeeName.Where(x => x.EmployeeId == documentView.ApprovedOrRejectedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            }
                        }
                    }
                    List<int?> DocumentTypeIds = requestedDocuments.DistinctBy(x => x.DocumentTypeId).Select(x => x.DocumentTypeId).ToList();
                    foreach (int? documentTypeId in DocumentTypeIds)
                    {
                        if (documentTypeId > 0)
                        {
                            RequestedDocumentViewGroupByDocumentType groupByDocumentType = new()
                            {
                                DocumentTypeId = documentTypeId,
                                DocumentType = requestedDocuments.Where(x => x.DocumentTypeId == documentTypeId).Select(x => x.DocumentType).FirstOrDefault()?.ToString(),
                                RequestedDocumentView = requestedDocuments.Where(x => x.DocumentTypeId == documentTypeId).ToList()
                            };
                            documentTypes.Add(groupByDocumentType);
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    RequestedDocuments = documentTypes
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetRequestedDocuments", Convert.ToString(UserId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                RequestedDocuments = documentTypes
            });
        }
        #endregion

        #region Save Requested Document
        [HttpPost]
        [Route("SaveRequestedDocument")]
        public async Task<IActionResult> SaveRequestedDocument(RequestedDocumentView requestedDocument)
        {
            string statusText;
            try
            {
                SuccessData data = await _client.PostAsJsonAsync(requestedDocument, _policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:SaveRequestedDocument"));
                requestedDocument.RequestedDocumentId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(data?.Data));
                if (data != null && data.StatusCode == "SUCCESS")
                {
                    if (requestedDocument.CreatedBy > 0)
                    {
                        List<Notifications> notifications = new();
                        Notifications notification = new()
                        {
                            CreatedBy = requestedDocument.CreatedBy ?? 0,
                            CreatedOn = DateTime.UtcNow,
                            FromId = requestedDocument.CreatedBy ?? 0,
                            ToId = 1, //srvnexusadmin@tvsnext.io
                            MarkAsRead = false,
                            NotificationSubject = "Document request has been " + requestedDocument.Status + " successfully.",
                            NotificationBody = "Requested Document has been " + requestedDocument.Status + " successfully.",
                            PrimaryKeyId = (int)requestedDocument.RequestedDocumentId,
                            ButtonName = "View Requested Document",
                            SourceType = "Requested Document"
                        };
                        notifications.Add(notification);
                        Uri uri = new(_configuration.GetValue<string>("ApplicationURL:Notifications"));
                        using var notificationClient = new HttpClient
                        {
                            BaseAddress = uri
                        };
                        HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                        SuccessData successData = notificationResponse.Content.ReadAsAsync<SuccessData>().Result;
                    }
                    //send an email to HR Adminq
                    string EmployeeFullName = "", EmployeeEmailAddess = "";
                    if (requestedDocument.CreatedBy > 0)
                    {
                        List<int> userId = new();
                        List<EmployeeName> lstEmployeeName = new();
                        userId.Add((int)requestedDocument.CreatedBy);
                        lstEmployeeName = await GetEmployeeNamesAsync(userId);
                        if (lstEmployeeName.Count > 0)
                        {
                            EmployeeFullName = lstEmployeeName.Where(x => x.EmployeeId == requestedDocument.CreatedBy).
                                                Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            EmployeeEmailAddess = lstEmployeeName.Where(x => x.EmployeeId == requestedDocument.CreatedBy).
                                                Select(y => y.EmployeeEmailId).FirstOrDefault().ToString();
                        }
                    }
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    string MainContent = "The below document has been requested by " + EmployeeFullName;
                    string MailSubject = "Document Request from " + EmployeeFullName;
                    string baseURL = appsetting.GetSection("BaseURL").Value;
                    List<SendEmailView> sendEmails = new();
                    string MailBody = @"<html>
<body>
    <p>Dear HR Admin,</p>
    <p>{MainContent} </p>
    <p>Document Name - {DocumentName}</p>
    <p>Purpose of the document - {PurposeDocument}</p>
    <p>Please click <a href='{link}/#/pmsnexus/policiesdocuments/document-requests?isAdmin=true&userid={userid}'>here</a> to view.</p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                    MailBody = MailBody.Replace("{MainContent}", MainContent);
                    MailBody = MailBody.Replace("{DocumentName}", requestedDocument.DocumentType ?? "");
                    MailBody = MailBody.Replace("{PurposeDocument}", requestedDocument.Reason ?? "");
                    MailBody = MailBody.Replace("{link}", baseURL);
                    MailBody = MailBody.Replace("{mailLink}", baseURL);
                    MailBody = MailBody.Replace("{userid}", requestedDocument.CreatedBy.ToString());
                    SendEmailView sendEmail = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = appsetting.GetSection("TVSN_HR").Value,
                        Subject = MailSubject,
                        MailBody = MailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                    };
                    sendEmails.Add(sendEmail);
                    if (sendEmails.Count > 0)
                    {
                        var mail = _commonFunction.NotificationMailForPolicyMgmt(sendEmails);
                    }
                    return Ok(new
                    {
                        data.StatusCode,
                        data.StatusText,
                        requestedDocument.RequestedDocumentId
                    });
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/SaveRequestedDocument", JsonConvert.SerializeObject(requestedDocument));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                RequestedDocumentId = 0
            });
        }
        #endregion

        #region Approve Or Reject Requested Document
        [HttpPost]
        [Route("ApproveOrRejectRequestedDocument")]
        public async Task<IActionResult> ApproveOrRejectRequestedDocument(RequestedDocumentView requestedDocument)
        {
            string statusText;
            bool ApproveOrReject = false;
            try
            {
                SuccessData data = await _client.PostAsJsonAsync(requestedDocument, _policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:ApproveOrRejectRequestedDocument"));
                ApproveOrReject = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(data?.Data));
                if (data != null && data.StatusCode == "SUCCESS")
                {
                    if (requestedDocument.ApprovedOrRejectedBy > 0 && requestedDocument.CreatedBy > 0)
                    {
                        List<Notifications> notifications = new();
                        Notifications notification = new()
                        {
                            CreatedBy = requestedDocument.ApprovedOrRejectedBy ?? 0,
                            CreatedOn = DateTime.UtcNow,
                            FromId = requestedDocument.ApprovedOrRejectedBy ?? 0,
                            ToId = requestedDocument.CreatedBy ?? 0,
                            MarkAsRead = false,
                            NotificationSubject = "Requested Document has been " + requestedDocument.Status + " successfully.",
                            NotificationBody = "Requested Document has been " + requestedDocument.Status + " successfully.",
                            PrimaryKeyId = (int)requestedDocument.RequestedDocumentId,
                            ButtonName = "View Requested Document",
                            SourceType = "Requested Document"
                        };
                        notifications.Add(notification);
                        Uri uri = new(_configuration.GetValue<string>("ApplicationURL:Notifications"));
                        using var notificationClient = new HttpClient
                        {
                            BaseAddress = uri
                        };
                        HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                        SuccessData successData = notificationResponse.Content.ReadAsAsync<SuccessData>().Result;
                    }
                    if (ApproveOrReject)
                    {
                        List<int> userIds = new();
                        List<EmployeeName> listEmployeeName = new();
                        string ApprovedOrRejectedByName = "", ToEmailAddress = "", CreatedByName = "";
                        if (requestedDocument.CreatedBy > 0) userIds.Add((int)requestedDocument.CreatedBy);
                        if (requestedDocument.ApprovedOrRejectedBy > 0) userIds.Add((int)requestedDocument.ApprovedOrRejectedBy);
                        if (userIds.Count > 0)
                        {
                            userIds = userIds.Distinct().ToList();
                            listEmployeeName = await GetEmployeeNamesAsync(userIds);
                            if (listEmployeeName.Count > 0)
                            {
                                if (requestedDocument.ApprovedOrRejectedBy > 0)
                                    ApprovedOrRejectedByName = listEmployeeName.Where(x => x.EmployeeId == requestedDocument.ApprovedOrRejectedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault();
                                if (requestedDocument.CreatedBy > 0)
                                {
                                    CreatedByName = listEmployeeName.Where(x => x.EmployeeId == requestedDocument.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault();
                                    ToEmailAddress = listEmployeeName.Where(x => x.EmployeeId == requestedDocument.CreatedBy).Select(y => y.EmployeeEmailId).FirstOrDefault();
                                }
                            }
                        }
                        //send an email to respective employee
                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                        string MainContent = "Your document request has been " + requestedDocument.Status;
                        string MailSubject = "Document Request Status Change";
                        if (requestedDocument.Status.ToLower() == "cancelled")
                        {
                            MailSubject = "Document Request Cancelled by " + CreatedByName;
                            MainContent = " The below document request raised by " + CreatedByName + " has been cancelled.";
                        }
                        string baseURL = appsetting.GetSection("BaseURL").Value;
                        List<SendEmailView> sendEmails = new();
                        string MailBody = @"<html>
<body>
    <p>Dear {EmployeeName},</p>
    <p>{MainContent}</p>
    <p>Document Name - {DocumentType} </p>
    <p>Please click <a href='{link}/#/pmsnexus/policiesdocuments/my-documents?isAdmin=false&documentid={PolicyDocumentId}'>here</a> to view.</p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                        if (requestedDocument.Status.ToLower() == "rejected")
                        {
                            MailBody = @"<html>
<body>
    <p>Dear {EmployeeName},</p>
    <p>{MainContent}</p>
    <p>Remarks: {RejectedReason}</p>
    <p>Document Name - {DocumentType} </p>
    <p>Purpose of the document - {DocumentPurpose} </p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                        }
                        if (requestedDocument.Status.ToLower() == "cancelled")
                        {
                            MailBody = @"<html>
<body>
    <p>Dear {EmployeeName},</p>
    <p>{MainContent}</p>
    <p>Document Name - {DocumentType} </p>
    <p>Purpose of the document - {DocumentPurpose} </p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                        }
                        MailBody = MailBody.Replace("{MainContent}", MainContent);
                        MailBody = MailBody.Replace("{link}", baseURL);
                        MailBody = MailBody.Replace("{mailLink}", baseURL);
                        if (requestedDocument.Status.ToLower() == "approved")
                        {
                            MailBody = MailBody.Replace("{PolicyDocumentId}", requestedDocument.PolicyDocumentId?.ToString());
                            MailBody = MailBody.Replace("{DocumentType}", requestedDocument.DocumentType + requestedDocument.RequestedDocumentId.ToString() + ".pdf");
                        }
                        else
                            MailBody = MailBody.Replace("{DocumentType}", requestedDocument.DocumentType ?? "");
                        MailBody = MailBody.Replace("{RejectedReason}", requestedDocument.RejectedReason ?? "");
                        MailBody = MailBody.Replace("{DocumentPurpose}", requestedDocument.DocumentPurpose ?? "");
                        if (requestedDocument.Status.ToLower() == "cancelled")
                        {
                            MailBody = MailBody.Replace("{EmployeeName}", "HR Admin");
                            ToEmailAddress = appsetting.GetSection("TVSN_HR").Value;
                        }
                        else
                            MailBody = MailBody.Replace("{EmployeeName}", CreatedByName);
                        SendEmailView sendEmail = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = ToEmailAddress,
                            Subject = MailSubject,
                            MailBody = MailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                        };
                        sendEmails.Add(sendEmail);
                        if (sendEmails.Count > 0)
                        {
                            var mail = _commonFunction.NotificationMailForPolicyMgmt(sendEmails);
                        }
                    }
                    return Ok(new
                    {
                        data.StatusCode,
                        data.StatusText,
                        ApproveOrReject
                    });
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/ApproveOrRejectRequestedDocument", JsonConvert.SerializeObject(requestedDocument));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                ApproveOrReject
            });
        }
        #endregion

        #region Generate Requested Document
        [HttpGet]
        [Route("GenerateRequestedDocument")]
        public async Task<IActionResult> GenerateRequestedDocument(int requestedDocumentId)
        {
            string statusText;
            RequestedDocumentView documentView = new();
            try
            {
                SuccessData data = await _client.GetAsync(_policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GenerateRequestedDocument") +
                                                            "?requestedDocumentId=" + requestedDocumentId);
                documentView = JsonConvert.DeserializeObject<RequestedDocumentView>(JsonConvert.SerializeObject(data?.Data));
                if (data != null && data.StatusCode == "SUCCESS")
                {
                    List<PlaceHolderValue> placeHolderValues = documentView.PlaceHolderValues;
                    if (placeHolderValues.Count > 0)
                    {
                        EmployeePlaceHolderValueView employeePlaceHolderValue = new();
                        data = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeePersonalInfo") +
                                                                        "?employeeID=" + documentView.CreatedBy);
                        employeePlaceHolderValue = JsonConvert.DeserializeObject<EmployeePlaceHolderValueView>(JsonConvert.SerializeObject(data?.Data));
                        if (employeePlaceHolderValue != null)
                        {
                            foreach (PlaceHolderValue placeHolderValue in placeHolderValues)
                            {
                                if (placeHolderValue.TagName.ToLower() == "currentdate")
                                    placeHolderValue.Value = DateTime.UtcNow.ToString("dd-MM-yyyy");
                                if (placeHolderValue.TagName.ToLower() == "entity")
                                    placeHolderValue.Value = employeePlaceHolderValue.Entity;
                                if (placeHolderValue.TagName.ToLower() == "designation")
                                    placeHolderValue.Value = employeePlaceHolderValue.Designation;
                                if (placeHolderValue.TagName.ToLower() == "fullname")
                                    placeHolderValue.Value = employeePlaceHolderValue.FullName;
                                if (placeHolderValue.TagName.ToLower() == "employeepermanentaddress")
                                    placeHolderValue.Value = employeePlaceHolderValue.PermanentAddress;
                                if (placeHolderValue.TagName.ToLower() == "employeecommunicationaddress")
                                    placeHolderValue.Value = employeePlaceHolderValue.CommunicationAddress;
                                if (placeHolderValue.TagName.ToLower() == "requestedreason")
                                    placeHolderValue.Value = documentView.Reason;
                                if (placeHolderValue.TagName.ToLower() == "department")
                                    placeHolderValue.Value = employeePlaceHolderValue.Department;
                                if (placeHolderValue.TagName.ToLower() == "baseworklocation")
                                    placeHolderValue.Value = employeePlaceHolderValue.BaseWorkLocation;
                                if (placeHolderValue.TagName.ToLower() == "currentworklocation")
                                    placeHolderValue.Value = employeePlaceHolderValue.CurrentWorkLocation;
                                if (placeHolderValue.TagName.ToLower() == "dateofjoining")
                                    placeHolderValue.Value = employeePlaceHolderValue.DateOfJoining?.ToString("dd-MM-yyyy");
                                if (placeHolderValue.TagName.ToLower() == "signature")
                                    placeHolderValue.Value = !string.IsNullOrEmpty(documentView.SignatureFilePath) ? documentView.SignatureFilePath : "";
                            }
                        }
                    }
                    return Ok(new
                    {
                        data.StatusCode,
                        documentView,
                        DocumentAsBase64 = !string.IsNullOrEmpty(documentView.DocumentFilePath) ? System.IO.File.Exists(documentView.DocumentFilePath) ? Convert.ToBase64String(System.IO.File.ReadAllBytes(documentView.DocumentFilePath)) : "" : ""
                    });
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GenerateRequestedDocument", requestedDocumentId.ToString());
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                documentView,
                DocumentAsBase64 = ""
            });
        }
        #endregion

        #region Save Document Type
        [HttpPost]
        [Route("SaveDocumentType")]
        public async Task<IActionResult> SaveDocumentType(DocumentTypeView document)
        {
            string statusText = "";
            try
            {
                string documentDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                string docPath = "";
                if (!string.IsNullOrEmpty(documentDirectory))
                {
                    //Create base directory
                    if (!Directory.Exists(documentDirectory))
                        Directory.CreateDirectory(documentDirectory);
                    //Create Organization directory
                    if (!Directory.Exists(Path.Combine(documentDirectory, "Organization")))
                        Directory.CreateDirectory(Path.Combine(documentDirectory, "Organization"));
                    //Create DocumentType directory
                    if (!Directory.Exists(Path.Combine(documentDirectory, "Organization", "DocumentType")))
                        Directory.CreateDirectory(Path.Combine(documentDirectory, "Organization", "DocumentType"));
                    docPath = Path.Combine(documentDirectory, "Organization", "DocumentType");
                }
                if (document.DocumentToUpload != null && document.DocumentToUpload.DocumentName != null && !System.IO.File.Exists(document.DocumentToUpload?.DocumentName))
                {
                    document.TemplateFile = Path.Combine(docPath, document.DocumentToUpload?.DocumentName);
                    if (document.ExistingFilePath != null && document.DocumentTypeId > 0 && document.TemplateFile != document.ExistingFilePath)
                    {
                        if (System.IO.File.Exists(document.ExistingFilePath))
                            System.IO.File.Delete(document.ExistingFilePath);
                    }
                    if (System.IO.File.Exists(document.TemplateFile) && document.DocumentToUpload?.DocumentSize > 0 && !(document.DocumentTypeId > 0))
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "File name is already exists.",
                            DocumentTypeId = 0,
                            CreatedByName = "",
                            ModifiedByName = ""
                        });
                    }
                    if (document.DocumentToUpload.DocumentAsBase64.Contains(','))
                        document.DocumentToUpload.DocumentAsBase64 = document.DocumentToUpload.DocumentAsBase64[(document.DocumentToUpload.DocumentAsBase64.IndexOf(",") + 1)..];
                    document.DocumentToUpload.DocumentAsByteArray = Convert.FromBase64String(document.DocumentToUpload.DocumentAsBase64);
                    using Stream fileStream = new FileStream(document.TemplateFile, FileMode.Create);
                    fileStream.Write(document.DocumentToUpload.DocumentAsByteArray, 0, document.DocumentToUpload.DocumentAsByteArray.Length);
                }
                if (document.Signature != null && document.Signature.DocumentAsBase64 != null)
                    document.SignatureFile = document.Signature.DocumentAsBase64;
                SuccessData data = await _client.PostAsJsonAsync(document, _policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:SaveDocumentType"));
                document.DocumentTypeId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(data?.Data));
                if (data != null && data.StatusCode == "SUCCESS")
                {
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    if (document.CreatedBy > 0) userIds.Add((int)document.CreatedBy);
                    if (document.ModifiedBy > 0) userIds.Add((int)document.ModifiedBy);
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            if (document.CreatedBy > 0)
                                document.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == document.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            if (document.ModifiedBy > 0)
                                document.ModifiedByName = listEmployeeName.Where(x => x.EmployeeId == document.ModifiedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                        }
                    }
                    return Ok(new
                    {
                        data.StatusCode,
                        data.StatusText,
                        document.DocumentTypeId,
                        document.CreatedByName,
                        document.ModifiedByName
                    });
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                if (document.DocumentToUpload != null)
                {
                    document.DocumentToUpload.DocumentAsBase64 = null;
                    document.DocumentToUpload.DocumentAsByteArray = null;
                }
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/SavePolicyDocument", JsonConvert.SerializeObject(document));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                DocumentTypeId = 0,
                CreatedByName = "",
                ModifiedByName = ""
            });
        }
        #endregion

        #region Update Policy Acknowledgement
        [HttpPost]
        [Route("UpdatePolicyAcknowledgement")]
        public async Task<IActionResult> UpdatePolicyAcknowledgement(PolicyAcknowledgementView document)
        {
            string statusText = "";
            try
            {
                int? DocCreatedBy = 0;
                SuccessData data = await _client.PostAsJsonAsync(document, _policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:UpdatePolicyAcknowledgement"));
                if (data != null && data.Data != null)
                    DocCreatedBy = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(data.Data));
                if (data != null && data.StatusCode == "SUCCESS" && DocCreatedBy > 0)
                {
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    userIds.Add((int)DocCreatedBy);
                    if (document.AcknowledgedBy > 0) userIds.Add((int)document.AcknowledgedBy);
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (DocCreatedBy > 0 && document.AcknowledgedBy > 0)
                        {
                            string AcknowledgementByName = listEmployeeName.Where(x => x.EmployeeId == document.AcknowledgedBy).
                                                            Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault();
                            string DocCreatedByName = listEmployeeName.Where(x => x.EmployeeId == DocCreatedBy).
                                                            Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault();
                            List<Notifications> notifications = new();
                            Notifications notification;
                            notification = new()
                            {
                                CreatedBy = document.AcknowledgedBy ?? 0,
                                CreatedOn = DateTime.UtcNow,
                                FromId = document.AcknowledgedBy ?? 0,
                                ToId = DocCreatedBy ?? 0,
                                MarkAsRead = false,
                                NotificationSubject = "Policy Document (" + document.FileName + ") has been successfully "
                                                        + document.AcknowledgedStatus + " by " + AcknowledgementByName + ".",
                                NotificationBody = "Policy Document (" + document.FileName + ") has been successfully "
                                                        + document.AcknowledgedStatus + " by " + AcknowledgementByName + ".",
                                PrimaryKeyId = document.PolicyDocumentId,
                                ButtonName = "View Policy Document",
                                SourceType = "Policy Management"
                            };
                            notification = new()
                            {
                                CreatedBy = document.AcknowledgedBy ?? 0,
                                CreatedOn = DateTime.UtcNow,
                                FromId = document.AcknowledgedBy ?? 0,
                                ToId = document.AcknowledgedBy ?? 0,
                                MarkAsRead = false,
                                NotificationSubject = "You have been " + document.AcknowledgedStatus +
                                                            " successfully for Policy Document (" + document.FileName + ")",
                                NotificationBody = "You have been " + document.AcknowledgedStatus +
                                                            " successfully for Policy Document (" + document.FileName + ")",
                                PrimaryKeyId = document.PolicyDocumentId,
                                ButtonName = "View Employee Document",
                                SourceType = "Policy Management"
                            };
                            notifications.Add(notification);
                            Uri uri = new(_configuration.GetValue<string>("ApplicationURL:Notifications"));
                            using var notificationClient = new HttpClient
                            {
                                BaseAddress = uri
                            };
                            HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                            SuccessData successData = notificationResponse.Content.ReadAsAsync<SuccessData>().Result;
                            //sending an email to specific user
                            var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                            string MainContent = "An organization policy has been agreed by you.";
                            string MailSubject = "Policy Acknowledged by " + AcknowledgementByName;
                            string baseURL = appsetting.GetSection("BaseURL").Value;
                            List<SendEmailView> sendEmails = new();
                            string MailBody = @"<html>
<body>
    <p>Dear {EmployeeName},</p>
    <p>{MainContent}</p>
    <p>File Name: {Filename}</p>
    <p>Please click <a href='{link}/#/pmsnexus/policiesdocuments/policies?isAdmin=false&policyid={PolicyDocumentId}'>here</a> to view.</p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                            MailBody = MailBody.Replace("{MainContent}", MainContent);
                            MailBody = MailBody.Replace("{Filename}", document.FileName);
                            MailBody = MailBody.Replace("{link}", baseURL);
                            MailBody = MailBody.Replace("{mailLink}", baseURL);
                            MailBody = MailBody.Replace("{PolicyDocumentId}", document.PolicyDocumentId!.ToString());
                            MailBody = MailBody.Replace("{EmployeeName}", AcknowledgementByName);
                            SendEmailView sendEmail = new()
                            {
                                FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                ToEmailID = listEmployeeName.Where(x => x.EmployeeId == document.AcknowledgedBy).Select(y => y.EmployeeEmailId).FirstOrDefault(),
                                Subject = MailSubject,
                                MailBody = MailBody,
                                ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                Host = appsetting.GetSection("EmailHost").Value,
                                FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                            };
                            sendEmails.Add(sendEmail);
                            MailBody = @"<html>
<body>
    <p>Dear {EmployeeName},</p> 
    <p>{MainContent}</p> 
    <p>File Name: {Filename}</p> 
    <p>Please click <a href='{link}/#/pmsnexus/policiesdocuments/policies?isAdmin=false&policyid={PolicyDocumentId}'>here</a> to view.</p>
    <p>Automated mail from <a href='{mailLink}' />TVS Next</p>
</body>
</html>";
                            MainContent = "You have been " + document.AcknowledgedStatus + " for the docuemnt " + document.FileName;
                            MailBody = MailBody.Replace("{MainContent}", MainContent);
                            MailBody = MailBody.Replace("{Filename}", document.FileName);
                            MailBody = MailBody.Replace("{link}", baseURL);
                            MailBody = MailBody.Replace("{mailLink}", baseURL);
                            MailBody = MailBody.Replace("{PolicyDocumentId}", document.PolicyDocumentId!.ToString());
                            MailBody = MailBody.Replace("{EmployeeName}", AcknowledgementByName);
                            sendEmail = new()
                            {
                                FromEmailID = appsetting.GetSection("FromEmailId").Value,
                                ToEmailID = listEmployeeName.Where(x => x.EmployeeId == DocCreatedBy).Select(y => y.EmployeeEmailId).FirstOrDefault(),
                                Subject = MailSubject,
                                MailBody = MailBody,
                                ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                                Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                                Host = appsetting.GetSection("EmailHost").Value,
                                FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                            };
                            sendEmails.Add(sendEmail);
                            if (sendEmails.Count > 0)
                            {
                                var mail = _commonFunction.NotificationMailForPolicyMgmt(sendEmails);
                            }
                        }
                        return Ok(new
                        {
                            data.StatusCode,
                            data.StatusText
                        });
                    }
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/UpdatePolicyAcknowledgement", JsonConvert.SerializeObject(document));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText
            });
        }
        #endregion

        #region Get Employee List For Acknowledgement
        [HttpGet]
        [Route("GetEmployeeListForAcknowledgement")]
        public async Task<IActionResult> GetEmployeeListForAcknowledgement(int policyDocumentId, string DocType)
        {
            List<PolicyAcknowledgementView> policyAcknowledgementView = new();
            try
            {
                policyAcknowledgementView = await GetEmployeeList(policyDocumentId, DocType);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    policyAcknowledgementView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetEmployeeListForAcknowledgement", policyDocumentId.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                policyAcknowledgementView
            });
        }
        #endregion

        #region Get Employee List for sending a Notification and Email
        private async Task<List<PolicyAcknowledgementView>> GetEmployeeList(int policyDocumentId, string DocType)
        {
            List<PolicyAcknowledgementView> policyAcknowledgementView = new();
            try
            {
                PolicyEmployeeAcknowledgementListView policyEmployeeAcknowledgement = new();
                var result = await _client.GetAsync(_policyMgmtBaseURL, _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetEmployeeListForAcknowledgement") + policyDocumentId);
                policyEmployeeAcknowledgement = JsonConvert.DeserializeObject<PolicyEmployeeAcknowledgementListView>(JsonConvert.SerializeObject(result?.Data));
                if (policyEmployeeAcknowledgement != null)
                {
                    List<int> employeeIds = new();
                    if (DocType.ToLower() == "policy")
                    {
                        var employeeResult = await _client.PostAsJsonAsync(policyEmployeeAcknowledgement, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForAcknowledgement"));
                        employeeIds = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(employeeResult.Data));
                    }
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    foreach (PolicyAcknowledgementView view in policyEmployeeAcknowledgement.PolicyAcknowledgements)
                    {
                        if (view.AcknowledgedBy > 0) userIds.Add((int)view.AcknowledgedBy);
                    }
                    if (employeeIds?.Count > 0)
                    {
                        foreach (int employeeId in employeeIds)
                        {
                            if (employeeId > 0) userIds.Add(employeeId);
                        }
                    }
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            foreach (EmployeeName employee in listEmployeeName)
                            {
                                PolicyAcknowledgementView view = policyEmployeeAcknowledgement.PolicyAcknowledgements.
                                                                Where(x => x.AcknowledgedBy == employee.EmployeeId).FirstOrDefault();
                                PolicyAcknowledgementView acknowledgementView = new()
                                {
                                    AcknowledgedAt = view?.AcknowledgedAt ?? null,
                                    AcknowledgedBy = employee.EmployeeId,
                                    AcknowledgedByName = employee.EmployeeFullName,
                                    FormattedEmployeeId = employee.FormattedEmployeeId,
                                    EmployeeEmail = employee.EmployeeEmailId,
                                    AcknowledgedStatus = view?.AcknowledgedStatus ?? null,
                                    PolicyAcknowledgedId = view?.PolicyAcknowledgedId ?? 0,
                                    PolicyDocumentId = view?.PolicyDocumentId ?? 0,
                                    EmployeeId = employee?.EmployeeId ?? 0,
                                    EmployeeName = employee.EmployeeFullName
                                };
                                policyAcknowledgementView.Add(acknowledgementView);
                            }
                            policyAcknowledgementView = policyAcknowledgementView.OrderByDescending(x => x.AcknowledgedAt).
                                                            ThenBy(y => y.AcknowledgedByName).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetEmployeeList", Convert.ToString(policyDocumentId));
            }
            return policyAcknowledgementView;
        }
        #endregion

        #region Save or Update an Announcement
        [HttpPost]
        [Route("SaveAnnouncement")]
        public async Task<IActionResult> SaveAnnouncement(AnnouncementView announcementView)
        {
            string statusText = "";
            try
            {
                SuccessData data = await _client.PostAsJsonAsync(announcementView, _policyMgmtBaseURL,
                            _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:SaveAnnouncement"));
                announcementView.AnnouncementId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(data?.Data));
                if (data != null && data.StatusCode == "SUCCESS")
                {
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    if (announcementView.CreatedBy > 0) userIds.Add((int)announcementView.CreatedBy);
                    if (announcementView.ModifiedBy > 0) userIds.Add((int)announcementView.ModifiedBy);
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            if (announcementView.CreatedBy > 0)
                                announcementView.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == announcementView.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            if (announcementView.ModifiedBy > 0)
                                announcementView.ModifiedByName = listEmployeeName.Where(x => x.EmployeeId == announcementView.ModifiedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                        }
                    }
                    return Ok(new
                    {
                        data.StatusCode,
                        data.StatusText,
                        announcementView.AnnouncementId,
                        announcementView.CreatedByName,
                        announcementView.ModifiedByName
                    });
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/SaveAnnouncement", JsonConvert.SerializeObject(announcementView));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                AnnouncementId = 0,
                CreatedByName = "",
                ModifiedByName = ""
            });
        }
        #endregion

        #region Get an Announcement(s)
        [HttpGet]
        [Route("GetAnnouncements")]
        public async Task<IActionResult> GetAnnouncements(int pAnnouncementId = 0)
        {
            string statusText = "";
            List<AnnouncementView> announcementViewList = new();
            try
            {
                SuccessData data = await _client.GetAsync(_policyMgmtBaseURL,
                                        _configuration.GetValue<string>("ApplicationURL:PolicyMgmt:GetAnnouncements") +
                                        "?pAnnouncementId=" + pAnnouncementId);
                announcementViewList = JsonConvert.DeserializeObject<List<AnnouncementView>>(JsonConvert.SerializeObject(data?.Data));
                if (data != null && data.StatusCode == "SUCCESS")
                {
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    foreach (AnnouncementView announcementView in announcementViewList)
                    {
                        if (announcementView.CreatedBy > 0) userIds.Add((int)announcementView.CreatedBy);
                        if (announcementView.ModifiedBy > 0) userIds.Add((int)announcementView.ModifiedBy);
                    }
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        listEmployeeName = await GetEmployeeNamesAsync(userIds);
                        if (listEmployeeName.Count > 0)
                        {
                            foreach (AnnouncementView announcementView in announcementViewList)
                            {
                                if (announcementView.CreatedBy > 0)
                                    announcementView.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == announcementView.CreatedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                                if (announcementView.ModifiedBy > 0)
                                    announcementView.ModifiedByName = listEmployeeName.Where(x => x.EmployeeId == announcementView.ModifiedBy).Select(y => y.EmployeeFullName + " - " + y.FormattedEmployeeId).FirstOrDefault().ToString();
                            }
                        }
                    }
                    return Ok(new
                    {
                        data.StatusCode,
                        data.StatusText,
                        announcementViewList
                    });
                }
                else
                    statusText = data?.StatusText;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "PolicyMgmt/GetAnnouncements");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                announcementViewList
            });
        }
        #endregion
    }
}