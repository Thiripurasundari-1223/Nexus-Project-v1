using ExcelDataReader;
using Newtonsoft.Json;
using ProjectManagement.DAL.Repository;
using SharedLibraries.Models.Accounts;
using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ImportExcelView = SharedLibraries.ViewModels.Projects.ImportExcelView;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Employees;
using static System.Net.WebRequestMethods;
using System.Runtime.InteropServices;
using SharedLibraries.ViewModels.Notifications;
using SharedLibraries;
using AppConstants = SharedLibraries.Models.Projects.AppConstants;

namespace ProjectManagement.DAL.Services
{
    public class PMServices
    {
        private readonly IProjectDetailsRepository _projectDetailsRepository;
        private readonly IChangeRequestDetailRepository _changeRequestDetailRepository;
        private readonly IResouceAllocationRepository _resouceAllocationRepository;
        private readonly IProjectDetailCommentsRepository _projectDetailCommentsRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly ICustomerSPOCDetailsRepository _customerSPOCDetailsRepository;
        private readonly IFixedIterationRepository _fixedIterationRepository;
        private readonly IProjectVersionDetailsCommentsRepository _projectVersionDetailsCommentsRepository;
        private readonly IProjectVersionDocumentRepository _projectVersionDocumentRepository;
        private IResourceAllocationVersionRespository _resourceAllocationVersionRepository;
        private ICustomerSPOCVersionDetailsRepository _customerSPOCVersionDetailsRepository;
        private readonly IFixedIterationVersionRepository _fixedIterationVersionRepository;
        public PMServices(IProjectDetailsRepository projectDetailsRepository, IChangeRequestDetailRepository changeRequestDetailRepository,
            IResouceAllocationRepository resouceAllocationRepository, IProjectDetailCommentsRepository projectDetailCommentsRepository, IProjectDocumentRepository projectDocumentRepository,
            IAuditRepository auditRepository, ICustomerSPOCDetailsRepository customerSPOCDetailsRepository, IFixedIterationRepository fixedIterationRepository,
            IProjectVersionDetailsCommentsRepository projectVersionDetailsCommentsRepository,
            IResourceAllocationVersionRespository resourceAllocationVersionRespository, IProjectVersionDocumentRepository projectVersionDocumentRepository, 
            ICustomerSPOCVersionDetailsRepository customerSPOCVersionDetailsRepository, IFixedIterationVersionRepository fixedIterationVersionRepository)
        {
            _projectDetailsRepository = projectDetailsRepository;
            _changeRequestDetailRepository = changeRequestDetailRepository;
            _resouceAllocationRepository = resouceAllocationRepository;
            _projectDetailCommentsRepository = projectDetailCommentsRepository;
            _auditRepository = auditRepository;
            _projectDocumentRepository = projectDocumentRepository;
            _customerSPOCDetailsRepository = customerSPOCDetailsRepository;
            _customerSPOCDetailsRepository = customerSPOCDetailsRepository;
            _fixedIterationRepository = fixedIterationRepository;
            _projectVersionDetailsCommentsRepository = projectVersionDetailsCommentsRepository;
            _resourceAllocationVersionRepository = resourceAllocationVersionRespository;
            _projectVersionDocumentRepository = projectVersionDocumentRepository;
            _customerSPOCVersionDetailsRepository = customerSPOCVersionDetailsRepository;
            _fixedIterationVersionRepository = fixedIterationVersionRepository;
        }

        /* public async Task<string> BulkInsertProject(ImportExcelView import)
         {
             IDictionary<string, string> output = new Dictionary<string, string>();
             if (!string.IsNullOrEmpty(import.Base64Format))
             {
                 Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                 byte[] bytes = Convert.FromBase64String(import.Base64Format);
                 MemoryStream ms = new MemoryStream(bytes);

                 using (var reader = ExcelReaderFactory.CreateReader(ms))
                 {
                     DataSet dataSet = reader?.AsDataSet();
                     if (dataSet?.Tables?.Count > 0)
                     {
                         DataTable projectDetails = dataSet?.Tables["Project_Details"];
                         DataTable changeRequestDetails = dataSet?.Tables["Change_Request_Details"];
                         DataTable projectResourceDetails = dataSet?.Tables["Project_Resource_Details"];

                         if (projectDetails?.Rows?.Count > 0)
                         {
                             output.Add("TotalCount", projectDetails?.Rows?.Count.ToString());
                             var projectDetailsModel = AppendProjectDetailsToView(projectDetails, projectResourceDetails, import.EmployeeDetails, import.AccountDetails, import.Skillsets);
                             var changeRequestModel = AppendChangeRequestDetailsToView(changeRequestDetails, import.EmployeeDetails);
                             //var validRecords = ValidateProjectDetailsRecords(projectDetailsModel, ref output);
                             foreach (var validRecord in projectDetailsModel)
                             {
                                 try
                                 {
                                     await BulkInsertandUpdateProject(validRecord);
                                     output.Add(validRecord.ProjectName, "Success");
                                 }
                                 catch (Exception ex)
                                 {

                                 }

                             }
                             if (changeRequestModel != null && changeRequestModel.Count > 0)
                             {
                                 foreach (var changeRequest in changeRequestModel)
                                 {
                                     try
                                     {
                                         await InsertOrUpdateChangeRequestDetail(changeRequest);
                                     }
                                     catch (Exception ex)
                                     {

                                     }

                                 }
                             }
                         }
                         else
                         {
                             output.Add("Error", "No Records found in the imported file");
                         }
                     }
                     else
                     {
                         output.Add("Error", "No Records found in the imported file");
                     }
                 }
             }
             return JsonConvert.SerializeObject(output);
         }
 */
        private List<ChangeRequestView> AppendChangeRequestDetailsToView(DataTable changeRequestDetails, List<EmployeeDetail> employeeDetails)
        {
            List<ChangeRequestView> changeRequestViews = new List<ChangeRequestView>();
            var currencyTypes = GetAllCurrencyType();
            var projects = GetAllActiveProjects();
            var changeRequestTypes = GetAllChangeRequestType();
            decimal amount = 0;
            int changeRequestDuration = 0;
            DateTime dt = new DateTime();
            for (int i = 1; i < changeRequestDetails?.Rows?.Count; i++)
            {
                ChangeRequestView changeRequestView = new ChangeRequestView();
                changeRequestView.ChangeRequestName = changeRequestDetails.Rows[i][0] != null ? changeRequestDetails.Rows[i][0].ToString()?.Trim() : "";
                changeRequestView.ProjectId = changeRequestDetails.Rows[i][1] != null ? GetProjectId(changeRequestDetails.Rows[i][1].ToString()?.Trim(), projects) : -1;
                changeRequestView.ChangeRequestTypeId = changeRequestDetails.Rows[i][2] != null ? GetChangeRequestTypeId(changeRequestDetails.Rows[i][2].ToString()?.Trim(), changeRequestTypes) : -1;
                changeRequestView.CurrencyId = changeRequestDetails.Rows[i][3] != null ? GetCurrencyTypeId(changeRequestDetails.Rows[i][3].ToString()?.Trim(), currencyTypes) : -1;
                if (changeRequestDetails.Rows[i][4] != null && decimal.TryParse(changeRequestDetails.Rows[i][4].ToString()?.Trim(), out amount))
                {
                    changeRequestView.SOWAmount = amount;
                }
                else
                {
                    changeRequestView.SOWAmount = amount;
                }
                if (changeRequestDetails.Rows[i][5] != null && int.TryParse(changeRequestDetails.Rows[i][5].ToString()?.Trim(), out changeRequestDuration))
                {
                    changeRequestView.ChangeRequestDuration = changeRequestDuration;
                }
                else
                {
                    changeRequestView.ChangeRequestDuration = changeRequestDuration;
                }
                if (changeRequestDetails.Rows[i][6] != null && DateTime.TryParse(changeRequestDetails.Rows[i][6].ToString()?.Trim(), out dt))
                {
                    changeRequestView.ChangeRequestStartDate = dt;
                }
                if (changeRequestDetails.Rows[i][7] != null && DateTime.TryParse(changeRequestDetails.Rows[i][7].ToString()?.Trim(), out dt))
                {
                    changeRequestView.ChangeRequestEndDate = dt;
                }
                changeRequestView.ChangeRequestDescription = changeRequestDetails.Rows[i][8] != null ? changeRequestDetails.Rows[i][8].ToString()?.Trim() : "";
                changeRequestView.ChangeRequestStatus = changeRequestDetails.Rows[i][9] != null ? changeRequestDetails.Rows[i][9].ToString()?.Trim() : "";
                changeRequestView.CreatedOn = DateTime.Now;
                changeRequestView.CreatedBy = changeRequestDetails.Rows[i][11] != null ? GetEmployeeIdByMail(changeRequestDetails.Rows[i][11].ToString()?.Trim(), employeeDetails) : -1;
                //changeRequestView.Re = changeRequestDetails.Rows[i][0] != null ? changeRequestDetails.Rows[i][0].ToString()?.Trim() : "";
                changeRequestViews.Add(changeRequestView);
            }
            return changeRequestViews;
        }

