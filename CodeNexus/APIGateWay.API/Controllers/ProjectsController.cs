using APIGateWay.API.Common;
using APIGateWay.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Notifications;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Accounts;
using SharedLibraries.ViewModels.Notifications;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Reports;
using SharedLibraries.ViewModels.Timesheet;
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
    public class ProjectsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HTTPClient _client;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _projectBaseURL = string.Empty;
        private readonly string _timesheetBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _notificationBaseURL = string.Empty;
        private readonly string _accountsBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";

        #region Constructor
        public ProjectsController(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _configuration = configuration;
            _projectBaseURL = _configuration.GetValue<string>("ApplicationURL:Projects:BaseURL");
            _timesheetBaseURL = _configuration.GetValue<string>("ApplicationURL:Timesheet:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _accountsBaseURL = _configuration.GetValue<string>("ApplicationURL:Accounts:BaseURL");
            _notificationBaseURL = _configuration.GetValue<string>("ApplicationURL:Notifications");
        }
        #endregion

        #region Bulk Insert Project
        [HttpPost]
        [Route("BulkInsertProject")]
        public async Task<IActionResult> BulkInsertProject(IFormFile uploadFile)
        {
            try
            {
                if (uploadFile.Length > 0)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using(var ms = new MemoryStream())
                    {
                        uploadFile.CopyTo(ms);
                        SharedLibraries.ViewModels.Projects.ImportExcelView import = new SharedLibraries.ViewModels.Projects.ImportExcelView();
                        import.Base64Format = Convert.ToBase64String(ms.ToArray());
                        var employeeDetails =await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllActiveEmployeeDetails"));
                        var accountDetails = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccountsDetails"));
                        var skillSetsDetails = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllSkillSets"));
                        import.EmployeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetail>>(JsonConvert.SerializeObject(employeeDetails.Data));
                        import.AccountDetails = JsonConvert.DeserializeObject<List<AccountDetails>>(JsonConvert.SerializeObject(accountDetails.Data));
                        import.Skillsets = JsonConvert.DeserializeObject<List<Skillsets>>(JsonConvert.SerializeObject(skillSetsDetails.Data));
                        var result = await _client.PostAsJsonAsync(import, _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:BulkInsertProject"));
                    }
                }
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = strErrorMsg
                });
            }
            catch(Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/BulkInsertProject");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg
                });
            }
        }
        #endregion
        #region Insert And Update project
        [HttpPost]
        [Route("InsertUpdateProject")]
        public async Task<IActionResult> InsertAndUpdateProject(AddProjectView AddProjectView)
        {
            string statusText = "", statusCode = ""; int projectId = 0;
            try
            {
                ProjectDetailView pProjectDetails = AddProjectView?.ProjectDetail;
                if (pProjectDetails.IsDraft == false)
                {
                    var managerResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetfinanceManagerId"));
                    pProjectDetails.FinanceManagerId = (int)managerResult?.Data;
                    List<int> accountIds = new();
                    List<AccountNames> lstAccName = new();
                    accountIds.Add((int)pProjectDetails.AccountId);
                    var accountNameResult = await _client.PostAsJsonAsync(accountIds, _accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAccountNameById"));
                    lstAccName = JsonConvert.DeserializeObject<List<AccountNames>>(JsonConvert.SerializeObject(accountNameResult.Data));
                    if (lstAccName?.Count > 0)
                    {
                        pProjectDetails.AccountName = lstAccName[0].AccountName;
                    }
                }
                var projectResult = await _client.PostAsJsonAsync(pProjectDetails, _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:InsertUpdateProject"));
                projectId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(projectResult?.Data));
                if (projectId > 0)
                {
                    if (pProjectDetails.IsDraft == false)
                    {
                        List<Notifications> notifications = new();
                        Notifications notification = new()
                        {
                            CreatedBy = pProjectDetails.CreatedBy == null ? 0 : (int)pProjectDetails.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            FromId = pProjectDetails.CreatedBy == null ? 0 : (int)pProjectDetails.CreatedBy,
                            ToId = pProjectDetails.CreatedBy == null ? 0 : (int)pProjectDetails.CreatedBy,
                            MarkAsRead = false,
                            NotificationSubject = "Project " + pProjectDetails.ProjectName + " for customer " + pProjectDetails.AccountName + " has been sent for approval.",
                            NotificationBody = "Project " + pProjectDetails.ProjectName + " for customer " + pProjectDetails.AccountName + " has been sent for approval.",
                            PrimaryKeyId = projectId,
                            ButtonName = "View Project",
                            SourceType = "Projects"
                        };
                        notifications.Add(notification);
                        if (pProjectDetails.EngineeringLeadId != null)
                        {
                            notification.ToId = (int)pProjectDetails.EngineeringLeadId;
                            notifications.Add(notification);
                        }
                        notification = new()
                        {
                            CreatedBy = pProjectDetails.CreatedBy == null ? 0 : (int)pProjectDetails.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            FromId = pProjectDetails.CreatedBy == null ? 0 : (int)pProjectDetails.CreatedBy,
                            ToId = (int)pProjectDetails.FinanceManagerId,
                            MarkAsRead = false,
                            NotificationSubject = "You have been requested to review project " + pProjectDetails.ProjectName + " for customer " + pProjectDetails.AccountName + ".",
                            NotificationBody = "You have been requested to review project " + pProjectDetails.ProjectName + " for customer " + pProjectDetails.AccountName + ".",
                            PrimaryKeyId = projectId,
                            ButtonName = "Review Project",
                            SourceType = "Projects"
                        };
                        notifications.Add(notification);
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
                    if (projectId > 0 && AddProjectView?.ListOfDocuments?.Count > 0)
                    {
                        SupportingDocumentsView supportingDocuments = new();
                        supportingDocuments.ListOfDocuments = new();
                        supportingDocuments.SourceId = projectId;
                        supportingDocuments.SourceType = _configuration.GetValue<string>("ProjectsSourceType");
                        supportingDocuments.BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                        supportingDocuments.CreatedBy = pProjectDetails?.CreatedBy == null ? 0 : (int)pProjectDetails?.CreatedBy;
                        //Add supporting documents
                        if (!string.IsNullOrEmpty(supportingDocuments.BaseDirectory))
                        {
                            //Create base directory
                            if (!Directory.Exists(supportingDocuments.BaseDirectory))
                            {
                                Directory.CreateDirectory(supportingDocuments.BaseDirectory);
                            }
                            //Create source type directory
                            if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType)))
                            {
                                Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType));
                            }
                            //Create accountId directory
                            if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString())))
                            {
                                Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString()));
                            }
                        }
                        string directoryPath = Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString());
                        List<DocumentDetails> docList = new();
                        foreach (var item in AddProjectView?.ListOfDocuments)
                        {
                            if (item.DocumentName == null) continue;
                            string documentPath = Path.Combine(directoryPath, item.DocumentName);
                            if (!System.IO.File.Exists(item.DocumentName) && item.DocumentSize > 0)
                            {
                                if (item.DocumentAsBase64.Contains(","))
                                {
                                    item.DocumentAsBase64 = item.DocumentAsBase64.Substring(item.DocumentAsBase64.IndexOf(",") + 1);
                                }
                                item.DocumentAsByteArray = Convert.FromBase64String(item.DocumentAsBase64);
                                using (Stream fileStream = new FileStream(documentPath, FileMode.Create))
                                {
                                    fileStream.Write(item.DocumentAsByteArray, 0, item.DocumentAsByteArray.Length);
                                }
                            }
                            if (item.DocumentCategory?.ToLower() == "logo")
                            {
                                UpdateProjectLogo updateProjectLogo = new()
                                {
                                    ProjectId = projectId,
                                    ModifiedBy = pProjectDetails?.CreatedBy == null ? 0 : (int)pProjectDetails?.CreatedBy,
                                    Logo = documentPath
                                };
                                using var logoClient = new HttpClient
                                {
                                    BaseAddress = new Uri(_projectBaseURL)
                                };
                                HttpResponseMessage logoResponse = await logoClient.PostAsJsonAsync("Projects/AssignLogoForProject", updateProjectLogo);
                                var logoResult = logoResponse.Content.ReadAsAsync<SuccessData>();
                                if (logoResponse?.IsSuccessStatusCode == false)
                                {
                                    statusCode = "FAILURE";
                                    statusText = logoResult?.Result?.StatusText;
                                }
                            }
                            else
                            {
                                DocumentDetails docDetails = new();
                                docDetails.DocumentCategory = item.DocumentCategory;
                                docDetails.IsApproved = false;
                                docDetails.DocumentSize = item.DocumentSize;
                                docDetails.DocumentName = item.DocumentName;
                                docDetails.DocumentType = Path.GetExtension(item.DocumentName);
                                docList.Add(docDetails);
                            }
                        }
                        supportingDocuments.ListOfDocuments = docList;
                        using var documentClient = new HttpClient
                        {
                            BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                        };
                        HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/AddSupportingDocuments", supportingDocuments);
                        var documentResult = documentResponse.Content.ReadAsAsync<SuccessData>();
                        if (documentResponse?.IsSuccessStatusCode == false)
                        {
                            statusCode = "FAILURE";
                            statusText = documentResult?.Result?.StatusText;
                        }
                    }
                }
                return Ok(new
                {
                    projectResult?.StatusCode,
                    projectResult?.StatusText,
                    ProjectId = projectId
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/InsertAndUpdateProject", JsonConvert.SerializeObject(AddProjectView));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText,
                ProjectId = projectId
            });
        }
        #endregion

        #region Delete Project
        [HttpGet]
        [Route("DeleteProject")]
        public async Task<IActionResult> DeleteProject(int pProjectId)
        {
            try
            {
                var projectResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:DeleteProject") + pProjectId);
                return Ok(new
                {
                    projectResult?.StatusCode,
                    projectResult?.StatusText
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/DeleteProject", JsonConvert.SerializeObject(pProjectId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get Project Detail By Id
        [HttpGet]
        [Route("GetProjectDetailById")]
        public async Task<IActionResult> GetProjectDetailById(int pProjectID)
        {
            ProjectsViewModel projectDetail = new();
            AccountView accountView = new();
            AccountView accountViewFinal = new();
            List<AccountView> listofActiveAccounts = new();
            List<EmployeeName> listEmployeeName = new();
            List<int?> listOfEmployeeId = new();
            try
            {
                var result = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectDetailById") + pProjectID);
                projectDetail = JsonConvert.DeserializeObject<ProjectsViewModel>(JsonConvert.SerializeObject(result.Data));
                if (projectDetail?.ProjectDetails != null)
                {
                    if (projectDetail?.ProjectDetails?.Logo != null && projectDetail?.ProjectDetails?.Logo != "")
                    {
                        projectDetail.ProjectDetails.LogoBase64 = ConvertToBase64String(projectDetail?.ProjectDetails?.Logo);
                    }
                    List<int?> resourEmpId = projectDetail?.ProjectDetails?.ResourceAllocation.Where(x => x.EmployeeId != null).Select(x => x.EmployeeId).ToList();
                    if (resourEmpId?.Count > 0)
                        listOfEmployeeId = resourEmpId;
                    listOfEmployeeId.Add(projectDetail.ProjectDetails?.ProjectSPOC == null ? 0 : projectDetail.ProjectDetails?.ProjectSPOC);
                    listOfEmployeeId.Add(projectDetail.ProjectDetails?.CreatedBy == null ? 0 : projectDetail.ProjectDetails?.CreatedBy);
                    //Get BU Accountable for the Project
                    if (projectDetail?.ProjectDetails?.bUAccountableForProject > 0)
                    {
                        projectDetail.ProjectDetails.BUHeadNameForProject = await GetBUHeadNameForProject(projectDetail.ProjectDetails.bUAccountableForProject);
                    }
                    //Get Status name
                    if (!string.IsNullOrEmpty(projectDetail.ProjectDetails?.ProjectStatusCode))
                    {
                        projectDetail.ProjectDetails.ProjectStatusName = await GetStatusNameByCode(projectDetail.ProjectDetails.ProjectStatusCode);
                    }
                    //Get Account name
                    projectDetail.ProjectDetails.AccountName = GetAccountNameById(new List<int?> { projectDetail.ProjectDetails.AccountId }).Result.Where(x => x.AccountId == projectDetail.ProjectDetails.AccountId).Select(x => x.AccountName).FirstOrDefault();
                    var accountResult = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccounts") + "false");
                    listofActiveAccounts = JsonConvert.DeserializeObject<List<AccountView>>(JsonConvert.SerializeObject(accountResult.Data));
                    accountView = listofActiveAccounts.Where(x => x.AccountId == projectDetail.ProjectDetails.AccountId).FirstOrDefault();
                    if (accountView != null)
                    {
                        //Project Detail
                        List<int?> listOfAccountManargerId = listofActiveAccounts?.Where(x => x.AccountId == projectDetail.ProjectDetails.AccountId).Select(x => x.AccountManagerId).ToList();
                        if (listOfAccountManargerId?.Count > 0)
                            listOfEmployeeId.AddRange(listOfAccountManargerId);
                        //List Of Comments
                        List<int?> listOfCommentsCreatedId = projectDetail.ProjectDetailsCommentsList?.Select(x => x.CreatedBy).ToList();
                        if (listOfCommentsCreatedId?.Count > 0)
                            listOfEmployeeId.AddRange(listOfCommentsCreatedId);
                        //List Of CR
                        List<int?> listOfCRCreatedBy = projectDetail.ChangeRequestList?.Select(x => x.CreatedBy).ToList();
                        if (listOfCRCreatedBy?.Count > 0)
                            listOfEmployeeId.AddRange(listOfCRCreatedBy);
                        var listOfCRResource = projectDetail.ChangeRequestList?.Select(x => x.ResourceAllocation).ToList();
                        if (listOfCRResource != null)
                        {
                            foreach (var item in listOfCRResource)
                            {
                                List<int?> listOfCRResourceId = item?.Select(x => x.EmployeeId).ToList();
                                if (listOfCRResourceId?.Count > 0)
                                    listOfEmployeeId.AddRange(listOfCRResourceId);
                            }
                        }
                        // Get employee Name
                        if (listOfEmployeeId?.Count > 0)
                            listEmployeeName = GetEmployeeNameById(listOfEmployeeId).Result;
                        projectDetail.ProjectDetails.ResourceAllocation?.ForEach(x => x.EmployeeName = listEmployeeName.Where(y => y.EmployeeId == x.EmployeeId).Select(z => z.EmployeeFullName).FirstOrDefault());
                        projectDetail.ProjectDetails.ProjectManager = listEmployeeName?.Where(y => y.EmployeeId == projectDetail.ProjectDetails.ProjectSPOC).Select(x => x.EmployeeFullName).FirstOrDefault();
                        projectDetail.ProjectDetails.ProjectManagerEmpId = listEmployeeName?.Where(y => y.EmployeeId == projectDetail.ProjectDetails.ProjectSPOC).Select(x => x.FormattedEmployeeId).FirstOrDefault();
                        projectDetail.ProjectDetails.CreatedByName = listEmployeeName?.Where(y => y.EmployeeId == projectDetail.ProjectDetails.CreatedBy).Select(x => x.EmployeeFullName).FirstOrDefault();
                        projectDetail.ProjectDetailsCommentsList.ForEach(x => x.CreatedByName = listEmployeeName.Where(y => y.EmployeeId == x.CreatedBy).Select(z => z.EmployeeFullName).FirstOrDefault());
                        if (projectDetail.ChangeRequestList?.Count > 0)
                        {
                            foreach (var item in projectDetail.ChangeRequestList)
                            {
                                item.ResourceAllocation?.ForEach(x => x.EmployeeName = listEmployeeName?.Where(y => y.EmployeeId == x.EmployeeId).Select(z => z.EmployeeFullName).FirstOrDefault());
                                item.CreatedByName = listEmployeeName?.Where(y => y.EmployeeId == item.CreatedBy).Select(z => z.EmployeeFullName).FirstOrDefault();
                            }
                        }
                        accountViewFinal = new()
                        {
                            AccountId = accountView.AccountId,
                            AccountManagerId = accountView.AccountManagerId,
                            AccountManager = listEmployeeName.Where(x => x.EmployeeId == accountView.AccountManagerId).Select(x => x.EmployeeFullName).FirstOrDefault(),
                            AccountStatus = accountView.AccountStatus,
                            AccountContact = accountView.AccountContact,
                            AccountSPOC = accountView.AccountSPOC,
                            AccountDescription = accountView.AccountDescription,
                            AccountName = accountView.AccountName,
                            Logo = accountView.Logo,
                            LogoBase64 = ConvertToBase64String(accountView.Logo)
                        };
                    }
                }
                
                if(projectDetail?.ProjectDetails !=null)
                {
                    List<int> projectIdList = new List<int>() { pProjectID };
                    var timesheetLogResult = await _client.PostAsJsonAsync(projectIdList, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetLogByProjectId"));
                    List<TimesheetLogView> listTimesheetLog = JsonConvert.DeserializeObject<List<TimesheetLogView>>(JsonConvert.SerializeObject(timesheetLogResult.Data));
                    long? totalTicks = listTimesheetLog.Where(x => x.ProjectId == pProjectID).Select(x => new TimeSpan(x.ClockedHours.Split(":").Length > 0 ? Convert.ToInt32(x.ClockedHours.Split(":")[0]) : 0,
                                                  x.ClockedHours.Split(":").Length > 1 ? Convert.ToInt32(x.ClockedHours.Split(":")[1]) : 0, 0).Ticks).Sum();
                    string totalHours = "00:00 Hours";
                    if (totalTicks != null && totalTicks > 0)
                    {
                        TimeSpan time = new TimeSpan((long)totalTicks);
                        totalHours = ((int)time.TotalHours).ToString("00") + ":" + time.Minutes.ToString("00") + " Hours";
                    }
                    projectDetail.ProjectDetails.ProjectTimesheetStatus = totalHours;
                }

                using (var documentClient = new HttpClient())
                {
                    List<int> crSourceId = new();
                    if (projectDetail?.ChangeRequestList != null)
                        crSourceId = projectDetail?.ChangeRequestList?.Select(x => x.ChangeRequestId).ToList();
                    crSourceId.Add(pProjectID);
                    SourceDocuments sourceDocuments = new();
                    sourceDocuments.SourceId = crSourceId;
                    sourceDocuments.SourceType = new List<string> { _configuration.GetValue<string>("ProjectsSourceType"), _configuration.GetValue<string>("ChangeRequestSourceType") };
                    documentClient.BaseAddress = new Uri(_notificationBaseURL);
                    HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/GetDocumentBySourceIdAndType", sourceDocuments);
                    if (documentResponse?.IsSuccessStatusCode == true)
                    {
                        projectDetail.ProjectDetails.ListOfDocuments = new List<DocumentDetails>();
                        var documentResult = documentResponse.Content.ReadAsAsync<SuccessData>();
                        List<SupportingDocuments> documentList = JsonConvert.DeserializeObject<List<SupportingDocuments>>(JsonConvert.SerializeObject(documentResult.Result.Data));
                        projectDetail.ProjectDetails.ListOfDocuments = documentList?.Where(x => x.SourceId == pProjectID && x.SourceType == _configuration.GetValue<string>("ProjectsSourceType")).Select(x => new DocumentDetails { DocumentId = x.DocumentId, DocumentName = x.DocumentName, DocumentSize = x.DocumentSize, DocumentCategory = x.DocumentCategory, IsApproved = x.IsApproved, DocumentType = x.DocumentType }).ToList();
                        if (projectDetail.ChangeRequestList != null)
                        {
                            foreach (var item in projectDetail.ChangeRequestList)
                            {
                                item.ListOfDocuments = documentList?.OrderBy(x => x.CreatedOn).Where(x => x.SourceId == item?.ChangeRequestId && x.SourceType == _configuration.GetValue<string>("ChangeRequestSourceType")).Select(x => new DocumentDetails { DocumentId = x.DocumentId, DocumentName = x.DocumentName, DocumentSize = x.DocumentSize, DocumentCategory = x.DocumentCategory, IsApproved = x.IsApproved, DocumentType = x.DocumentType }).ToList();
                            }
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    ProjectDetailView = projectDetail.ProjectDetails,
                    ProjectDetailComments = projectDetail.ProjectDetailsCommentsList,
                    ChangeRequests = projectDetail.ChangeRequestList,
                    accountView = accountViewFinal
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetProjectDetailById", Convert.ToString(pProjectID));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    ProjectDetailView = projectDetail.ProjectDetails,
                    ProjectDetailComments = projectDetail.ProjectDetailsCommentsList,
                    ChangeRequests = projectDetail.ChangeRequestList,
                    accountView = accountViewFinal
                });
            }
        }
        #endregion

        #region Convert Logo To Base64 String
        private static string ConvertToBase64String(string pLogo)
        {
            string strValue = null;
            if (pLogo != null && pLogo != "")
            {
                string extension = Path.GetExtension(pLogo);
                if (extension != "")
                    strValue = "data:image/" + extension.ToLower().Replace(".", "") + ";base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(pLogo));
            }
            return strValue;
        }
        #endregion

        #region Get Project's Master Data
        [HttpGet]
        [Route("GetProjectsMasterData")]
        public async Task<IActionResult> GetProjectsMasterData()
        {
            try
            {
                var result = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectsMasterData"));
                ProjectMasterData masterData = JsonConvert.DeserializeObject<ProjectMasterData>(JsonConvert.SerializeObject(result.Data));
                if (result != null)
                {
                    var employeeMaster = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetProjectEmployeeMasterData"));
                    EmployeesMasterData listOfEmployeeMaster = JsonConvert.DeserializeObject<EmployeesMasterData>(JsonConvert.SerializeObject(employeeMaster.Result.Data));
                    if (listOfEmployeeMaster != null)
                    {
                        masterData.ListOfRoleName = listOfEmployeeMaster?.RoleNameList;
                        masterData.ListOfRequiredSkillSets = listOfEmployeeMaster?.Skillsets;
                        masterData.ListOfDesignation = listOfEmployeeMaster?.ListOfDesignation;
                        masterData.BUAccountableForProjects = listOfEmployeeMaster?.BUAccountableForProjects;
                    }
                    var listOfStatusResult = _client.GetAsync(_configuration.GetValue<string>("ApplicationURL:Notification:BaseURL"), _configuration.GetValue<string>("ApplicationURL:Notification:GetAllStatusList"));
                    List<StatusViewModel> listOfStatus = JsonConvert.DeserializeObject<List<StatusViewModel>>(JsonConvert.SerializeObject(listOfStatusResult.Result.Data));
                    if (listOfStatus?.Count > 0)
                    {
                        masterData.ProjectStatusList = listOfStatus;
                    }
                    return Ok(new
                    {
                        result.StatusCode,
                        ProjectMasterData = masterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetProjectsMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ProjectMasterData = new ProjectMasterData()
            });
        }
        #endregion

        #region Get All Accounts
        [HttpGet]
        [Route("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            List<AccountView> accountViews = new();
            List<EmployeeName> listEmployeeName = new();
            List<AccountView> listofActiveAccounts = new();
            try
            {
                var result = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccounts") + "false");
                accountViews = JsonConvert.DeserializeObject<List<AccountView>>(JsonConvert.SerializeObject(result.Data));
                if (accountViews?.Count > 0)
                {
                    listEmployeeName = GetEmployeeNameById(accountViews?.Select(x => x.AccountManagerId).ToList()).Result;
                    foreach (var accList in accountViews.OrderByDescending(x => x.AccountId))
                    {
                        AccountView accountList = new()
                        {
                            AccountId = accList.AccountId,
                            AccountManagerId = accList.AccountManagerId,
                            AccountManager = listEmployeeName.Where(x => x.EmployeeId == accList.AccountManagerId).Select(x => x.EmployeeFullName).FirstOrDefault(),
                            AccountStatus = accList.AccountStatus,
                            AccountContact = accList.AccountContact,
                            AccountSPOC = accList.AccountSPOC,
                            AccountDescription = accList.AccountDescription,
                            AccountName = accList.AccountName,
                            Logo = accList.Logo,
                            LogoBase64 = ConvertToBase64String(accList.Logo)
                        };
                        listofActiveAccounts.Add(accountList);
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    listofActiveAccounts
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetAllAccounts");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    listofActiveAccounts
                });
            }
        }
        #endregion

        #region Get All Project Details By ResourceId
        [HttpGet]
        [Route("GetProjectDetailsByResourceId")]
        public async Task<IActionResult> GetProjectDetailsByResourceId(int pResourceId, bool pIsAllProjects = false)
        {
            List<ProjectListView> projectListViews = new();
            List<AccountNames> lstAccountName = new();
            List<EmployeeName> lstEmployeeName = new();
            List<RoleName> lstRoleName = new();
            List<TimesheetLogView> listTimesheetLog = new();
            //List<ProjectListWithTimesheet> projectListWithTimesheets = new();
            //List<int> listResourceId = new();
            try
            {
                var RoleList = _configuration.GetValue<string>("CustomerProjectManagementRole:RoleList");

                //if (pIsAllProjects)
                //{
                //    var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForManager") + pResourceId);
                //    List<EmployeeList> listOfEmployees = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                //    if (listOfEmployees?.Count > 0)
                //    {
                //        listResourceId = listOfEmployees.Select(x => x.EmployeeId).Distinct().ToList();
                //    }
                //}
                //if (listResourceId.Count == 0)
                //    listResourceId = new List<int> { pResourceId };

                var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForProjectAndCustomer") + pResourceId+ "&isAllEmployee="+pIsAllProjects);
                ProjectCustomerEmployeeList listOfEmployees = JsonConvert.DeserializeObject<ProjectCustomerEmployeeList>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                listOfEmployees.ManagementRole = RoleList.ToLower().Split(',').ToList();
                //if (listOfEmployees?.Count > 0)
                //{
                //    listResourceId = listOfEmployees.Select(x => x.EmployeeId).Distinct().ToList();
                //}

                var result = await _client.PostAsJsonAsync(listOfEmployees, _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectDetailsByResourceId"));
                projectListViews = JsonConvert.DeserializeObject<List<ProjectListView>>(JsonConvert.SerializeObject(result.Data));
                var timesheetLogResult = await _client.PostAsJsonAsync(projectListViews?.Select(x => x.ProjectId).ToList(), _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetLogByProjectId"));
                listTimesheetLog = JsonConvert.DeserializeObject<List<TimesheetLogView>>(JsonConvert.SerializeObject(timesheetLogResult.Data));
                //Assign account name
                lstAccountName = GetAccountNameById(projectListViews.Select(x => x.AccountId).ToList()).Result;
                projectListViews.ForEach(x => x.AccountName = lstAccountName.Where(y => y.AccountId == x.AccountId).Select(y => y.AccountName).FirstOrDefault());
                //Assign employee name
                lstEmployeeName = GetEmployeeNameById(projectListViews.Select(x => (int?)x.SPOCId).ToList()).Result;
                projectListViews.ForEach(x => x.SPOC = lstEmployeeName.Where(y => y.EmployeeId == x.SPOCId).Select(y => y.EmployeeFullName).FirstOrDefault());
                //Assign role name
                lstRoleName = GetRoleNameById(new List<int?> { pResourceId }).Result;
                projectListViews.ForEach(x => x.UserRole = lstRoleName.Select(y => y.RoleFullName).FirstOrDefault());
                //Convert Logo as Base64 string
                projectListViews.ForEach(x => x.LogoBase64 = ConvertToBase64String(x.Logo));
                foreach (var project in projectListViews)
                {
                    long? totalTicks = listTimesheetLog?.Where(x => x.ProjectId == project.ProjectId)?.Select(x => new TimeSpan(x.ClockedHours?.Split(":")?.Length > 0 ? Convert.ToInt32(x.ClockedHours?.Split(":")[0]) : 0,
                                              x.ClockedHours?.Split(":").Length > 1 ? Convert.ToInt32(x.ClockedHours?.Split(":")[1]) : 0, 0).Ticks)?.Sum();
                    string totalHours = "00:00 Hours";
                    if(totalTicks !=null && totalTicks>0)
                    {
                        TimeSpan time = new TimeSpan((long)totalTicks);
                        totalHours = ((int)time.TotalHours).ToString("00") + ":" + time.Minutes.ToString("00") + " Hours";
                    }
                    project.Timesheets = totalHours;
                }
                    //if (listTimesheetLog?.Count > 0)
                    //{
                    //    foreach (var project in projectListViews)
                    //    {
                    //        string projectTimesheetStatus = "", projectName = "", projectStatus = "";
                    //        int? resourceCount = 0;
                    //        decimal? projectDuration = 0;
                    //        DateTime? projectStartDate, projectEndDate;
                    //        long clockedHoursTick = listTimesheetLog.Where(x => x.ProjectId == project.ProjectId).Select(x =>
                    //        new TimeSpan(x.ClockedHours.Split(":").Length > 0 ? Convert.ToInt32(x.ClockedHours.Split(":")[0]) : 0,
                    //                x.ClockedHours.Split(":").Length > 1 ? Convert.ToInt32(x.ClockedHours.Split(":")[1]) : 0, 0).Ticks).Sum();
                    //        TimeSpan? totalClockedHours = new TimeSpan(clockedHoursTick);
                    //        projectDuration = project.ProjectDuration; //project.Where(x => x.ProjectId == project.Key.ProjectId).Select(x => x.ProjectDuration).FirstOrDefault();
                    //        projectStartDate = project.ProjectStartDate; // project.Where(x => x.ProjectId == project.Key.ProjectId).Select(x => x.ProjectStartDate).FirstOrDefault();
                    //        projectEndDate = project.ProjectEndDate; // project.Where(x => x.ProjectId == project.Key.ProjectId).Select(x => x.ProjectEndDate).FirstOrDefault();
                    //        resourceCount = project.ResourceCount; // project.Where(x => x.ProjectId == project.Key.ProjectId).Select(x => x.ResourceCount).FirstOrDefault();
                    //        if (projectEndDate != null && projectStartDate != null && resourceCount >0 && projectEndDate?.Date != DateTime.MinValue.Date)
                    //        {
                    //            projectDuration = projectEndDate.Value.Subtract(projectStartDate.Value).Days * resourceCount * 8;
                    //        }
                    //        projectName = project.ProjectName; // project.Select(x => x.ProjectName).FirstOrDefault();
                    //        projectStatus = project.ProjectStatus; // project.Select(x => x.ProjectStatus).FirstOrDefault();
                    //        if (projectStatus == "Completed")
                    //        {
                    //            if (projectTimesheetStatus != "") projectTimesheetStatus = ", ";
                    //            projectTimesheetStatus += "Completed";
                    //        }
                    //        else if (totalClockedHours.Value.Ticks > TimeSpan.FromHours((double)projectDuration).Ticks)
                    //        {
                    //            if (projectTimesheetStatus != "") projectTimesheetStatus = ", ";
                    //            projectTimesheetStatus += "Overdue";
                    //        }
                    //        else if (totalClockedHours.Value.Ticks < TimeSpan.FromHours((double)projectDuration).Ticks)
                    //        {
                    //            if (projectTimesheetStatus != "") projectTimesheetStatus = ", ";
                    //            projectTimesheetStatus += "On Track";
                    //        }
                    //        ProjectListWithTimesheet projectListWithTimesheet = new()
                    //        {
                    //            ProjectId = project.ProjectId,
                    //            TimesheetStatus = projectTimesheetStatus
                    //        };
                    //        projectListWithTimesheets.Add(projectListWithTimesheet);
                    //    }
                    //    projectListViews.ForEach(x => x.Timesheets = projectListWithTimesheets.Where(y => y.ProjectId == x.ProjectId).Select(y => y.TimesheetStatus).FirstOrDefault());
                    //}
                    return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    projectListViews
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetProjectDetailsByResourceId", " EmployeeId- " + pResourceId.ToString() + " AllProjects- " + pIsAllProjects.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    projectListViews
                });
            }
        }
        #endregion

        #region Get All Drafted Project Details By ResourceId
        [HttpGet]
        [Route("GetDraftedProjectDetailsByResourceId")]
        public async Task<IActionResult> GetDraftedProjectDetailsByResourceId(int pResourceId)
        {
            List<ProjectListView> projectListViews = new();
            try
            {
                var result = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetDraftedProjectDetailsByResourceId") + pResourceId);
                projectListViews = JsonConvert.DeserializeObject<List<ProjectListView>>(JsonConvert.SerializeObject(result.Data));
                if (projectListViews != null && projectListViews.Count > 0)
                {
                    projectListViews.ForEach(x => x.LogoBase64 = ConvertToBase64String(x.Logo));
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    ListofProjects = projectListViews
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetDraftedProjectDetailsByResourceId", Convert.ToString(pResourceId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    ListofProjects = projectListViews
                });
            }
        }
        #endregion

        #region Approve or Reject the Project
        [HttpPost]
        [Route("ApproveOrRejectProject")]
        public async Task<IActionResult> ApproveOrRejectProject(ApproveOrRejectProject pApproveOrRejectProject)
        {
            int projectId = 0;
            string statusCode = "", statusText = "";
            List<EmployeeList> listEmployeeList = new();
            ProjectsViewModel projectDetail = new();
            try
            {
                //listEmployeeList = await GetEmployeesByRole(_configuration.GetValue<string>("EngineeringLeadPrimaryRoleName"));
                //if (listEmployeeList?.Count == 0) listEmployeeList = await GetEmployeesByRole(_configuration.GetValue<string>("EngineeringLeadSecondaryRoleName"));
                //if (listEmployeeList?.Count > 0) pApproveOrRejectProject.DepartmentHeadId = listEmployeeList[0].EmployeeId;
                var projectResult = await _client.PostAsJsonAsync(pApproveOrRejectProject, _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:ApproveOrRejectProject"));
                projectId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(projectResult?.Data));
                if (projectId > 0 && pApproveOrRejectProject.ProjectStatus != "Action Required")
                {
                    using (var documentClient = new HttpClient())
                    {
                        SourceDocuments sourceDocuments = new();
                        sourceDocuments.SourceId = new List<int> { pApproveOrRejectProject.ProjectId };
                        sourceDocuments.SourceType = new List<string> { _configuration.GetValue<string>("ProjectsSourceType") };
                        documentClient.BaseAddress = new Uri(_notificationBaseURL);
                        HttpResponseMessage appDocumentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/ApprovedDocumentsBySourceIdAndType", sourceDocuments);
                        if (appDocumentResponse?.IsSuccessStatusCode == false)
                        {
                            var appDocumentResult = appDocumentResponse.Content.ReadAsAsync<SuccessData>();
                            statusCode = "FAILURE";
                            statusText = appDocumentResult?.Result?.StatusText;
                        }
                    }
                }
                var result = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectDetailById") + projectId);
                projectDetail = JsonConvert.DeserializeObject<ProjectsViewModel>(JsonConvert.SerializeObject(result.Data));
                if (projectDetail?.ProjectDetails != null && statusCode != "FAILURE")
                {
                    string ApproveOrReject = "approved", financeManager = "", buttonName = "View Project";
                    int toId = pApproveOrRejectProject.DepartmentHeadId;
                    List<EmployeeName> listEmployeeName = new();
                    List<int?> listOfEmployeeId = new();
                    List<Notifications> notifications = new();
                    projectDetail.ProjectDetails.AccountName = GetAccountNameById(new List<int?> { projectDetail.ProjectDetails.AccountId }).Result.Where(x => x.AccountId == projectDetail.ProjectDetails.AccountId).Select(x => x.AccountName).FirstOrDefault();
                    listOfEmployeeId.Add(projectDetail.ProjectDetails.FinanceManagerId);
                    listEmployeeName = GetEmployeeNameById(listOfEmployeeId).Result;
                    if (listEmployeeName?.Count > 0) financeManager = listEmployeeName[0].EmployeeFullName;
                    if (pApproveOrRejectProject.ProjectStatus == "Action Required")
                    {
                        ApproveOrReject = "requested changes in";
                        buttonName = "Edit Project";
                        toId = (int)projectDetail.ProjectDetails.CreatedBy;
                    }
                    Notifications notification = new()
                    {
                        CreatedBy = projectDetail.ProjectDetails.FinanceManagerId == null ? 0 : (int)projectDetail.ProjectDetails.FinanceManagerId,
                        CreatedOn = DateTime.UtcNow,
                        FromId = projectDetail.ProjectDetails.FinanceManagerId == null ? 0 : (int)projectDetail.ProjectDetails.FinanceManagerId,
                        ToId = projectDetail.ProjectDetails.CreatedBy == null ? 0 : (int)projectDetail.ProjectDetails.CreatedBy,
                        MarkAsRead = false,
                        NotificationSubject = financeManager + " has " + ApproveOrReject + " project " + projectDetail.ProjectDetails.ProjectName + " for customer " + projectDetail.ProjectDetails.AccountName + ".",
                        NotificationBody = financeManager + " has " + ApproveOrReject + " project " + projectDetail.ProjectDetails.ProjectName + " for customer " + projectDetail.ProjectDetails.AccountName + ".",
                        PrimaryKeyId = projectId,
                        ButtonName = buttonName,
                        SourceType = "Projects"
                    };
                    notifications.Add(notification);
                    if (pApproveOrRejectProject.ProjectStatus != "Action Required")
                    {
                        notification = new()
                        {
                            CreatedBy = (int)projectDetail.ProjectDetails.FinanceManagerId,
                            CreatedOn = DateTime.UtcNow,
                            FromId = (int)projectDetail.ProjectDetails.FinanceManagerId,
                            ToId = toId,
                            MarkAsRead = false,
                            NotificationSubject = "You have been requested to assign a SPOC to project " + projectDetail.ProjectDetails.ProjectName + " for customer " + projectDetail.ProjectDetails.AccountName + ".",
                            NotificationBody = "You have been requested to assign a SPOC to project " + projectDetail.ProjectDetails.ProjectName + " for customer " + projectDetail.ProjectDetails.AccountName + ".",
                            PrimaryKeyId = projectId,
                            ButtonName = buttonName,
                            SourceType = "Projects"
                        };
                        notifications.Add(notification);
                    }
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
                    else
                    {
                        statusCode = projectResult?.StatusCode;
                        statusText = projectResult?.StatusText;
                    }
                }
                return Ok(new
                {
                    StatusCode = statusCode,
                    StatusText = statusText,
                    ProjectId = projectId
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/ApproveOrRejectProject", JsonConvert.SerializeObject(pApproveOrRejectProject));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                ProjectId = 0
            });
        }
        #endregion

        #region Assign Project SPOC For Project
        [HttpPost]
        [Route("AssignProjectSPOCForProject")]
        public async Task<IActionResult> AssignProjectSPOCForProject(UpdateProjectSPOC pUpdateProjectSPOC)
        {
            //string statusText = "", statusCode = "";
            int projectId = 0;
            try
            {
                using var assignProjectSPOCForProjectClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectSPOCResponse = await assignProjectSPOCForProjectClient.PostAsJsonAsync("Projects/AssignProjectSPOCForProject", pUpdateProjectSPOC);
                if (projectSPOCResponse?.IsSuccessStatusCode == true)
                {
                    var projectResult = projectSPOCResponse.Content.ReadAsAsync<SuccessData>();
                    projectId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(projectResult?.Result?.Data));
                    SendNotificationToSPOC(projectId, pUpdateProjectSPOC);
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = projectResult?.Result?.StatusText,
                       ProjectId = projectId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/AssignProjectSPOCForProject", JsonConvert.SerializeObject(pUpdateProjectSPOC));
                
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ProjectId = projectId
            });
        }
        #endregion
        #region Send notification to SPOC
        [NonAction]
        public async Task<string> SendNotificationToSPOC(int projectId, UpdateProjectSPOC pUpdateProjectSPOC)
        {
            string mail = "";
            
            ProjectsViewModel projectDetail = new();
            try
            {
                
                using var projectClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectResponse = await projectClient.GetAsync("Projects/GetProjectDetailById?pProjectID=" + projectId);
                if (projectResponse?.IsSuccessStatusCode == true)
                {
                    var result = projectResponse.Content.ReadAsAsync<SuccessData>();
                    projectDetail = JsonConvert.DeserializeObject<ProjectsViewModel>(JsonConvert.SerializeObject(result.Result.Data));
                }
                if (projectDetail?.ProjectDetails != null)
                {
                    string accountName = GetAccountNameById(new List<int?> { projectDetail.ProjectDetails.AccountId }).Result.Where(x => x.AccountId == projectDetail.ProjectDetails.AccountId).Select(x => x.AccountName).FirstOrDefault();
                    List<Notifications> notifications = new();
                    Notifications notification = new()
                    {
                        CreatedBy = (int)pUpdateProjectSPOC.ModifiedBy,
                        CreatedOn = DateTime.UtcNow,
                        FromId = (int)pUpdateProjectSPOC.ModifiedBy,
                        ToId = (int)pUpdateProjectSPOC.ProjectSpoc,
                        MarkAsRead = false,
                        NotificationSubject = "Congratulations! As the SPOC for " + projectDetail.ProjectDetails.ProjectName + " for customer " + accountName + ", now you can assign contributor(s) or resource(s).",
                        NotificationBody = "Congratulations! As the SPOC for " + projectDetail.ProjectDetails.ProjectName + " for customer " + accountName + ", now you can assign contributor(s) or resource(s).",
                        PrimaryKeyId = pUpdateProjectSPOC.ProjectId,
                        ButtonName = "Assign Contributors",
                        SourceType = "Projects"
                    };
                    notifications.Add(notification);
                    using var notificationClient = new HttpClient
                    {
                        BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                    };
                    HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                    var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();
                    
                }

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/SendNotificationToSPOC", JsonConvert.SerializeObject(pUpdateProjectSPOC));
            }

            return mail;
        }
        #endregion

        #region Get Change Request Detail By Id
        [HttpGet]
        [Route("GetChangeRequestDetailById")]
        public async Task<IActionResult> GetChangeRequestDetailById(int pChangeRequestID)
        {
            CRViewModel CRView = new();
            try
            {
                var result = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetChangeRequestDetailById") + pChangeRequestID);
                CRView = JsonConvert.DeserializeObject<CRViewModel>(JsonConvert.SerializeObject(result.Data));
                if (CRView?.CRDetails?.ChangeRequestId > 0)
                {
                    //Get Status name
                    if (!string.IsNullOrEmpty(CRView.CRDetails.CRStatusCode))
                    {
                        CRView.CRDetails.CRStatusName = await GetStatusNameByCode(CRView.CRDetails.CRStatusCode);
                    }
                    using (var documentClient = new HttpClient())
                    {
                        List<int> crSourceId = new();
                        crSourceId.Add(CRView.CRDetails.ChangeRequestId);
                        SourceDocuments sourceDocuments = new();
                        sourceDocuments.SourceId = crSourceId;
                        sourceDocuments.SourceType = new List<string> { _configuration.GetValue<string>("ChangeRequestSourceType") };
                        documentClient.BaseAddress = new Uri(_notificationBaseURL);
                        HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/GetDocumentBySourceIdAndType", sourceDocuments);
                        if (documentResponse?.IsSuccessStatusCode == true)
                        {
                            CRView.CRDetails.ListOfDocuments = new List<DocumentDetails>();
                            var documentResult = documentResponse.Content.ReadAsAsync<SuccessData>();
                            List<SupportingDocuments> documentList = JsonConvert.DeserializeObject<List<SupportingDocuments>>(JsonConvert.SerializeObject(documentResult.Result.Data));
                            CRView.CRDetails.ListOfDocuments = documentList.Where(x => x.SourceId == CRView.CRDetails.ChangeRequestId && x.SourceType == _configuration.GetValue<string>("ChangeRequestSourceType")).Select(x => new DocumentDetails { DocumentId = x.DocumentId, DocumentName = x.DocumentName, DocumentSize = x.DocumentSize, DocumentCategory = x.DocumentCategory, IsApproved = x.IsApproved, DocumentType = x.DocumentType }).ToList();
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    ChangeRequestView = CRView?.CRDetails,
                    CRDetailComments = CRView?.CRCommentsList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetChangeRequestDetailById", Convert.ToString(pChangeRequestID));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    ChangeRequestView = CRView?.CRDetails,
                    CRDetailComments = CRView?.CRCommentsList
                });
            }
        }
        #endregion

        #region Insert Or Update Change Request Detail
        [HttpPost]
        [Route("InsertOrUpdateChangeRequestDetail")]
        public async Task<IActionResult> InsertOrUpdateChangeRequestDetail(AddCRView AddCRView)
        {
            try
            {
                ChangeRequestView pChangeRequestDetails = AddCRView?.ChangeRequest;
                int CRId = 0;
                string statusCode = "SUCCESS", statusText = "SUCCESS";
                using var projectClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectResponse = await projectClient.PostAsJsonAsync("Projects/InsertOrUpdateChangeRequestDetail", pChangeRequestDetails);
                if (projectResponse?.IsSuccessStatusCode == true)
                {
                    var projectResult = projectResponse.Content.ReadAsAsync<SuccessData>();
                    CRId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(projectResult?.Result?.Data));
                    if (CRId > 0)
                    {
                        SuccessData pResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectById") + pChangeRequestDetails.ProjectId);
                        ProjectDetails projectDet = JsonConvert.DeserializeObject<ProjectDetails>(JsonConvert.SerializeObject(pResult.Data));
                        string AccountName = GetAccountNameById(new List<int?> { projectDet.AccountId }).Result.Where(x => x.AccountId == projectDet.AccountId).Select(x => x.AccountName).FirstOrDefault();
                        List<Notifications> notifications = new();
                        Notifications notification = new()
                        {
                            CreatedBy = pChangeRequestDetails.CreatedBy == null ? 0 : (int)pChangeRequestDetails.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            FromId = pChangeRequestDetails.CreatedBy == null ? 0 : (int)pChangeRequestDetails.CreatedBy,
                            ToId = pChangeRequestDetails.CreatedBy == null ? 0 : (int)pChangeRequestDetails.CreatedBy,
                            MarkAsRead = false,
                            NotificationSubject = "Change Request " + pChangeRequestDetails.ChangeRequestName + " for Project " + projectDet.ProjectName + " [ Customer " + AccountName + "] has been sent for approval.",
                            NotificationBody = "Change Request " + pChangeRequestDetails.ChangeRequestName + " for Project " + projectDet.ProjectName + " [ Customer " + AccountName + "] has been sent for approval.",
                            PrimaryKeyId = pChangeRequestDetails.ProjectId,
                            ButtonName = "View Change Request",
                            SourceType = "Projects"
                        };
                        notifications.Add(notification);
                        if (projectDet.EngineeringLeadId != null)
                        {
                            notification.ToId = (int)projectDet.EngineeringLeadId;
                            notifications.Add(notification);
                        }

                        notification = new()
                        {
                            CreatedBy = pChangeRequestDetails.CreatedBy == null ? 0 : (int)pChangeRequestDetails.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            FromId = pChangeRequestDetails.CreatedBy == null ? 0 : (int)pChangeRequestDetails.CreatedBy,
                            ToId = projectDet.FinanceManagerId == null ? 0 : (int)projectDet.FinanceManagerId,
                            MarkAsRead = false,
                            NotificationSubject = "You have been requested to review the Change Request " + pChangeRequestDetails.ChangeRequestName + " for Project " + projectDet.ProjectName + " [ Customer :" + AccountName + "].",
                            NotificationBody = "You have been requested to review the Change Request " + pChangeRequestDetails.ChangeRequestName + " for Project " + projectDet.ProjectName + " [ Customer :" + AccountName + "].",
                            PrimaryKeyId = pChangeRequestDetails.ProjectId,
                            ButtonName = "Review Change Request",
                            SourceType = "Projects"
                        };
                        notifications.Add(notification);
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
                        if (AddCRView?.ListOfDocuments?.Count > 0)
                        {
                            SupportingDocumentsView supportingDocuments = new();
                            supportingDocuments.ListOfDocuments = new();
                            supportingDocuments.SourceId = CRId;
                            supportingDocuments.SourceType = _configuration.GetValue<string>("ChangeRequestSourceType");
                            supportingDocuments.BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory");
                            supportingDocuments.CreatedBy = pChangeRequestDetails?.CreatedBy == null ? 0 : (int)pChangeRequestDetails?.CreatedBy;
                            //Add supporting documents
                            if (!string.IsNullOrEmpty(supportingDocuments.BaseDirectory))
                            {
                                //Create base directory
                                if (!Directory.Exists(supportingDocuments.BaseDirectory))
                                {
                                    Directory.CreateDirectory(supportingDocuments.BaseDirectory);
                                }
                                //Create source type directory
                                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType)))
                                {
                                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType));
                                }
                                //Create accountId directory
                                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString())))
                                {
                                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString()));
                                }
                            }
                            string directoryPath = Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.SourceId.ToString());
                            List<DocumentDetails> docList = new();
                            foreach (var item in AddCRView?.ListOfDocuments)
                            {
                                string documentPath = Path.Combine(directoryPath, item.DocumentName);
                                if (!System.IO.File.Exists(item.DocumentName) && item.DocumentSize > 0)
                                {
                                    if (item.DocumentAsBase64.Contains(","))
                                    {
                                        item.DocumentAsBase64 = item.DocumentAsBase64.Substring(item.DocumentAsBase64.IndexOf(",") + 1);
                                    }
                                    item.DocumentAsByteArray = Convert.FromBase64String(item.DocumentAsBase64);
                                    using (Stream fileStream = new FileStream(documentPath, FileMode.Create))
                                    {
                                        fileStream.Write(item.DocumentAsByteArray, 0, item.DocumentAsByteArray.Length);
                                    }
                                }
                                DocumentDetails docDetails = new()
                                {
                                    IsApproved = false,
                                    DocumentCategory = item.DocumentCategory,
                                    DocumentSize = item.DocumentSize,
                                    DocumentName = item.DocumentName,
                                    DocumentType = Path.GetExtension(item.DocumentName)
                                };
                                docList.Add(docDetails);
                            }
                            supportingDocuments.ListOfDocuments = docList;
                            using var documentClient = new HttpClient
                            {
                                BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                            };
                            HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/AddSupportingDocuments", supportingDocuments);
                            var documentResult = documentResponse.Content.ReadAsAsync<SuccessData>();
                            if (documentResponse?.IsSuccessStatusCode == false)
                            {
                                statusCode = "FAILURE";
                                statusText = documentResult?.Result?.StatusText;
                            }
                        }
                    }
                    return Ok(new
                    {
                        StatusCode = statusCode,
                        StatusText = projectResult?.Result?.StatusText,
                        ChangeRequestId = CRId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = string.Empty,
                        ChangeRequestId = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/InsertOrUpdateChangeRequestDetail", JsonConvert.SerializeObject(AddCRView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    ChangeRequestId = 0
                });
            }
        }
        #endregion

        #region Approve or Reject the Change Request
        [HttpPost]
        [Route("ApproveOrRejectChangeRequest")]
        public async Task<IActionResult> ApproveOrRejectChangeRequest(ApproveOrRejectChangeRequest pApproveOrRejectCR)
        {
            string statusCode = "", statusText = "";
            int projectId = 0, CRId = 0;
            try
            {
                SuccessData _CRResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetChangeRequestById") + pApproveOrRejectCR.ChangeRequestId);
                ChangeRequest CRDet = JsonConvert.DeserializeObject<ChangeRequest>(JsonConvert.SerializeObject(_CRResult.Data));
                SuccessData pResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectById") + pApproveOrRejectCR.ProjectId);
                ProjectDetails projectDet = JsonConvert.DeserializeObject<ProjectDetails>(JsonConvert.SerializeObject(pResult.Data));
                if (projectDet != null)
                {
                    projectId = projectDet.ProjectId;
                    pApproveOrRejectCR.ProjectId = projectId;
                    pApproveOrRejectCR.EngineeringLeadId = projectDet.EngineeringLeadId == null ? 0 : (int)projectDet.EngineeringLeadId;
                    pApproveOrRejectCR.FinanceManagerId = projectDet.FinanceManagerId == null ? 0 : (int)projectDet.FinanceManagerId;
                    var CRResult = await _client.PostAsJsonAsync(pApproveOrRejectCR, _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:ApproveOrRejectChangeRequest"));
                    CRId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(CRResult?.Data));
                    statusCode = CRResult.StatusCode;
                    statusText = CRResult.StatusText;
                    if (CRId > 0 && pApproveOrRejectCR.ChangeRequestStatus != "Action Required")
                    {
                        using (var documentClient = new HttpClient())
                        {
                            SourceDocuments sourceDocuments = new();
                            sourceDocuments.SourceId = new List<int> { pApproveOrRejectCR.ChangeRequestId };
                            sourceDocuments.SourceType = new List<string> { _configuration.GetValue<string>("ChangeRequestSourceType") };
                            documentClient.BaseAddress = new Uri(_notificationBaseURL);
                            HttpResponseMessage appDocumentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/ApprovedDocumentsBySourceIdAndType", sourceDocuments);
                            if (appDocumentResponse?.IsSuccessStatusCode == false)
                            {
                                var appDocumentResult = appDocumentResponse.Content.ReadAsAsync<SuccessData>();
                                statusCode = "FAILURE";
                                statusText = appDocumentResult?.Result?.StatusText;
                            }
                        }
                    }
                }
                if (CRId > 0)
                {
                    string ApproveOrReject = "approved", financeManager = "", buttonName = "View Change Request";
                    int primaryKeyId = projectId;
                    int toId = projectDet.EngineeringLeadId == null ? 0 : (int)projectDet.EngineeringLeadId;
                    List<EmployeeName> listEmployeeName = new();
                    List<int?> listOfEmployeeId = new();
                    List<Notifications> notifications = new();
                    string AccountName = GetAccountNameById(new List<int?> { projectDet.AccountId }).Result.Where(x => x.AccountId == projectDet.AccountId).Select(x => x.AccountName).FirstOrDefault();
                    listOfEmployeeId.Add(projectDet.FinanceManagerId);
                    listEmployeeName = GetEmployeeNameById(listOfEmployeeId).Result;
                    if (listEmployeeName?.Count > 0) financeManager = listEmployeeName[0].EmployeeFullName;
                    if (pApproveOrRejectCR.ChangeRequestStatus == "Action Required")
                    {
                        ApproveOrReject = "requested changes in";
                        buttonName = "Edit Change Request";
                        toId = (int)CRDet.CreatedBy;
                        primaryKeyId = pApproveOrRejectCR.ChangeRequestId;
                    }
                    Notifications notification = new()
                    {
                        CreatedBy = projectDet.FinanceManagerId == null ? 0 : (int)projectDet.FinanceManagerId,
                        CreatedOn = DateTime.UtcNow,
                        FromId = projectDet.FinanceManagerId == null ? 0 : (int)projectDet.FinanceManagerId,
                        ToId = CRDet.CreatedBy == null ? 0 : (int)CRDet.CreatedBy,
                        MarkAsRead = false,
                        NotificationSubject = financeManager + " has " + ApproveOrReject + " Change Request " + CRDet.ChangeRequestName + " for project " + projectDet.ProjectName + "[ Customer " + AccountName + " ].",
                        NotificationBody = financeManager + " has " + ApproveOrReject + " Change Request " + CRDet.ChangeRequestName + " for project " + projectDet.ProjectName + "[ Customer " + AccountName + " ].",
                        PrimaryKeyId = primaryKeyId,
                        ButtonName = buttonName,
                        SourceType = "Projects"
                    };
                    notifications.Add(notification);
                    if (pApproveOrRejectCR.ChangeRequestStatus != "Action Required")
                    {
                        notification = new()
                        {
                            CreatedBy = projectDet.FinanceManagerId == null ? 0 : (int)projectDet.FinanceManagerId,
                            CreatedOn = DateTime.UtcNow,
                            FromId = projectDet.FinanceManagerId == null ? 0 : (int)projectDet.FinanceManagerId,
                            ToId = projectDet.EngineeringLeadId == null ? 0 : (int)projectDet.EngineeringLeadId,
                            MarkAsRead = false,
                            NotificationSubject = "You have been requested to assign a SPOC to project " + projectDet.ProjectName + " customer " + AccountName + ".",
                            NotificationBody = "You have been requested to assign a SPOC to project " + projectDet.ProjectName + " customer " + AccountName + ".",
                            PrimaryKeyId = projectId,
                            ButtonName = buttonName,
                            SourceType = "Projects"
                        };
                        notifications.Add(notification);
                    }
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
                return Ok(new
                {
                    StatusCode = statusCode,
                    StatusText = statusText,
                    ProjectId = projectId
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/ApproveOrRejectChangeRequest", JsonConvert.SerializeObject(pApproveOrRejectCR));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                ProjectId = 0
            });
        }
        #endregion

        #region Add Associates For Additional Resource
        [HttpPost]
        [Route("AddAssociatesForAdditionalresource")]
        public async Task<IActionResult> AddAssociatesForAdditionalresource(ResourceAllocationList pResourceAllocation)
        {
            string statusText = "", statusCode = "";
            ProjectsViewModel projectDetail = new();
            try
            {
                int ProjectId = 0;
                using var resourceAllocationClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectResourceAllocationResponse = await resourceAllocationClient.PostAsJsonAsync("Projects/AssignAssociatesForResourceAllocation", pResourceAllocation);
                if (projectResourceAllocationResponse?.IsSuccessStatusCode == true)
                {
                    var projectResult = projectResourceAllocationResponse.Content.ReadAsAsync<SuccessData>();
                    ProjectId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(projectResult?.Result?.Data));
                    if (ProjectId > 0)
                    {
                        using var projectClient = new HttpClient
                        {
                            BaseAddress = new Uri(_projectBaseURL)
                        };
                        HttpResponseMessage projectResponse = await projectClient.GetAsync("Projects/GetProjectDetailById?pProjectID=" + ProjectId);
                        if (projectResponse?.IsSuccessStatusCode == true)
                        {
                            var result = projectResponse.Content.ReadAsAsync<SuccessData>();
                            projectDetail = JsonConvert.DeserializeObject<ProjectsViewModel>(JsonConvert.SerializeObject(result.Result.Data));
                            if (projectDetail?.ProjectDetails != null)
                            {
                                string accountName = GetAccountNameById(new List<int?> { projectDetail.ProjectDetails.AccountId }).Result.Where(x => x.AccountId == projectDetail.ProjectDetails.AccountId).Select(x => x.AccountName).FirstOrDefault();
                                List<Notifications> notifications = new();
                                Notifications notification = new()
                                {
                                    CreatedBy = (int)pResourceAllocation.ModifiedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    FromId = (int)pResourceAllocation.ModifiedBy,
                                    ToId = (int)pResourceAllocation.EmployeeId,
                                    MarkAsRead = false,
                                    NotificationSubject = "Congratulations! You have been assigned as a contributor or resource to project " + projectDetail.ProjectDetails.ProjectName + " for account " + accountName + ".",
                                    NotificationBody = "Congratulations! You have been assigned as a contributor or resource to project " + projectDetail.ProjectDetails.ProjectName + " for account " + accountName + ".",
                                    PrimaryKeyId = ProjectId,
                                    ButtonName = "View Project",
                                    SourceType = "Projects"
                                };
                                notifications.Add(notification);
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
                                else
                                {
                                    statusCode = projectResult?.Result?.StatusCode;
                                    statusText = projectResult?.Result?.StatusText;
                                }
                            }
                        }
                    }
                    else
                    {
                        statusCode = projectResult?.Result?.StatusCode;
                        statusText = projectResult?.Result?.StatusText;
                    }
                    return Ok(new
                    {
                        StatusCode = statusCode,
                        StatusText = statusText,
                        ProjectId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/AddAssociatesForAdditionalresource", JsonConvert.SerializeObject(pResourceAllocation));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                ProjectId = 0
            });
        }
        #endregion

        #region Assign Associates For Resource Allocation
        [HttpPost]
        [Route("AssignAssociatesForResourceAllocation")]
        public async Task<IActionResult> AssignAssociatesForResourceAllocation(UpdateResourceAllocation pUpdateResourceAllocation)
        {
            string statusText = "", statusCode = "";
            
            try
            {
                int ProjectId = 0;
                using var resourceAllocationClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectResourceAllocationResponse = await resourceAllocationClient.PostAsJsonAsync("Projects/AssignAssociatesForResourceAllocation", pUpdateResourceAllocation);
                if (projectResourceAllocationResponse?.IsSuccessStatusCode == true)
                {
                    var projectResult = projectResourceAllocationResponse.Content.ReadAsAsync<SuccessData>();
                    ProjectId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(projectResult?.Result?.Data));
                    if (ProjectId > 0)
                    {
                        SendNotificationToAssociate(ProjectId, pUpdateResourceAllocation);
                        statusText = projectResult?.Result?.StatusText;
                        statusCode = "SUCCESS";
                    }
                    else
                    {
                        statusCode = projectResult?.Result?.StatusCode;
                        statusText = projectResult?.Result?.StatusText;
                    }
                    return Ok(new
                    {
                        StatusCode = statusCode,
                        StatusText = statusText,
                        ProjectId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/AssignAssociatesForResourceAllocation", JsonConvert.SerializeObject(pUpdateResourceAllocation));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                ProjectId = 0
            });
        }
        #endregion
        #region Send notification to associate
        [NonAction]
        public async Task<string> SendNotificationToAssociate(int ProjectId, UpdateResourceAllocation pUpdateResourceAllocation)
        {
            string mail = "";
            ProjectsViewModel projectDetail = new();
            try
            {
                using var projectClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectResponse = await projectClient.GetAsync("Projects/GetProjectDetailById?pProjectID=" + ProjectId);
                if (projectResponse?.IsSuccessStatusCode == true)
                {
                    var result = projectResponse.Content.ReadAsAsync<SuccessData>();
                    projectDetail = JsonConvert.DeserializeObject<ProjectsViewModel>(JsonConvert.SerializeObject(result.Result.Data));
                    if (projectDetail?.ProjectDetails != null)
                    {
                        string accountName = GetAccountNameById(new List<int?> { projectDetail.ProjectDetails.AccountId }).Result.Where(x => x.AccountId == projectDetail.ProjectDetails.AccountId).Select(x => x.AccountName).FirstOrDefault();
                        List<Notifications> notifications = new();
                        Notifications notification = new()
                        {
                            CreatedBy = (int)pUpdateResourceAllocation.ModifiedBy,
                            CreatedOn = DateTime.UtcNow,
                            FromId = (int)pUpdateResourceAllocation.ModifiedBy,
                            ToId = (int)pUpdateResourceAllocation.EmployeeId,
                            MarkAsRead = false,
                            NotificationSubject = "Congratulations! You have been assigned as a contributor or resource to project " + projectDetail.ProjectDetails.ProjectName + " for customer " + accountName + ".",
                            NotificationBody = "Congratulations! You have been assigned as a contributor or resource to project " + projectDetail.ProjectDetails.ProjectName + " for customer " + accountName + ".",
                            PrimaryKeyId = ProjectId,
                            ButtonName = "View Project",
                            SourceType = "Projects"
                        };
                        notifications.Add(notification);
                        using var notificationClient = new HttpClient
                        {
                            BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                        };
                        HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                        var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();
                        
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "ExitManagement/SendNotificationToSPOC", JsonConvert.SerializeObject(pUpdateResourceAllocation));
            }

            return mail;
        }
        #endregion

        #region Remove Associates
        [HttpPost]
        [Route("RemoveAssociates")]
        public async Task<IActionResult> RemoveAssociates(UpdateResourceAllocation pUpdateResourceAllocation)
        {
            try
            {
                int ProjectId = 0;
                using var projectClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectResponse = await projectClient.PostAsJsonAsync("Projects/RemoveAssociates", pUpdateResourceAllocation);
                if (projectResponse?.IsSuccessStatusCode == true)
                {
                    var projectResult = projectResponse.Content.ReadAsAsync<SuccessData>();
                    ProjectId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(projectResult?.Result?.Data));
                    return Ok(new
                    {
                        projectResult?.Result?.StatusCode,
                        projectResult?.Result?.StatusText,
                        ProjectId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = string.Empty,
                        ProjectId = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/RemoveAssociates", JsonConvert.SerializeObject(pUpdateResourceAllocation));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    ProjectId = 0
                });
            }
        }
        #endregion

        #region Delete Resource Allocation
        [HttpDelete]
        [Route("DeleteResourceAllocation")]
        public async Task<IActionResult> DeleteResourceAllocation(int ResourceAllocationId)
        {
            try
            {
                using var projectClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectResponse = await projectClient.DeleteAsync("Projects/DeleteResourceAllocation?ResourceAllocationId=" + ResourceAllocationId);
                if (projectResponse?.IsSuccessStatusCode == true)
                {
                    var projectResult = projectResponse.Content.ReadAsAsync<SuccessData>();
                    return Ok(new
                    {
                        projectResult?.Result?.StatusCode,
                        projectResult?.Result?.StatusText
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/DeleteResourceAllocation", Convert.ToString(ResourceAllocationId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg
                });
            }
        }
        #endregion

        #region Get CR master data
        [HttpGet]
        [Route("GetCRMasterData")]
        public async Task<IActionResult> GetCRMasterData()
        {
            List<ChangeRequestType> changeRequestTypes = new();
            List<ProjectListView> projectListViews = new();
            List<AccountNames> lstAccountName = new();
            List<EmployeeName> lstEmployeeName = new();
            string statusText = "";
            try
            {
                using var CRClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage CRResponse = await CRClient.GetAsync("Projects/GetAllChangeRequestType");
                if (CRResponse?.IsSuccessStatusCode == true)
                {
                    var result = CRResponse.Content.ReadAsAsync<SuccessData>();
                    changeRequestTypes = JsonConvert.DeserializeObject<List<ChangeRequestType>>(JsonConvert.SerializeObject(result.Result.Data));
                }
                HttpResponseMessage projectResponse = await CRClient.GetAsync("Projects/GetAllActiveProjects");
                if (projectResponse?.IsSuccessStatusCode == true)
                {
                    var result = projectResponse.Content.ReadAsAsync<SuccessData>();
                    projectListViews = JsonConvert.DeserializeObject<List<ProjectListView>>(JsonConvert.SerializeObject(result.Result.Data));
                }
                if (projectListViews?.Count > 0)
                {
                    var lstAccountId = projectListViews.Select(x => x.AccountId).ToList();
                    lstAccountName = GetAccountNameById(lstAccountId).Result;
                    projectListViews.ForEach(x => x.AccountName = lstAccountName.Where(y => y.AccountId == x.AccountId).Select(z => z.AccountName).FirstOrDefault());
                    var lstEmployeeId = projectListViews.Select(x => (int?)x.SPOCId).ToList();
                    lstEmployeeName = GetEmployeeNameById(lstEmployeeId).Result;
                    projectListViews.ForEach(x => x.SPOC = lstEmployeeName.Where(y => y.EmployeeId == x.SPOCId).Select(z => z.EmployeeFullName).FirstOrDefault());
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    ListofChangeRequestType = changeRequestTypes,
                    ListofActiveProjects = projectListViews
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetCRMasterData");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                ListofChangeRequestType = changeRequestTypes,
                ListofActiveProjects = projectListViews
            });
        }
        #endregion

        #region Get employee name 
        [NonAction]
        public async Task<List<EmployeeName>> GetEmployeeNameById(List<int?> lstEmployeeId)
        {
            List<EmployeeName> lstEmpName = new();
            try
            {
                using var employeeClient = new HttpClient
                {
                    BaseAddress = new Uri(_employeeBaseURL)
                };
                var lstEmpId = lstEmployeeId.Where(x => x != null && x != 0).Select(x => x).Distinct().ToList();
                HttpResponseMessage employeeResponse = await employeeClient.PostAsJsonAsync("Employee/GetEmployeeNameById", lstEmpId);
                if (employeeResponse?.IsSuccessStatusCode == true)
                {
                    var employeeResult = employeeResponse.Content.ReadAsAsync<SuccessData>();
                    lstEmpName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeResult.Result.Data));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetEmployeeNameById", JsonConvert.SerializeObject(lstEmployeeId));
            }
            return lstEmpName;
        }
        #endregion

        #region Get account name by id
        [NonAction]
        public async Task<List<AccountNames>> GetAccountNameById(List<int?> lstAccountId)
        {
            List<AccountNames> lstAccName = new();
            try
            {
                using var accountNameClient = new HttpClient
                {
                    BaseAddress = new Uri(_accountsBaseURL)
                };
                var lstAccId = lstAccountId.Where(x => x != null && x != 0).Select(x => x).Distinct().ToList();
                HttpResponseMessage accountNameResponse = await accountNameClient.PostAsJsonAsync("Accounts/GetAccountNameById", lstAccId);
                if (accountNameResponse?.IsSuccessStatusCode == true)
                {
                    var accountNameResult = accountNameResponse.Content.ReadAsAsync<SuccessData>();
                    lstAccName = JsonConvert.DeserializeObject<List<AccountNames>>(JsonConvert.SerializeObject(accountNameResult.Result.Data));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetAccountNameById", JsonConvert.SerializeObject(lstAccountId));
            }
            return lstAccName;
        }
        #endregion

        #region Get role name by id
        [NonAction]
        public async Task<List<RoleName>> GetRoleNameById(List<int?> lstRoleId)
        {
            List<RoleName> lstRoleName = new();
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_employeeBaseURL)
                };
                var lstroleId = lstRoleId.Where(x => x != null && x != 0).Select(x => x).Distinct().ToList();
                HttpResponseMessage response = await client.PostAsJsonAsync("Employee/GetRoleNameById", lstroleId);
                if (response?.IsSuccessStatusCode == true)
                {
                    var result = response.Content.ReadAsAsync<SuccessData>();
                    lstRoleName = JsonConvert.DeserializeObject<List<RoleName>>(JsonConvert.SerializeObject(result.Result.Data));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetRoleNameById", JsonConvert.SerializeObject(lstRoleId));
            }
            return lstRoleName;
        }
        #endregion

        #region Get Employees By Role
        private async Task<List<EmployeeList>> GetEmployeesByRole(string pRole)
        {
            List<EmployeeList> listOfEmployees = new();
            try
            {
                using (var listOfEmployeesClient = new HttpClient())
                {
                    if (pRole == "") pRole = "Account Manager";
                    listOfEmployeesClient.BaseAddress = new Uri(_employeeBaseURL);
                    HttpResponseMessage listOfEmployeesResponse = await listOfEmployeesClient.GetAsync("Employee/GetEmployeeList?pRole=" + pRole);
                    if (listOfEmployeesResponse?.IsSuccessStatusCode == true)
                    {
                        var listOfEmployeesResult = listOfEmployeesResponse.Content.ReadAsAsync<SuccessData>();
                        listOfEmployees = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Result.Data));
                        if (listOfEmployees?.Count > 0)
                        {
                            return listOfEmployees;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetEmployeesByRole", pRole);
            }
            return listOfEmployees;
        }
        #endregion

        #region check team members by resource id
        [HttpGet]
        [Route("CheckTeamMembersByResourceId")]
        public async Task<IActionResult> CheckTeamMembersByResourceId(int resourceId)
        {
            string statusTest = "";
            try
            {
                using var projectClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                HttpResponseMessage projectResponse = await projectClient.GetAsync("Projects/CheckTeamMembersByResourceId?resourceId=" + resourceId);
                if (projectResponse?.IsSuccessStatusCode == true)
                {
                    var projectResult = projectResponse.Content.ReadAsAsync<SuccessData>();
                    return Ok(new
                    {
                        projectResult?.Result?.StatusCode,
                        projectResult?.Result?.StatusText,
                        Data = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(projectResult?.Result?.Data))
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/CheckTeamMembersByResourceId", Convert.ToString(resourceId));
                statusTest = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusTest,
                Data = false
            });
        }
        #endregion

        #region Get BU Head Name For Project
        [NonAction]
        public async Task<string> GetBUHeadNameForProject(int? departmentHeadId)
        {
            string departmentHeadName = "";
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_employeeBaseURL)
                };
                HttpResponseMessage response = await client.GetAsync("Employee/GetBUHeadNameForProject?departmentHeadId=" + departmentHeadId);
                if (response?.IsSuccessStatusCode == true)
                {
                    var result = response.Content.ReadAsAsync<SuccessData>();
                    List<BUAccountableForProject> bUAccountableForProjects = JsonConvert.DeserializeObject<List<BUAccountableForProject>>(JsonConvert.SerializeObject(result.Result.Data));
                    if (bUAccountableForProjects?.Count > 0)
                    {
                        departmentHeadName = bUAccountableForProjects?[0].DepartmentHead;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetBUHeadNameForProject", Convert.ToString(departmentHeadId));
            }
            return departmentHeadName;
        }
        #endregion

        #region Get Status name by code
        [NonAction]
        public async Task<string> GetStatusNameByCode(string pStatusCode)
        {
            string _statusName = "";
            try
            {
                var statusResult = await _client.GetAsync(_configuration.GetValue<string>("ApplicationURL:Notification:BaseURL"), _configuration.GetValue<string>("ApplicationURL:Notification:GetStatusByCode") + pStatusCode);
                StatusViewModel status = JsonConvert.DeserializeObject<StatusViewModel>(JsonConvert.SerializeObject(statusResult.Data));
                if (!string.IsNullOrEmpty(status?.StatusCode))
                {
                    _statusName = status?.StatusName;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/GetStatusNameByCode", pStatusCode);
            }
            return _statusName;
        }
        #endregion

        #region Remove Project Logo
        [HttpPost]
        [Route("RemoveProjectLogo")]
        public async Task<IActionResult> RemoveProjectLogo(RemoveProjectLogo removeProjectLogo)
        {
            try
            {
                using var _logoClient = new HttpClient
                {
                    BaseAddress = new Uri(_projectBaseURL)
                };
                var logoResponse = await _logoClient.PostAsJsonAsync("Projects/RemoveProjectLogo", removeProjectLogo);
                var logoResult = logoResponse.Content.ReadAsAsync<SuccessData>();
                if (logoResult != null)
                {
                    return Ok(new
                    {
                        logoResult?.Result?.StatusCode,
                        logoResult?.Result?.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Projects/RemoveProjectLogo", Convert.ToString(removeProjectLogo));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion
    }
}