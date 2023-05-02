using APIGateWay.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Reports;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SharedLibraries.ViewModels.Leaves.WeeklyOverviewReportView;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HTTPClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _timesheetBaseURL = string.Empty;
        private readonly string _projectBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _accountsBaseURL = string.Empty;
        private readonly string _leaveBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";

        #region Constructor
        public ReportsController(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _configuration = configuration;
            _timesheetBaseURL = _configuration.GetValue<string>("ApplicationURL:Timesheet:BaseURL");
            _projectBaseURL = _configuration.GetValue<string>("ApplicationURL:Projects:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _accountsBaseURL = _configuration.GetValue<string>("ApplicationURL:Accounts:BaseURL");
            _leaveBaseURL = _configuration.GetValue<string>("ApplicationURL:Leaves:BaseURL");
        }
        #endregion

        #region Get Nexus info
        [HttpGet]
        [Route("GetNexusInfo")]
        public async Task<IActionResult> GetNexusInfo(int resourceId, bool isAllDetails = false)
        {
            NexusInfo ReportData = new();
            try
            {
                List<int> listResourceId = new();
                if (isAllDetails)
                {
                    var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForManager") + resourceId);
                    List<EmployeeList> listOfEmployees = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                    if (listOfEmployees?.Count > 0)
                    {
                        listResourceId = listOfEmployees.Select(x => x.EmployeeId).Distinct().ToList();
                    }
                }
                else
                {
                    listResourceId = new List<int> { resourceId };
                }
                //Get accounts count
                var accountResult = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccountsDetails"));
                List<AccountDetails> AccountDetails = JsonConvert.DeserializeObject<List<AccountDetails>>(JsonConvert.SerializeObject(accountResult.Data));
                //Get projects count
                var projectResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetAllProjectsDetails"));
                List<ProjectDetails> ProjectDetails = JsonConvert.DeserializeObject<List<ProjectDetails>>(JsonConvert.SerializeObject(projectResult.Data));
                ReportData = new()
                {
                    NoOfUsers = listResourceId.Count,
                    NoOfProjects = (from PD in ProjectDetails
                                    where PD.ProjectStatus != "Draft" && (
                                                                       listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                       listResourceId.Contains(PD.FinanceManagerId == null ? 0 : (int)PD.FinanceManagerId) ||
                                                                       listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                       listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC))
                                    select PD.ProjectId).ToList().Distinct().Count(),
                    NoOfAccounts = (from AC in AccountDetails
                                    where AC.AccountStatus != "Draft" && (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                       listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                       listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId)
                                                                       )
                                    select AC.AccountId).ToList().Distinct().Count()
                };
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    ReportData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetNexusInfo", " ResourceId- " + resourceId.ToString() + " isAllDetails- " + isAllDetails.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ReportData
            });
        }
        #endregion

        #region Get Accounts report
        [HttpGet]
        [Route("GetAccountReport")]
        public async Task<IActionResult> GetAccountReport(int resourceId, bool isAllAccount = false)
        {
            ReportsModel ReportData = new();
            try
            {
                List<int> listResourceId = new();
                if (isAllAccount)
                {
                    var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForManager") + resourceId);
                    List<EmployeeList> listOfEmployees = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                    if (listOfEmployees?.Count > 0)
                    {
                        listResourceId = listOfEmployees.Select(x => x.EmployeeId).Distinct().ToList();
                    }
                }
                else
                {
                    listResourceId = new List<int> { resourceId };
                }
                //Get accounts
                var accountResult = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccountsDetails"));
                List<AccountDetails> AccountDetails = JsonConvert.DeserializeObject<List<AccountDetails>>(JsonConvert.SerializeObject(accountResult.Data));
                //Get projects
                var projectResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetAllProjectsDetails"));
                List<ProjectDetails> ProjectDetails = JsonConvert.DeserializeObject<List<ProjectDetails>>(JsonConvert.SerializeObject(projectResult.Data));
                List<ReportData> reportData = (from AC in AccountDetails
                                               where (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                                  listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                                  listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                                  listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId))
                                               group AC by AC.AccountStatus into report
                                               select new ReportData { Status = report.Key, Count = report.Count() }).ToList();
                List<AccountReport> accountData = (from AC in AccountDetails
                                                   where (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                                      listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                                      listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                                      listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId))
                                                   select new AccountReport
                                                   {
                                                       AccountId = AC.AccountId,
                                                       FormattedAccountId = AC.FormattedAccountId,
                                                       AccountName = AC.AccountName,
                                                       AccountStatus = AC.AccountStatus,
                                                       ContactPersonName = "",
                                                       ProjectCount = ProjectDetails.Where(x => x.AccountId == AC.AccountId).ToList().Count,
                                                       OwnerName = string.Concat(AC.DirectorFirstName, " ", AC.DirectorLastName)
                                                   }).ToList();
                ReportData.ReportData = JsonConvert.SerializeObject(reportData);
                ReportData.ReportDetail = JsonConvert.SerializeObject(accountData);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    ReportData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetAccountReport", " ResourceId- " + resourceId.ToString() + " AllAccount- " + isAllAccount.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ReportData
            });
        }
        #endregion

        #region Get Projects report
        [HttpGet]
        [Route("GetProjectReport")]
        public async Task<IActionResult> GetProjectReport(int resourceId, bool isAllProject)
        {
            ReportsModel projectReportData = new();
            try
            {
                List<int> listResourceId = new();
                if (isAllProject)
                {
                    var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForManager") + resourceId);
                    List<EmployeeList> listOfEmployees = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                    if (listOfEmployees?.Count > 0)
                    {
                        listResourceId = listOfEmployees.Select(x => x.EmployeeId).Distinct().ToList();
                    }
                }
                else
                {
                    listResourceId = new List<int> { resourceId };
                }
                //Get accounts
                var accountResult = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccountsDetails"));
                List<AccountDetails> AccountDetails = JsonConvert.DeserializeObject<List<AccountDetails>>(JsonConvert.SerializeObject(accountResult.Data));
                //Get projects
                var projectResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectReportMasterData"));
                ProjectReportMasterData ProjectData = JsonConvert.DeserializeObject<ProjectReportMasterData>(JsonConvert.SerializeObject(projectResult.Data));
                List<ReportData> reportData = (from PD in ProjectData?.ProjectDetails
                                               where (
                                                    listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                    listResourceId.Contains(PD.FinanceManagerId == null ? 0 : (int)PD.FinanceManagerId) ||
                                                    listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                    listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC))
                                               group PD by PD.ProjectStatus into report
                                               select new ReportData { Status = report.Key, Count = report.Count() }).ToList();
                List<ProjectReport> projectReport = (from PD in ProjectData?.ProjectDetails
                                                     join PT in ProjectData?.ProjectType on PD.ProjectTypeId equals PT.ProjectTypeId
                                                     where (
                                                        listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                        listResourceId.Contains(PD.FinanceManagerId == null ? 0 : (int)PD.FinanceManagerId) ||
                                                        listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                        listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC))
                                                     select new ProjectReport
                                                     {
                                                         FormattedProjectId = PD.FormattedProjectId,
                                                         ProjectId = PD.ProjectId,
                                                         ProjectName = PD.ProjectName,
                                                         ProjectType = PT.ProjectTypeDescription,
                                                         ProjectStartDate = PD.ProjectStartDate,
                                                         ProjectEndDate = PD.ProjectEndDate,
                                                         ProjectDuration = PD.ProjectDuration,
                                                         ProjectStatus = PD.ProjectStatus,
                                                         AccountId = PD.AccountId == null ? 0 : (int)PD.AccountId,
                                                         OwnerName = AccountDetails.Where(x => x.AccountId == PD.AccountId).Select(x => (x.DirectorFirstName ?? "") + " " + (x.DirectorLastName ?? "")).FirstOrDefault(),
                                                         AccountName = AccountDetails.Where(x => x.AccountId == PD.AccountId).Select(x => x.AccountName).FirstOrDefault(),
                                                         ProjectSPOCId = PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC
                                                     }).ToList();
                //Get Spoc name
                if (projectReport?.Count > 0)
                {
                    var spocNameResult = await _client.PostAsJsonAsync(projectReport.Select(x => x.ProjectSPOCId).ToList(), _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeNameById"));
                    List<EmployeeName> lstEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(spocNameResult.Data));
                    projectReport.ForEach(x => x.ProjectSPOC = lstEmployeeName.Where(y => y.EmployeeId == x.ProjectSPOCId).Select(z => z.EmployeeFullName).FirstOrDefault());
                }
                projectReportData.ReportData = JsonConvert.SerializeObject(reportData);
                projectReportData.ReportDetail = JsonConvert.SerializeObject(projectReport);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    ReportData = projectReportData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetProjectReport", " ResourceId- " + resourceId.ToString() + " AllProject- " + isAllProject.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ReportData = projectReportData
            });
        }
        #endregion

        #region Get Resource report
        [HttpGet]
        [Route("GetResourceReport")]
        public async Task<IActionResult> GetResourceReport(int resourceId, bool isAllResource)
        {
            ResourceReportView reportData = new()
            {
                ResourceBillabilityStatusReport = new(),
                ProjectWiseResourceUtilisationReport = new(),
                ResourceWiseUtillisationReport = new(),
                ProjectSkillsSetReport = new(),
                ResourceReport = new(),
                ActiveProjectList = new(),
                SkillSetWiseBenchReport = new(),
                SkillSetWiseResourceReport = new()
            };
            ResourceReportMasterData projectMasterData = new();
            List<AccountDetails> accountData = new();
            List<Employees> lstEmployee = new();
            List<int> projectResource = new();
            try
            {
                List<int> listResourceId = new();
                //Get skill set wise bench report && Get skillset wise resource report
                var resourceEmployeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResourceEmployeeReport") + resourceId);
                EmployeesMasterData employeeMasterData = JsonConvert.DeserializeObject<EmployeesMasterData>(JsonConvert.SerializeObject(resourceEmployeeResult.Data));
                reportData.Skillsets = employeeMasterData?.Skillsets.ToList();
                if (isAllResource)
                {
                    listResourceId = employeeMasterData?.AllLevelEmployee?.Select(x => x.EmployeeId).Distinct().ToList();
                }
                else
                {
                    listResourceId.Add(resourceId);
                }
                lstEmployee = employeeMasterData?.Employees;
                //Get project master data
                var projectResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetResourceProjectData"));
                projectMasterData = JsonConvert.DeserializeObject<ResourceReportMasterData>(JsonConvert.SerializeObject(projectResult.Data));
                //Get account master data
                var accountResult = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccountsDetails"));
                accountData = JsonConvert.DeserializeObject<List<AccountDetails>>(JsonConvert.SerializeObject(accountResult.Data));
                if (projectMasterData != null)
                {
                    if (projectMasterData?.ResourceAllocation != null)
                    {
                        //Get resource billability status
                        reportData.ResourceBillabilityStatusReport = (from AC in accountData
                                                                      join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                                      join RA in projectMasterData?.ResourceAllocation on PD.ProjectId equals RA.ProjectId
                                                                      where RA.EmployeeId.HasValue && RA.ProjectId > 0
                                                                      && (RA.StartDate.HasValue && RA.StartDate.Value.Date <= CommonLib.GetTodayStartTime().Date)
                                                                      && (RA.EndDate.HasValue && RA.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date)
                                                                      && (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                      listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                      listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                      listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                      listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                      listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId)
                                                                      )
                                                                      group new { RA } by new { RA.IsBillable } into report
                                                                      select new ReportData
                                                                      {
                                                                          Id = report?.Key?.IsBillable == true ? 1 : 2,
                                                                          Count = report.Where(x => x.RA.EmployeeId > 0).Select(x => x.RA.EmployeeId).Distinct().Count(),
                                                                          Status = report?.Key?.IsBillable == true ? "Billable" : "Non-Billable"
                                                                      }).ToList();
                        if (reportData.ResourceBillabilityStatusReport != null)
                        {
                            projectResource = (from RA in projectMasterData?.ResourceAllocation
                                               where RA.EmployeeId.HasValue && RA.ProjectId > 0
                                                             && (RA.StartDate.HasValue && RA.StartDate.Value.Date <= CommonLib.GetTodayStartTime().Date)
                                                             && (RA.EndDate.HasValue && RA.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date)
                                               select RA.EmployeeId == null ? 0 : (int)RA.EmployeeId).ToList();
                            ReportData bench = new()
                            {
                                Status = "Bench",
                                Count = lstEmployee.Select(x => x.EmployeeID).Except(projectResource).ToList().Count
                            };
                            reportData.ResourceBillabilityStatusReport?.Add(bench);
                        }
                        //Get project wise resource utilization
                        reportData.ProjectWiseResourceUtilisationReport = (from AC in accountData
                                                                           join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                                           join RA in projectMasterData?.ResourceAllocation on PD.ProjectId equals RA.ProjectId
                                                                           where RA.EmployeeId.HasValue && RA.ProjectId > 0
                                                                         && (RA.StartDate.HasValue && RA.StartDate.Value.Date <= CommonLib.GetTodayStartTime().Date)
                                                                         && (RA.EndDate.HasValue && RA.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date)
                                                                         && (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                              listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                              listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                              listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                              listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                              listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId))
                                                                           group new { RA, PD } by new { RA.ProjectId, PD.ProjectName } into report
                                                                           select new ResourceUtilization
                                                                           {
                                                                               Id = report.Key.ProjectId == null ? 0 : (int)report.Key.ProjectId,
                                                                               Billable = report.Where(x => x.RA.IsBillable == true).Select(x => x.RA.EmployeeId).Distinct().Count(),
                                                                               NonBillable = report.Where(x => x.RA.IsBillable == false).Select(x => x.RA.EmployeeId).Distinct().Count(),
                                                                               ProjectName = report.Key.ProjectName
                                                                           }).ToList();
                        //Get resource wise utilization
                        reportData.ResourceWiseUtillisationReport = (from AC in accountData
                                                                     join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                                     join resource in projectMasterData?.ResourceAllocation on PD.ProjectId equals resource.ProjectId
                                                                     join allocation in projectMasterData?.Allocation on resource.AllocationId equals allocation.AllocationId
                                                                     where (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                                    listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                                    listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                                    listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                                    listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                                    listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId)) &&
                                                                                    resource.EmployeeId != null
                                                                     group new { resource, allocation } by new { EmployeeId = resource.EmployeeId == null ? 0 : (int)resource.EmployeeId, resource.ProjectId } into report
                                                                     select new ReportData
                                                                     {
                                                                         ProjectId = report.Key.ProjectId == null ? 0 : (int)report.Key.ProjectId,
                                                                         Id = report.Key.EmployeeId,
                                                                         Count = report.Sum(x => string.IsNullOrEmpty(x.allocation.AllocationDescription) ? 0 : Convert.ToInt32(x.allocation.AllocationDescription[0..^1]))
                                                                     }).ToList();
                        //Project wise skillset
                        reportData.ProjectSkillsSetReport = (from AC in accountData
                                                             join
                                      PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                             join RA in projectMasterData?.ResourceAllocation on PD.ProjectId equals RA.ProjectId
                                                             join RSS in employeeMasterData?.Skillsets on RA.RequiredSkillSetId equals RSS.SkillsetId
                                                             where RA.ProjectId > 0 && (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                                    listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                                    listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                                    listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                      listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                      listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId))
                                                             group new { RSS, RA } by new { RSS.SkillsetId, RSS.Skillset, RA.ProjectId } into report
                                                             select new ReportData
                                                             {
                                                                 Id = report.Key.SkillsetId,
                                                                 Count = report.Where(x => x.RA.EmployeeId > 0).Select(x => x.RA.EmployeeId).Distinct().Count(),
                                                                 Status = report.Key.Skillset,
                                                                 ProjectId = report.Key.ProjectId == null ? 0 : (int)report.Key.ProjectId
                                                             }).ToList();
                        //Get Resource report
                        reportData.ResourceReport = (from AC in accountData
                                                     join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                     join RA in projectMasterData?.ResourceAllocation on PD.ProjectId equals RA.ProjectId
                                                     join allocation in projectMasterData?.Allocation on RA.AllocationId equals allocation.AllocationId
                                                     join employee in employeeMasterData?.Employees on RA.EmployeeId equals employee.EmployeeID
                                                     join role in employeeMasterData?.RoleNameList on employee.RoleId equals role.RoleId
                                                     where (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                          listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                          listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                          listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                      listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                      listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId)) && RA.EmployeeId != null
                                                     select new ResourceReport
                                                     {
                                                         FormattedProjectId = PD.FormattedProjectId,
                                                         EmployeeId = RA.EmployeeId == null ? 0 : (int)RA.EmployeeId,
                                                         ProjectId = PD.ProjectId,
                                                         ProjectName = PD.ProjectName,
                                                         Role = role.RoleFullName,
                                                         Billability = RA.IsBillable == true ? "Billable" : "Non Billable",
                                                         Utilization = string.IsNullOrEmpty(allocation.AllocationDescription) ? 0 : Convert.ToDecimal(allocation.AllocationDescription[0..^1])
                                                     }).ToList();
                        //Get Active project list
                        reportData.ActiveProjectList = (from AC in accountData
                                                        join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                        join RA in projectMasterData?.ResourceAllocation on PD.ProjectId equals RA.ProjectId
                                                        where PD.ProjectStatus == "Ongoing"
                                                        && (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                            listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                            listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                            listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                            listResourceId.Contains(RA.EmployeeId == null ? 0 : (int)RA.EmployeeId) ||
                                                            listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                            listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId))
                                                        group PD by new { PD.ProjectId, PD.ProjectName } into project
                                                        select new ReportData
                                                        {
                                                            Id = project.Key.ProjectId,
                                                            Count = project.Key.ProjectId,
                                                            Status = project.Key.ProjectName
                                                        }).ToList();
                    }

                    if (employeeMasterData != null)
                    {
                        //Skillset wise resource report
                        reportData.SkillSetWiseResourceReport = (from AC in accountData
                                                                 join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                                 join RA in projectMasterData?.ResourceAllocation on PD.ProjectId equals RA.ProjectId
                                                                 join E in employeeMasterData?.Employees on RA.EmployeeId equals E.EmployeeID
                                                                 join ESS in employeeMasterData?.EmployeesSkillset on E.EmployeeID equals ESS.EmployeeId
                                                                 join SS in employeeMasterData?.Skillsets on ESS.SkillsetId equals SS.SkillsetId
                                                                 where E.IsActive == true && (listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                    listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                    listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                    listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                    listResourceId.Contains(RA.EmployeeId == null ? 0 : (int)RA.EmployeeId) ||
                                                                    listResourceId.Contains(AC.FinanceManagerId == null ? 0 : (int)AC.FinanceManagerId) ||
                                                                    listResourceId.Contains(AC.AccountManagerId == null ? 0 : (int)AC.AccountManagerId))
                                                                 group new { SS, ESS, PD } by new { SS.SkillsetId, SS.Skillset, PD.ProjectId } into report
                                                                 select new ReportData
                                                                 {
                                                                     Id = report.Key.SkillsetId,
                                                                     ProjectId = report.Key.ProjectId,
                                                                     Count = report.Where(x => x.ESS.EmployeeId > 0).Select(x => x.ESS.EmployeeId).Distinct().Count(),
                                                                     Status = report.Key.Skillset
                                                                 }).ToList();
                        //Skillset wise bench report
                        List<int> benchResource = employeeMasterData?.Employees?.Select(x => x.EmployeeID).Except(projectResource).ToList();
                        reportData.SkillSetWiseBenchReport = (from E in employeeMasterData?.Employees
                                                              join ESS in employeeMasterData?.EmployeesSkillset on E.EmployeeID equals ESS.EmployeeId
                                                              join SS in employeeMasterData?.Skillsets on ESS.SkillsetId equals SS.SkillsetId
                                                              where E.IsActive == true && benchResource.Contains(E.EmployeeID)
                                                              group new { SS, ESS } by new { SS.SkillsetId, SS.Skillset } into report
                                                              select new ReportData
                                                              {
                                                                  Id = report.Key.SkillsetId,
                                                                  Count = report.Where(x => x.ESS.EmployeeId > 0).Select(x => x.ESS.EmployeeId).Distinct().Count(),
                                                                  Status = report.Key.Skillset
                                                              }).ToList();
                    }
                    if (reportData?.ResourceWiseUtillisationReport?.Count > 0)
                    {
                        reportData?.ResourceWiseUtillisationReport.ForEach(x => x.Status = lstEmployee.Where(y => y.EmployeeID == x.Id).Select(z => string.Concat(z.FirstName, " ", z.LastName)).FirstOrDefault());
                    }
                    if (reportData?.ResourceReport?.Count > 0)
                    {
                        foreach (var item in reportData?.ResourceReport)
                        {
                            item.FormattedEmployeeId = lstEmployee.Where(x => x.EmployeeID == item.EmployeeId).Select(x => x.FormattedEmployeeId).FirstOrDefault();
                            item.ResourceName = lstEmployee.Where(x => x.EmployeeID == item.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                            List<string> empskillset = (from es in employeeMasterData?.EmployeesSkillset
                                                        join sk in employeeMasterData?.Skillsets on es.SkillsetId equals sk.SkillsetId
                                                        where es.EmployeeId == item.EmployeeId
                                                        select sk.Skillset).ToList();
                            if (empskillset?.Count > 0)
                            {
                                item.SKillSet = string.Join(",", empskillset);
                            }
                        }
                    }
                }
                return Ok(new
                {
                    projectResult.StatusCode,
                    projectResult.StatusText,
                    reportData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetResourceReport", " ResourceId- " + resourceId.ToString() + " AllResource- " + isAllResource.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                reportData
            });
        }
        #endregion

        #region Get Timesheet report
        [HttpGet]
        [Route("GetTimesheetReport")]
        public async Task<IActionResult> GetTimesheetReport(int resourceId, bool isAllTimesheet)
        {
            TimesheetReportView reportData = new();
            TimesheetMasterData timesheetMasterData = new();
            ResourceReportMasterData projectMasterData = new();
            List<AccountDetails> accountData = new();
            try
            {
                List<int> listResourceId = new();
                var timesheetEmployeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetResourceEmployeeReport") + resourceId);
                EmployeesMasterData employeeMasterData = JsonConvert.DeserializeObject<EmployeesMasterData>(JsonConvert.SerializeObject(timesheetEmployeeResult.Data));
                reportData.Skillsets = employeeMasterData?.Skillsets.ToList();
                if (isAllTimesheet)
                {
                    listResourceId = employeeMasterData?.AllLevelEmployee?.Select(x => x.EmployeeId).Distinct().ToList();
                }
                else
                {
                    listResourceId.Add(resourceId);
                }
                if (isAllTimesheet)
                {
                    var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForManager") + resourceId);
                    List<EmployeeList> listOfEmployees = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                    if (listOfEmployees?.Count > 0)
                    {
                        listResourceId = listOfEmployees.Select(x => x.EmployeeId).Distinct().ToList();
                    }
                }
                else
                {
                    listResourceId = new List<int> { resourceId };
                }
                //Get project master data
                var projectResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetResourceProjectData"));
                projectMasterData = JsonConvert.DeserializeObject<ResourceReportMasterData>(JsonConvert.SerializeObject(projectResult.Data));
                //Get account master data
                var accountResult = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccountsDetails"));
                accountData = JsonConvert.DeserializeObject<List<AccountDetails>>(JsonConvert.SerializeObject(accountResult.Data));
                List<EmployeeName> lstEmployeeName = new();
                //Get timesheet report
                var masterDataResult = await _client.PostAsJsonAsync(listResourceId, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetMasterData"));
                timesheetMasterData = JsonConvert.DeserializeObject<TimesheetMasterData>(JsonConvert.SerializeObject(masterDataResult.Data));
                if (timesheetMasterData != null)
                {
                    reportData.ProjectWiseTimesheetPlannedActualReport = (from AC in accountData
                                                                          join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                                          join timesheetLog in timesheetMasterData?.TimesheetLog on PD.ProjectId equals timesheetLog.ProjectId
                                                                          where listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                              listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                              listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                              listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                              listResourceId.Contains(timesheetLog.ResourceId == null ? 0 : (int)timesheetLog.ResourceId)
                                                                          group timesheetLog by timesheetLog.ProjectId into report
                                                                          select new TimesheetReport
                                                                          {
                                                                              ProjectId = report.Key.Value,
                                                                              PlannedHours = string.Format("{0}:{1}", (new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Days * 24 +
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Hours),
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Minutes),
                                                                              ClockedHours = string.Format("{0}:{1}", (new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Days * 24 +
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Hours),
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Minutes)
                                                                          }).ToList();
                    reportData.ResourceWiseTimesheetPlannedActualReport = (from AC in accountData
                                                                           join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                                           join timesheetLog in timesheetMasterData?.TimesheetLog on PD.ProjectId equals timesheetLog.ProjectId
                                                                           join employee in employeeMasterData?.Employees on timesheetLog.ResourceId equals employee.EmployeeID
                                                                           where listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                                  listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                                  listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                                  listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                                  listResourceId.Contains(timesheetLog.ResourceId == null ? 0 : (int)timesheetLog.ResourceId)
                                                                           group timesheetLog by new { timesheetLog.ProjectId, timesheetLog.ResourceId, employee.FirstName, employee.LastName } into report
                                                                           select new TimesheetReport
                                                                           {
                                                                               ProjectId = report.Key.ProjectId == null ? 0 : (int)report.Key.ProjectId,
                                                                               ResourceName = (report.Key.FirstName ?? "") + " " + (report.Key.LastName ?? ""),
                                                                               ResourceId = report.Key.ResourceId == null ? 0 : (int)report.Key.ResourceId,
                                                                               PlannedHours = string.Format("{0}:{1}", (new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Days * 24 +
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Hours),
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Minutes),
                                                                               ClockedHours = string.Format("{0}:{1}", (new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Days * 24 +
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Hours),
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Minutes)
                                                                           }).ToList();
                    reportData.ResourceWiseWeeklyTimesheetReport = (from AC in accountData
                                                                    join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                                    join timesheetLog in timesheetMasterData?.TimesheetLog on PD.ProjectId equals timesheetLog.ProjectId
                                                                    join employee in employeeMasterData?.Employees on timesheetLog.ResourceId equals employee.EmployeeID
                                                                    where listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                      listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                      listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                      listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                      listResourceId.Contains(timesheetLog.ResourceId == null ? 0 : (int)timesheetLog.ResourceId)
                                                                    group timesheetLog by new { timesheetLog.ProjectId, timesheetLog.ResourceId, timesheetLog.IsSubmitted, PD.ProjectName, employee.FirstName, employee.LastName } into report
                                                                    select new TimesheetReport
                                                                    {
                                                                        ProjectId = report.Key.ProjectId == null ? 0 : (int)report.Key.ProjectId,
                                                                        ResourceId = report.Key.ResourceId == null ? 0 : (int)report.Key.ResourceId,
                                                                        ResourceName = (report.Key.FirstName ?? "") + " " + (report.Key.LastName ?? ""),
                                                                        IsSubmitted = report.Key.IsSubmitted != null && (bool)report.Key.IsSubmitted,
                                                                        PlannedHours = string.Format("{0}:{1}", (new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Days * 24 +
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Hours),
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Minutes),
                                                                        ClockedHours = string.Format("{0}:{1}", (new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Days * 24 +
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Hours),
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Minutes),
                                                                        ProjectName = report.Key.ProjectName ?? ""
                                                                    }).ToList();
                    reportData.TimesheetReportDetails = (from AC in accountData
                                                         join PD in projectMasterData?.ProjectDetails on AC.AccountId equals PD.AccountId
                                                         join timesheetLog in timesheetMasterData?.TimesheetLog on PD.ProjectId equals timesheetLog.ProjectId
                                                         join employee in employeeMasterData?.Employees on timesheetLog.ResourceId equals employee.EmployeeID
                                                         join role in employeeMasterData?.RoleNameList on employee.RoleId equals role.RoleId
                                                         where listResourceId.Contains(AC.CreatedBy == null ? 0 : (int)AC.CreatedBy) ||
                                                                listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                                listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                                listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC) ||
                                                                listResourceId.Contains(timesheetLog.ResourceId == null ? 0 : (int)timesheetLog.ResourceId)
                                                         group timesheetLog by new { timesheetLog.ProjectId, timesheetLog.ResourceId, employee.FirstName, employee.LastName, role.RoleFullName, PD.ProjectName } into report
                                                         select new TimesheetReportDetails
                                                         {
                                                             ProjectId = report.Key.ProjectId == null ? 0 : (int)report.Key.ProjectId,
                                                             FormattedProjectId = report.Key.ProjectId == null ? "" : "PRJ1" + report.Key.ProjectId.ToString().PadLeft(4, '0'),
                                                             FormattedEmployeeId = lstEmployeeName.Where(x => x.EmployeeId == report.Key.ResourceId).Select(x => x.FormattedEmployeeId).FirstOrDefault(),
                                                             ResourceId = report.Key.ResourceId == null ? 0 : (int)report.Key.ResourceId,
                                                             ResourceName = (report.Key.FirstName ?? "") + " " + (report.Key.LastName ?? ""),
                                                             RoleName = report.Key.RoleFullName ?? "",
                                                             PlannedHours = string.Format("{0}:{1}", (new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Days * 24 +
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Hours),
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.RequiredHours.Value.Ticks)).Minutes),
                                                             ClockedHours = string.Format("{0}:{1}", (new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Days * 24 +
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Hours),
                                                                              new TimeSpan(report.Sum(x => x == null ? 0 : x.ClockedHours.Value.Ticks)).Minutes),
                                                             ProjectName = report.Key.ProjectName ?? ""
                                                         }).ToList();
                }
                if (reportData?.TimesheetReportDetails?.Count > 0)
                {
                    foreach (var item in reportData?.TimesheetReportDetails)
                    {
                        List<string> empskillset = (from es in employeeMasterData?.EmployeesSkillset
                                                    join sk in employeeMasterData?.Skillsets on es.SkillsetId equals sk.SkillsetId
                                                    where es.EmployeeId == item.ResourceId
                                                    select sk.Skillset).ToList();
                        if (empskillset?.Count > 0)
                        {
                            item.SKillSet = string.Join(",", empskillset);
                        }
                    }
                }
                if (reportData?.ResourceWiseWeeklyTimesheetReport?.Count > 0 || reportData?.TimesheetReportDetails?.Count > 0)
                {
                    //Assign project name
                    List<int> resourceWeekly = reportData?.ResourceWiseWeeklyTimesheetReport.Select(x => x.ProjectId).ToList();
                    List<int> timesheetReport = reportData?.TimesheetReportDetails?.Select(x => x.ProjectId).ToList();
                    var projectNameResult = await _client.PostAsJsonAsync(resourceWeekly.Concat(timesheetReport).Distinct().ToList(), _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectNameById"));
                    List<ProjectNames> lstProjectName = JsonConvert.DeserializeObject<List<ProjectNames>>(JsonConvert.SerializeObject(projectNameResult.Data));
                    reportData?.ResourceWiseWeeklyTimesheetReport.ForEach(x => x.ProjectName = projectMasterData?.ProjectDetails?.Where(y => y.ProjectId == x.ProjectId).Select(z => z.ProjectName).FirstOrDefault());
                    reportData?.TimesheetReportDetails.ForEach(x => x.ProjectName = projectMasterData?.ProjectDetails?.Where(y => y.ProjectId == x.ProjectId).Select(z => z.ProjectName).FirstOrDefault());
                }
                if (reportData?.ResourceWiseWeeklyTimesheetReport?.Count > 0 || reportData?.ResourceWiseTimesheetPlannedActualReport?.Count > 0 || reportData?.TimesheetReportDetails?.Count > 0)
                {
                    List<int> resourceWeekly = reportData?.ResourceWiseWeeklyTimesheetReport?.Select(x => x.ResourceId).ToList();
                    List<int> resourceTimesheet = reportData?.ResourceWiseTimesheetPlannedActualReport?.Select(x => x.ResourceId).ToList();
                    List<int> timesheetReport = reportData?.TimesheetReportDetails?.Select(x => x.ResourceId).ToList();
                    //Get employee name
                    var employeeNameResult = await _client.PostAsJsonAsync(resourceWeekly.Concat(resourceTimesheet).Concat(timesheetReport).Distinct().ToList(), _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeNameById"));
                    lstEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeeNameResult.Data));
                    reportData?.ResourceWiseWeeklyTimesheetReport.ForEach(x => x.ResourceName = lstEmployeeName.Where(y => y.EmployeeId == x.ResourceId).Select(z => z.EmployeeFullName).FirstOrDefault());
                    reportData?.ResourceWiseTimesheetPlannedActualReport.ForEach(x => x.ResourceName = lstEmployeeName.Where(y => y.EmployeeId == x.ResourceId).Select(z => z.EmployeeFullName).FirstOrDefault());
                    reportData?.TimesheetReportDetails.ForEach(x => x.ResourceName = lstEmployeeName.Where(y => y.EmployeeId == x.ResourceId).Select(z => z.EmployeeFullName).FirstOrDefault());
                }
                return Ok(new
                {
                    masterDataResult.StatusCode,
                    masterDataResult.StatusText,
                    reportData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetTimesheetReport", " ResourceId- " + resourceId.ToString() + " AllTimesheet- " + isAllTimesheet.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                reportData
            });
        }
        #endregion

        #region Get home projects report
        [HttpGet]
        [Route("GetHomeProjectReport")]
        public async Task<IActionResult> GetHomeProjectReport(int resourceId, bool isAllProject)
        {
            HomeProjectReport projectReportData = new();
            try
            {
                List<int> listResourceId = new();
                if (isAllProject)
                {
                    var listOfEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeListForManager") + resourceId);
                    List<EmployeeList> listOfEmployees = JsonConvert.DeserializeObject<List<EmployeeList>>(JsonConvert.SerializeObject(listOfEmployeesResult.Data));
                    if (listOfEmployees?.Count > 0)
                    {
                        listResourceId = listOfEmployees.Select(x => x.EmployeeId).Distinct().ToList();
                    }
                }
                else
                {
                    listResourceId = new List<int> { resourceId };
                }
                //Get accounts
                var accountResult = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetAllAccountsDetails"));
                List<AccountDetails> AccountDetails = JsonConvert.DeserializeObject<List<AccountDetails>>(JsonConvert.SerializeObject(accountResult.Data));
                //Get projects
                var projectResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectReportMasterData"));
                ProjectReportMasterData ProjectData = JsonConvert.DeserializeObject<ProjectReportMasterData>(JsonConvert.SerializeObject(projectResult.Data));
                List<ReportData> reportData = (from PD in ProjectData?.ProjectDetails
                                               where (
                                                    listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                    listResourceId.Contains(PD.FinanceManagerId == null ? 0 : (int)PD.FinanceManagerId) ||
                                                    listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                    listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC))
                                               group PD by PD.ProjectStatus into report
                                               select new ReportData { Status = report.Key, Count = report.Count() }).ToList();
                List<ProjectReport> projectReport = (from PD in ProjectData?.ProjectDetails
                                                     join PT in ProjectData?.ProjectType on PD.ProjectTypeId equals PT.ProjectTypeId
                                                     where (
                                                            listResourceId.Contains(PD.CreatedBy == null ? 0 : (int)PD.CreatedBy) ||
                                                            listResourceId.Contains(PD.FinanceManagerId == null ? 0 : (int)PD.FinanceManagerId) ||
                                                            listResourceId.Contains(PD.EngineeringLeadId == null ? 0 : (int)PD.EngineeringLeadId) ||
                                                            listResourceId.Contains(PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC))
                                                     select new ProjectReport
                                                     {
                                                         FormattedProjectId = PD.FormattedProjectId,
                                                         ProjectId = PD.ProjectId,
                                                         ProjectName = PD.ProjectName,
                                                         ProjectType = PT.ProjectTypeDescription,
                                                         ProjectStartDate = PD.ProjectStartDate,
                                                         ProjectEndDate = PD.ProjectEndDate,
                                                         ProjectDuration = PD.ProjectDuration,
                                                         ProjectStatus = PD.ProjectStatus,
                                                         AccountId = PD.AccountId == null ? 0 : (int)PD.AccountId,
                                                         OwnerName = AccountDetails.Where(x => x.AccountId == PD.AccountId).Select(x => (x.DirectorFirstName ?? "") + " " + (x.DirectorLastName ?? "")).FirstOrDefault(),
                                                         AccountName = AccountDetails.Where(x => x.AccountId == PD.AccountId).Select(x => x.AccountName).FirstOrDefault(),
                                                         ProjectSPOCId = PD.ProjectSPOC == null ? 0 : (int)PD.ProjectSPOC,
                                                         ProjectTeamDetail = ProjectData?.ResourceAllocation.Where(x => x.ProjectId == PD.ProjectId && x.EmployeeId != null).Select(x =>
                                                                   new ProjectTeamDetail { EmployeeId = x.EmployeeId }).ToList()
                                                     }).ToList();
                //Get Spoc name
                if (projectReport?.Count > 0)
                {
                    List<int> employeeList = new();
                    employeeList = projectReport.Select(x => x.ProjectSPOCId).ToList();
                    foreach (var item in projectReport)
                    {
                        if (item?.ProjectTeamDetail?.Count > 0)
                        {
                            employeeList = employeeList.Concat(item?.ProjectTeamDetail?.Select(x => x.EmployeeId == null ? 0 : (int)x.EmployeeId).ToList()).ToList();
                        }
                    }
                    var spocNameResult = await _client.PostAsJsonAsync(employeeList.Distinct().ToList(), _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeNameById"));
                    List<EmployeeName> lstEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(spocNameResult.Data));
                    projectReport.ForEach(x => x.ProjectSPOC = lstEmployeeName.Where(y => y.EmployeeId == x.ProjectSPOCId).Select(z => z.EmployeeFullName).FirstOrDefault());
                    foreach (var item in projectReport)
                    {
                        item?.ProjectTeamDetail.ForEach(x => x.EmployeeName = lstEmployeeName.Where(y => y.EmployeeId == x.EmployeeId).Select(z => z.EmployeeFullName).FirstOrDefault());
                    }
                }
                projectReportData.ReportData = reportData;
                projectReportData.ReportDetail = projectReport;
                projectReportData.TotalProject = projectReport.Select(x => x.ProjectId).Count();
                projectReportData.TotalCustomerProject = projectReport.Where(x => x.ProjectType != "Non Billable").Select(x => x.ProjectId).Count();
                projectReportData.TotalInternalProject = projectReport.Where(x => x.ProjectType == "Non Billable").Select(x => x.ProjectId).Count();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    ReportData = projectReportData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetProjectReport", " ResourceId- " + resourceId.ToString() + " AllProject- " + isAllProject.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ReportData = projectReportData
            });
        }
        #endregion

        #region Get Home Resource Utilization By ResourceId
        [HttpGet]
        [Route("GetHomeProjectsResourceUtilizationReport")]
        public IActionResult GetHomeProjectsResourceUtilizationReport(int pResourceId, DateTime? pWeekStartDate)
        {
            List<ProjectNames> projectsList = new();
            List<TimesheetUtilizationView> projectTimesheet = new();
            List<TimesheetUtilizationView> projectTimesheetReport = new();
            TimesheetUtilizationReport TimesheetUtilizationReport = new();
            try
            {
                var listOfProjects = _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectsByResourceId") + pResourceId);
                projectsList = JsonConvert.DeserializeObject<List<ProjectNames>>(JsonConvert.SerializeObject(listOfProjects.Result.Data));
                var projectResult = _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetUtilizationDetails") + pResourceId);
                projectTimesheet = JsonConvert.DeserializeObject<List<TimesheetUtilizationView>>(JsonConvert.SerializeObject(projectResult.Result.Data));
                if (projectsList != null)
                {
                    foreach (var projectIds in projectsList)
                    {
                        var timelog = (from project in projectTimesheet
                                       where project.ResourceId == pResourceId && project.ProjectId == projectIds.ProjectId
                                       && project.Date >= pWeekStartDate.Value.Date
                                       && project.Date <= pWeekStartDate.Value.Date.AddDays(6)
                                       group project by project.Date into log
                                       select new
                                       {
                                           PlannedHour = new TimeSpan(log.Sum(x => x.PlannedHour == null ? 0 : x.PlannedHour.Value.Ticks)),
                                           ActualHour = new TimeSpan(log.Sum(x => x.ActualHour == null ? 0 : x.ActualHour.Value.Ticks)),
                                           Date = log.Key
                                       });
                        foreach (var logs in timelog)
                        {
                            TimesheetUtilizationView Timeutilization = new()
                            {
                                ProjectId = projectIds.ProjectId,
                                ResourceId = pResourceId,
                                ProjectName = projectIds.ProjectName,
                                PlannedHour = logs.PlannedHour,
                                ActualHour = logs.ActualHour,
                                Date = logs.Date
                            };
                            projectTimesheetReport.Add(Timeutilization);
                        }
                    }
                    TimesheetUtilizationReport TimesheetUtilization = new()
                    {
                        projectNamesList = projectsList,
                        timesheetUtilization = projectTimesheetReport
                    };
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        ReportData = TimesheetUtilization
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetHomeProjectsResourceUtilizationReport", " ResourceId- " + pResourceId.ToString() + " WeekStartDate- " + pWeekStartDate.ToString());
            }
            return Ok(new
            {
                StatusCode = "SUCCESS",
                StatusText = strErrorMsg,
                ReportData = TimesheetUtilizationReport
            });
        }
        #endregion

        #region Get Home Employee Utilization Report By ResourceId
        [HttpGet]
        [Route("GetHomeEmployeeResourceUtilizationReport")]
        public IActionResult GetHomeEmployeeResourceUtilizationReport(int pResourceId, DateTime? pWeekStartDate)
        {
            List<EmployeeProjectNames> projectsList = new();
            List<EmployeeTimeUtilizationView> employeeTimesheet = new();
            List<EmployeeTimeUtilizationView> employeeTimesheetReport = new();
            try
            {
                var listOfProjects = _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectsAndEmpByResourceId") + pResourceId);
                projectsList = JsonConvert.DeserializeObject<List<EmployeeProjectNames>>(JsonConvert.SerializeObject(listOfProjects.Result.Data));
                if (projectsList != null)
                {
                    var projectResult = _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetEmployeeTimesheetUtilizationDetails") + pResourceId);
                    employeeTimesheet = JsonConvert.DeserializeObject<List<EmployeeTimeUtilizationView>>(JsonConvert.SerializeObject(projectResult.Result.Data));

                    foreach (var emplyeeIds in projectsList)
                    {
                        var timelog = (from employee in employeeTimesheet
                                       where employee.EmployeeId == emplyeeIds.EmployeeId && employee.ResourceId == pResourceId
                                       && employee.Date >= pWeekStartDate.Value.Date
                                       && employee.Date <= pWeekStartDate.Value.Date.AddDays(6)
                                       group employee by employee.EmployeeId into log
                                       select new
                                       {
                                           EmployeeId = log.Key,
                                           PlannedHour = new TimeSpan(log.Sum(x => x.PlannedHour == null ? 0 : x.PlannedHour.Value.Ticks)),
                                           ActualHour = new TimeSpan(log.Sum(x => x.ActualHour == null ? 0 : x.ActualHour.Value.Ticks)),
                                           Utilization = (log.Sum(x => x.ActualHour.Value.Ticks) / log.Sum(x => x.PlannedHour.Value.Ticks)) * 100
                                       });
                        foreach (var logs in timelog)
                        {
                            EmployeeTimeUtilizationView Timeutilization = new()
                            {
                                EmployeeId = emplyeeIds.EmployeeId,
                                EmployeeName = " ",
                                ProjectId = emplyeeIds.ProjectId,
                                ProjectName = emplyeeIds.ProjectName,
                                IsBillable = emplyeeIds.IsBillable,
                                ResourceId = pResourceId,
                                PlannedHour = logs.PlannedHour,
                                ActualHour = logs.ActualHour,
                                Utilization = (decimal)logs.Utilization
                            };
                            employeeTimesheetReport.Add(Timeutilization);
                        }
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    ReportData = employeeTimesheetReport
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetHomeEmployeeResourceUtilizationReport", " ResourceId- " + pResourceId.ToString() + " WeekStartDate- " + pWeekStartDate.ToString());
            }
            return Ok(new
            {
                StatusCode = "SUCCESS",
                StatusText = strErrorMsg,
                ReportData = employeeTimesheetReport
            });
        }
        #endregion

        #region GetHome Employee Timesheet Detail Report
        [HttpGet]
        [Route("GetHomeEmployeeTimesheetDetailReport")]
        public async Task<IActionResult> GetHomeEmployeeTimesheetDetailReport(int pResourceId, DateTime? pWeekStartDate)
        {
            List<EmployeeProjectNames> projectList = new();
            List<EmployeeTimesheetHourView> timesheetdetail = new();
            List<EmployeeTimesheetDetailsView> EmployeeTimesheetDetails = new();
            List<EmployeeTimesheetGridView> timesheetData = new();
            List<EmployeeTimesheetGridView> Timesheetdetail = new();
            List<Timesheetgrid> Timesheetgrid = new();
            WeeklyLeaveHolidayOverview WeeklyLeaveHolidayOverview = new();
            List<WeeklyOverview> WeeklyOverview = new();
            HomeTimesheetReportData homeTimesheetReportData = new();
            try
            {
                var listOfProjects = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetProjectsByEmployeeId") + pResourceId);
                projectList = JsonConvert.DeserializeObject<List<EmployeeProjectNames>>(JsonConvert.SerializeObject(listOfProjects.Data));
                if (projectList != null)
                {
                    var projectResult = await _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetEmployeeTimesheetByEmployeeId") + pResourceId);
                    timesheetdetail = JsonConvert.DeserializeObject<List<EmployeeTimesheetHourView>>(JsonConvert.SerializeObject(projectResult.Data));
                    foreach (var project in projectList)
                    {
                        var employeeTimesheet = (from timesheet in timesheetdetail
                                                 where timesheet.employeeId == pResourceId
                                                 && timesheet.projectId == project.ProjectId
                                                 && timesheet.date >= pWeekStartDate.Value.Date
                                                 && timesheet.date <= pWeekStartDate.Value.Date.AddDays(6)
                                                 group timesheet by timesheet.projectId into pro
                                                 select new
                                                 {
                                                     ProjectId = pro.Key,
                                                     RequiredHours = pro.Sum(x => x.RequiredHours.Value.Ticks),
                                                     ClockedHours = pro.Sum(x => x.clockedHours.Value.Ticks),
                                                     Percentage = (pro.Sum(x => x.clockedHours.Value.Ticks) / pro.Sum(x => x.RequiredHours.Value.Ticks)) * 100
                                                 }).FirstOrDefault();
                        EmployeeTimesheetDetailsView EmployeeTimesheetDetailsView = new()
                        {
                            Type = "timespentbyproject",
                            ProjectId = employeeTimesheet == null ? 0 : employeeTimesheet.ProjectId,
                            ProjectName = project.ProjectName ?? "",
                            Hours = employeeTimesheet == null ? "00" : string.Format("{0}:{1}", new TimeSpan(employeeTimesheet.ClockedHours).Days * 24 + new TimeSpan(employeeTimesheet.ClockedHours).Hours, new TimeSpan(employeeTimesheet.ClockedHours).Minutes),
                            Percentage = employeeTimesheet == null ? 00 : (decimal)employeeTimesheet.Percentage
                        };
                        EmployeeTimesheetDetails.Add(EmployeeTimesheetDetailsView);
                    }
                    homeTimesheetReportData.EmployeeTimesheetDetails = EmployeeTimesheetDetails;

                    var projectResult1 = await _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetGridDetailByEmployeeId") + pResourceId);
                    timesheetData = JsonConvert.DeserializeObject<List<EmployeeTimesheetGridView>>(JsonConvert.SerializeObject(projectResult1.Data));
                    foreach (var project in projectList)
                    {
                        var Timesheet = (from timesheet in timesheetData
                                         where timesheet.EmployeeId == pResourceId
                                         && timesheet.projectId == project.ProjectId
                                         && timesheet.Date >= pWeekStartDate.Value.Date
                                         && timesheet.Date <= pWeekStartDate.Value.Date.AddDays(6)
                                         group timesheet by timesheet.projectId into pro
                                         select new
                                         {
                                             status = pro.Select(x => x.status).FirstOrDefault(),
                                             Utilization = (pro.Sum(x => x.clockedHour.Value.TotalHours) + " / " + pro.Sum(x => x.RequiredHour.Value.TotalHours)),
                                         }).ToList();
                        var Timesheet1 = (from timesheet in timesheetData
                                          where timesheet.EmployeeId == pResourceId
                                          && timesheet.projectId == project.ProjectId
                                          && timesheet.Date >= pWeekStartDate.Value.Date
                                          && timesheet.Date <= pWeekStartDate.Value.Date.AddDays(6)
                                          select new
                                          {
                                              timesheet.Date,
                                              Hours = timesheet.clockedHour
                                          }).ToList();
                        int totalDays = 6; DateTime wkdate = pWeekStartDate.Value.Date;
                        List<DatesandHours> EmployeeTime = new();
                        for (int days = 0; days <= totalDays; days++)
                        {
                            int i = 0;
                            bool isExists = false;
                            foreach (var item in Timesheet1?.Select(x => x.Date))
                            {
                                if (wkdate.Date == item?.Date)
                                {
                                    DatesandHours EmployeeTimesheet = new()
                                    {
                                        Dates = item?.Date,
                                        Hours = Timesheet1[i].Hours.ToString().Split(":")[0] + ":" + Timesheet1[i].Hours.ToString().Split(":")[1],
                                    };
                                    EmployeeTime.Add(EmployeeTimesheet);
                                    i++;
                                    isExists = true;
                                    continue;
                                }
                            }
                            if (!isExists)
                            {
                                DatesandHours EmployeeTimesheet = new()
                                {
                                    Dates = wkdate.Date,
                                    Hours = "00:00",
                                };
                                EmployeeTime.Add(EmployeeTimesheet);
                            }
                            wkdate = wkdate.AddDays(1);
                        }
                        List<DatesandHours> TotalEmployeeTime = new();
                        foreach (var datesandHours in EmployeeTime.GroupBy(x => new { x.Dates }).OrderBy(x => x.Key.Dates))
                        {
                            DatesandHours EmployeeTimesheet = new()
                            {
                                Dates = datesandHours.Key.Dates,
                                Hours = Timesheet1.Where(x => x.Date.Value == datesandHours.Key.Dates.Value).Sum(x => x.Hours.Value.Hours).ToString()
                            };
                            TotalEmployeeTime.Add(EmployeeTimesheet);
                        }
                        Timesheetgrid EmployeeTimesheetGrid = new()
                        {
                            Type = "timesheetdetail",
                            EmployeeId = pResourceId,
                            projectId = project.ProjectId,
                            projectName = project == null ? "" : project.ProjectName,
                            status = Timesheet.Count > 0 ? Timesheet.Select(x => x.status).FirstOrDefault() : "Yet to submit",
                            Utilization = Timesheet.Count > 0 ? Timesheet.Select(x => x.Utilization).FirstOrDefault() : "0/0",
                            DatesandHours = EmployeeTime,
                            TotalEmployeeTime = TotalEmployeeTime
                        };
                        Timesheetgrid.Add(EmployeeTimesheetGrid);
                    }
                    homeTimesheetReportData.Timesheetgrid = Timesheetgrid;

                    var WeeklyResult = await _client.GetAsync(_leaveBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetWeeklyLeavesHolidayByEmployeeId") + pResourceId);
                    WeeklyLeaveHolidayOverview = JsonConvert.DeserializeObject<WeeklyLeaveHolidayOverview>(JsonConvert.SerializeObject(WeeklyResult.Data));
                    if (WeeklyLeaveHolidayOverview.LeaveData != null && WeeklyLeaveHolidayOverview.LeaveData.Count > 0)
                    {
                        var leaves = (from week in WeeklyLeaveHolidayOverview.LeaveData
                                      where week.EmployeeId == pResourceId
                                      && week.FromDate >= pWeekStartDate.Value.Date
                                      && week.ToDate <= pWeekStartDate.Value.Date.AddDays(6)
                                      group week by week.EmployeeId into leave
                                      select new
                                      {
                                          Type = "Leave",
                                          Hours = leave.Sum(x => x.NoOfDays) * 8,
                                          Percentage = leave.Sum(x => x.NoOfDays) / leave.Sum(x => x.TotalLeaves) * 100
                                      });
                        WeeklyOverview lOverview = new()
                        {
                            Type = leaves.Select(x => x.Type).FirstOrDefault(),
                            Hours = leaves.Select(x => x.Hours.ToString()).FirstOrDefault(),
                            Percentage = leaves.Select(x => x.Percentage).FirstOrDefault()
                        };
                        WeeklyOverview.Add(lOverview);
                    }
                    if (WeeklyLeaveHolidayOverview.HolidaysData != null && WeeklyLeaveHolidayOverview.HolidaysData.Count > 0)
                    {
                        var holidays = (from week in WeeklyLeaveHolidayOverview.HolidaysData
                                        where week.EmployeeDepartmentId == pResourceId
                                         && week.HolidayDate >= pWeekStartDate.Value.Date
                                         && week.HolidayDate <= pWeekStartDate.Value.Date.AddDays(6)
                                        group week by week.EmployeeDepartmentId into holiday
                                        select new
                                        {
                                            Type = "Holiday",
                                            Hours = (holiday.Select(x => x.HolidayDate).Count()) * 8,
                                            Percentage = holiday.Select(x => x.HolidayDate).Count() / holiday.Select(x => x.HolidayDate).Count() * 100
                                        }).ToList();
                        if (holidays != null && holidays.Count > 0)
                        {
                            WeeklyOverview hOverview = new()
                            {
                                Type = holidays.Select(x => x.Type).FirstOrDefault(),
                                Hours = holidays.Select(x => x.Hours.ToString()).FirstOrDefault(),
                                Percentage = holidays.Select(x => x.Percentage).FirstOrDefault()
                            };
                            WeeklyOverview.Add(hOverview);
                        }
                    }
                    if (timesheetdetail != null && timesheetdetail.Count > 0)
                    {
                        var Customer = (from timesheet in timesheetdetail
                                        join pro in projectList
                                        on timesheet.projectId equals pro.ProjectId
                                        where pro.EmployeeId == pResourceId
                                        && timesheet.date >= pWeekStartDate.Value.Date
                                        && timesheet.date <= pWeekStartDate.Value.Date.AddDays(6)
                                        && (pro.IsBillable == "Billable" || pro.IsBillable == "Time and material" || pro.IsBillable == "Fix bid")
                                        group timesheet by timesheet.employeeId into time
                                        select new
                                        {
                                            Type = "Customer",
                                            Hours = time.Sum(x => x.clockedHours.Value.Ticks),
                                            Percentage = time.Sum(x => x.clockedHours.Value.TotalHours) / time.Sum(x => x.RequiredHours.Value.TotalHours) * 100
                                        }).ToList();
                        if (Customer != null && Customer.Count > 0)
                        {
                            WeeklyOverview COverview = new()
                            {
                                Type = Customer.Select(x => x.Type).FirstOrDefault(),
                                Hours = Customer.Select(x => string.Format("{0}:{1}", new TimeSpan(x.Hours).Days * 24 + new TimeSpan(x.Hours).Hours, new TimeSpan(x.Hours).Minutes)).FirstOrDefault(),
                                Percentage = (int)Customer.Select(x => x.Percentage).FirstOrDefault()
                            };
                            WeeklyOverview.Add(COverview);
                        }
                        var Internl = (from timesheet in timesheetdetail
                                       join pro in projectList
                                       on timesheet.projectId equals pro.ProjectId
                                       where pro.EmployeeId == pResourceId
                                       && timesheet.date >= pWeekStartDate.Value.Date
                                       && timesheet.date <= pWeekStartDate.Value.Date.AddDays(6)
                                       && pro.IsBillable == "nonBillable"
                                       group timesheet by timesheet.employeeId into time
                                       select new
                                       {
                                           Type = "Customer",
                                           Hours = time.Sum(x => x.clockedHours.Value.Ticks),
                                           Percentage = time.Sum(x => x.clockedHours.Value.TotalHours) / time.Sum(x => x.RequiredHours.Value.TotalHours) * 100
                                       }).ToList();
                        if (Internl != null && Internl.Count > 0)
                        {
                            WeeklyOverview IOverview = new()
                            {
                                Type = Internl.Select(x => x.Type).FirstOrDefault(),
                                Hours = Internl.Select(x => string.Format("{0}:{1}", new TimeSpan(x.Hours).Days * 24 + new TimeSpan(x.Hours).Hours, new TimeSpan(x.Hours).Minutes)).FirstOrDefault(),
                                Percentage = (int)Internl.Select(x => x.Percentage).FirstOrDefault()
                            };
                            WeeklyOverview.Add(IOverview);
                        }
                    }
                    homeTimesheetReportData.WeeklyOverview = WeeklyOverview;
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        Data = homeTimesheetReportData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetHomeEmployeeTimesheetDetailReport", " ResourceId - " + pResourceId.ToString() + " WeekStartDate - " + pWeekStartDate.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                statusText = strErrorMsg,
                Data = homeTimesheetReportData
            });
        }
        #endregion

        #region Get Home Resource Wise Timesheet Report By Manager EmployeeId
        [HttpGet]
        [Route("GetResourceWiseTimesheetReportByEmployeeId")]
        public IActionResult GetResourceWiseTimesheetReportByEmployeeId(int pResourceId, DateTime pWeekStartDate)
        {
            List<Employees> employeesList = new();
            List<EmployeeTimeUtilizationView> employeeTimesheet = new();
            List<EmployeeTimeUtilizationView> employeeTimesheetReport = new();
            List<EmployeeTimesheetWeeklyReport> EmployeeTimesheetWeeklyReport = new();
            try
            {
                var employees = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesForManagerId") + pResourceId);
                employeesList = JsonConvert.DeserializeObject<List<Employees>>(JsonConvert.SerializeObject(employees.Result.Data));
                if (employeesList != null)
                {
                    var projectResult = _client.GetAsync(_timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetEmployeeTimesheetUtilizationDetails") + pResourceId);
                    employeeTimesheet = JsonConvert.DeserializeObject<List<EmployeeTimeUtilizationView>>(JsonConvert.SerializeObject(projectResult.Result.Data));

                    foreach (var emplyeeIds in employeesList)
                    {
                        var timelog = (from employee in employeeTimesheet
                                       where employee.EmployeeId == emplyeeIds.EmployeeID
                                       && employee.Date >= pWeekStartDate.Date
                                       && employee.Date <= pWeekStartDate.Date.AddDays(6)
                                       group employee by employee.EmployeeId into log
                                       select new
                                       {
                                           status = log.Select(x => x.Status).FirstOrDefault(),
                                           Utilization = log.Sum(x => x.ActualHour.Value.Ticks) + " / " + log.Sum(x => x.PlannedHour.Value.Ticks)
                                       }).ToList();
                        var timeloggedHours = (from employee in employeeTimesheet
                                               where employee.EmployeeId == emplyeeIds.EmployeeID
                                               && employee.Date >= pWeekStartDate.Date
                                               && employee.Date <= pWeekStartDate.Date.AddDays(6)
                                               select new
                                               {
                                                   employee.Date,
                                                   Hours = employee.ActualHour
                                               }).ToList();
                        int i = 0;
                        List<WeekDatesandHours> EmployeeTime = new();
                        foreach (var item in timeloggedHours.Select(x => x.Date))
                        {
                            WeekDatesandHours EmployeeTimesheet = new()
                            {
                                Dates = item.Date,
                                Hours = timeloggedHours[i].Hours.ToString()
                            };
                            EmployeeTime.Add(EmployeeTimesheet);
                            i++;
                        };
                        EmployeeTimesheetWeeklyReport Timesheet = new()
                        {
                            EmployeeId = emplyeeIds.EmployeeID,
                            EmployeeName = emplyeeIds.FirstName + "" + emplyeeIds.LastName,
                            Utilization = timelog.Select(x => x.Utilization).FirstOrDefault(),
                            Status = timelog.Select(x => x.status).ToString(),
                            WeekDatesandHours = EmployeeTime
                        };
                        EmployeeTimesheetWeeklyReport.Add(Timesheet);
                    }
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    ReportData = EmployeeTimesheetWeeklyReport
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetResourceWiseTimesheetReportByEmployeeId", " ResourceId- " + pResourceId.ToString() + " WeekStartDate- " + pWeekStartDate.ToString());
            }
            return Ok(new
            {
                StatusCode = "SUCCESS",
                StatusText = strErrorMsg,
                ReportData = EmployeeTimesheetWeeklyReport
            });
        }
        #endregion

        #region Get Employee Resource Availability Report
        [HttpGet]
        [Route("GetResourceAvailabilityReport")]
        public async Task<IActionResult> GetResourceAvailabilityReport(int pResourceId, DateTime? pDate)
        {
            List<ResourceAvailability> percentList = new();
            List<EmployeeDetails> employeeList = new();
            List<EmployeeDepartmentAvailability> EmployeeDepartmentAvailability = new();
            List<EmployeeAvailabilityGridView> EmployeeAvailabilityGrid = new();
            List<EmployeeAvailability> EmployeeAvailability = new();
            ResourceAvailabilityReport availabilityReports = new();
            try
            {
                //Availability Percentage
                var listOfPercentage = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetResourceAvailabilityEmployeeDetails") + pResourceId);
                percentList = JsonConvert.DeserializeObject<List<ResourceAvailability>>(JsonConvert.SerializeObject(listOfPercentage.Data));
                if (percentList != null)
                {
                    var Available = from pro in percentList
                                    group pro by pro.ProjectId into percent
                                    select new
                                    {
                                        AllocatedPercent = percent.Sum(x => x.AllocationPercent),
                                        TotalPercent = percent.Select(x => x.EmployeeId).Count() * 100
                                    };
                    var UnAvailable = from pro in percentList
                                      where pro.AllocationPercent == 100
                                      group pro by pro.ProjectId into percent
                                      select new
                                      {
                                          AllocatedPercent = percent.Sum(x => x.AllocationPercent)
                                      };
                    var InnovationHub = from pro in percentList
                                        where pro.AllocationPercent == 0
                                        group pro by pro.ProjectId into percent
                                        select new
                                        {
                                            AllocatedPercent = percent.Sum(x => x.AllocationPercent)
                                        };
                    double AvailablePercent = (Available.Select(x => x.AllocatedPercent).FirstOrDefault().HasValue ? (double)Available.Select(x => x.AllocatedPercent).FirstOrDefault() / Available.Select(x => x.TotalPercent).FirstOrDefault() : 0) * 100;
                    double UnAvailablePercent = (UnAvailable.Select(x => x.AllocatedPercent).FirstOrDefault().HasValue ? (double)UnAvailable.Select(x => x.AllocatedPercent).FirstOrDefault() / Available.Select(x => x.TotalPercent).FirstOrDefault() : 0) * 100;
                    double InnovationHubPercent = (InnovationHub.Select(x => x.AllocatedPercent).FirstOrDefault().HasValue ? (double)InnovationHub.Select(x => x.AllocatedPercent).FirstOrDefault() / Available.Select(x => x.TotalPercent).FirstOrDefault() : 0) * 100;
                    EmployeeAvailability.Add(new EmployeeAvailability { Type = "Available", Count = AvailablePercent });
                    EmployeeAvailability.Add(new EmployeeAvailability { Type = "UnAvailable", Count = UnAvailablePercent });
                    EmployeeAvailability.Add(new EmployeeAvailability { Type = "InnovationHub", Count = InnovationHubPercent });
                    //Grid Date
                    var employees = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAvailabilityDetails") + pResourceId);
                    employeeList = JsonConvert.DeserializeObject<List<EmployeeDetails>>(JsonConvert.SerializeObject(employees.Data));
                    EmployeeAvailabilityGrid = (from pro in percentList
                                                join emp in employeeList on pro.EmployeeId equals emp.EmployeeId
                                                select new EmployeeAvailabilityGridView
                                                {
                                                    EmployeeId = emp.EmployeeId,
                                                    EmployeeName = emp.EmployeeName,
                                                    Department = emp.Department,
                                                    Role = emp.Role,
                                                    SkillSet = emp.Skillset,
                                                    UtilizedPercent = (double)pro.AllocationPercent,
                                                    AvailablePercent = (double)pro.Availability
                                                }).ToList();
                    //Department wise
                    var DepartmentIds = employeeList.Select(x => x.DepartmentId).Distinct().ToList();
                    foreach (var departmentId in DepartmentIds)
                    {
                        var DeptAvailable = from pro in percentList
                                            join emp in employeeList on pro.EmployeeId equals emp.EmployeeId
                                            where emp.DepartmentId == departmentId
                                            group pro by pro.ProjectId into percent
                                            select new
                                            {
                                                AllocatedPercent = percent.Sum(x => x.AllocationPercent),
                                                TotalPercent = percent.Select(x => x.EmployeeId).Count() * 100
                                            };
                        var DeptUnAvailable = from pro in percentList
                                              join emp in employeeList on pro.EmployeeId equals emp.EmployeeId
                                              where emp.DepartmentId == departmentId && pro.AllocationPercent == 100
                                              group pro by pro.ProjectId into percent
                                              select new
                                              {
                                                  AllocatedPercent = percent.Sum(x => x.AllocationPercent)
                                              };
                        var DeptInnovationHub = from pro in percentList
                                                join emp in employeeList on pro.EmployeeId equals emp.EmployeeId
                                                where emp.DepartmentId == departmentId && pro.AllocationPercent == 0
                                                group pro by pro.ProjectId into percent
                                                select new
                                                {
                                                    AllocatedPercent = percent.Sum(x => x.AllocationPercent)
                                                };
                        var Department = employeeList.Where(x => x.DepartmentId == departmentId).Select(x => x.Department).FirstOrDefault();
                        EmployeeDepartmentAvailability Availability = new()
                        {
                            DepartmentId = (int)departmentId,
                            Department = Department,
                            Available = (DeptAvailable.Select(x => x.AllocatedPercent).FirstOrDefault().HasValue ? (double)DeptAvailable.Select(x => x.AllocatedPercent).FirstOrDefault() / DeptAvailable.Select(x => x.TotalPercent).FirstOrDefault() : 0) * 100,
                            UnAvailable = (DeptUnAvailable.Select(x => x.AllocatedPercent).FirstOrDefault().HasValue ? (double)DeptUnAvailable.Select(x => x.AllocatedPercent).FirstOrDefault() / DeptAvailable.Select(x => x.TotalPercent).FirstOrDefault() : 0) * 100,
                            InnovationHub = (DeptInnovationHub.Select(x => x.AllocatedPercent).FirstOrDefault().HasValue ? (double)DeptInnovationHub.Select(x => x.AllocatedPercent).FirstOrDefault() / DeptAvailable.Select(x => x.TotalPercent).FirstOrDefault() : 0) * 100
                        };
                        EmployeeDepartmentAvailability.Add(Availability);
                    }
                    ResourceAvailabilityReport ResourceAvailabilityReport = new()
                    {
                        EmployeeAvailability = EmployeeAvailability,
                        EmployeeAvailabilityGridView = EmployeeAvailabilityGrid,
                        EmployeeDepartmentAvailability = EmployeeDepartmentAvailability
                    };
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        ReportData = ResourceAvailabilityReport
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetResourceAvailabilityReport", " ResourceId- " + pResourceId.ToString() + " WeekStartDate- " + pDate.ToString());
            }
            return Ok(new
            {
                StatusCode = "SUCCESS",
                StatusText = strErrorMsg,
                ReportData = availabilityReports
            });
        }
        #endregion

        #region Get Associate Report
        [HttpGet]
        [Route("GetAssociateReport")]
        public IActionResult GetAssociateReport(int employeeId)
        {
            List<EmployeeAssociates> EmployeeList = new();
            List<TotalEmployees> TotalEmployeesReport = new();
            List<LocationwiseEmployee> LocationwiseEmployeeReport = new();
            List<ResourceAvailability> inProjectEmployee = new();
            List<InnovationHub> InnovationHub = new();
            EmployeeAssociateReport EmployeeAssociate = new();
            try
            {
                //Total Employee
                var Employee = _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeDetailByManagerId") + employeeId);
                EmployeeList = JsonConvert.DeserializeObject<List<EmployeeAssociates>>(JsonConvert.SerializeObject(Employee.Result.Data));
                int pResourceId = employeeId;
                var InProEmployee = _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetResourceAvailabilityEmployeeDetails") + pResourceId);
                inProjectEmployee = JsonConvert.DeserializeObject<List<ResourceAvailability>>(JsonConvert.SerializeObject(InProEmployee.Result.Data));
                if (EmployeeList != null)
                {
                    int TotalEmployee = EmployeeList.Select(x => x.EmployeeId).Distinct().Count();
                    int TotalMale = EmployeeList.Where(x => x.Gender == "Male").Select(x => x.EmployeeId).Distinct().Count();
                    int TotalFemale = EmployeeList.Where(x => x.Gender == "Female").Select(x => x.EmployeeId).Distinct().Count();
                    TotalEmployeesReport = (from emp in EmployeeList
                                            group emp by emp.Department into dept
                                            select new TotalEmployees
                                            {
                                                TotalEmployee = EmployeeList.Select(x => x.EmployeeId).Distinct().Count(),
                                                TotalMale = EmployeeList.Where(x => x.Gender == "Male").Select(x => x.EmployeeId).Distinct().Count(),
                                                TotalFemale = EmployeeList.Where(x => x.Gender == "Female").Select(x => x.EmployeeId).Distinct().Count(),
                                                Department = dept.Select(x => x.Department).FirstOrDefault(),
                                                DeptMaleCount = dept.Where(x => x.Gender == "Male").Select(x => x.EmployeeId).Distinct().Count(),
                                                DeptFemaleCount = dept.Where(x => x.Gender == "Female").Select(x => x.EmployeeId).Distinct().Count(),
                                            }).ToList();
                    var LocationwiseEmployee = (from emp in EmployeeList
                                                group emp by emp.Location into loc
                                                select new
                                                {
                                                    Location = loc.Select(x => x.Location).FirstOrDefault(),
                                                    Count = loc.Select(x => x.EmployeeId).Distinct().Count()
                                                }).ToList();
                    foreach (var locationwise in LocationwiseEmployee)
                    {
                        double per = locationwise.Count;
                        LocationwiseEmployee Location = new()
                        {
                            Location = locationwise.Location,
                            Count = locationwise.Count,
                            Percentage = (double)(per / TotalEmployee) * 100
                        };
                        LocationwiseEmployeeReport.Add(Location);
                    };
                    var SpecialAbility = (from emp in EmployeeList
                                          group emp by emp.IsSpecialAbility into spe
                                          select new
                                          {
                                              IsSpecialAbility = spe.Where(x => x.IsSpecialAbility == true).Select(x => x.EmployeeId).Distinct().Count(),
                                              Other = spe.Where(x => x.IsSpecialAbility == false).Select(x => x.EmployeeId).Distinct().Count(),
                                          });
                    double specialAbilityPercent = (SpecialAbility.Select(x => x.IsSpecialAbility).FirstOrDefault() / TotalEmployee) * 100;
                    double otherPercent = (SpecialAbility.Select(x => x.Other).FirstOrDefault() / TotalEmployee) * 100;
                    List<SpecialAbility> SpecialReport = new();
                    SpecialReport.Add(new SpecialAbility { Type = "SpecialAbility", Count = SpecialAbility.Select(x => x.IsSpecialAbility).FirstOrDefault(), Percentage = specialAbilityPercent });
                    SpecialReport.Add(new SpecialAbility { Type = "Other", Count = SpecialAbility.Select(x => x.Other).FirstOrDefault(), Percentage = otherPercent });
                    var innovation = (from emp in EmployeeList
                                      join inpro in inProjectEmployee on emp.EmployeeId equals inpro.EmployeeId
                                      group inpro by inpro.EmployeeId into inno
                                      select new
                                      {
                                          InProject = inno.Select(x => x.EmployeeId).Distinct().Count()
                                      });
                    InnovationHub.Add(new InnovationHub { Type = "InProject", Count = innovation.Select(x => x.InProject).FirstOrDefault(), Percentage = (innovation.Select(x => x.InProject).FirstOrDefault() / TotalEmployee) * 100 });
                    InnovationHub.Add(new InnovationHub { Type = "Others", Count = innovation.Select(x => x.InProject).FirstOrDefault(), Percentage = ((TotalEmployee - innovation.Select(x => x.InProject).FirstOrDefault()) / TotalEmployee) * 100 });
                    EmployeeAssociateReport EployeeAssociateReport = new()
                    {
                        TotalEmployees = TotalEmployeesReport,
                        LocationwiseEmployee = LocationwiseEmployeeReport,
                        SpecialAbility = SpecialReport,
                        EmployeeInnovationHub = InnovationHub
                    };
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        ReportData = EployeeAssociateReport
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Report/GetAssociateReport", " ResourceId- " + employeeId.ToString());
            }
            return Ok(new
            {
                StatusCode = "SUCCESS",
                StatusText = strErrorMsg,
                ReportData = EmployeeAssociate
            });
        }
        #endregion
    }
}