        /*   private List<ProjectDetailView> AppendProjectDetailsToView(DataTable projectDetails, DataTable projectResourceDetails, List<EmployeeDetail> employeeDetails, List<AccountDetails> accountDetails, List<Skillsets> skillsets)
           {
               List<ProjectDetailView> projectDetailViews = new List<ProjectDetailView>();

               int projectDuration = 0;
               DateTime dt = new DateTime();
               var projectTypes = GetAllProjectType();
               var currencyTypes = GetAllCurrencyType();
               var rateFrequencies = GetAllRateFrequency();
               var allocations = GetAllAllocation();
               decimal amount = 0;
               for (int i = 1; i < projectDetails?.Rows?.Count; i++)
               {
                   #region ProjectDetailView
                   ProjectDetailView projectDetailView = new ProjectDetailView();
                   projectDetailView.AccountId = projectDetails.Rows[i][0] != null ? GetAccountIdByName(projectDetails.Rows[i][0].ToString()?.Trim(), accountDetails) : -1;
                   projectDetailView.AccountName = projectDetails.Rows[i][0] != null ? projectDetails.Rows[i][0].ToString()?.Trim() : "";
                   projectDetailView.ProjectName = projectDetails.Rows[i][1] != null ? projectDetails.Rows[i][1].ToString()?.Trim() : "";
                   projectDetailView.ProjectDescription = projectDetails.Rows[i][2] != null ? projectDetails.Rows[i][2].ToString()?.Trim() : "";
                   if (projectDetails.Rows[i][3] != null && int.TryParse(projectDetails.Rows[i][3].ToString()?.Trim(), out projectDuration))
                   {
                       projectDetailView.ProjectDuration = projectDuration;
                   }
                   else
                   {
                       projectDetailView.ProjectDuration = projectDuration;
                   }
                   if (projectDetails.Rows[i][4] != null && DateTime.TryParse(projectDetails.Rows[i][4].ToString()?.Trim(), out dt))
                   {
                       projectDetailView.ProjectStartDate = dt;
                   }
                   if (projectDetails.Rows[i][5] != null && DateTime.TryParse(projectDetails.Rows[i][5].ToString()?.Trim(), out dt))
                   {
                       projectDetailView.ProjectEndDate = dt;
                   }
                   projectDetailView.AdditionalComments = projectDetails.Rows[i][6] != null ? projectDetails.Rows[i][6].ToString()?.Trim() : "";
                   projectDetailView.ProjectType = projectDetails.Rows[i][7] != null ? projectDetails.Rows[i][7].ToString()?.Trim() : "";
                   projectDetailView.ProjectTypeId = GetProjectType(projectDetailView.ProjectType, projectTypes);
                   projectDetailView.ProjectSPOC = projectDetails.Rows[i][8] != null ? GetEmployeeIdByMail(projectDetails.Rows[i][8].ToString()?.Trim(), employeeDetails) : -1;
                   projectDetailView.CurrencyType = projectDetails.Rows[i][9] != null ? projectDetails.Rows[i][9].ToString()?.Trim() : "";
                   projectDetailView.CurrencyTypeId = (int)GetCurrencyTypeId(projectDetailView.CurrencyType, currencyTypes);
                   projectDetailView.FinanceManagerId = projectDetails.Rows[i][10] != null ? GetEmployeeIdByMail(projectDetails.Rows[i][10].ToString()?.Trim(), employeeDetails) : -1;
                   if (projectDetails.Rows[i][11] != null && decimal.TryParse(projectDetails.Rows[i][11].ToString()?.Trim(), out amount))
                   {
                       projectDetailView.TotalSOWAmount = amount;
                   }
                   else
                   {
                       projectDetailView.TotalSOWAmount = amount;
                   }
                   projectDetailView.ProjectStatusCode = projectDetails.Rows[i][12] != null ? projectDetails.Rows[i][12].ToString()?.Trim() : "";
                   projectDetailView.CreatedOn = DateTime.Now;
                   projectDetailView.CreatedBy = projectDetails.Rows[i][14] != null ? GetEmployeeIdByMail(projectDetails.Rows[i][14].ToString()?.Trim(), employeeDetails) : -1;
                   projectDetailView.EngineeringLeadId = projectDetails.Rows[i][15] != null ? GetEmployeeIdByMail(projectDetails.Rows[i][15].ToString()?.Trim(), employeeDetails) : -1;
                   projectDetailView.bUAccountableForProject = projectDetails.Rows[i][15] != null ? GetEmployeeIdByMail(projectDetails.Rows[i][15].ToString()?.Trim(), employeeDetails) : -1;
                   projectDetailView.ProjectStatus = projectDetails.Rows[i][16] != null ? projectDetails.Rows[i][16].ToString()?.Trim() : "";
                   projectDetailView.IsDraft = false;
                   projectDetailView.ResourceAllocation = new List<ResourceAllocationList>();
                   List<ResourceAllocationList> resourceAllocationList = new();
                   for (int j = 1; j < projectResourceDetails.Rows.Count; j++)
                   {
                       if (projectResourceDetails.Rows[j][2] != null && projectResourceDetails.Rows[j][2].ToString()?.ToLower().Trim() == projectDetailView.ProjectName.ToLower().Trim())
                       {
                           decimal skillRate = 0;
                           decimal experience = 0;
                           DateTime dtStartDate = new DateTime();
                           DateTime dtEndDate = new DateTime();
                           bool isBillable = false;
                           if (projectResourceDetails.Rows[j][5] != null && decimal.TryParse(projectResourceDetails.Rows[j][5].ToString()?.Trim(), out amount))
                           {
                               skillRate = amount;
                           }
                           if (projectResourceDetails.Rows[j][8] != null && DateTime.TryParse(projectResourceDetails.Rows[j][8].ToString()?.Trim(), out dt))
                           {
                               dtStartDate = dt;
                           }
                           if (projectResourceDetails.Rows[j][9] != null && DateTime.TryParse(projectResourceDetails.Rows[j][9].ToString()?.Trim(), out dt))
                           {
                               dtEndDate = dt;
                           }
                           if (projectResourceDetails.Rows[j][10] != null && bool.TryParse(projectResourceDetails.Rows[j][10].ToString()?.Trim(), out isBillable))
                           {
                               isBillable = isBillable;
                           }
                           else
                           {
                               isBillable = false;
                           }
                           if (projectResourceDetails.Rows[j][11] != null && decimal.TryParse(projectResourceDetails.Rows[j][11].ToString()?.Trim(), out amount))
                           {
                               experience = amount;
                           }
                           projectDetailView.ResourceAllocation.Add(new ResourceAllocationList
                           {
                               EmployeeId = projectResourceDetails.Rows[j][0] != null ? GetEmployeeIdByMail(projectResourceDetails.Rows[j][0].ToString()?.Trim(), employeeDetails) : -1,
                               RequiredSkillSetId = projectResourceDetails.Rows[j][4] != null ? GetSkillSetId(projectResourceDetails.Rows[j][4].ToString()?.Trim(), skillsets) : -1,
                               SkillRate = skillRate,
                               RateFrequencyId = projectResourceDetails.Rows[j][6] != null ? GetRateFrequencyId(projectResourceDetails.Rows[j][6].ToString()?.Trim(), rateFrequencies) : -1,
                               AllocationId = projectResourceDetails.Rows[j][7] != null ? GetAllocationId(projectResourceDetails.Rows[j][7].ToString()?.Trim(), allocations) : 0,
                               StartDate = dtStartDate,
                               EndDate = dtEndDate,
                               CreatedOn = DateTime.Now,
                               CreatedBy = projectDetailView.CreatedBy,
                               IsBillable = isBillable,
                               Experience = experience
                           });
                       }
                   }
                   projectDetailViews.Add(projectDetailView);
                   #endregion
               }
               return projectDetailViews;
           }
   */
        private int GetAllocationId(string allocationName, List<Allocation> allocations)
        {
            var allocationId = -1;
            try
            {
                if (!string.IsNullOrEmpty(allocationName))
                {
                    string lastChar = allocationName.Substring(allocationName.Length - 1);
                    if (lastChar != "%")
                    {
                        allocationName = allocationName + "%";
                    }
                }
                if (!string.IsNullOrEmpty(allocationName))
                {
                    allocationId = allocations.Find(x => x.AllocationDescription.ToLower().Trim() == allocationName.ToLower().Trim()).AllocationId;
                    if (allocationId > 0)
                    {
                        return allocationId;
                    }
                    return -1;
                }

            }
            catch (Exception ex)
            {
                allocationId = -1;
            }
            return allocationId;
        }

