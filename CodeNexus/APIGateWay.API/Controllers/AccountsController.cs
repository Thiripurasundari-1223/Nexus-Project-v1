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
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Accounts;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;
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
    public class AccountsController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly HTTPClient _client;
        private readonly string _projectBaseURL = string.Empty;
        private readonly string _timesheetBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _notificationBaseURL = string.Empty;
        private readonly string _accountsBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";
        private readonly CommonFunction _commonFunction;

        #region Constructor
        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new();
            _projectBaseURL = _configuration.GetValue<string>("ApplicationURL:Projects:BaseURL");
            _timesheetBaseURL = _configuration.GetValue<string>("ApplicationURL:Timesheet:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _notificationBaseURL = _configuration.GetValue<string>("ApplicationURL:Notifications");
            _accountsBaseURL = _configuration.GetValue<string>("ApplicationURL:Accounts:BaseURL");
            _commonFunction = new CommonFunction(configuration);
        }
        #endregion

        #region Bulk Insert Account
        [HttpPost]
        [Route("BulkInsertAccount")]
        public async Task<IActionResult> BulkInsertAccount(IFormFile uploadFile)
        {
            try
            {
                if (uploadFile.Length > 0)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = new MemoryStream())
                    {
                        uploadFile.CopyTo(stream);
                        ImportExcelView import = new ImportExcelView();
                        import.Base64Format = Convert.ToBase64String(stream.ToArray());
                        var employeeDetails = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesList"));
                        import.EmployeeDetails = JsonConvert.DeserializeObject<List<EmployeeDetail>>(JsonConvert.SerializeObject(employeeDetails.Result.Data));
                        var result = await _client.PostAsJsonAsync(import, _accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:BulkInsertAccount"));
                        return Ok(new
                        {
                            StatusCode = "Success",
                            StatusText = "Completed Successfully",
                            Data = result?.Data
                        });
                    }

                }
                return Ok(new
                {
                    StatusCode = "Failed",
                    StatusText = "Not a valid File",
                    Data = 0
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/BulkInsertAccount");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Insert or Update Account
        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("InsertAndUpdateAccount")]
        public async Task<IActionResult> InsertAndUpdateAccount(AddAccountView AddAccountView)
        {
            string statusText = "", statusCode = "";
            try
            {
                AccountDetails accountDetails = AddAccountView?.AccountDetails;
                var financeManagerDetail = new FinanceManagerDetails();
                if (accountDetails?.IsDraft == false)
                {
                    var managerResult = await _client.PostAsJsonAsync(accountDetails,_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetfinanceManagerDetails"));
                    financeManagerDetail=JsonConvert.DeserializeObject<FinanceManagerDetails>(JsonConvert.SerializeObject(managerResult?.Data));
                    accountDetails.FinanceManagerId = financeManagerDetail?.FinanceId;
                }
                var result = await _client.PostAsJsonAsync(AddAccountView, _accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:InsertAndUpdateAccount"));
                if (result != null && (int)result?.Data > 0)
                {
                    int AccountId = (int)result.Data;
                    if (accountDetails?.IsDraft == false)
                    {
                        List<Notifications> notifications = new();
                        Notifications notification = new()
                        {
                            CreatedBy = accountDetails?.CreatedBy == null ? 0 : (int)accountDetails?.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            FromId = accountDetails?.CreatedBy == null ? 0 : (int)accountDetails?.CreatedBy,
                            ToId = accountDetails?.CreatedBy == null ? 0 : (int)accountDetails?.CreatedBy,
                            MarkAsRead = false,
                            NotificationSubject = "Customer " + accountDetails?.AccountName + " has been sent for approval",
                            NotificationBody = "Customer " + accountDetails?.AccountName + " has been sent for approval",
                            PrimaryKeyId = AccountId,
                            ButtonName = "View Customer",
                            SourceType = "Accounts"
                        };
                        notifications.Add(notification);
                        if (accountDetails.AccountId > 0)
                        {
                            List<EmployeeList> listEmployeeList = new();
                            var leadResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeList") + "Engineering Leader");
                            listEmployeeList = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(leadResult.Data));
                            if (listEmployeeList?.Count == 0)
                            {
                                leadResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeList") + "Engineering Lead");
                                listEmployeeList = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(leadResult.Data));
                            }
                            if (listEmployeeList?.Count > 0)
                            {
                                notification.ToId = listEmployeeList[0].EmployeeId;
                                notifications.Add(notification);
                            }
                        }
                        notification = new()
                        {
                            CreatedBy = accountDetails?.CreatedBy == null ? 0 : (int)accountDetails?.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            FromId = accountDetails?.CreatedBy == null ? 0 : (int)accountDetails?.CreatedBy,
                            ToId = (int)accountDetails?.FinanceManagerId,
                            MarkAsRead = false,
                            NotificationSubject = "You have been requested to review customer " + accountDetails?.AccountName + ".",
                            NotificationBody = "You have been requested to review customer " + accountDetails?.AccountName + ".",
                            PrimaryKeyId = AccountId,
                            ButtonName = "Review Customer",
                            SourceType = "Accounts"
                        };
                        notifications.Add(notification);
                        using var notificationClient = new HttpClient
                        {
                            BaseAddress = new(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                        };
                        HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                        var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();
                        if (notificationResponse?.IsSuccessStatusCode == false)
                        {
                            statusCode = "FAILURE";
                            statusText = notificationResult?.Result?.StatusText;
                        }
                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                        string MailSubject = "Request for cutomer review";
                        string MailBody = "Hi ," + financeManagerDetail.FinanceName + "</br> You have been requested to review customer " + accountDetails?.AccountName + ".";


                        SendEmailView sendEmail;
                        sendEmail = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = financeManagerDetail.FinanceEmail,
                            Subject = MailSubject,
                            MailBody = MailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                            CC = accountDetails.ModifiedBy == null ? accountDetails.CreatedBy == accountDetails.AccountManagerId ? financeManagerDetail.AccountManagerEmailId : financeManagerDetail.CreatedByEmailId + ","  + financeManagerDetail.AccountManagerEmailId  : financeManagerDetail.AccountManagerEmailId,

                        };
                        if (sendEmail != null)
                        {
                            var mail = _commonFunction.NotificationMail(sendEmail).Result;
                        };
                    }
                    if (AccountId > 0 && AddAccountView?.ListOfDocuments?.Count > 0)
                    {
                        SupportingDocumentsView supportingDocuments = new()
                        {
                            ListOfDocuments = new(),
                            SourceId = AccountId,
                            SourceType = _configuration.GetValue<string>("AccountsSourceType"),
                            BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory"),
                            CreatedBy = accountDetails?.CreatedBy == null ? 0 : (int)accountDetails?.CreatedBy
                        };
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
                        foreach (var item in AddAccountView?.ListOfDocuments)
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
                                UpdateAccountLogo updateAccountLogo = new()
                                {
                                    AccountId = AccountId,
                                    ModifiedBy = accountDetails?.CreatedBy == null ? 0 : (int)accountDetails?.CreatedBy,
                                    Logo = documentPath
                                };
                                using var logoClient = new HttpClient
                                {
                                    BaseAddress = new Uri(_accountsBaseURL)
                                };
                                HttpResponseMessage logoResponse = await logoClient.PostAsJsonAsync("Accounts/AssignLogoForAccount", updateAccountLogo);
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
                            BaseAddress = new(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                        };
                        HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/AddSupportingDocuments", supportingDocuments);
                        var documentResult = documentResponse.Content.ReadAsAsync<SuccessData>();
                        if (documentResponse?.IsSuccessStatusCode == false)
                        {
                            statusCode = "FAILURE";
                            statusText = documentResult?.Result?.StatusText;
                        }
                    }
                    if (statusCode == "")
                    {
                        return Ok(new
                        {
                            result?.StatusCode,
                            result?.StatusText,
                            AccountId
                        });
                    }
                }
                else
                    statusText = result?.StatusText;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/InsertAndUpdateAccount", JsonConvert.SerializeObject(AddAccountView));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                AccountId = 0
            });
        }
        #endregion

        #region Get Account's Master Data
        [HttpGet]
        [Route("GetMasterDataForCreateCustomer")]
        public async Task<IActionResult> GetMasterDataForCreateCustomer()
        {
            try
            {
                var result = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetMasterDataForCreateCustomer"));
                AccountMasterData masterData = JsonConvert.DeserializeObject<AccountMasterData>(JsonConvert.SerializeObject(result.Data));

                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        AccountMasterData = masterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/GetMasterDataForCreateCustomer");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                AccountMasterData = new AccountMasterData()
            });
        }
        #endregion

        #region Delete Account
        [HttpPost]
        [Route("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(DeleteAccount deleteAccount)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(deleteAccount, _accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:DeleteAccount"));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/DeleteAccount", JsonConvert.SerializeObject(deleteAccount));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get Account By Id
        [HttpGet]
        [Route("GetAccountById")]
        public async Task<IActionResult> GetAccountById(int pAccountId, int versionId, bool isLastVersion)
        {
            AccountDetailView accountDetails = new();
            string statusText = "";
            try
            {
                List<EmployeeName> listEmployeeName = new();
                var result = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAccountById") + pAccountId + "&versionId=" + versionId + "&isLastVersion=" + isLastVersion);
                accountDetails = JsonConvert.DeserializeObject<AccountDetailView>(JsonConvert.SerializeObject(result.Data));
                if (accountDetails != null)
                {
                    List<int> userIds = new();
                    //if (accountDetails.AccountManagerId > 0)
                    //    userIds.Add((int)accountDetails.AccountManagerId);
                    if (accountDetails.accountChangeRequestLists?.Count > 0)
                        userIds.Add((int)accountDetails.accountChangeRequestLists[0].CreatedBy);
                    //if (accountDetails.accountComments?.Count > 0)
                    //{
                    //    foreach (AccountCommentsView comments in accountDetails.accountComments)
                    //    {
                    //        if (comments.CreatedBy > 0)
                    //            userIds.Add((int)comments.CreatedBy);
                    //    }
                    //}
                    if (userIds.Count > 0)
                    {
                        userIds = userIds.Distinct().ToList();
                        var employeeResult = await _client.PostAsJsonAsync(userIds, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeNameById"));
                        listEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeResult.Data));
                        if (listEmployeeName.Count > 0)
                        {
                            //accountDetails.AccountManager = listEmployeeName.Where(x => x.EmployeeId == accountDetails.AccountManagerId).Select(x => x.EmployeeFullName).FirstOrDefault();
                            if (accountDetails.accountChangeRequestLists?.Count > 0)
                            {
                                accountDetails.accountChangeRequestLists[0].CreatedByName = listEmployeeName.Where(x => x.EmployeeId == accountDetails.accountChangeRequestLists[0].CreatedBy).Select(x => x.EmployeeFullName).FirstOrDefault();
                            }
                            //if (accountDetails.accountComments?.Count > 0)
                            //{
                            //    foreach (AccountCommentsView comments in accountDetails.accountComments)
                            //    {
                            //        comments.CreatedByName = listEmployeeName.Where(x => x.EmployeeId == comments.CreatedBy).Select(x => x.EmployeeFullName).FirstOrDefault();
                            //    }
                            //}
                        }
                    }
                    using HttpClient documentClient = new();
                    SourceDocuments sourceDocuments = new()
                    {
                        SourceId = new List<int> { pAccountId },
                        SourceType = new List<string> { _configuration.GetValue<string>("AccountsSourceType") }
                    };
                    documentClient.BaseAddress = new(_notificationBaseURL);
                    HttpResponseMessage documentResponse = await documentClient.PostAsJsonAsync("SupportingDocuments/GetDocumentBySourceIdAndType", sourceDocuments);
                    if (documentResponse?.IsSuccessStatusCode == true)
                    {
                        var documentResults = documentResponse.Content.ReadAsAsync<SuccessData>();
                        List<SupportingDocuments> lstOfSupDocument = JsonConvert.DeserializeObject<List<SupportingDocuments>>(JsonConvert.SerializeObject(documentResults.Result.Data));
                        if (lstOfSupDocument?.Count > 0)
                        {
                            accountDetails.ListOfDocuments = lstOfSupDocument.Where(x => x.DocumentName != null).Select(x => new DocumentDetails { DocumentId = x.DocumentId, DocumentName = x.DocumentName, DocumentSize = x.DocumentSize, DocumentCategory = x.DocumentCategory, IsApproved = x.IsApproved, DocumentType = x.DocumentType }).ToList();
                        }
                    }
                }
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        AccountDetails = accountDetails
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/GetAccountById", Convert.ToString(pAccountId));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                AccountDetails = new AccountDetailView()
            });
        }
        #endregion

        #region List All Draft Accounts
        [HttpGet]
        [Route("ListAllDraftAccounts")]
        public async Task<IActionResult> ListAllDraftAccounts(int pResourceId)
        {
            try
            {
                //List<EmployeeName> listEmployeeName = new();
                //List<ProjectDetails> listProjectDetails = new();
                List<AccountListView> lstAccountDetails = new();
                //List<AccountListView> accountLists = new();
                //List<ResourceAllocation> resourceAllocation = new();
                var result = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:ListAllDraftAccounts") + pResourceId);
                lstAccountDetails = JsonConvert.DeserializeObject<List<AccountListView>>(JsonConvert.SerializeObject(result.Data));
                //if (lstAccountDetails.Count > 0)
                //{
                //    var projectResult = await _client.PostAsJsonAsync(lstAccountDetails?.Select(x => x.AccountId).ToList(), _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectDetailByaccountId"));
                //    listProjectDetails = JsonConvert.DeserializeObject<List<ProjectDetails>>(JsonConvert.SerializeObject(projectResult.Data));
                //    resourceAllocation = JsonConvert.DeserializeObject<List<ResourceAllocation>>(JsonConvert.SerializeObject(projectResult.Resource));
                //    var employeeResult = await _client.PostAsJsonAsync(lstAccountDetails?.Select(x => x.AccountManagerId).ToList(), _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeNameById"));
                //    listEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeResult.Data));
                //    foreach (var accList in lstAccountDetails.OrderByDescending(x => x.AccountId))
                //    {
                //        List<int> projectId = listProjectDetails.Where(x => x.AccountId == accList.AccountId).Select(x => x.ProjectId).ToList();
                //        AccountListView accountList = new()
                //        {
                //            AccountId = accList.AccountId,
                //            CompanyName = accList.CompanyName,
                //            AccountManagerId = accList.AccountManagerId,
                //            AccountManager = listEmployeeName.Where(x => x.EmployeeId == accList.AccountManagerId).Select(x => x.EmployeeFullName).FirstOrDefault(),
                //            AccountStatus = accList.AccountStatus,
                //            ContactPerson = accList.ContactPerson,
                //            AcountSPOC = accList.AcountSPOC,
                //            ProjectTimesheet = "",
                //            Associates = resourceAllocation.Where(x => projectId.Contains((int)x.ProjectId)).Count(),
                //            AccountType = accList.AccountType,
                //            Logo = accList.Logo
                //        };
                //        accountLists.Add(accountList);
                //    }
                //    //Convert Logo as Base64 string
                //    accountLists.ForEach(x => x.LogoBase64 = ConvertToBase64String(x.Logo));
                //}
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        ListOfAccounts = lstAccountDetails
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/ListAllDraftAccounts", Convert.ToString(pResourceId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfAccounts = new List<AccountListView>()
            });
        }
        #endregion

        #region List All Accounts By ResourceId
        [HttpPost]
        [Route("ListAllAccountsByResourceId")]
        public async Task<IActionResult> ListAllAccountsByResourceId(AccountInput inputData)
        {
            try
            {
                List<AccountListView> lstAccountDetails = new();
                var RoleList = _configuration.GetValue<string>("CustomerProjectManagementRole:RoleList");
                List<int> listResourceId = new();
                if (inputData?.IsAllAccount == true)
                {
                    var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForManager") + inputData.EmployeeId);
                    List<EmployeeList> employeesList = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                    if (employeesList?.Count > 0)
                    {
                        listResourceId = employeesList.Select(x => x.EmployeeId).Distinct().ToList();
                    }
                }
                if (listResourceId.Count == 0)
                    listResourceId = new List<int> { inputData.EmployeeId };
                inputData.ManagementRole = RoleList.ToLower().Split(',').ToList();
                inputData.EmployeeList = listResourceId;
                var result = await _client.PostAsJsonAsync(inputData, _accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:ListAllAccountsByResourceId"));
                lstAccountDetails = JsonConvert.DeserializeObject<List<AccountListView>>(JsonConvert.SerializeObject(result?.Data));

                return Ok(new
                {
                    result.StatusCode,
                    ListOfAccounts = lstAccountDetails
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/ListAllAccountsByResourceId");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfAccounts = new List<AccountListView>()
            });
        }
        #endregion

        #region List All Accounts count
        [HttpPost]
        [Route("GetAllAccountsCount")]
        public async Task<IActionResult> GetAllAccountsCount(AccountInput inputData)
        {
            try
            {
                var RoleList = _configuration.GetValue<string>("CustomerProjectManagementRole:RoleList");
                List<int> listResourceId = new();
                if (inputData?.IsAllAccount == true)
                {
                    var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForManager") + inputData.EmployeeId);
                    List<EmployeeList> employeesList = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                    if (employeesList?.Count > 0)
                    {
                        listResourceId = employeesList.Select(x => x.EmployeeId).Distinct().ToList();
                    }
                }
                if (listResourceId.Count == 0)
                    listResourceId = new List<int> { inputData.EmployeeId };
                inputData.ManagementRole = RoleList.ToLower().Split(',').ToList();
                inputData.EmployeeList = listResourceId;
                var result = await _client.PostAsJsonAsync(inputData, _accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccountsCount"));
                int? data = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));

                return Ok(new
                {
                    result.StatusCode,
                    accountCount = data
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/GetAllAccountsCount");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                accountCount = 0
            });
        }
        #endregion

        #region Approve Account
        [HttpPost]
        [Route("ApproveAccount")]
        public async Task<IActionResult> ApproveAccount(ApproveAccount pApproveAccount)
        {
            string statusText = "";
            try
            {
                var result = await _client.PostAsJsonAsync(pApproveAccount, _accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:ApproveAccount"));
                bool status = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result.Data));
                if (result != null && status == true)
                {
                    await SendApproveNotification(pApproveAccount);
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/ApproveAccount", JsonConvert.SerializeObject(pApproveAccount));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText
            });
        }
        #endregion

        #region Send Notification
        [NonAction]
        public async Task<bool> SendApproveNotification(ApproveAccount pApproveAccount)
        {
            try
            {
                var accountDetailsResponse = _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAccountById") + pApproveAccount.AccountId);
                AccountDetailView accountDetails = JsonConvert.DeserializeObject<AccountDetailView>(JsonConvert.SerializeObject(accountDetailsResponse.Result.Data));
                if (accountDetails != null)
                {
                    List<Notifications> notifications = new();
                    Notifications notification = new()
                    {
                        CreatedBy = pApproveAccount.ApprovedBy,
                        CreatedOn = DateTime.UtcNow,
                        FromId = pApproveAccount.ApprovedBy,
                        ToId = accountDetails.CreatedBy == null ? 0 : (int)accountDetails.CreatedBy,
                        MarkAsRead = false,
                        NotificationSubject = pApproveAccount.ApprovedByName + " has approved your customer " + accountDetails.AccountName + ".",
                        NotificationBody = pApproveAccount.ApprovedByName + " has approved your customer " + accountDetails.AccountName + ".",
                        PrimaryKeyId = pApproveAccount.AccountId,
                        ButtonName = "View Customer",
                        SourceType = "Accounts"
                    };
                    notifications.Add(notification);
                    Uri uri = new(_configuration.GetValue<string>("ApplicationURL:Notifications"));
                    using var notificationClient = new HttpClient
                    {
                        BaseAddress = uri
                    };
                    HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                    var idList = new List<int>();
                    idList.Add(pApproveAccount.ApprovedBy);
                    idList.Add(accountDetails.CreatedBy == null ? 0 : (int)accountDetails.CreatedBy);
                    var employeeResult = await _client.PostAsJsonAsync(idList, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeNameById"));
                    List<EmployeeName> listEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeResult.Data));
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    string MailSubject = "Customer Approved";
                    string MailBody = pApproveAccount.ApprovedByName + " has approved your customer " + accountDetails.AccountName + ".";
                    SendEmailView sendEmail;
                    var approverEmail = listEmployeeName.Where(x => x.EmployeeId == pApproveAccount.ApprovedBy).FirstOrDefault()?.EmployeeEmailId;
                    var toEmail = listEmployeeName.Where(x => x.EmployeeId == accountDetails.CreatedBy).FirstOrDefault()?.EmployeeEmailId;

                    sendEmail = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = toEmail,
                        Subject = MailSubject,
                        MailBody = MailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = approverEmail

                    };
                    if (sendEmail != null)
                    {
                        var mail = _commonFunction.NotificationMail(sendEmail).Result;
                    };
                }

                return true;

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/ApproveAccount", JsonConvert.SerializeObject(pApproveAccount));
                return false;
            }
        }
        #endregion

        #region Request Changes Account
        [HttpPost]
        [Route("RequestChangesAccount")]
        public async Task<IActionResult> RequestChangesAccount(RequestChangesAccount requestChangesAccount)
        {
            string statusText = "";
            try
            {
                if (requestChangesAccount.CreatedById == 0)
                    requestChangesAccount.CreatedById = requestChangesAccount.FinanceManagerId;
                var result = await _client.PostAsJsonAsync(requestChangesAccount, _accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:RequestChangesAccount"));
                bool status = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result.Data));
                if (result != null && status == true)
                {
                    await SendRequestChangesNotification(requestChangesAccount);
                    return Ok(new
                    {
                        result.StatusCode,
                        result.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/RequestChangesAccount", JsonConvert.SerializeObject(requestChangesAccount));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText
            });
        }
        #endregion

        #region send notification
        [NonAction]
        public async Task<bool> SendRequestChangesNotification(RequestChangesAccount requestChangesAccount)
        {
            try
            {
                var accountDetailsResponse = _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAccountById") + requestChangesAccount.AccountId +
                    "&versionId=0&isLastVersion=true");
                AccountDetailView accountDetails = JsonConvert.DeserializeObject<AccountDetailView>(JsonConvert.SerializeObject(accountDetailsResponse.Result.Data));
                if (accountDetails != null)
                {
                    List<int> userIds = new();
                    List<EmployeeName> listEmployeeName = new();
                    userIds.Add(requestChangesAccount.FinanceManagerId);
                    userIds.Add(accountDetails?.AccountManagerId == null ? 0 : (int)accountDetails?.AccountManagerId);
                    listEmployeeName = await GetEmployeeNamesAsync(userIds);
                    var financeManagerDetail = listEmployeeName.Where(x => x.EmployeeId == requestChangesAccount?.FinanceManagerId).FirstOrDefault();
                    var AccountManagerDetail = listEmployeeName.Where(x => x.EmployeeId == accountDetails?.AccountManagerId).FirstOrDefault();
                    List<Notifications> notifications = new();
                    Notifications notification = new()
                    {
                        CreatedBy = requestChangesAccount.FinanceManagerId,
                        CreatedOn = DateTime.UtcNow,
                        FromId = requestChangesAccount.FinanceManagerId,
                        ToId = accountDetails.AccountManagerId == null ? 0 : (int)accountDetails.AccountManagerId,
                        MarkAsRead = false,
                        NotificationSubject = financeManagerDetail?.EmployeeFullName + " has requested changes in customer " + accountDetails.AccountName + ".",
                        NotificationBody = financeManagerDetail?.EmployeeFullName + " has requested changes in customer " + accountDetails.AccountName + ".",
                        PrimaryKeyId = requestChangesAccount.AccountId,
                        ButtonName = "Edit Customer",
                        SourceType = "Accounts"
                    };
                    notifications.Add(notification);
                    using var notificationClient = new HttpClient
                    {
                        BaseAddress = new(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                    };
                    HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                  var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    string MailSubject = "Requested changes in customer";
                    string MailBody = "Hi,"+ AccountManagerDetail?.EmployeeFullName+ "</br>" + financeManagerDetail?.EmployeeFullName + " has requested changes in customer " + accountDetails.AccountName + ".";
                    SendEmailView sendEmail;

                    sendEmail = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = AccountManagerDetail?.EmployeeEmailId,
                        Subject = MailSubject,
                        MailBody = MailBody,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = financeManagerDetail?.EmployeeEmailId
                    };
                    if (sendEmail != null)
                    {
                        var mail = _commonFunction.NotificationMail(sendEmail).Result;
                    };
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/RequestChangesAccount", JsonConvert.SerializeObject(requestChangesAccount));
                return false;
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
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/ListAllStateByCountryId", Convert.ToString(CountryId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfStates = states
            });
        }
        #endregion

        #region List All Account Related Issues
        [HttpGet]
        [Route("ListAllAccountRelatedIssues")]
        public async Task<IActionResult> ListAllAccountRelatedIssues()
        {
            List<AccountRelatedIssue> lstAccountRelatedIssueList = new();
            try
            {
                var result = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:ListAllAccountRelatedIssues"));
                lstAccountRelatedIssueList = JsonConvert.DeserializeObject<List<AccountRelatedIssue>>(JsonConvert.SerializeObject(result.Data));
                if (lstAccountRelatedIssueList.Count > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        ListOfAccountRelatedIssues = lstAccountRelatedIssueList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/ListAllAccountRelatedIssues");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ListOfAccountRelatedIssues = lstAccountRelatedIssueList
            });
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
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/GetStatusNameByCode", pStatusCode);
            }
            return _statusName;
        }
        #endregion

        #region Remove Customer Logo
        [HttpPost]
        [Route("RemoveCustomerLogo")]
        public async Task<IActionResult> RemoveCustomerLogo(RemoveLogoAccount removeLogoAccount)
        {
            try
            {
                using var _logoClient = new HttpClient
                {
                    BaseAddress = new Uri(_accountsBaseURL)
                };
                var logoResponse = await _logoClient.PostAsJsonAsync("Accounts/RemoveCustomerLogo", removeLogoAccount);
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
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/RemoveCustomerLogo", Convert.ToString(removeLogoAccount));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get Account's Master Data
        [HttpGet]
        [Route("GetMasterDataForCustomerDetails")]
        public async Task<IActionResult> GetMasterDataForCustomerDetails()
        {
            try
            {
                var result = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetMasterDataForCustomerDetails"));
                AccountMasterData masterData = JsonConvert.DeserializeObject<AccountMasterData>(JsonConvert.SerializeObject(result.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        AccountMasterData = masterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/GetMasterDataForCustomerDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                AccountMasterData = new AccountMasterData()
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

        #region Get version list
        [HttpGet]
        [Route("GetAllVersionByAccountId")]
        public async Task<IActionResult> GetAllVersionByAccountId(int accountId)
        {
            try
            {
                var result = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllVersionByAccountId") + accountId);
                List<KeyWithValue> data = JsonConvert.DeserializeObject<List<KeyWithValue>>(JsonConvert.SerializeObject(result.Data));

                if (result != null)
                {
                    return Ok(new
                    {
                        result.StatusCode,
                        Data = data
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Accounts/GetAllVersionByAccountId");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = new List<KeyWithValue>()
            });
        }
        #endregion

        [NonAction]
        public async Task<string> EmailNotificationForNewManager(EmployeeNotificationData data)
        {
            try
            {
                

                return "Success";
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employee/getEmployeeEmailTemplate");
                return "Failure";
            }


        }
    }
}