using ProjectManagement.DAL.DBContext;
using SharedLibraries;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Notifications;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectManagement.DAL.Repository
{
    public interface IProjectDetailsRepository : IBaseRepository<ProjectDetails>
    {
        ProjectDetails GetByName(string pProjectName, int pProjectID = 0);
        ProjectDetails GetByProjectName(string pProjectName);
        ProjectDetails GetByID(int pProjectID);
        List<ProjectDetails> GetByName(string[] pProjectName);
        ProjectDetailView GetProjectDetailById(int pProjectID);
        List<ProjectListView> GetProjectDetailsByResourceId(ProjectCustomerEmployeeList listResourceId);
        List<ProjectListView> GetDraftedProjectDetailsByResourceId(int pResourceId);
        List<CurrencyType> GetAllCurrencyType();
        List<ProjectType> GetAllProjectType();
        List<RateFrequency> GetAllRateFrequency();
        List<Allocation> GetAllAllocation();
        bool ProjectNameDuplication(string pProjectName, int pProjectID, int pAccountID);
        List<ProjectDetails> GetProjectDetailByAccountId(List<int?> lstAccountId);
        List<ResourceAllocation> GetResourceAllocationByAccountId(List<int?> lstAccountId);
        List<ResourceProjectList> GetResourceProjectList(int ResourceId = 0);
        List<TeamMemberDetails> GetReportingPersonTeamMember(int resourceId, DateTime? weekStartDay);
        ProjectTimesheet GetProjectTimesheet(int resourceId);
        List<ProjectSPOC> GetProjectSPOCByProjectId(List<int> lstProjectId);
        bool CheckTeamMembersByResourceId(int resourceId);
        List<ProjectDetails> GetAllProjectsDetails();
        List<ProjectNames> GetProjectNameById(List<int> lstProjectId);
        List<ReportData> GetResourceUtilisationData();
        List<ProjectDetailView> GetProjectDetailsByAccountId(int pAccountID);
        List<ProjectNames> GetProjectsByResourceId(int resourceId);
        List<EmployeeProjectNames> GetProjectsAndEmpByResourceId(int resourceId);
        List<EmployeeProjectNames> GetProjectsByEmployeeId(int pResourceId);
        List<ProjectDetails> GetActiveProjectDetailsByResourceId(int resourceId);
        List<ResourceAvailability> GetResourceAvailabilityEmployeeDetails(int pResourceId);
        List<int?> GetAccountIdByBUHeadId(int resourceId);
        List<EmployeeProjectList> GetEmployeeProjectListById(int EmployeeId, DateTime fromdate, DateTime todate);

        List<ProjectDetails> GetFinanceHeadProjectsByEmployeeId(int employeeId);
        List<ProjectDetails> GetProjectsByBU(int employeeId);
     
        VersionProjectDetail GetVersionByID(int projectId);
    
        List<ProjectDetails> GetAllProjects();
        List<VersionProjectDetail> GetAllVersion();
        Task AddProjectVersion(VersionProjectDetail versionProjectDetail);
        List<AppConstants> GetAllProjectSubType();
        List<ProjectRole> GetAllProjectRole();
    }
    public class ProjectDetailsRepository : BaseRepository<ProjectDetails>, IProjectDetailsRepository
    {
        private readonly PMDBContext dbContext;
        public ProjectDetailsRepository(PMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public ProjectDetails GetByID(int pProjectID)
        {
            return dbContext.ProjectDetails.Where(x => x.ProjectId == pProjectID).FirstOrDefault();
        }
        public List<ProjectDetails> GetAllProjects()
        {
            return dbContext.ProjectDetails.ToList();
        }
        public ProjectDetails GetByName(string pProjectName, int pProjectID = 0)
        {
            if (pProjectID > 0)
            {
                return dbContext.ProjectDetails.Where(x => x.ProjectName == pProjectName && x.ProjectId == pProjectID).FirstOrDefault();
            }
            return dbContext.ProjectDetails.Where(x => x.ProjectId == pProjectID).FirstOrDefault();
        }
        public ProjectDetails GetByProjectName(string pProjectName)
        {
            if (!string.IsNullOrEmpty(pProjectName))
            {
                return dbContext.ProjectDetails.Where(x => x.ProjectName.ToLower() == pProjectName.ToLower()).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        public bool ProjectNameDuplication(string pProjectName, int pProjectID, int pAccountID)
        {
            List<ProjectDetails> projectDetails = dbContext.ProjectDetails.Where(x => x.ProjectName == pProjectName &&
                                                    x.ProjectId != pProjectID && x.AccountId == pAccountID).ToList();
            if (projectDetails.Count > 0)
                return true;
            return false;
        }
        public List<ProjectDetails> GetByName(string[] pProjectName)
        {
            return dbContext.ProjectDetails.Where(x => pProjectName.Contains(x.ProjectName)).ToList();
        }
        /* public ProjectDetailView GetProjectDetailById(int pProjectID)
         {
             ProjectDetails projectDetails = dbContext.ProjectDetails.Where(x => x.ProjectId == pProjectID).FirstOrDefault();
             if (projectDetails != null && projectDetails.ProjectId > 0)
             {
                 ProjectDetailView projectDetailView = new();
                 string ProjectName = "";
                 projectDetailView.ProjectId = projectDetails.ProjectId;
                 projectDetailView.AccountId = projectDetails.AccountId;
                 projectDetailView.AccountName = "";
                 projectDetailView.CreatedBy = projectDetails.CreatedBy;
                 projectDetailView.CreatedByName = "";
                 projectDetailView.CreatedOn = projectDetails.CreatedOn;
                 projectDetailView.CurrencyType = dbContext.CurrencyType.Where(x => x.CurrencyTypeId == projectDetails.CurrencyTypeId).Select(x => x.CurrencyCode).FirstOrDefault();
                 projectDetailView.CurrencyTypeId = projectDetails.CurrencyTypeId;
                 projectDetailView.Experience = projectDetails.Experience;
                 projectDetailView.FinanceManagerId = projectDetails.FinanceManagerId;
                 projectDetailView.FormattedProjectId = "PRJ1" + projectDetails.ProjectId.ToString().PadLeft(4, '0');
                 projectDetailView.ProjectManagerEmpId = "";
                 //if (projectDetails.ProjectSPOC != null && projectDetails.ProjectSPOC > 0)
                 //    projectDetailView.ProjectManagerEmpId = "NXT " + (projectDetails?.ProjectSPOC).ToString();
                 //else
                 //    projectDetailView.ProjectManagerEmpId = "";
                 projectDetailView.IsDraft = projectDetails.IsDraft;
                 projectDetailView.ModifiedBy = projectDetails.ModifiedBy;
                 projectDetailView.ModifiedOn = projectDetails.ModifiedOn;
                 projectDetailView.ProjectDescription = projectDetails.ProjectDescription;
                 projectDetailView.ProjectDuration = projectDetails.ProjectDuration;
                 projectDetailView.ProjectEndDate = projectDetails.ProjectEndDate;
                 projectDetailView.ProjectManager = "";
                 projectDetailView.ProjectManagerRole = "";
                 projectDetailView.ProjectName = projectDetails.ProjectName;
                 projectDetailView.ProjectSPOC = projectDetails.ProjectSPOC;
                 projectDetailView.ProjectStartDate = projectDetails.ProjectStartDate;
                 projectDetailView.ProjectStatus = projectDetails.ProjectStatus;
                 projectDetailView.ProjectType = dbContext.ProjectType.Where(x => x.ProjectTypeId == projectDetails.ProjectTypeId).Select(x => x.ProjectTypeDescription).FirstOrDefault();
                 projectDetailView.ProjectTypeId = projectDetails.ProjectTypeId;
                 projectDetailView.RateFrequency = projectDetails.RateFrequency;
                 projectDetailView.SkillRate = projectDetails.SkillRate;
                 projectDetailView.SkillSet = projectDetails.SkillSet;
                 projectDetailView.TotalSOWAmount = projectDetails.TotalSOWAmount;
                 projectDetailView.AdditionalComments = projectDetails.Comments;
                 projectDetailView.ProjectStatusCode = projectDetails.ProjectStatusCode;
                 projectDetailView.ProjectStatusName = "";
                 projectDetailView.bUAccountableForProject = projectDetails.bUAccountableForProject;
                 projectDetailView.Logo = projectDetails.Logo;
                 projectDetailView.ProjectChanges = string.IsNullOrEmpty(projectDetails.ProjectChanges) ? string.Empty : CommonLib.RemoveDuplicateFromJsonString(projectDetails.ProjectChanges);
                 ProjectName = projectDetails.ProjectName;
                 projectDetailView.ResourceAllocation = dbContext.ResourceAllocation.Where(x => x.ProjectId == pProjectID).Select(resourceAllocation =>
                   new ResourceAllocationList
                   {
                       Allocation = dbContext.Allocation.Where(x => x.AllocationId == resourceAllocation.AllocationId).Select(x => x.AllocationDescription).FirstOrDefault(),
                       AllocationId = resourceAllocation.AllocationId,
                       CreatedBy = resourceAllocation.CreatedBy,
                       CreatedOn = resourceAllocation.CreatedOn,
                       EmployeeId = resourceAllocation.EmployeeId,
                       EmployeeName = "",
                       EndDate = resourceAllocation.EndDate,
                       ModifiedBy = resourceAllocation.ModifiedBy,
                       ModifiedOn = resourceAllocation.ModifiedOn,
                       ProjectId = resourceAllocation.ProjectId,
                       ProjectName = ProjectName,
                       RateFrequency = dbContext.RateFrequency.Where(x => x.RateFrequencyId == resourceAllocation.RateFrequencyId).Select(x => x.RateFrequencyDescription).FirstOrDefault(),
                       RateFrequencyId = resourceAllocation.RateFrequencyId,
                       RequiredSkillSet = "",
                       RequiredSkillSetId = resourceAllocation.RequiredSkillSetId,
                       ResourceAllocationId = resourceAllocation.ResourceAllocationId,
                       SkillRate = resourceAllocation.SkillRate,
                       StartDate = resourceAllocation.StartDate,
                       IsBillable = resourceAllocation.IsBillable,
                       Experience = resourceAllocation.Experience,
                       IsSPOC = resourceAllocation.IsSPOC,
                       IsAdditionalResource=resourceAllocation.IsAdditionalResource
                   }).ToList();


                 return projectDetailView;
             }
             return null;
         }
         public List<ProjectListView> GetProjectDetailsByResourceId(ProjectCustomerEmployeeList projectDetails)
         {
             //if(projectDetails?.RoleName?.ToLower()=="pmo")
             if (projectDetails.ManagementRole.Contains((projectDetails?.RoleName?.ToLower())))
             {
                 return dbContext.ProjectDetails.Where(x => (x.IsDraft == false || x.IsDraft == null)).Select(proList =>


                 new ProjectListView
                 {
                     ProjectId = proList.ProjectId,
                     ProjectName = proList.ProjectName,
                     AccountName = "",
                     AccountId = proList.AccountId,
                     //Associates = (from resourceAllocation in dbContext.ResourceAllocation
                     //              where resourceAllocation.ProjectId == proList.ProjectId && resourceAllocation.EmployeeId != null
                     //              select new { resourceAllocation.ProjectId }).Count().ToString() + " Associates",
                     SPOC = "",
                     SPOCId = proList.ProjectSPOC == null ? 0 : (int)proList.ProjectSPOC,
                     Timesheets = "",
                     ProjectStatus = proList.ProjectStatus,
                     UserRole = "",
                     CreatedBy = proList.CreatedBy,
                     ProjectDuration = proList.ProjectDuration,
                     ProjectEndDate = proList.ProjectEndDate,
                     ProjectStartDate = proList.ProjectStartDate,
                     ResourceCount = (from resourceAllocation in dbContext.ResourceAllocation
                                      where resourceAllocation.ProjectId == proList.ProjectId && resourceAllocation.IsSPOC !=true &&
                                      (resourceAllocation.EndDate == null || (resourceAllocation.EndDate.HasValue && resourceAllocation.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date))
                                      select new { resourceAllocation.ProjectId }).Count(),
                     ProjectStatusCode = proList.ProjectStatusCode,
                     ProjectStatusName = "",
                     ProjectChanges = string.IsNullOrEmpty(proList.ProjectChanges) ? string.Empty : CommonLib.RemoveDuplicateFromJsonString(proList.ProjectChanges),
                     Logo = proList.Logo
                 }).ToList();
             }
             else
             { 
             return dbContext.ProjectDetails.Where(x => (( projectDetails.EmployeeList !=null && projectDetails.EmployeeList.Contains(x.CreatedBy==null?0:(int)x.CreatedBy))
                                                 || ( projectDetails.EmployeeList != null && projectDetails.EmployeeList.Contains(x.EngineeringLeadId == null ? 0 : (int)x.EngineeringLeadId))
                                                 || (projectDetails.EmployeeList != null &&  projectDetails.EmployeeList.Contains(x.FinanceManagerId == null ? 0 : (int)x.FinanceManagerId))
                                                 || ( projectDetails.EmployeeList != null && projectDetails.EmployeeList.Contains(x.ProjectSPOC == null ? 0 : (int)x.ProjectSPOC)))
                                                 && (x.IsDraft == false || x.IsDraft == null)).Select(proList =>


                 new ProjectListView
                 {
                     ProjectId = proList.ProjectId,
                     ProjectName = proList.ProjectName,
                     AccountName = "",
                     AccountId = proList.AccountId,
                     //Associates = (from resourceAllocation in dbContext.ResourceAllocation
                     //             where resourceAllocation.ProjectId == proList.ProjectId && resourceAllocation.EmployeeId != null
                     //             select new { resourceAllocation.ProjectId }).Count().ToString() + " Associates",
                     SPOC = "",
                     SPOCId = proList.ProjectSPOC == null ? 0 : (int)proList.ProjectSPOC,
                     Timesheets = "",
                     ProjectStatus = proList.ProjectStatus,
                     UserRole = "",
                     CreatedBy = proList.CreatedBy,
                     ProjectDuration = proList.ProjectDuration,
                     ProjectEndDate = proList.ProjectEndDate,
                     ProjectStartDate = proList.ProjectStartDate,
                     ResourceCount = (from resourceAllocation in dbContext.ResourceAllocation
                                      where resourceAllocation.ProjectId == proList.ProjectId && resourceAllocation.IsSPOC != true &&
                                      (resourceAllocation.EndDate == null || (resourceAllocation.EndDate.HasValue && resourceAllocation.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date))
                                      select new { resourceAllocation.ProjectId }).Count(),
                     ProjectStatusCode = proList.ProjectStatusCode,
                     ProjectStatusName = "",
                     ProjectChanges = string.IsNullOrEmpty(proList.ProjectChanges) ? string.Empty : CommonLib.RemoveDuplicateFromJsonString(proList.ProjectChanges),
                     Logo = proList.Logo
                 }).ToList();
             }
         }
         public List<ProjectListView> GetDraftedProjectDetailsByResourceId(int pResourceId)
         {

             return dbContext.ProjectDetails.Where(x => x.IsDraft == true && x.CreatedBy == pResourceId).Select(proList =>


                 new ProjectListView
                 {
                     ProjectId = proList.ProjectId,
                     ProjectName = proList.ProjectName,
                     AccountName = "",
                     AccountId = proList.AccountId,
                     Associates = (from resourceAllocation in dbContext.ResourceAllocation
                                   where resourceAllocation.ProjectId == proList.ProjectId && resourceAllocation.EmployeeId != null
                                   select new { resourceAllocation.ProjectId }).Count().ToString() + " Associates",
                     SPOC = "",
                     SPOCId = proList.ProjectSPOC == null ? 0 : (int)proList.ProjectSPOC,
                     Timesheets = proList.ProjectStatus,
                     ProjectStatus = proList.ProjectStatus,
                     UserRole = "",
                     CreatedBy = proList.CreatedBy,
                     ProjectDuration = proList.ProjectDuration,
                     ProjectEndDate = proList.ProjectEndDate,
                     ProjectStartDate = proList.ProjectStartDate,
                     ResourceCount = (from resourceAllocation in dbContext.ResourceAllocation
                                      where resourceAllocation.ProjectId == proList.ProjectId
                                      select new { resourceAllocation.ProjectId }).Count(),
                     ProjectStatusCode = proList.ProjectStatusCode,
                     ProjectStatusName = "",
                     ProjectChanges = string.IsNullOrEmpty(proList.ProjectChanges) ? string.Empty : CommonLib.RemoveDuplicateFromJsonString(proList.ProjectChanges),
                     Logo = proList.Logo
                 }).ToList();
         }*/
        //private List<ProjectListView> GetprojectLists(int pResourceId = 0, bool pIsDraft = false, List<int> listResourceId = null)
        //{
        //    List<ProjectListView> projectListViews = new();
        //    List<ProjectDetails> proLists = new();
        //    proLists = dbContext.ProjectDetails.Select(x => x).ToList();
        //    if (pResourceId > 0 && !pIsDraft)
        //    {
        //        proLists = proLists.Where(x => (x.CreatedBy == pResourceId || x.EngineeringLeadId == pResourceId
        //                                        || x.FinanceManagerId == pResourceId || x.ProjectSPOC == pResourceId)
        //                                    && (x.IsDraft == false || x.IsDraft == null)).ToList();
        //    }
        //    else if (pIsDraft)
        //    {
        //        proLists = proLists.Where(x => x.IsDraft == pIsDraft && x.CreatedBy == pResourceId).ToList();
        //    }
        //    else if (listResourceId != null && listResourceId.Count > 0)
        //    {
        //        proLists = proLists.Where(x => ((x.CreatedBy.HasValue && listResourceId.Contains((int)x.CreatedBy))
        //                                        || (x.EngineeringLeadId.HasValue && listResourceId.Contains((int)x.EngineeringLeadId))
        //                                        || (x.FinanceManagerId.HasValue && listResourceId.Contains((int)x.FinanceManagerId))
        //                                        || (x.ProjectSPOC.HasValue && listResourceId.Contains((int)x.ProjectSPOC)))
        //                                        && (x.IsDraft == false || x.IsDraft == null)).ToList();
        //    }
        //    else
        //    {
        //        proLists = proLists.Where(x => (x.IsDraft == false || x.IsDraft == null)).ToList();
        //    }
        //    foreach (var proList in proLists)
        //    {
        //        int associates = 0, resourceCount = 0;
        //        resourceCount = (from resourceAllocation in dbContext.ResourceAllocation
        //                         where resourceAllocation.ProjectId == proList.ProjectId && resourceAllocation.EmployeeId != null
        //                         select new { resourceAllocation.ProjectId }).Count();
        //        var projectListsBasedRA = (from projectDetails in dbContext.ProjectDetails
        //                                   join resourceAllocation in dbContext.ResourceAllocation on projectDetails.ProjectId equals resourceAllocation.ProjectId
        //                                   where projectDetails.AccountId == proList.AccountId && projectDetails.ProjectId == proList.ProjectId && resourceAllocation.EmployeeId != null
        //                                   select new { resourceAllocation.EmployeeId, projectDetails.ProjectName }).ToList();
        //        foreach (var project in projectListsBasedRA.GroupBy(x => x.ProjectName))
        //        {
        //            associates = project.Select(x => x.EmployeeId).Count();
        //        }
        //        ProjectListView projectList = new()
        //        {
        //            ProjectId = proList.ProjectId,
        //            ProjectName = proList.ProjectName,
        //            AccountName = "",
        //            AccountId = proList.AccountId,
        //            Associates = Convert.ToString(associates) + " Associates",
        //            SPOC = "",
        //            SPOCId = proList.ProjectSPOC == null ? 0 : (int)proList.ProjectSPOC,
        //            Timesheets = "",
        //            ProjectStatus = proList.ProjectStatus,
        //            UserRole = "",
        //            CreatedBy = proList.CreatedBy,
        //            ProjectDuration = proList.ProjectDuration,
        //            ProjectEndDate = proList.ProjectEndDate,
        //            ProjectStartDate = proList.ProjectStartDate,
        //            ResourceCount = resourceCount,
        //            ProjectStatusCode = proList.ProjectStatusCode,
        //            ProjectStatusName = "",
        //            ProjectChanges = string.IsNullOrEmpty(proList.ProjectChanges) ? string.Empty : CommonLib.RemoveDuplicateFromJsonString(proList.ProjectChanges),
        //            Logo = proList.Logo
        //        };
        //        projectListViews.Add(projectList);
        //    }
        //    return projectListViews;
        //}
        public List<CurrencyType> GetAllCurrencyType()
        {
            return dbContext.CurrencyType.ToList();
        }
        public List<ProjectType> GetAllProjectType()
        {
            return dbContext.ProjectType.ToList();
        }
        public List<RateFrequency> GetAllRateFrequency()
        {
            return dbContext.RateFrequency.ToList();
        }
        public List<Allocation> GetAllAllocation()
        {
            return dbContext.Allocation.ToList();
        }

        List<ProjectRole> IProjectDetailsRepository.GetAllProjectRole()
        {
            return dbContext.ProjectRole.ToList();
        }
        public List<ProjectDetails> GetProjectDetailByAccountId(List<int?> lstAccountId)
        {
            return dbContext.ProjectDetails.Where(x => lstAccountId.Contains(x.AccountId)).ToList();
        }
        public List<ResourceAllocation> GetResourceAllocationByAccountId(List<int?> lstAccountId)
        {
            return (from a in dbContext.ProjectDetails
                    join b in dbContext.ResourceAllocation
                   on a.ProjectId equals b.ProjectId
                    where lstAccountId.Contains(a.AccountId)
                    select b).ToList();
        }
        public List<ResourceProjectList> GetResourceProjectList(int ResourceId = 0)
        {
            List<ResourceProjectList> projectLists = new();
            if (ResourceId > 0)
            {
                projectLists = (from projectDetails in dbContext.ProjectDetails
                                join resourceAllocation in dbContext.ResourceAllocation on projectDetails.ProjectId equals resourceAllocation.ProjectId
                                where resourceAllocation.EmployeeId == ResourceId && projectDetails.ProjectStatus == "Ongoing"
                                select new ResourceProjectList { ProjectId = projectDetails.ProjectId, ProjectName = projectDetails.ProjectName }).ToList();
            }
            return projectLists;
        }
        /* public List<TeamMemberDetails> GetReportingPersonTeamMember(int resourceId, DateTime? weekStartDay)
         {
             return (from projectDetails in dbContext.ProjectDetails
                     join resourceAllocation in dbContext.ResourceAllocation on projectDetails.ProjectId equals resourceAllocation.ProjectId
                     where projectDetails.ProjectSPOC == resourceId && projectDetails.ProjectStatus == "Ongoing"
                     //&& resourceAllocation.EmployeeId != resourceId
                     && (projectDetails.ProjectStartDate != null && projectDetails.ProjectStartDate <= weekStartDay.Value.Date)
                     && (projectDetails.ProjectEndDate != null && projectDetails.ProjectEndDate >= weekStartDay.Value.Date.AddDays(6))
                     && (resourceAllocation.StartDate.HasValue && resourceAllocation.StartDate.Value.Date <= CommonLib.GetTodayStartTime().Date)
                     && (resourceAllocation.EndDate == null || (resourceAllocation.EndDate.HasValue && resourceAllocation.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date))
                     select new TeamMemberDetails
                     {
                       UserId=  resourceAllocation.EmployeeId == null ? 0 : (int)resourceAllocation.EmployeeId,
                       ProjectId= projectDetails.ProjectId,
                       Allocation= dbContext.Allocation.Where(x => x.AllocationId == resourceAllocation.AllocationId).Select(x => string.IsNullOrEmpty(x.AllocationDescription) ? 0 : Convert.ToDecimal(x.AllocationDescription.Substring(0, x.AllocationDescription.Length - 1))).FirstOrDefault()
                     }).ToList();
         }*/
        public ProjectTimesheet GetProjectTimesheet(int resourceId)
        {
            List<ProjectDetails> resourceProjectDetails = (from project in dbContext.ProjectDetails
                                                           join resource in dbContext.ResourceAllocation on project.ProjectId equals resource.ProjectId
                                                           where resource.EmployeeId == resourceId
                                                           select project).ToList();
            //List<ProjectDetails> spocProjectDetails = (from project in dbContext.ProjectDetails
            //                                           where project.ProjectSPOC == resourceId
            //                                           select project).ToList();
            ProjectTimesheet ProjectTimesheet = new()
            {
                //ProjectDetails = (resourceProjectDetails == null ? new List<ProjectDetails>() : resourceProjectDetails).Concat(spocProjectDetails == null ? new List<ProjectDetails>() : spocProjectDetails).ToList(),
                ProjectDetails = resourceProjectDetails == null ? new List<ProjectDetails>() : resourceProjectDetails,
                ResourceAllocation = dbContext.ResourceAllocation.Where(x => x.EmployeeId == resourceId).Select(x => x).ToList(),
                Allocation = dbContext.Allocation.ToList()
            };
            return ProjectTimesheet;
        }
        /* public List<ProjectSPOC> GetProjectSPOCByProjectId(List<int> lstProjectId)
         {
             return dbContext.ProjectDetails.Where(y => lstProjectId.Contains(y.ProjectId)).Select(x => new
                             ProjectSPOC
             { projectId = x.ProjectId, SPOCId = x.ProjectSPOC, projectName=x.ProjectName }).ToList();
         }*/
        /*  public bool CheckTeamMembersByResourceId(int resourceId)
          {
              List<int> teamMember = (from project in dbContext.ProjectDetails
                                      join resource in dbContext.ResourceAllocation on project.ProjectId equals resource.ProjectId
                                      where project.ProjectStatus == "Ongoing" && project.ProjectSPOC == resourceId
                                      select (project.ProjectId)).ToList();
              return teamMember?.Count > 0;
          }*/
        public List<ProjectDetails> GetAllProjectsDetails()
        {
            return dbContext.ProjectDetails.ToList();
        }
        public List<ProjectNames> GetProjectNameById(List<int> lstProjectId)
        {
            return dbContext.ProjectDetails.Where(x => lstProjectId.Contains(x.ProjectId)).Select(y => new ProjectNames { ProjectId = y.ProjectId, ProjectName = y.ProjectName }).ToList();
        }
        public List<ReportData> GetResourceUtilisationData()
        {
            List<ReportData> resourceData = (from RA in dbContext.ResourceAllocation
                                             join A in dbContext.Allocation on RA.AllocationId equals A.AllocationId
                                             join PD in dbContext.ProjectDetails on RA.ProjectId equals PD.ProjectId
                                             where RA.EmployeeId.HasValue && RA.ProjectId > 0
                                                        && (RA.StartDate.HasValue && RA.StartDate.Value.Date <= CommonLib.GetTodayStartTime().Date)
                                                        && (RA.EndDate == null || (RA.EndDate.HasValue && RA.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date))
                                             group new { RA, A } by new { RA.EmployeeId } into report
                                             select new ReportData
                                             {
                                                 Id = report.Key.EmployeeId == null ? 0 : (int)report.Key.EmployeeId,
                                                 Count = report.Sum(x => string.IsNullOrEmpty(x.A.AllocationDescription) ? 0 : Convert.ToInt32(x.A.AllocationDescription.Substring(0, x.A.AllocationDescription.Length - 1)))
                                             }).ToList();
            return resourceData;
        }
        /*  public List<ProjectDetailView> GetProjectDetailsByAccountId(int pAccountID)
          {
              List<ProjectDetailView> projectDetailListView = new();
              List<ProjectDetails> projectDetailsList = dbContext.ProjectDetails.Where(x => x.AccountId == pAccountID).ToList();
              if (projectDetailsList != null && projectDetailsList.Count > 0)
              {
                  foreach (var projectDetails in projectDetailsList)
                  {
                      ProjectDetailView projectDetailView = new()
                      {
                          ProjectId = projectDetails.ProjectId,
                          AccountId = projectDetails.AccountId,
                          AccountName = "",
                          CreatedBy = projectDetails.CreatedBy,
                          CreatedByName = "",
                          CreatedOn = projectDetails.CreatedOn,
                          CurrencyType = dbContext.CurrencyType.Where(x => x.CurrencyTypeId == projectDetails.CurrencyTypeId).Select(x => x.CurrencyCode).FirstOrDefault(),
                          CurrencyTypeId = projectDetails.CurrencyTypeId,
                          Experience = projectDetails.Experience,
                          FinanceManagerId = projectDetails.FinanceManagerId,
                          FormattedProjectId = "PRJ1" + projectDetails.ProjectId.ToString().PadLeft(4, '0')
                      };
                      //if (projectDetails.ProjectSPOC != null && projectDetails.ProjectSPOC > 0)
                      //    projectDetailView.ProjectManagerEmpId = "NXT " + (projectDetails?.ProjectSPOC).ToString();
                      //else
                      //    projectDetailView.ProjectManagerEmpId = "";
                      projectDetailView.ProjectManagerEmpId = "";
                      projectDetailView.IsDraft = projectDetails.IsDraft;
                      projectDetailView.ProjectDescription = projectDetails.ProjectDescription;
                      projectDetailView.ProjectDuration = projectDetails.ProjectDuration;
                      projectDetailView.ProjectEndDate = projectDetails.ProjectEndDate;
                      projectDetailView.ProjectManager = "";
                      projectDetailView.ProjectManagerRole = "";
                      projectDetailView.ProjectName = projectDetails.ProjectName;
                      projectDetailView.ProjectSPOC = projectDetails.ProjectSPOC;
                      projectDetailView.ProjectStartDate = projectDetails.ProjectStartDate;
                      projectDetailView.ProjectStatus = projectDetails.ProjectStatus;
                      projectDetailView.ProjectType = dbContext.ProjectType.Where(x => x.ProjectTypeId == projectDetails.ProjectTypeId).Select(x => x.ProjectTypeDescription).FirstOrDefault();
                      projectDetailView.ProjectTypeId = projectDetails.ProjectTypeId;
                      projectDetailView.RateFrequency = projectDetails.RateFrequency;
                      projectDetailView.TotalSOWAmount = projectDetails.TotalSOWAmount;
                      projectDetailView.AdditionalComments = projectDetails.Comments;
                      projectDetailView.ProjectStatusCode = projectDetails.ProjectStatusCode;
                      projectDetailView.ProjectStatusName = "";
                      projectDetailView.bUAccountableForProject = projectDetails.bUAccountableForProject;
                      projectDetailView.Logo = projectDetails.Logo;
                      List<ResourceAllocationList> resourceAllocationLists = new();
                      List<ResourceAllocation> lstResourceAllocation = dbContext.ResourceAllocation.Where(x => x.ProjectId == projectDetails.ProjectId).ToList();
                      foreach (ResourceAllocation resourceAllocation in lstResourceAllocation)
                      {
                          ResourceAllocationList resourceAllocationList = new()
                          {
                              Allocation = dbContext.Allocation.Where(x => x.AllocationId == resourceAllocation.AllocationId).Select(x => x.AllocationDescription).FirstOrDefault(),
                              AllocationId = resourceAllocation.AllocationId,
                              CreatedBy = resourceAllocation.CreatedBy,
                              CreatedOn = resourceAllocation.CreatedOn,
                              EmployeeId = resourceAllocation.EmployeeId,
                              EmployeeName = "",
                              EndDate = resourceAllocation.EndDate,
                              ModifiedBy = resourceAllocation.ModifiedBy,
                              ModifiedOn = resourceAllocation.ModifiedOn,
                              ProjectId = resourceAllocation.ProjectId,
                              ProjectName = projectDetails.ProjectName,
                              RateFrequency = dbContext.RateFrequency.Where(x => x.RateFrequencyId == resourceAllocation.RateFrequencyId).Select(x => x.RateFrequencyDescription).FirstOrDefault(),
                              RateFrequencyId = resourceAllocation.RateFrequencyId,
                              RequiredSkillSet = "",
                              RequiredSkillSetId = resourceAllocation.RequiredSkillSetId,
                              ResourceAllocationId = resourceAllocation.ResourceAllocationId,
                              SkillRate = resourceAllocation.SkillRate,
                              StartDate = resourceAllocation.StartDate,
                              IsBillable = resourceAllocation.IsBillable,
                              Experience = resourceAllocation.Experience,
                              IsAdditionalResource=resourceAllocation.IsAdditionalResource
                          };
                          resourceAllocationLists.Add(resourceAllocationList);
                      }
                      projectDetailView.ResourceAllocation = resourceAllocationLists;
                      projectDetailListView.Add(projectDetailView);
                  }
              }
              return projectDetailListView;
          }*/
        /* public List<ProjectNames> GetProjectsByResourceId(int resourceId)
         {
             return (from Projects in dbContext.ProjectDetails
                     where Projects.ProjectSPOC == resourceId && Projects.ProjectEndDate >= DateTime.Today && Projects.IsDraft == false
                     && Projects.ProjectStatus == "Ongoing"
                     select new ProjectNames
                     {
                         ProjectId = Projects.ProjectId,
                         ProjectName = Projects.ProjectName
                     }).ToList();
         }
       */
        /*  public List<EmployeeProjectNames> GetProjectsAndEmpByResourceId(int resourceId)
          {
              return (from projects in dbContext.ProjectDetails
                      join employees in dbContext.ResourceAllocation
                      on projects.ProjectId equals employees.ProjectId
                      where projects.ProjectSPOC == resourceId && projects.ProjectEndDate >= DateTime.Today && projects.IsDraft == false
                      && employees.EndDate >= DateTime.Today && projects.ProjectStatus == "Ongoing"
                      select new EmployeeProjectNames
                      {
                          EmployeeId = (int)employees.EmployeeId,
                          ProjectId = projects.ProjectId,
                          ProjectName = projects.ProjectName,
                          IsBillable = employees.IsBillable == true ? "Billable" : "Non Billable"
                      }).ToList();
          }
          public List<ProjectDetails> GetActiveProjectDetailsByResourceId(int resourceId)
          {
              List<ProjectDetails> data = (from projects in dbContext.ProjectDetails
                                               *//*join resource in dbContext.ResourceAllocation
                                               on projects.ProjectId equals resource.ProjectId
                                               where (resource.EmployeeId == resourceId ||*//*
                                           where (projects.ProjectSPOC == resourceId || projects.CreatedBy == resourceId || projects.FinanceManagerId == resourceId)
                                           && projects.ProjectEndDate >= DateTime.Today && projects.IsDraft == false
                                           && projects.ProjectStatus == "Ongoing"
                                           select new ProjectDetails
                                           {
                                               ProjectId = projects.ProjectId,
                                               ProjectName = projects.ProjectName,
                                               AccountId = projects.AccountId
                                           }).ToList();
              return data;
          }*/
        public List<EmployeeProjectNames> GetProjectsByEmployeeId(int pResourceId)
        {
            return (from projects in dbContext.ProjectDetails
                    join resource in dbContext.ResourceAllocation on projects.ProjectId equals resource.ProjectId
                    join pType in dbContext.ProjectType on projects.ProjectTypeId equals pType.ProjectTypeId
                    where resource.EmployeeId == pResourceId && projects.ProjectEndDate >= DateTime.Today && projects.IsDraft == false
                    && (resource.EndDate == null || resource.EndDate >= DateTime.Today) && projects.ProjectStatus == "Ongoing"
                    select new EmployeeProjectNames
                    {
                        EmployeeId = (int)resource.EmployeeId,
                        ProjectId = projects.ProjectId,
                        ProjectName = projects.ProjectName,
                        IsBillable = pType.ProjectTypeDescription,
                    }).ToList();
        }
        /*     public List<ResourceAvailability> GetResourceAvailabilityEmployeeDetails(int pResourceId)
             {
                 List<ResourceAvailability> ResourceAvailability = new();
                 ResourceAvailability = (from projects in dbContext.ProjectDetails
                                         join resource in dbContext.ResourceAllocation on projects.ProjectId equals resource.ProjectId
                                         where projects.ProjectSPOC == pResourceId && projects.ProjectEndDate >= DateTime.Today && projects.IsDraft == false
                                         && (resource.EndDate == null || resource.EndDate >= DateTime.Today) && projects.ProjectStatus == "Ongoing"
                                         select new ResourceAvailability
                                         {
                                             EmployeeId = resource.EmployeeId,
                                             ProjectId = resource.ProjectId,
                                             AllocationPercent = resource.AllocationId == 1 ? 10 : resource.AllocationId == 2 ? 25 : resource.AllocationId == 3 ? 50 : resource.AllocationId == 4 ? 75 : resource.AllocationId == 5 ? 100 : 0,
                                             Availability = resource.AllocationId == 1 ? 100 - 10 : resource.AllocationId == 2 ? 100 - 25 : resource.AllocationId == 3 ? 100 - 50 : resource.AllocationId == 4 ? 100 - 75 : resource.AllocationId == 5 ? 100 - 100 : 0,
                                             SkillsetId = resource.RequiredSkillSetId
                                         }).ToList();
                 return ResourceAvailability;
             }
             public List<int?> GetAccountIdByBUHeadId(int resourceId)
             {
                 return (from projects in dbContext.ProjectDetails                                             
                                              where projects.bUAccountableForProject== resourceId && projects.IsDraft == false                                         
                                              select projects.AccountId).ToList();
             }
     */
        /*  public List<EmployeeProjectList> GetEmployeeProjectListById(int EmployeeId, DateTime fromdate, DateTime todate)
          {
              List<EmployeeProjectList> employeeProjectLists = new();
              int totalDays = Convert.ToInt32((todate - fromdate).TotalDays);
              for ( int days = 0; days <= totalDays; days++)
              {
                  EmployeeProjectList employeeProject = new();
                  employeeProject.Date = fromdate.AddDays(days);
                   employeeProject.ProjectList = dbContext.ProjectDetails.Join(dbContext.ResourceAllocation, proj => proj.ProjectId, res => res.ProjectId, (proj, res) => new { proj, res }).
                   Where(x => x.res.EmployeeId == EmployeeId && (x.res.StartDate == null || x.res.StartDate.Value.Date <= employeeProject.Date.Date ) && ( x.res.EndDate == null || x.res.EndDate.Value.Date >= employeeProject.Date.Date) &&
                    (x.proj.ProjectStartDate == null || x.proj.ProjectStartDate.Value.Date <= employeeProject.Date.Date) && (x.proj.ProjectEndDate == null || x.proj.ProjectEndDate.Value.Date >= employeeProject.Date.Date) ).
                   Select(x => new EmployeeProjectNames
                   {
                     EmployeeId = x.res.EmployeeId == null ? 0 : (int)x.res.EmployeeId,
                     ProjectId = x.proj.ProjectId,
                     ProjectName = x.proj.ProjectName,
                     ProjectAllocation = dbContext.Allocation.Where(y=>y.AllocationId ==  x.res.AllocationId).Select(x=>x.AllocationDescription).FirstOrDefault()
                   }).ToList();
                  employeeProjectLists.Add(employeeProject);
              }
              return employeeProjectLists;
          }
  */
        ProjectDetails IProjectDetailsRepository.GetByName(string pProjectName, int pProjectID)
        {
            throw new NotImplementedException();
        }

        ProjectDetails IProjectDetailsRepository.GetByProjectName(string pProjectName)
        {
            throw new NotImplementedException();
        }



        List<ProjectDetails> IProjectDetailsRepository.GetByName(string[] pProjectName)
        {
            throw new NotImplementedException();
        }

        ProjectDetailView IProjectDetailsRepository.GetProjectDetailById(int pProjectID)
        {
            throw new NotImplementedException();
        }

        List<ProjectListView> IProjectDetailsRepository.GetProjectDetailsByResourceId(ProjectCustomerEmployeeList listResourceId)
        {
            throw new NotImplementedException();
        }

        List<ProjectListView> IProjectDetailsRepository.GetDraftedProjectDetailsByResourceId(int pResourceId)
        {
            throw new NotImplementedException();
        }

        List<CurrencyType> IProjectDetailsRepository.GetAllCurrencyType()
        {
            return dbContext.CurrencyType.ToList();
        }



        List<RateFrequency> IProjectDetailsRepository.GetAllRateFrequency()
        {
            throw new NotImplementedException();
        }

        List<Allocation> IProjectDetailsRepository.GetAllAllocation()
        {
            throw new NotImplementedException();
        }



        List<ProjectDetails> IProjectDetailsRepository.GetProjectDetailByAccountId(List<int?> lstAccountId)
        {
            throw new NotImplementedException();
        }

        List<ResourceAllocation> IProjectDetailsRepository.GetResourceAllocationByAccountId(List<int?> lstAccountId)
        {
            throw new NotImplementedException();
        }

        List<ResourceProjectList> IProjectDetailsRepository.GetResourceProjectList(int ResourceId)
        {
            throw new NotImplementedException();
        }

        List<TeamMemberDetails> IProjectDetailsRepository.GetReportingPersonTeamMember(int resourceId, DateTime? weekStartDay)
        {
            throw new NotImplementedException();
        }

        ProjectTimesheet IProjectDetailsRepository.GetProjectTimesheet(int resourceId)
        {
            throw new NotImplementedException();
        }

        List<ProjectSPOC> IProjectDetailsRepository.GetProjectSPOCByProjectId(List<int> lstProjectId)
        {
            throw new NotImplementedException();
        }

        bool IProjectDetailsRepository.CheckTeamMembersByResourceId(int resourceId)
        {
            throw new NotImplementedException();
        }

        List<ProjectDetails> IProjectDetailsRepository.GetAllProjectsDetails()
        {
            throw new NotImplementedException();
        }

        List<ProjectNames> IProjectDetailsRepository.GetProjectNameById(List<int> lstProjectId)
        {
            throw new NotImplementedException();
        }

        List<ReportData> IProjectDetailsRepository.GetResourceUtilisationData()
        {
            throw new NotImplementedException();
        }

        List<ProjectDetailView> IProjectDetailsRepository.GetProjectDetailsByAccountId(int pAccountID)
        {
            throw new NotImplementedException();
        }

        List<ProjectNames> IProjectDetailsRepository.GetProjectsByResourceId(int resourceId)
        {
            throw new NotImplementedException();
        }

        List<EmployeeProjectNames> IProjectDetailsRepository.GetProjectsAndEmpByResourceId(int resourceId)
        {
            throw new NotImplementedException();
        }

        List<EmployeeProjectNames> IProjectDetailsRepository.GetProjectsByEmployeeId(int pResourceId)
        {
            throw new NotImplementedException();
        }

        List<ProjectDetails> IProjectDetailsRepository.GetActiveProjectDetailsByResourceId(int resourceId)
        {
            throw new NotImplementedException();
        }

        List<ResourceAvailability> IProjectDetailsRepository.GetResourceAvailabilityEmployeeDetails(int pResourceId)
        {
            throw new NotImplementedException();
        }

        List<int?> IProjectDetailsRepository.GetAccountIdByBUHeadId(int resourceId)
        {
            throw new NotImplementedException();
        }

        List<EmployeeProjectList> IProjectDetailsRepository.GetEmployeeProjectListById(int EmployeeId, DateTime fromdate, DateTime todate)
        {
            throw new NotImplementedException();
        }

        #region Get Finance Head Projects by employee Id
        List<ProjectDetails> IProjectDetailsRepository.GetFinanceHeadProjectsByEmployeeId(int employeeId)
        {

            return dbContext.ProjectDetails.Where(x => x.FinanceManagerId == employeeId).ToList();
        }
        #endregion

        #region Get Projects by BU
        List<ProjectDetails> IProjectDetailsRepository.GetProjectsByBU(int employeeId)
        {
            return dbContext.ProjectDetails.Where(x => x.BuAccountableForProject == employeeId).ToList();

        }




        /*  void IProjectDetailsRepository.CreateResourceAllocationVersion(ResourceAllocation resourceAllocation)
          {
              float Increase = 0.0F;

              if (resourceAllocation != null)
              {
                  Increase += 0.1F;
                  String VersionId = "V" + Increase;
              }
          }*/



        VersionProjectDetail IProjectDetailsRepository.GetVersionByID(int projectId)
        {
            return dbContext.VersionProjectDetail.Where(x => x.ProjectId == projectId).FirstOrDefault();
        }


        Task IProjectDetailsRepository.AddProjectVersion(VersionProjectDetail versionProjectDetail)
        {
            var data = dbContext.VersionProjectDetail.Add(versionProjectDetail);
            return null;
        }



        List<ProjectDetails> IProjectDetailsRepository.GetAllProjects()
        {
            throw new NotImplementedException();
        }


        List<VersionProjectDetail> IProjectDetailsRepository.GetAllVersion()
        {
            return dbContext.VersionProjectDetail.ToList();
        }

        ProjectDetails IProjectDetailsRepository.GetByID(int pProjectID)
        {
            throw new NotImplementedException();
        }

        List<ProjectType> IProjectDetailsRepository.GetAllProjectType()
        {
            throw new NotImplementedException();
        }

        bool IProjectDetailsRepository.ProjectNameDuplication(string pProjectName, int pProjectID, int pAccountID)
        {
            throw new NotImplementedException();
        }

        List<AppConstants> IProjectDetailsRepository.GetAllProjectSubType()
        {
            return dbContext.AppConstants.ToList();





            #endregion


        }

    }
    }