        private int GetRateFrequencyId(string rateFrequency, List<RateFrequency> rateFrequencies)
        {
            var rateFrequencyId = -1;
            try
            {
                if (!string.IsNullOrEmpty(rateFrequency))
                {
                    rateFrequencyId = rateFrequencies.Find(x => x.RateFrequencyDescription.ToLower().Trim() == rateFrequency.ToLower().Trim()).RateFrequencyId;
                    if (rateFrequencyId > 0)
                    {
                        return rateFrequencyId;
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                rateFrequencyId = -1;
            }


            return rateFrequencyId;
        }

        private int GetSkillSetId(string skillSetName, List<Skillsets> skillsets)
        {
            var skillSetId = -1;
            try
            {
                if (!string.IsNullOrEmpty(skillSetName))
                {
                    skillSetId = skillsets == null ? -1 : skillsets.Find(x => x.Skillset?.ToLower()?.Trim() == skillSetName?.ToLower()?.Trim()).SkillsetId;
                    if (skillSetId > 0)
                    {
                        return skillSetId;
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                skillSetId = -1;
            }

            return skillSetId;
        }

        private int GetChangeRequestTypeId(string changeRequestType, List<ChangeRequestType> changeRequestTypes)
        {
            var changeRequestTypeId = -1;
            try
            {
                if (!string.IsNullOrEmpty(changeRequestType))
                {
                    changeRequestTypeId = changeRequestTypes.Find(x => x.ChangeRequestTypeDescription.ToLower().Trim() == changeRequestType.ToLower().Trim()).ChangeRequestTypeId;
                    if (changeRequestTypeId > 0)
                    {
                        return changeRequestTypeId;
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                changeRequestTypeId = -1;
            }


            return changeRequestTypeId;
        }

        private int GetProjectId(string projectName, List<ProjectListView> projects)
        {
            var projectId = -1;
            try
            {
                if (!string.IsNullOrEmpty(projectName))
                {
                    projectId = projects.Find(x => x.ProjectName.ToLower().Trim() == projectName.ToLower().Trim()).ProjectId;
                    if (projectId > 0)
                    {
                        return projectId;
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {

            }


            return projectId;
        }

        private int? GetCurrencyTypeId(string currencyType, List<CurrencyType> currencyTypes)
        {
            var currencyTypeId = -1;
            try
            {
                if (!string.IsNullOrEmpty(currencyType))
                {
                    currencyTypeId = currencyTypes.Find(x => x.CurrencyCode.ToLower().Trim() == currencyType.ToLower().Trim()).CurrencyTypeId;
                    if (currencyTypeId > 0)
                    {
                        return currencyTypeId;
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                currencyTypeId = -1;
            }


            return currencyTypeId;
        }

        private int GetEmployeeIdByMail(string employeeEmail, List<EmployeeDetail> employeeDetails)
        {
            var employeeId = 0;
            try
            {
                if (!string.IsNullOrEmpty(employeeEmail))
                {
                    employeeId = employeeDetails == null ? 0 : employeeDetails.Find(x => x.EmailAddress.ToLower().Trim() == employeeEmail.ToLower().Trim()).EmployeeID;
                    if (employeeId > 0)
                    {
                        return employeeId;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                employeeId = 0;
            }


            return employeeId;
        }

        private int GetProjectType(string projectType, List<ProjectType> projectTypes)
        {
            var projectTypeId = -1;
            try
            {
                if (!string.IsNullOrEmpty(projectType))
                {
                    projectTypeId = projectTypes.Find(x => x.ProjectTypeDescription.ToLower().Trim() == projectType.ToLower().Trim()).ProjectTypeId;
                    if (projectTypeId > 0)
                    {
                        return projectTypeId;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                projectTypeId = -1;
            }


            return projectTypeId;
        }

        private int GetAccountIdByName(string accountName, List<AccountDetails> accountDetails)
        {
            var accountId = -1;
            try
            {
                if (!string.IsNullOrEmpty(accountName))
                {
                    accountId = accountDetails.Find(x => x.AccountName.ToLower().Trim() == accountName.ToLower().Trim()).AccountId;
                    if (accountId > 0)
                    {
                        return accountId;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                accountId = -1;
            }


            return accountId;
        }

        /*   private List<ProjectDetailView> ValidateProjectDetailsRecords(List<ProjectDetailView> projectDetailsModel, ref IDictionary<string, string> output)
           {
               List<ProjectDetailView> projectDetailViews = new List<ProjectDetailView>();
               foreach (var projectDetail in projectDetailsModel)
               {
                   try
                   {
                       if (string.IsNullOrEmpty(projectDetail.AccountName))
                       {
                           output.Add(projectDetail.AccountName == null ? "" : projectDetail.AccountName, "Account Name is Missing");
                           continue;
                       }
                       if (!string.IsNullOrEmpty(projectDetail.AccountName))
                       {
                           if (string.IsNullOrEmpty(projectDetail.ProjectName))
                           {
                               output.Add(projectDetail.AccountName + " - Project Name", "Project is not found");
                               continue;
                           }
                           if (projectDetail.ProjectTypeId <= 0)
                           {
                               output.Add(projectDetail.AccountName + " - Project Type", "Project Type not found");
                               //continue;
                           }
                           if (projectDetail.ProjectSPOC <= 0)
                           {
                               output.Add(projectDetail.AccountName + " - Project SPOC", "SPOC not found");
                               //continue;
                           }
                           if (projectDetail.CurrencyTypeId <= 0)
                           {
                               output.Add(projectDetail.AccountName + " - Project Currency", "Currency not found");
                               //continue;
                           }
                           if (projectDetail.FinanceManagerId <= 0)
                           {
                               output.Add(projectDetail.AccountName + " - Finance Manager", "Finance Manager not found");
                               //continue;
                           }
                           if (projectDetail.ResourceAllocation != null && projectDetail.ResourceAllocation.Count > 0)
                           {
                               int i = 0;
                               foreach (var resourceAllocation in projectDetail.ResourceAllocation)
                               {
                                   try
                                   {
                                       if (resourceAllocation.EmployeeId <= 0)
                                       {
                                           output.Add(projectDetail.AccountName + " - Resource Employee Name" + i, "Employee not found for allocation");
                                           continue;
                                       }
                                       if (resourceAllocation.ProjectId <= 0)
                                       {
                                           output.Add(projectDetail.AccountName + " - Resource Project Name" + i, "Project not found for allocation");
                                           continue;
                                       }
                                       if (resourceAllocation.RequiredSkillSetId <= 0)
                                       {
                                           output.Add(projectDetail.AccountName + " - Resource Skillset" + i, "Skill set not found for allocation");
                                           //continue;
                                       }
                                       if (resourceAllocation.RateFrequencyId <= 0)
                                       {
                                           output.Add(projectDetail.AccountName + " - Resource Rate frequency" + i, "Rate Frequency not found for allocation");
                                           //continue;
                                       }
                                   }
                                   catch
                                   {

                                   }
                                   i++;
                               }
                           }
                           projectDetailViews.Add(projectDetail);
                       }

                   }
                   catch (Exception ex)
                   {

                   }

               }
               return projectDetailViews;
           }
   */
        #region Project Details

        #region Project Name Duplication
        public bool ProjectNameDuplication(ProjectDetailView pProjectDetails)
        {
            if (pProjectDetails.IsDraft == true) return false;
            return _projectDetailsRepository.ProjectNameDuplication(pProjectDetails.ProjectName, pProjectDetails.ProjectId, (int)pProjectDetails.AccountId);
        }
        #endregion

        

        

        #region Delete Project
        public bool DeleteProject(int pProjectID)
        {
            try
            {
                if (pProjectID > 0)
                {
                    List<ResourceAllocation> resourceAllocations = _resouceAllocationRepository.GetResourceByProjectID(pProjectID);
                    foreach (ResourceAllocation resourceAllocation in resourceAllocations)
                    {
                        _resouceAllocationRepository.Delete(resourceAllocation);
                        _resouceAllocationRepository.SaveChangesAsync();
                    }
                    ProjectDetails projectDetails = _projectDetailsRepository.GetByID(pProjectID);
                    if (projectDetails != null && projectDetails.ProjectId > 0)
                    {
                        _projectDetailsRepository.Delete(projectDetails);
                        _projectDetailsRepository.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Get Project Detail By Id
        public ProjectDetailView GetProjectDetailById(int pProjectID)
        {
            return _projectDetailsRepository.GetProjectDetailById(pProjectID);
        }
        #endregion

        #region Get Project Comments By Project Id
        public List<ProjectDetailCommentsList> GetProjectCommentsByProjectId(int pProjectID)
        {
            return _projectDetailCommentsRepository.GetProjectCommentsByProjectId(pProjectID);
        }
        #endregion

        #region Get All Project Details By ResourceId
        public List<ProjectListView> GetProjectDetailsByResourceId(ProjectCustomerEmployeeList listResourceId)
        {
            return _projectDetailsRepository.GetProjectDetailsByResourceId(listResourceId);
        }
        #endregion

        #region Get All Drafted Project Details By ResourceId
        public List<ProjectListView> GetDraftedProjectDetailsByResourceId(int pResourceId)
        {
            return _projectDetailsRepository.GetDraftedProjectDetailsByResourceId(pResourceId);
        }
        #endregion

        /*#region Approve or Reject the Project
        public async Task<bool> ApproveOrRejectProjectByProjectId(ApproveOrRejectProject pApproveOrRejectProject)
        {
            try
            {
                ProjectDetails project = _projectDetailsRepository.GetByID(pApproveOrRejectProject.ProjectId);
                if (project != null)
                {
                    if (pApproveOrRejectProject.ProjectStatus == "Ongoing")
                        project.ProjectChanges = string.Empty;
                    project.ProjectStatus = pApproveOrRejectProject.ProjectStatus;
                    project.EngineeringLeadId = pApproveOrRejectProject.DepartmentHeadId;
                    _projectDetailsRepository.Update(project);
                    await _projectDetailsRepository.SaveChangesAsync();
                    ProjectDetails projectDetails = _projectDetailsRepository.Get(pApproveOrRejectProject.ProjectId);
                    if (pApproveOrRejectProject.Comments != "")
                    {
                        ProjectDetailComments projectDetailComments = new()
                        {
                            ProjectDetailId = pApproveOrRejectProject.ProjectId,
                            Comments = pApproveOrRejectProject.Comments,
                            CreatedBy = projectDetails.FinanceManagerId,
                            CreatedOn = DateTime.UtcNow
                        };
                        await _projectDetailCommentsRepository.AddAsync(projectDetailComments);
                        await _projectDetailCommentsRepository.SaveChangesAsync();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion*/

        #region Get All Currency Type
        public List<CurrencyType> GetAllCurrencyType()
        {
            return _projectDetailsRepository.GetAllCurrencyType();
        }
        #endregion

        #region Get All Project Type
        public List<ProjectType> GetAllProjectType()
        {
            return _projectDetailsRepository.GetAllProjectType();
        }
        #endregion

        #region Get All Rate Frequency
        public List<RateFrequency> GetAllRateFrequency()
        {
            return _projectDetailsRepository.GetAllRateFrequency();
        }
        #endregion

        #region Get All Allocation
        public List<Allocation> GetAllAllocation()
        {
            return _projectDetailsRepository.GetAllAllocation();
        }

        public List<AppConstants> GetAllProjectSubType()
        {
            return _projectDetailsRepository.GetAllProjectSubType();
        }

        public List<RateFrequency> GetFrequency()
        {
            return _projectDetailsRepository.GetAllRateFrequency();
        }


        public List< ProjectRole> GetAllProjectRole()
        {
            return _projectDetailsRepository.GetAllProjectRole();
        }
        #endregion



        /*     #region Assign Project SPOC For Project
             public async Task<bool> AssignProjectSPOCForProject(UpdateProjectSPOC pUpdateProjectSPOC)
             {
                 try
                 {
                     ProjectDetails project = _projectDetailsRepository.GetByID(pUpdateProjectSPOC.ProjectId);
                     if (project != null)
                     {
                         ResourceAllocation resourceAllocation = _resouceAllocationRepository.GetResourceByEmployeeId(pUpdateProjectSPOC.ProjectId, project.ProjectSPOC == null ? 0 : (int)project.ProjectSPOC);
                         if (resourceAllocation != null)
                         {
                             resourceAllocation.ModifiedBy = pUpdateProjectSPOC.ModifiedBy;
                             resourceAllocation.ModifiedOn = DateTime.UtcNow;
                             resourceAllocation.EndDate = pUpdateProjectSPOC?.StartDate?.AddDays(-1).Date;
                             _resouceAllocationRepository.Update(resourceAllocation);
                             await _resouceAllocationRepository.SaveChangesAsync();
                         }

                         //int oldSPOC = project.ProjectSPOC==null?0:(int)project.ProjectSPOC;
                         project.ProjectSPOC = pUpdateProjectSPOC.ProjectSpoc;
                         project.ModifiedBy = pUpdateProjectSPOC.ModifiedBy;
                         project.ModifiedOn = DateTime.UtcNow;
                         _projectDetailsRepository.Update(project);
                         await _projectDetailsRepository.SaveChangesAsync();

                         ResourceAllocation allocation = _resouceAllocationRepository.GetResourceByEmployeeId(pUpdateProjectSPOC.ProjectId, pUpdateProjectSPOC.ProjectSpoc);
                         if (allocation != null)
                         {
                             allocation.ModifiedBy = pUpdateProjectSPOC.ModifiedBy;
                             allocation.ModifiedOn = DateTime.UtcNow;
                             allocation.StartDate = pUpdateProjectSPOC.StartDate;
                             allocation.EmployeeId = pUpdateProjectSPOC.ProjectSpoc;
                             allocation.EndDate = pUpdateProjectSPOC.EndDate;
                             allocation.IsBillable = pUpdateProjectSPOC.IsBillable;
                             allocation.AllocationId = pUpdateProjectSPOC.AllocationId;
                             allocation.RequiredSkillSetId = pUpdateProjectSPOC.RequiredSkillSetId;
                             allocation.IsSPOC = true;
                             _resouceAllocationRepository.Update(allocation);
                         }
                         else
                         {
                             allocation = new()
                             {
                                 ProjectId = pUpdateProjectSPOC.ProjectId,
                                 CreatedBy = pUpdateProjectSPOC.ModifiedBy,
                                 CreatedOn = DateTime.UtcNow,
                                 StartDate = pUpdateProjectSPOC.StartDate,
                                 EmployeeId = pUpdateProjectSPOC.ProjectSpoc,
                                 EndDate = pUpdateProjectSPOC.EndDate,
                                 IsBillable = pUpdateProjectSPOC.IsBillable,
                                 AllocationId = pUpdateProjectSPOC.AllocationId,
                                 RequiredSkillSetId = pUpdateProjectSPOC.RequiredSkillSetId,
                                 IsSPOC = true
                         };
                             await _resouceAllocationRepository.AddAsync(allocation);
                         }
                         await _resouceAllocationRepository.SaveChangesAsync();

                         return true;
                     }

                 }
                 catch (Exception)
                 {
                     throw;
                 }
                 return false;
             }
             #endregion
     */
        /*   #region Assign Logo For Project
           public async Task<bool> AssignLogoForProject(UpdateProjectLogo pUpdateProjectLogo)
           {
               try
               {
                   ProjectDetails project = _projectDetailsRepository.GetByID(pUpdateProjectLogo.ProjectId);
                   if (project != null)
                   {
                       project.Logo = pUpdateProjectLogo.Logo;
                       project.ModifiedBy = pUpdateProjectLogo.ModifiedBy;
                       project.ModifiedOn = DateTime.UtcNow;
                       _projectDetailsRepository.Update(project);
                       await _projectDetailsRepository.SaveChangesAsync();
                       return true;
                   }
               }
               catch (Exception)
               {
                   throw;
               }
               return false;
           }
           #endregion
   */
        #endregion

        #region Change Request Details

        #region Get All Change Request Type
        public List<ChangeRequestType> GetAllChangeRequestType()
        {
            return _changeRequestDetailRepository.GetAllChangeRequestType();
        }
        #endregion

           #region Insert or Update Change Request Detail
           public async Task<int> InsertOrUpdateChangeRequestDetail(ChangeRequestView pChangeRequestDetails)
           {
               try
               {
                   int ChangeRequestId = pChangeRequestDetails.ChangeRequestId;
                   ChangeRequest changeRequestDetails = _changeRequestDetailRepository.GetByID(pChangeRequestDetails.ChangeRequestId);
                   if (changeRequestDetails != null)
                   {
                       changeRequestDetails.FormattedChangeRequestId = pChangeRequestDetails.FormattedChangeRequestId;
                       changeRequestDetails.ChangeRequestName = pChangeRequestDetails.ChangeRequestName;
                       changeRequestDetails.ChangeRequestDescription = pChangeRequestDetails.ChangeRequestDescription;
                       changeRequestDetails.ChangeRequestTypeId = pChangeRequestDetails.ChangeRequestTypeId;
                       changeRequestDetails.ChangeRequestDuration = pChangeRequestDetails.ChangeRequestDuration;
                       changeRequestDetails.ChangeRequestStartDate = pChangeRequestDetails.ChangeRequestStartDate;
                       changeRequestDetails.ChangeRequestEndDate = pChangeRequestDetails.ChangeRequestEndDate;
                       changeRequestDetails.CurrencyId = pChangeRequestDetails.CurrencyId;
                       changeRequestDetails.SOWAmount = pChangeRequestDetails.SOWAmount;
                       changeRequestDetails.ChangeRequestStatus = pChangeRequestDetails.ChangeRequestStatus;
                       changeRequestDetails.ProjectId = pChangeRequestDetails.ProjectId;
                       changeRequestDetails.ModifiedBy = pChangeRequestDetails.ModifiedBy;
                       changeRequestDetails.CRStatusCode = pChangeRequestDetails.CRStatusCode;
                       changeRequestDetails.CRChanges = pChangeRequestDetails.CRChanges;
                       if (!string.IsNullOrEmpty(pChangeRequestDetails.CRChanges))
                       {
                           changeRequestDetails.CRChanges = string.IsNullOrEmpty(changeRequestDetails.CRChanges) ? pChangeRequestDetails.CRChanges : changeRequestDetails.CRChanges + "," + pChangeRequestDetails.CRChanges;
                       }
                       changeRequestDetails.ModifiedOn = DateTime.UtcNow;
                       _changeRequestDetailRepository.Update(changeRequestDetails);
                       await _changeRequestDetailRepository.SaveChangesAsync();
                       await SaveOrUpdateResourceAllocation(pChangeRequestDetails, "Update", pChangeRequestDetails.ChangeRequestId);
                   }
                   else
                   {
                       changeRequestDetails = new()
                       {
                           FormattedChangeRequestId = pChangeRequestDetails.FormattedChangeRequestId,
                           ChangeRequestDescription = pChangeRequestDetails.ChangeRequestDescription,
                           ChangeRequestDuration = pChangeRequestDetails.ChangeRequestDuration,
                           ChangeRequestEndDate = pChangeRequestDetails.ChangeRequestEndDate,
                           ChangeRequestName = pChangeRequestDetails.ChangeRequestName,
                           ChangeRequestStartDate = pChangeRequestDetails.ChangeRequestStartDate,
                           ChangeRequestStatus = pChangeRequestDetails.ChangeRequestStatus,
                           ChangeRequestTypeId = pChangeRequestDetails.ChangeRequestTypeId,
                           CreatedBy = pChangeRequestDetails.CreatedBy,
                           CreatedOn = DateTime.UtcNow,
                           CurrencyId = pChangeRequestDetails.CurrencyId,
                           ProjectId = pChangeRequestDetails.ProjectId,
                           SOWAmount = pChangeRequestDetails.SOWAmount,
                           CRStatusCode = pChangeRequestDetails.CRStatusCode,
                           CRChanges = pChangeRequestDetails.CRChanges
                       };
                       await _changeRequestDetailRepository.AddAsync(changeRequestDetails);
                       await _changeRequestDetailRepository.SaveChangesAsync();
                       ChangeRequestId = changeRequestDetails.ChangeRequestId;
                       await SaveOrUpdateResourceAllocation(pChangeRequestDetails, "Insert", ChangeRequestId);
                   }
                   if (pChangeRequestDetails.AdditionalComments != "" && pChangeRequestDetails.AdditionalComments != null)
                   {
                       ProjectDetailComments projectDetailComments = new()
                       {
                           ProjectId = pChangeRequestDetails.ProjectId,
                           ChangeRequestId = ChangeRequestId,
                           Comments = pChangeRequestDetails.AdditionalComments,
                           CreatedBy = pChangeRequestDetails.CreatedBy,
                           CreatedOn = DateTime.UtcNow
                       };
                       await _projectDetailCommentsRepository.AddAsync(projectDetailComments);
                       await _projectDetailCommentsRepository.SaveChangesAsync();
                   }

                   return changeRequestDetails.ChangeRequestId;
               }
               catch (Exception)
               {
                   throw;
               }
           }
           #endregion
   
        #region Approve or Reject the Change Request
        public async Task<bool> ApproveOrRejectChangeRequestById(ApproveOrRejectChangeRequest pApproveOrRejectCR)
        {
            try
            {
                ChangeRequest changeRequestDetails = _changeRequestDetailRepository.GetByID(pApproveOrRejectCR.ChangeRequestId);
                if (changeRequestDetails != null)
                {
                    if (pApproveOrRejectCR.ChangeRequestStatus == "Approved")
                        changeRequestDetails.CRChanges = string.Empty;
                    changeRequestDetails.ChangeRequestStatus = pApproveOrRejectCR.ChangeRequestStatus;
                    _changeRequestDetailRepository.Update(changeRequestDetails);
                    await _projectDetailsRepository.SaveChangesAsync();
                    if (pApproveOrRejectCR.Comments != "")
                    {
                        ProjectDetailComments projectDetailComments = new()
                        {
                            ProjectId = changeRequestDetails.ProjectId,
                            ChangeRequestId = changeRequestDetails.ChangeRequestId,
                            Comments = pApproveOrRejectCR.Comments,
                            CreatedBy = pApproveOrRejectCR.FinanceManagerId,
                            CreatedOn = DateTime.UtcNow
                        };
                        await _projectDetailCommentsRepository.AddAsync(projectDetailComments);
                        await _projectDetailCommentsRepository.SaveChangesAsync();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

         #region Save Or Update Resource Allocation
          private async Task SaveOrUpdateResourceAllocation(ChangeRequestView pChangeRequestDetails, string pMode, int pChangeRequestId)
          {
              if (pChangeRequestDetails.AdditionalComments != "" && pChangeRequestDetails.AdditionalComments != null)
              {
                  ProjectDetailComments projectDetailComments = new()
                  {
                      ChangeRequestId = pChangeRequestId,
                      Comments = pChangeRequestDetails.AdditionalComments,
                      CreatedBy = (pMode == "Insert" ? pChangeRequestDetails.CreatedBy : pChangeRequestDetails.ModifiedBy),
                      CreatedOn = DateTime.UtcNow
                  };
                  await _projectDetailCommentsRepository.AddAsync(projectDetailComments);
                  await _projectDetailCommentsRepository.SaveChangesAsync();
              }
              if (pChangeRequestDetails?.ResourceAllocation?.Count > 0)
              {
                  foreach (ResourceAllocationList resourceAllocationList in pChangeRequestDetails.ResourceAllocation)
                  {
                      ResourceAllocation resourceAllocation = _resouceAllocationRepository.GetByID(resourceAllocationList.ResourceAllocationId);
                      if (resourceAllocation == null || (resourceAllocation != null && resourceAllocation.ResourceAllocationId == 0))
                      {
                          resourceAllocation = new()
                          {
                              ChangeRequestId = pChangeRequestId,
                              ProjectId = pChangeRequestDetails.ProjectId,
                              RequiredSkillSetId = resourceAllocationList.RequiredSkillSetId,
                              SkillRate = resourceAllocationList.SkillRate,
                              FrequencyId = resourceAllocationList.FrequencyId,
                              AllocationId = resourceAllocationList.AllocationId,
                              IsBillable = true,
                              Experience = resourceAllocationList.Experience,
                              CreatedBy = resourceAllocationList.CreatedBy,
                              CreatedOn = DateTime.UtcNow,
                              IsAdditionalResource = false
                          };
                          await _resouceAllocationRepository.AddAsync(resourceAllocation);
                      }
                      else
                      {
                          resourceAllocation.ChangeRequestId = pChangeRequestId;
                          resourceAllocation.ProjectId = pChangeRequestDetails.ProjectId;
                          resourceAllocation.RequiredSkillSetId = resourceAllocationList.RequiredSkillSetId;
                          resourceAllocation.SkillRate = resourceAllocationList.SkillRate;
                          resourceAllocation.FrequencyId = resourceAllocationList.FrequencyId;
                          resourceAllocation.AllocationId = resourceAllocationList.AllocationId;
                          resourceAllocation.Experience = resourceAllocationList.Experience;
                          resourceAllocation.ModifiedBy = resourceAllocationList.ModifiedBy;
                          resourceAllocation.ModifiedOn = DateTime.UtcNow;
                          _resouceAllocationRepository.Update(resourceAllocation);
                      }
                  }
                  await _resouceAllocationRepository.SaveChangesAsync();
              }
          }
          #endregion        

        #region Delete Change Request Detail
        public async Task<bool> DeleteChangeRequestDetail(int pChangeRequestID)
        {
            try
            {
                ChangeRequest ChangeRequestDetails = _changeRequestDetailRepository.Get(pChangeRequestID);
                if (pChangeRequestID > 0 || ChangeRequestDetails != null)
                {
                    _changeRequestDetailRepository.Delete(ChangeRequestDetails);
                    await _changeRequestDetailRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Get Change Request Detail By Project Id
        public List<ChangeRequestView> GetChangeRequestDetailByProjectId(int pProjectId)
        {
            return _changeRequestDetailRepository.GetChangeRequestDetailByProjectId(pProjectId);
        }
        #endregion

        #region Get Change Request Detail By Id
        public ChangeRequestView GetChangeRequestDetailById(int pChangeRequestId)
        {
            return _changeRequestDetailRepository.GetChangeRequestDetailById(pChangeRequestId);
        }
        #endregion

        #region Get Change Request Comments By Change Request Id
        public List<ProjectDetailCommentsList> GetChangeRequestCommentsById(int pChangeRequestID)
        {
            return _changeRequestDetailRepository.GetChangeRequestCommentsById(pChangeRequestID);
        }
        #endregion

        #region Get All Active Projects
        public List<ProjectListView> GetAllActiveProjects()
        {
            return _changeRequestDetailRepository.GetAllActiveProjects();
        }
        #endregion

        #endregion

        #region Resource Allocation

        #region Resource Allocation Duplication
        public string ResourceAllocationDuplication(UpdateResourceAllocation pUpdateResourceAllocation)
        {
            return _resouceAllocationRepository.ResourceAllocationDuplication(pUpdateResourceAllocation);
        }
        #endregion

        #region Insert Resource Allocation
        public bool InsertResourceAllocation(ResourceAllocation pResourceAllocation)
        {
            try
            {
                if (pResourceAllocation.ResourceAllocationId == 0)
                {
                    ResourceAllocation resourceAllocation = new ResourceAllocation();
                    resourceAllocation = pResourceAllocation;
                    _resouceAllocationRepository.AddAsync(resourceAllocation);
                    _resouceAllocationRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        /*#region Assign Associates For Resource Allocation
        public async Task<bool> AddAssociatesForAdditionalresource(ResourceAllocationList pResourceAllocation)
        {
            try
            {
                ResourceAllocation resourceAllocation = _resouceAllocationRepository.GetByID(pResourceAllocation.ResourceAllocationId);
                if (resourceAllocation != null)
                {
                    resourceAllocation.ModifiedBy = pResourceAllocation.ModifiedBy;
                    resourceAllocation.ModifiedOn = DateTime.UtcNow;
                    resourceAllocation.Experience = pResourceAllocation.Experience;
                    resourceAllocation.RequiredSkillSetId = pResourceAllocation.RequiredSkillSetId;
                    resourceAllocation.SkillRate = pResourceAllocation.SkillRate;
                    resourceAllocation.RateFrequencyId = pResourceAllocation.RateFrequencyId;
                    resourceAllocation.AllocationId = pResourceAllocation.AllocationId;
                    resourceAllocation.IsBillable = pResourceAllocation.IsBillable;
                    resourceAllocation.IsAdditionalResource = pResourceAllocation.IsAdditionalResource;
                    _resouceAllocationRepository.Update(resourceAllocation);
                }
                else
                {
                    resourceAllocation = new()
                    {
                        ProjectId = pResourceAllocation.ProjectId,
                        CreatedBy = pResourceAllocation.ModifiedBy,
                        CreatedOn = DateTime.UtcNow,
                        IsBillable = pResourceAllocation.IsBillable,
                        Experience = pResourceAllocation.Experience,
                        RequiredSkillSetId = pResourceAllocation.RequiredSkillSetId,
                        SkillRate = pResourceAllocation.SkillRate,
                        RateFrequencyId = pResourceAllocation.RateFrequencyId,
                        AllocationId = pResourceAllocation.AllocationId,
                        IsAdditionalResource = pResourceAllocation.IsAdditionalResource
                    };
                    await _resouceAllocationRepository.AddAsync(resourceAllocation);
                }
                await _resouceAllocationRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion*/

        #region Additional Resource Allocation Duplication
        public string AdditionalResourceAllocationDuplication(ResourceAllocationList pResourceAllocation)
        {
            UpdateResourceAllocation updateResource = new UpdateResourceAllocation();
            updateResource.EmployeeId = pResourceAllocation.EmployeeId == null ? 0 : (int)pResourceAllocation.EmployeeId;
            updateResource.ResourceAllocationId = pResourceAllocation.ResourceAllocationId;
            updateResource.StartDate = pResourceAllocation.StartDate;
            updateResource.EndDate = pResourceAllocation.EndDate;
            updateResource.ProjectId = pResourceAllocation.ProjectId == null ? 0 : (int)pResourceAllocation.ProjectId;
            updateResource.AllocationId = pResourceAllocation.AllocationId;
            return _resouceAllocationRepository.ResourceAllocationDuplication(updateResource);
        }

        #endregion

        #region Update Resource Allocation
        public bool UpdateResourceAllocation(ResourceAllocation pResourceAllocation)
        {
            try
            {
                ResourceAllocation resourceAllocation = _resouceAllocationRepository.Get(pResourceAllocation.ResourceAllocationId);
                if (resourceAllocation != null && pResourceAllocation.ResourceAllocationId > 0)
                {
                    resourceAllocation.EmployeeId = pResourceAllocation.EmployeeId;
                    _resouceAllocationRepository.Update(resourceAllocation);
                    _resouceAllocationRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Assign Associates For Resource Allocation
        public async Task<bool> AssignAssociatesForResourceAllocation(UpdateResourceAllocation pUpdateResourceAllocation)
        {
            try
            {
                ResourceAllocation resourceAllocation = _resouceAllocationRepository.GetByID(pUpdateResourceAllocation.ResourceAllocationId);
                if (resourceAllocation != null)
                {
                    resourceAllocation.ModifiedBy = pUpdateResourceAllocation.ModifiedBy;
                    resourceAllocation.ModifiedOn = DateTime.UtcNow;
                    resourceAllocation.StartDate = pUpdateResourceAllocation.StartDate;
                    resourceAllocation.EmployeeId = pUpdateResourceAllocation.EmployeeId;
                    resourceAllocation.EndDate = pUpdateResourceAllocation.EndDate;
                    resourceAllocation.IsBillable = pUpdateResourceAllocation.IsBillable;
                    resourceAllocation.RequiredSkillSetId = pUpdateResourceAllocation.RequiredSkillSetId;
                    resourceAllocation.AllocationId = pUpdateResourceAllocation.AllocationId;
                    resourceAllocation.IsAdditionalResource = pUpdateResourceAllocation.IsAdditionalResource;
                    _resouceAllocationRepository.Update(resourceAllocation);
                }
                else
                {
                    resourceAllocation = new()
                    {
                        ProjectId = pUpdateResourceAllocation.ProjectId,
                        CreatedBy = pUpdateResourceAllocation.ModifiedBy,
                        CreatedOn = DateTime.UtcNow,
                        StartDate = pUpdateResourceAllocation.StartDate,
                        EmployeeId = pUpdateResourceAllocation.EmployeeId,
                        EndDate = pUpdateResourceAllocation.EndDate,
                        IsBillable = pUpdateResourceAllocation.IsBillable,
                        Experience = pUpdateResourceAllocation.Experience,
                        RequiredSkillSetId = pUpdateResourceAllocation.RequiredSkillSetId,
                        AllocationId = pUpdateResourceAllocation.AllocationId,
                        IsAdditionalResource = pUpdateResourceAllocation.IsAdditionalResource
                    };
                    await _resouceAllocationRepository.AddAsync(resourceAllocation);
                }
                await _resouceAllocationRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Remove Associates
        public async Task<bool> RemoveAssociates(UpdateResourceAllocation pUpdateResourceAllocation)
        {
            try
            {
                ResourceAllocation resourceAllocation = _resouceAllocationRepository.GetByID(pUpdateResourceAllocation.ResourceAllocationId);
                if (resourceAllocation != null)
                {
                    if (resourceAllocation.IsAdditionalResource != true)
                    {
                        resourceAllocation.ModifiedBy = pUpdateResourceAllocation.ModifiedBy;
                        resourceAllocation.ModifiedOn = DateTime.UtcNow;
                        resourceAllocation.StartDate = null;
                        resourceAllocation.EmployeeId = null;
                        resourceAllocation.EndDate = null;
                        _resouceAllocationRepository.Update(resourceAllocation);
                    }
                    else
                        _resouceAllocationRepository.Delete(resourceAllocation);
                    await _resouceAllocationRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Delete Resource Allocation
        public async Task<bool> DeleteResourceAllocation(int ResourceAllocationId)
        {
            try
            {
                ResourceAllocation resourceAllocation = _resouceAllocationRepository.GetByID(ResourceAllocationId);
                if (resourceAllocation != null)
                {
                    _resouceAllocationRepository.Delete(resourceAllocation);
                    await _resouceAllocationRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #endregion

        #region Get project detail by account id
        public List<ProjectDetails> GetProjectDetailByAccountId(List<int?> lstAccountId)
        {
            try
            {
                return _projectDetailsRepository.GetProjectDetailByAccountId(lstAccountId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get resource count
        public List<ResourceAllocation> GetResourceAllocationByAccountId(List<int?> lstAccountId)
        {
            try
            {
                return _projectDetailsRepository.GetResourceAllocationByAccountId(lstAccountId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get resource project list
        public List<ResourceProjectList> GetResourceProjectList(int ResourceId = 0)
        {
            return _projectDetailsRepository.GetResourceProjectList(ResourceId);
        }
        #endregion

        #region Get Reporting Person TeamMember
        public List<TeamMemberDetails> GetReportingPersonTeamMember(int resourceId, DateTime? weekStartDay)
        {
            return _projectDetailsRepository.GetReportingPersonTeamMember(resourceId, weekStartDay);
        }
        #endregion

        #region Get Reporting Person TeamMember
        public ProjectTimesheet GetProjectTimesheet(int resourceId)
        {
            return _projectDetailsRepository.GetProjectTimesheet(resourceId);
        }
        #endregion

        #region Get project spoc by project id
        public List<ProjectSPOC> GetProjectSPOCByProjectId(List<int> lstProjectId)
        {
            return _projectDetailsRepository.GetProjectSPOCByProjectId(lstProjectId);
        }
        #endregion

        #region check team members by resource id
        public bool CheckTeamMembersByResourceId(int resourceId)
        {
            return _projectDetailsRepository.CheckTeamMembersByResourceId(resourceId);
        }
        #endregion

        #region get all project details
        public List<ProjectDetails> GetAllProjectsDetails()
        {
            return _projectDetailsRepository.GetAllProjectsDetails();
        }
        #endregion

        #region Get project name by id
        public List<ProjectNames> GetProjectNameById(List<int> lstProjectId)
        {
            return _projectDetailsRepository.GetProjectNameById(lstProjectId);
        }
        #endregion

        #region Get Project By Id
        public ProjectDetails GetProjectById(int projectId)
        {
            return _projectDetailsRepository.GetByID(projectId);
        }
        #endregion

        #region Get Change Request By Id
        public ChangeRequest GetChangeRequestById(int CRId)
        {
            return _changeRequestDetailRepository.GetByID(CRId);
        }
        #endregion

        #region Get resource utilisation Data
        public List<ReportData> GetResourceUtilisationData()
        {
            return _projectDetailsRepository.GetResourceUtilisationData();
        }
        #endregion

        #region Get project details by account id
        public List<ProjectDetailView> GetProjectDetailsByAccountId(int pAccountID)
        {
            List<ProjectDetailView> projectDetailList = _projectDetailsRepository.GetProjectDetailsByAccountId(pAccountID);
            if (projectDetailList?.Count > 0)
            {
                foreach (var project in projectDetailList)
                {
                    project.ChangeRequestList = _changeRequestDetailRepository.GetChangeRequestDetailByProjectId(project.ProjectId);
                }
            }
            return projectDetailList;
        }
        #endregion

        #region Get project allocated active resource list
        public List<int> GetProjectAllocatedResourceList()
        {
            return _resouceAllocationRepository.GetProjectAllocatedResourceList();
        }
        #endregion        

        #region get all resource allocation details
        public List<ResourceAllocation> GetAllResourceAllocation()
        {
            return _resouceAllocationRepository.GetAllResourceAllocation();
        }
        #endregion 

        #region Get Contribution Home Report
        public int GetContributionHomeReport(int employeeId)
        {
            return _resouceAllocationRepository.GetContributionHomeReport(employeeId);
        }
        #endregion

        #region Get Projects List By ResourceId
        public List<ProjectNames> GetProjectsByResourceId(int rResourceId)
        {
            return _projectDetailsRepository.GetProjectsByResourceId(rResourceId);
        }
        #endregion

        #region Get Projects And Emp By ResourceId
        public List<EmployeeProjectNames> GetProjectsAndEmpByResourceId(int rResourceId)
        {
            return _projectDetailsRepository.GetProjectsAndEmpByResourceId(rResourceId);
        }
        #endregion 

        #region Get Projects Details By ResourceId
        public List<ProjectDetails> GetActiveProjectDetailsByResourceId(int resourceId)
        {
            return _projectDetailsRepository.GetActiveProjectDetailsByResourceId(resourceId);
        }
        #endregion

        #region Get Projects List By ResourceId
        public List<EmployeeProjectNames> GetProjectsByEmployeeId(int pResourceId)
        {
            return _projectDetailsRepository.GetProjectsByEmployeeId(pResourceId);
        }
        #endregion

        #region GetResourceAvailabilityEmployeeDetails
        public List<ResourceAvailability> GetResourceAvailabilityEmployeeDetails(int pResourceId)
        {
            return _projectDetailsRepository.GetResourceAvailabilityEmployeeDetails(pResourceId);
        }
        #endregion

        #region Get Resource Billability Home Report
        public HomeReportData GetResourceBillabilityHomeReport()
        {
            return _resouceAllocationRepository.GetResourceBillabilityHomeReport();
        }
        #endregion 

        #region Get Resource Availability Home Report
        public HomeReportData GetResourceAvailabilityHomeReport()
        {
            return _resouceAllocationRepository.GetResourceAvailabilityHomeReport();
        }
        #endregion

        /*   #region Remove Project Logo
           public async Task<bool> RemoveProjectLogo(int pProjectID)
           {
               try
               {
                   ProjectDetails projectDetails = _projectDetailsRepository.GetByID(pProjectID);
                   if (projectDetails != null)
                   {
                       if (projectDetails.Logo != null && File.Exists(projectDetails.Logo))
                       {
                           File.Delete(projectDetails.Logo);
                       }
                       projectDetails.Logo = null;
                       _projectDetailsRepository.Update(projectDetails);
                       await _projectDetailsRepository.SaveChangesAsync();
                       return true;
                   }
               }
               catch (Exception)
               {
                   throw;
               }
               return false;
           }
           #endregion*/

        #region Get account id by buhead id
        public List<int?> GetAccountIdByBUHeadId(int resourceId)
        {
            return _projectDetailsRepository.GetAccountIdByBUHeadId(resourceId);
        }
        #endregion

        #region Get Employee Project list 
        public List<EmployeeProjectList> GetEmployeeProjectListById(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
        {
            return _projectDetailsRepository.GetEmployeeProjectListById(appraisalWorkDayFilterView.EmployeeId, appraisalWorkDayFilterView.StartDate, appraisalWorkDayFilterView.EndDate);
        }
        #endregion

        #region Approve or Reject the Project
        public async Task<bool> ApproveOrRejectProjectByProjectId(ApproveOrRejectProject pApproveOrRejectProject)
        {
            try
            {
                ProjectDetails project = _projectDetailsRepository.GetByID(pApproveOrRejectProject.ProjectId);
                if (project != null)
                {
                    if (pApproveOrRejectProject.ProjectStatus == "Ongoing")
                        project.ProjectChanges = string.Empty;
                    project.ProjectStatus = pApproveOrRejectProject.ProjectStatus;
                    project.EngineeringLeadId = pApproveOrRejectProject.DepartmentHeadId;
                    _projectDetailsRepository.Update(project);
                    await _projectDetailsRepository.SaveChangesAsync();
                    ProjectDetails projectDetails = _projectDetailsRepository.Get(pApproveOrRejectProject.ProjectId);
                    if (pApproveOrRejectProject.Comments != "")
                    {
                        ProjectDetailComments projectDetailComments = new()
                        {
                            ProjectId = pApproveOrRejectProject.ProjectId,
                            Comments = pApproveOrRejectProject.Comments,
                            CreatedBy = projectDetails.FinanceManagerId,
                            CreatedOn = DateTime.UtcNow
                        };
                        await _projectDetailCommentsRepository.AddAsync(projectDetailComments);
                        await _projectDetailCommentsRepository.SaveChangesAsync();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion


        #region Approve or Reject Project Project by the Finance Head
        public async Task<bool> ApproveOrRejectProjectByFinanceHead(ApproveOrRejectProject pApproveOrRejectProject)
        {

            try
            {


                ProjectDetails project = _projectDetailsRepository.GetByID(pApproveOrRejectProject.ProjectId);
                if (project != null)
                {
                    if (pApproveOrRejectProject.ProjectStatus == "Ongoing")
                        project.ProjectChanges = string.Empty;
                    project.ProjectStatus = pApproveOrRejectProject.ProjectStatus;
                    project.BuAccountableForProject = pApproveOrRejectProject.BUManagerId;
                    project.ProjectManagerOfficerId = pApproveOrRejectProject.ProjectManagerOfficerId;
                    _projectDetailsRepository.Update(project);
                    await _projectDetailsRepository.SaveChangesAsync();
                    ProjectDetails projectDetails = _projectDetailsRepository.Get(pApproveOrRejectProject.ProjectId);
                    if (pApproveOrRejectProject.Comments != "")
                    {
                        ProjectDetailComments projectDetailComments = new()
                        {
                            ProjectId = pApproveOrRejectProject.ProjectId,
                            Comments = pApproveOrRejectProject.Comments,
                            CreatedBy = projectDetails.FinanceManagerId,
                            CreatedOn = DateTime.UtcNow
                        };
                        await _projectDetailCommentsRepository.AddAsync(projectDetailComments);
                        await _projectDetailCommentsRepository.AddAsync(projectDetailComments);
                        await _projectDetailCommentsRepository.SaveChangesAsync();
                    }

                    var a = projectDetails.ProjectId;

                    /*ProjectAudit projectAudit = new()
                    {
                        ProjectID = projectDetails.ProjectId,
                        Field = "Add",
                        OldValue = null,
                        NewValue = "Approved",
                        Status = projectDetails.ProjectStatus,
                        Remark = "Approved"
                    };

                    bool check = await AddProjectChangesToAudit(projectAudit);
*/
                    return true;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }




        #endregion

        #region
        private async Task<bool> AddProjectChangesToAudit(ProjectAudit objProjectAduitDetails)
        {
            try
            {
                ProjectAudit objProjectAudit = new ProjectAudit();
                objProjectAudit.ProjectID = objProjectAduitDetails.ProjectID;
                objProjectAudit.ActionType = objProjectAduitDetails.ActionType;
                objProjectAudit.Field = objProjectAduitDetails.Field;
                objProjectAudit.OldValue = objProjectAduitDetails.OldValue;
                objProjectAudit.NewValue = objProjectAduitDetails.NewValue;
                objProjectAudit.CreatedBy = objProjectAduitDetails.CreatedBy;
                objProjectAudit.CreatedOn = DateTime.UtcNow;
                await _auditRepository.AddAsync(objProjectAudit);
                await _auditRepository.SaveChangesAsync();

            }
            catch (Exception e)
            {
                throw;
            }

            return true;
        }


        #endregion


        #region Get Finance Head Projects by employee Id
        public List<ProjectDetails> GetFinanceHeadProjectsByEmployeeId(int employeeId)
        {
            return _projectDetailsRepository.GetFinanceHeadProjectsByEmployeeId(employeeId);
        }
        #endregion

        #region Get Projects by BU
        public List<ProjectDetails> GetProjectsByBU(int employeeId)
        {
            return _projectDetailsRepository.GetProjectsByBU(employeeId);
        }
        #endregion


        #region Insert And Update Project
        public async Task<int> InsertandUpdateProject(ProjectDetailView pProjectDetails)
        {
            try
            {
                int ProjectId = 0;

                #region Add And UpdateProject
                ProjectDetails projectDetails = _projectDetailsRepository.GetByID(pProjectDetails.ProjectId);
                if (projectDetails != null)
                {
                    projectDetails.AccountId = pProjectDetails.AccountId;
                    projectDetails.FormattedProjectId = pProjectDetails.FormattedProjectId;
                    projectDetails.ProjectName = pProjectDetails.ProjectName;
                    projectDetails.ProjectTypeId = pProjectDetails.ProjectTypeId;
                    projectDetails.CurrencyTypeId = pProjectDetails.CurrencyTypeId;
                    projectDetails.TotalSOWAmount = pProjectDetails.TotalSOWAmount;
                    projectDetails.ProjectDuration = pProjectDetails.ProjectDuration;
                    projectDetails.ProjectDescription = pProjectDetails.ProjectDescription;
                    projectDetails.ProjectStartDate = pProjectDetails.ProjectStartDate;
                    projectDetails.ProjectEndDate = pProjectDetails.ProjectEndDate;
                    projectDetails.ModifiedBy = pProjectDetails.CreatedBy;
                    projectDetails.ModifiedOn = DateTime.UtcNow;
                    projectDetails.FinanceManagerId = pProjectDetails.FinanceManagerId;
                    projectDetails.BuAccountableForProject = pProjectDetails.bUAccountableForProject;
                    if (pProjectDetails.IsDraft == false)
                        projectDetails.Comments = null;
                    else
                        projectDetails.Comments = pProjectDetails.AdditionalComments;
                    if (pProjectDetails.ProjectStatus != null)
                        projectDetails.ProjectStatus = pProjectDetails.ProjectStatus;
                    if (pProjectDetails.IsDraft != null)
                        projectDetails.IsDraft = pProjectDetails.IsDraft;
                    projectDetails.ProjectStatusCode = pProjectDetails.ProjectStatusCode;
                    if (pProjectDetails.IsDraft == false && !string.IsNullOrEmpty(pProjectDetails.ProjectChanges))
                    {
                        projectDetails.ProjectChanges = string.IsNullOrEmpty(projectDetails.ProjectChanges) ? pProjectDetails.ProjectChanges : projectDetails.ProjectChanges + "," + pProjectDetails.ProjectChanges;
                    }
                    _projectDetailsRepository.Update(projectDetails);
                }
                else
                {
                    projectDetails = new ProjectDetails
                    {
                        AccountId = pProjectDetails.AccountId,
                        ProjectName = pProjectDetails.ProjectName,
                        ProjectTypeId = pProjectDetails.ProjectTypeId,
                        FrequencyId = pProjectDetails.FrequencyId,
                        FrequencyValue = pProjectDetails.FrequencyValue,
                        CurrencyTypeId = pProjectDetails.CurrencyTypeId,
                        TotalSOWAmount = pProjectDetails.TotalSOWAmount,
                        ProjectStartDate = pProjectDetails.ProjectStartDate,
                        ProjectEndDate = pProjectDetails.ProjectEndDate,
                        ProjectDescription = pProjectDetails.ProjectDescription,
                        ProjectDuration = pProjectDetails.ProjectDuration,
                        NumberOfIteration = pProjectDetails.NumberOfIteration,
                        ProjectStatusCode = pProjectDetails.ProjectStatusCode,
                        ProjectChanges = pProjectDetails.ProjectChanges,
                        EngineeringLeadId = pProjectDetails.EngineeringLeadId,
                        AppConstantId = pProjectDetails.AppConstantId,
                        EmployeeLeavesRetained = pProjectDetails.EmployeeLeavesRetained,
                        CreatedBy = pProjectDetails.CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        FinanceManagerId = pProjectDetails.FinanceManagerId,
                        FormattedProjectId = pProjectDetails.FormattedProjectId,
                        BuAccountableForProject = pProjectDetails.bUAccountableForProject
                    };
                    if (pProjectDetails.ProjectStatus != null)
                        projectDetails.ProjectStatus = pProjectDetails.ProjectStatus;
                    if (pProjectDetails.IsDraft != null)
                        projectDetails.IsDraft = pProjectDetails.IsDraft;
                    if (pProjectDetails.IsDraft == false)
                        projectDetails.Comments = null;
                    else
                        projectDetails.Comments = pProjectDetails.AdditionalComments;

                    await _projectDetailsRepository.AddAsync(projectDetails);
                }
                await _projectDetailsRepository.SaveChangesAsync();

                #endregion

                #region AddProjectVersion
                String VersionName = AddProjectVersion(projectDetails);
                #endregion

                #region Project Comments
                ProjectDetailComments projectDetailComments = _projectDetailCommentsRepository.GetByID(ProjectId);
                ProjectId = projectDetails.ProjectId;
                if (pProjectDetails.AdditionalComments != "" && pProjectDetails.AdditionalComments != null && pProjectDetails.IsDraft == false)
                {
                    projectDetailComments = new()
                    {

                        ProjectId = ProjectId,
                        Comments = pProjectDetails.AdditionalComments,
                        CreatedBy = pProjectDetails.CreatedBy,
                        CreatedOn = DateTime.UtcNow
                    };
                    await _projectDetailCommentsRepository.AddAsync(projectDetailComments);
                    await _projectDetailCommentsRepository.SaveChangesAsync();


                }

                #endregion

                #region  ProjectDetail Version Comments

                if (projectDetailComments != null)
                {
                    VersionProjectDetailComments versionProjectDetailComments = new()
                    {
                        VersionName = VersionName,
                        ProjectDetailCommentId = projectDetailComments.ProjectDetailCommentId,
                        ChangeRequestId = projectDetailComments.ChangeRequestId,
                        Comments = projectDetailComments.Comments,
                        CreatedBy = projectDetailComments.CreatedBy,
                        CreatedOn = DateTime.UtcNow
                    };
                    await _projectVersionDetailsCommentsRepository.AddAsync(versionProjectDetailComments);
                    await _projectVersionDetailsCommentsRepository.SaveChangesAsync();

                    #endregion

                    #region Resource Allocation
                    if (pProjectDetails?.ResourceAllocation?.Count > 0)
                    {

                        foreach (ResourceAllocationList resourceAllocationList in pProjectDetails.ResourceAllocation)
                        {

                            ResourceAllocation resourceAllocation = _resouceAllocationRepository.Get(resourceAllocationList.ResourceAllocationId);


                            if (resourceAllocation == null || (resourceAllocation != null && resourceAllocation.ResourceAllocationId == 0))
                            {
                                resourceAllocation = new()
                                {
                                    ProjectId = ProjectId,
                                    IterationID = resourceAllocationList.IterationID,
                                    EmployeeId = resourceAllocationList.EmployeeId,
                                    RequiredSkillSetId = resourceAllocationList.RequiredSkillSetId,
                                    SkillRate = resourceAllocationList.SkillRate,
                                    FrequencyId = resourceAllocationList.FrequencyId,
                                    AllocationId = resourceAllocationList.AllocationId,
                                    StartDate = resourceAllocationList.StartDate,
                                    EndDate = resourceAllocationList.EndDate,
                                    PlannedHours = resourceAllocationList.PlannedHours,
                                    Contribution = resourceAllocationList.Contribution,
                                    IsBillable = resourceAllocationList.IsBillable,
                                    IsAdditionalResource = resourceAllocationList.IsAdditionalResource,
                                    IsActive = resourceAllocationList.IsActive,
                                    Experience = resourceAllocationList.Experience,
                                    DeliverySupervisorId = resourceAllocationList.DeliverySupervisorId,
                                    ProjectRoleID = resourceAllocationList.ProjectRoleID,
                                    CreatedBy = resourceAllocationList.CreatedBy,
                                    CreatedOn = DateTime.UtcNow
                                };



                                await _resouceAllocationRepository.AddAsync(resourceAllocation);
                            }
                            else
                            {
                                resourceAllocation.ProjectId = resourceAllocation.ProjectId;
                                resourceAllocation.IterationID = resourceAllocation.IterationID;
                                resourceAllocation.EmployeeId = resourceAllocation.EmployeeId;
                                resourceAllocation.RequiredSkillSetId = resourceAllocation.RequiredSkillSetId;
                                resourceAllocation.SkillRate = resourceAllocation.SkillRate;
                                resourceAllocation.FrequencyId = resourceAllocation.FrequencyId;
                                resourceAllocation.AllocationId = resourceAllocation.AllocationId;
                                resourceAllocation.StartDate = resourceAllocation.StartDate;
                                resourceAllocation.EndDate = resourceAllocation.EndDate;
                                resourceAllocation.PlannedHours = resourceAllocation.PlannedHours;
                                resourceAllocation.Contribution = resourceAllocation.Contribution;
                                resourceAllocation.IsBillable = resourceAllocation.IsBillable;
                                resourceAllocation.IsAdditionalResource = resourceAllocation.IsAdditionalResource;
                                resourceAllocation.IsActive = resourceAllocation.IsActive;
                                resourceAllocation.Experience = resourceAllocation.Experience;
                                resourceAllocation.DeliverySupervisorId = resourceAllocation.DeliverySupervisorId;
                                resourceAllocation.ModifiedBy = resourceAllocation.ModifiedBy;
                                resourceAllocation.ModifiedOn = DateTime.UtcNow;
                                _resouceAllocationRepository.Update(resourceAllocation);



                            }

                            await _resouceAllocationRepository.SaveChangesAsync();

                            #region  Resource Allocation Version
                            String Version = "1";
                            VersionResourceAllocation versionResourceAllocation = _resouceAllocationRepository.GetVersionByID(resourceAllocation.ResourceAllocationId);

                            if (versionResourceAllocation != null)
                            {
                                Version = versionResourceAllocation.VersionName;
                                int v = int.Parse(Version) + 1;
                                Version = v.ToString();
                                versionResourceAllocation.VersionName = Version;
                                //  versionResourceAllocation.ProjectId = resourceAllocationList.ProjectId;
                                versionResourceAllocation.IterationID = resourceAllocation.IterationID;
                                versionResourceAllocation.EmployeeId = resourceAllocation.EmployeeId;
                                versionResourceAllocation.RequiredSkillSetId = resourceAllocation.RequiredSkillSetId;
                                versionResourceAllocation.SkillRate = resourceAllocation.SkillRate;
                                versionResourceAllocation.FrequencyId = resourceAllocation.FrequencyId;
                                versionResourceAllocation.AllocationId = resourceAllocation.AllocationId;
                                versionResourceAllocation.StartDate = resourceAllocation.StartDate;
                                versionResourceAllocation.EndDate = resourceAllocation.EndDate;
                                versionResourceAllocation.PlannedHours = resourceAllocation.PlannedHours;
                                versionResourceAllocation.Contribution = resourceAllocation.Contribution;
                                versionResourceAllocation.IsBillable = resourceAllocation.IsBillable;
                                versionResourceAllocation.IsAdditionalResource = resourceAllocation.IsAdditionalResource;
                                versionResourceAllocation.IsActive = resourceAllocation.IsActive;
                                versionResourceAllocation.Experience = resourceAllocation.Experience;
                                versionResourceAllocation.DeliverySupervisorId = resourceAllocation.DeliverySupervisorId;
                                versionResourceAllocation.ModifiedBy = resourceAllocation.ModifiedBy;
                                versionResourceAllocation.ModifiedOn = DateTime.UtcNow;
                                _resourceAllocationVersionRepository.Update(versionResourceAllocation);

                            }
                            else
                            {
                                versionResourceAllocation = new()
                                {
                                    VersionName = Version,
                                    ResourceAllocationId = resourceAllocation.ResourceAllocationId,
                                    IterationID = resourceAllocation.IterationID,
                                    EmployeeId = resourceAllocation.EmployeeId,
                                    RequiredSkillSetId = resourceAllocation.RequiredSkillSetId,
                                    SkillRate = resourceAllocation.SkillRate,
                                    FrequencyId = resourceAllocation.FrequencyId,
                                    AllocationId = resourceAllocation.AllocationId,
                                    StartDate = resourceAllocation.StartDate,
                                    EndDate = resourceAllocation.EndDate,
                                    PlannedHours = resourceAllocation.PlannedHours,
                                    Contribution = resourceAllocation.Contribution,
                                    IsBillable = resourceAllocation.IsBillable,
                                    IsAdditionalResource = resourceAllocation.IsAdditionalResource,
                                    IsActive = resourceAllocation.IsActive,
                                    Experience = resourceAllocation.Experience,
                                    DeliverySupervisorId = resourceAllocation.DeliverySupervisorId,
                                    ProjectRoleID = resourceAllocation.ProjectRoleID,
                                    CreatedBy = resourceAllocation.CreatedBy,
                                    CreatedOn = DateTime.UtcNow,

                                };
                                _resourceAllocationVersionRepository.AddResourceAllocationVersion(versionResourceAllocation);
                                await _resourceAllocationVersionRepository.SaveChangesAsync();

                            }

                            #endregion




                        }

                        #endregion
                    }


                    #region Fixed Iteration
                    //Iteration
                    FixedIteration fixedIteration;
                    if (pProjectDetails.NumberOfIteration > 0 && !CheckProjectAuditRecord(ProjectId))
                    {
                        if (pProjectDetails?.FixedIteration?.Count > 0)
                        {

                            foreach (FixedIterationList fixedIterationList in pProjectDetails.FixedIteration)
                            {

                                fixedIteration = _fixedIterationRepository.GetFixedIterationByID(fixedIterationList.IterationID, pProjectDetails.ProjectId);

                                if (fixedIteration != null)
                                {
                                    fixedIteration.ProjectID = fixedIterationList.ProjectID;
                                    fixedIteration.IterationID = fixedIterationList.IterationID;
                                    fixedIteration.StartDate = fixedIterationList.StartDate;
                                    fixedIteration.IterationScope = fixedIterationList.IterationScope;
                                    fixedIteration.EndDate = fixedIterationList.EndDate;
                                    fixedIteration.ModifiedBy = pProjectDetails.CreatedBy;
                                    fixedIteration.ModifiedOn = DateTime.UtcNow;

                                    _fixedIterationRepository.Update(fixedIteration);
                                }
                                else
                                {
                                    fixedIteration = new()
                                    {
                                        ProjectID = ProjectId,
                                        IterationID = fixedIterationList.IterationID,
                                        IterationScope = fixedIterationList.IterationScope,
                                        StartDate = fixedIterationList.StartDate,
                                        EndDate = fixedIterationList.EndDate,
                                        CreatedBy = pProjectDetails.CreatedBy,
                                        CreatedOn = DateTime.UtcNow


                                    };
                                    await _fixedIterationRepository.AddAsync(fixedIteration);

                                }

                                await _fixedIterationRepository.SaveChangesAsync();

                                VersionFixedIteration versionFixedIteration = _fixedIterationRepository.GetByID(fixedIteration.IterationID);


                                string Version = "1";
                                if (versionFixedIteration != null)
                                {
                                    Version = versionFixedIteration.VersionName;
                                    int v = int.Parse(Version) + 1;
                                    Version = v.ToString();
                                    versionFixedIteration.VersionName = Version;

                                    versionFixedIteration.IterationID = fixedIteration.IterationID;
                                    versionFixedIteration.StartDate = fixedIteration.StartDate;
                                    versionFixedIteration.IterationScope = fixedIteration.IterationScope;
                                    versionFixedIteration.EndDate = fixedIteration.EndDate;
                                    versionFixedIteration.ModifiedBy = fixedIteration.ModifiedBy;
                                    versionFixedIteration.ModifiedOn = DateTime.UtcNow;

                                    _fixedIterationVersionRepository.Update(versionFixedIteration);
                                }
                                else
                                {
                                    versionFixedIteration = new()
                                    {
                                        VersionName = Version,
                                        //  ProjectID = ProjectId,
                                        IterationID = fixedIteration.IterationID,
                                        IterationScope = fixedIteration.IterationScope,
                                        StartDate = fixedIteration.StartDate,
                                        EndDate = fixedIteration.EndDate,
                                        CreatedBy = fixedIteration.CreatedBy,
                                        CreatedOn = DateTime.UtcNow
                                    };
                                    await _fixedIterationVersionRepository.AddAsync(versionFixedIteration);
                                }

                                await _fixedIterationVersionRepository.SaveChangesAsync();

                                int iterationId = fixedIterationList.IterationID;

                                if (fixedIterationList.ResourceAllocation.Count > 0)
                                {

                                    foreach (ResourceAllocationList fixedIterationResourceAllocationList in fixedIterationList.ResourceAllocation)
                                    {
                                        ResourceAllocation resourceAllocation = _resouceAllocationRepository.GetResourceAllocationById(fixedIterationResourceAllocationList.ResourceAllocationId, fixedIterationList.IterationID, pProjectDetails.ProjectId);

                                        if (resourceAllocation == null || (resourceAllocation != null && resourceAllocation.ResourceAllocationId == 0))
                                        {
                                            resourceAllocation = new()
                                            {
                                                ProjectId = ProjectId,
                                                IterationID = iterationId,
                                                EmployeeId = fixedIterationResourceAllocationList.EmployeeId,
                                                RequiredSkillSetId = fixedIterationResourceAllocationList.RequiredSkillSetId,
                                                SkillRate = fixedIterationResourceAllocationList.SkillRate,
                                                FrequencyId = fixedIterationResourceAllocationList.FrequencyId,
                                                AllocationId = fixedIterationResourceAllocationList.AllocationId,
                                                StartDate = fixedIterationResourceAllocationList.StartDate,
                                                EndDate = fixedIterationResourceAllocationList.EndDate,
                                                PlannedHours = fixedIterationResourceAllocationList.PlannedHours,
                                                Contribution = fixedIterationResourceAllocationList.Contribution,
                                                IsBillable = fixedIterationResourceAllocationList.IsBillable,
                                                IsAdditionalResource = fixedIterationResourceAllocationList.IsAdditionalResource,
                                                IsActive = fixedIterationResourceAllocationList.IsActive,
                                                Experience = fixedIterationResourceAllocationList.Experience,
                                                DeliverySupervisorId = fixedIterationResourceAllocationList.DeliverySupervisorId,
                                                ProjectRoleID = fixedIterationResourceAllocationList.ProjectRoleID,
                                                CreatedBy = fixedIterationResourceAllocationList.CreatedBy,
                                                CreatedOn = DateTime.UtcNow
                                            };
                                            await _resouceAllocationRepository.AddAsync(resourceAllocation);
                                        }
                                        else
                                        {
                                            resourceAllocation.ProjectId = fixedIterationResourceAllocationList.ProjectId;
                                            resourceAllocation.IterationID = fixedIterationResourceAllocationList.IterationID;
                                            resourceAllocation.EmployeeId = fixedIterationResourceAllocationList.EmployeeId;
                                            resourceAllocation.RequiredSkillSetId = fixedIterationResourceAllocationList.RequiredSkillSetId;
                                            resourceAllocation.SkillRate = fixedIterationResourceAllocationList.SkillRate;
                                            resourceAllocation.FrequencyId = fixedIterationResourceAllocationList.FrequencyId;
                                            resourceAllocation.AllocationId = fixedIterationResourceAllocationList.AllocationId;
                                            resourceAllocation.StartDate = fixedIterationResourceAllocationList.StartDate;
                                            resourceAllocation.EndDate = fixedIterationResourceAllocationList.EndDate;
                                            resourceAllocation.PlannedHours = fixedIterationResourceAllocationList.PlannedHours;
                                            resourceAllocation.Contribution = fixedIterationResourceAllocationList.Contribution;
                                            resourceAllocation.IsBillable = fixedIterationResourceAllocationList.IsBillable;
                                            resourceAllocation.IsAdditionalResource = fixedIterationResourceAllocationList.IsAdditionalResource;
                                            resourceAllocation.IsActive = fixedIterationResourceAllocationList.IsActive;
                                            resourceAllocation.Experience = fixedIterationResourceAllocationList.Experience;
                                            resourceAllocation.DeliverySupervisorId = fixedIterationResourceAllocationList.DeliverySupervisorId;
                                            resourceAllocation.ModifiedBy = fixedIterationResourceAllocationList.ModifiedBy;
                                            resourceAllocation.ModifiedOn = DateTime.UtcNow;
                                            _resouceAllocationRepository.Update(resourceAllocation);
                                        }


                                        await _resouceAllocationRepository.SaveChangesAsync();
                                        #region  Resource Allocation Version
                                        Version = "1";
                                        VersionResourceAllocation versionResourceAllocation = _resouceAllocationRepository.GetVersionByID(resourceAllocation.ResourceAllocationId);

                                        if (versionResourceAllocation != null)
                                        {
                                            Version = versionResourceAllocation.VersionName;
                                            int v = int.Parse(Version) + 1;
                                            Version = v.ToString();
                                            versionResourceAllocation.VersionName = Version;
                                            //  versionResourceAllocation.ProjectId = resourceAllocationList.ProjectId;
                                            versionResourceAllocation.IterationID = resourceAllocation.IterationID;
                                            versionResourceAllocation.EmployeeId = resourceAllocation.EmployeeId;
                                            versionResourceAllocation.RequiredSkillSetId = resourceAllocation.RequiredSkillSetId;
                                            versionResourceAllocation.SkillRate = resourceAllocation.SkillRate;
                                            versionResourceAllocation.FrequencyId = resourceAllocation.FrequencyId;
                                            versionResourceAllocation.AllocationId = resourceAllocation.AllocationId;
                                            versionResourceAllocation.StartDate = resourceAllocation.StartDate;
                                            versionResourceAllocation.EndDate = resourceAllocation.EndDate;
                                            versionResourceAllocation.PlannedHours = resourceAllocation.PlannedHours;
                                            versionResourceAllocation.Contribution = resourceAllocation.Contribution;
                                            versionResourceAllocation.IsBillable = resourceAllocation.IsBillable;
                                            versionResourceAllocation.IsAdditionalResource = resourceAllocation.IsAdditionalResource;
                                            versionResourceAllocation.IsActive = resourceAllocation.IsActive;
                                            versionResourceAllocation.Experience = resourceAllocation.Experience;
                                            versionResourceAllocation.DeliverySupervisorId = resourceAllocation.DeliverySupervisorId;
                                            versionResourceAllocation.ModifiedBy = resourceAllocation.ModifiedBy;
                                            versionResourceAllocation.ModifiedOn = DateTime.UtcNow;
                                            _resourceAllocationVersionRepository.Update(versionResourceAllocation);

                                        }
                                        else
                                        {
                                            versionResourceAllocation = new()
                                            {
                                                VersionName = Version,
                                                ResourceAllocationId = resourceAllocation.ResourceAllocationId,
                                                IterationID = resourceAllocation.IterationID,
                                                EmployeeId = resourceAllocation.EmployeeId,
                                                RequiredSkillSetId = resourceAllocation.RequiredSkillSetId,
                                                SkillRate = resourceAllocation.SkillRate,
                                                FrequencyId = resourceAllocation.FrequencyId,
                                                AllocationId = resourceAllocation.AllocationId,
                                                StartDate = resourceAllocation.StartDate,
                                                EndDate = resourceAllocation.EndDate,
                                                PlannedHours = resourceAllocation.PlannedHours,
                                                Contribution = resourceAllocation.Contribution,
                                                IsBillable = resourceAllocation.IsBillable,
                                                IsAdditionalResource = resourceAllocation.IsAdditionalResource,
                                                IsActive = resourceAllocation.IsActive,
                                                Experience = resourceAllocation.Experience,
                                                DeliverySupervisorId = resourceAllocation.DeliverySupervisorId,
                                                ProjectRoleID = resourceAllocation.ProjectRoleID,
                                                CreatedBy = resourceAllocation.CreatedBy,
                                                CreatedOn = DateTime.UtcNow,

                                            };
                                            _resourceAllocationVersionRepository.AddResourceAllocationVersion(versionResourceAllocation);
                                            await _resourceAllocationVersionRepository.SaveChangesAsync();

                                        }

                                        #endregion

                                    }

                                }



                            }
                        }
                        #endregion


                        #region Project Document
                        //project Document

                        if (pProjectDetails?.ProjectDocuments?.Count > 0)
                        {
                            foreach (ProjectDocumentList ProjectDocumentsList in pProjectDetails.ProjectDocuments)
                            {
                                ProjectDocument projectDocuments = _projectDocumentRepository.Get(ProjectDocumentsList.ProjectDocumentID);

                                if (projectDocuments == null || (projectDocuments != null && projectDocuments.ProjectDocumentID == 0))
                                {
                                    projectDocuments = new()
                                    {
                                        ProjectID = ProjectId,
                                        DocumentPath = ProjectDocumentsList.DocumentPath,
                                        DocumentType = ProjectDocumentsList.DocumentType,
                                        CreatedBy = ProjectDocumentsList.CreatedBy,
                                        CreatedOn = DateTime.UtcNow
                                    };
                                    await _projectDocumentRepository.AddAsync(projectDocuments);
                                }
                                else
                                {
                                    projectDocuments.ProjectID = ProjectDocumentsList.ProjectID;
                                    projectDocuments.DocumentPath = ProjectDocumentsList.DocumentPath;
                                    projectDocuments.ProjectDocumentID = ProjectDocumentsList.ProjectDocumentID;
                                    projectDocuments.DocumentType = ProjectDocumentsList.DocumentType;
                                    projectDocuments.ModifiedOn = DateTime.UtcNow;
                                    projectDocuments.ModifiedBy = ProjectDocumentsList.ModifiedBy;

                                    _projectDocumentRepository.Update(projectDocuments);
                                }
                                await _projectDocumentRepository.SaveChangesAsync();

                            }
                            #endregion

                            #region Project Version Document
                            ProjectDocument projectDocument = _projectDocumentRepository.GetProjectDocumentByID(ProjectId);

                            VersionProjectDocument versionProjectDocument = new()
                            {
                                //  ProjectID = projectDocuments. ProjectId,
                                VersionName = VersionName,
                                DocumentPath = projectDocument.DocumentPath,
                                DocumentType = projectDocument.DocumentType,
                                ProjectDocumentID = projectDocument.ProjectDocumentID,
                                CreatedBy = projectDocument.CreatedBy,
                                CreatedOn = DateTime.UtcNow,
                                ModifiedOn = DateTime.UtcNow,
                                ModifiedBy = projectDocument.ModifiedBy

                            };

                            await _projectVersionDocumentRepository.AddAsync(versionProjectDocument);
                            await _projectVersionDocumentRepository.SaveChangesAsync();

                            #endregion

                            #region Customer SPOC Details
                            //Customer SPOC Details

                            if (pProjectDetails?.CustomerSPOCDetails?.Count > 0)
                            {
                                foreach (CustomerSPOCDetailsList CustomerSPOCDetailsList in pProjectDetails.CustomerSPOCDetails)
                                {

                                    CustomerSPOCDetails customerSPOCDetails = _customerSPOCDetailsRepository.Get(CustomerSPOCDetailsList.CustomerSPOCDetailsID);

                                    if (customerSPOCDetails == null || (customerSPOCDetails != null && customerSPOCDetails.CustomerSPOCDetailsID == 0))
                                    {
                                        customerSPOCDetails = new()
                                        {

                                            ProjectID = ProjectId,
                                            CustomerSPOCDetailsID = CustomerSPOCDetailsList.CustomerSPOCDetailsID,
                                            CustomerSPOCDetailsName = CustomerSPOCDetailsList.CustomerSPOCDetailsName,
                                            MobileNumber = CustomerSPOCDetailsList.MobileNumber,
                                            Email = CustomerSPOCDetailsList.Email,
                                            Address = CustomerSPOCDetailsList.Address,
                                            Description = CustomerSPOCDetailsList.Description,
                                            CreatedOn = CustomerSPOCDetailsList.CreatedOn,
                                            CreatedBy = CustomerSPOCDetailsList.CreatedBy

                                        };
                                        await _customerSPOCDetailsRepository.AddAsync(customerSPOCDetails);
                                    }
                                    else
                                    {
                                        // CustomerSPOCDetails CustomerSPOCDetail = new CustomerSPOCDetails();
                                        customerSPOCDetails.ProjectID = CustomerSPOCDetailsList.ProjectID;
                                        customerSPOCDetails.CustomerSPOCDetailsID = CustomerSPOCDetailsList.CustomerSPOCDetailsID;
                                        customerSPOCDetails.MobileNumber = CustomerSPOCDetailsList.MobileNumber;
                                        customerSPOCDetails.Email = CustomerSPOCDetailsList.Email;
                                        customerSPOCDetails.Address = CustomerSPOCDetailsList.Address;
                                        customerSPOCDetails.Description = CustomerSPOCDetailsList.Description;
                                        customerSPOCDetails.ModifiedBy = CustomerSPOCDetailsList.ModifiedBy;
                                        customerSPOCDetails.ModifiedOn = CustomerSPOCDetailsList.ModifiedOn;
                                        customerSPOCDetails.CustomerSPOCDetailsName = CustomerSPOCDetailsList.CustomerSPOCDetailsName;

                                        _customerSPOCDetailsRepository.Update(customerSPOCDetails);
                                    }
                                    await _customerSPOCDetailsRepository.SaveChangesAsync();
                                }
                            }
                            #endregion

                            #region Customer Version  SPOC Details

                            CustomerSPOCDetails customerSPOCDetail = _customerSPOCDetailsRepository.GetPCustomerSPOCDetailsByID(ProjectId);

                            VersionCustomerSPOCDetails versionCustomerSPOCDetails = new()
                            {
                                // ProjectID = ProjectId,
                                VersionName = VersionName,
                                CustomerSPOCDetailsID = customerSPOCDetail.CustomerSPOCDetailsID,
                                CustomerSPOCDetailsName = customerSPOCDetail.CustomerSPOCDetailsName,
                                CustomerSPOCId = customerSPOCDetail.CustomerSPOCId,
                                MobileNumber = customerSPOCDetail.MobileNumber,
                                Email = customerSPOCDetail.Email,
                                Address = customerSPOCDetail.Address,
                                Description = customerSPOCDetail.Description,
                                CreatedOn = customerSPOCDetail.CreatedOn,
                                CreatedBy = customerSPOCDetail.CreatedBy,
                                ModifiedBy = customerSPOCDetail.ModifiedBy,
                                ModifiedOn = customerSPOCDetail.ModifiedOn

                            };

                            await _customerSPOCVersionDetailsRepository.AddAsync(versionCustomerSPOCDetails);
                            await _customerSPOCVersionDetailsRepository.SaveChangesAsync();

                            #endregion

                        }

                    }
                }
                return ProjectId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region check project in Audit Table
        public bool CheckProjectAuditRecord(int ProjectId)
        {
            return _auditRepository.GetProjectId(ProjectId);
        }
        #endregion

        

        /*   public void AddProjectChangesToAudit(ProjectAudit objProjectAduitDetails)
           {
               try
               {
                   ProjectAudit objProjectAudit = new ProjectAudit();
                   objProjectAudit.ProjectID = objProjectAduitDetails.ProjectID;
                   objProjectAudit.ActionType = objProjectAduitDetails.ActionType;
                   objProjectAudit.Field = objProjectAduitDetails.Field;
                   objProjectAudit.OldValue = objProjectAduitDetails.OldValue;
                   objProjectAudit.NewValue = objProjectAduitDetails.NewValue;
                   objProjectAudit.CreatedBy = objProjectAduitDetails.CreatedBy;
                   objProjectAudit.CreatedOn = DateTime.UtcNow;
                   _auditRepository.AddAsync(objProjectAudit);
                   _auditRepository.SaveChangesAsync();

               }
               catch (Exception e)
               {
                   throw;
               }

           }
    */
        #region Add Project Version
        public String AddProjectVersion(ProjectDetails pProjectDetails)
        {
            int? VersionName = 0;

            VersionProjectDetail versionProjectDetail = _projectDetailsRepository.GetVersionByID(pProjectDetails.ProjectId);

            if (versionProjectDetail != null)
            {
                VersionName = int.Parse(versionProjectDetail.VersionName);
                VersionName = VersionName + 1;
            }
            else
                VersionName = 1;

            versionProjectDetail = new()
            {
                VersionName = VersionName.ToString(),
                ProjectId = pProjectDetails.ProjectId,
                AccountId = pProjectDetails.AccountId,
                FormattedProjectId = pProjectDetails.FormattedProjectId,
                ProjectName = pProjectDetails.ProjectName,
                ProjectTypeId = pProjectDetails.ProjectTypeId,
                CurrencyTypeId = pProjectDetails.CurrencyTypeId,
                TotalSOWAmount = pProjectDetails.TotalSOWAmount,
                ProjectDuration = pProjectDetails.ProjectDuration,
                ProjectDescription = pProjectDetails.ProjectDescription,
                ProjectStartDate = pProjectDetails.ProjectStartDate,
                ProjectEndDate = pProjectDetails.ProjectEndDate,
                CreatedBy = pProjectDetails.CreatedBy,
                CreatedOn = pProjectDetails.CreatedOn,
                ModifiedBy = pProjectDetails.ModifiedBy,
                ModifiedOn = pProjectDetails.ModifiedOn,
                FinanceManagerId = pProjectDetails.FinanceManagerId,
                bUAccountableForProject = pProjectDetails.BuAccountableForProject,
                ProjectStatus = pProjectDetails.ProjectStatus,
                IsDraft = pProjectDetails.IsDraft,
                ProjectStatusCode = pProjectDetails.ProjectStatusCode,
                ProjectChanges = pProjectDetails.ProjectChanges
            };
            _projectDetailsRepository.AddProjectVersion(versionProjectDetail);

            _projectDetailsRepository.SaveChangesAsync();
            return versionProjectDetail.VersionName;
        }

       



        #endregion


    }


}