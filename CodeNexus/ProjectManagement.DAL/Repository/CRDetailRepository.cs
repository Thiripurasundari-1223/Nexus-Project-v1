using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.DAL.Repository
{
    public interface IChangeRequestDetailRepository : IBaseRepository<ChangeRequest>
    {
        List<ChangeRequestView> GetChangeRequestDetailByProjectId(int pProjectId);
        List<ChangeRequestType> GetAllChangeRequestType();
        ChangeRequestView GetChangeRequestDetailById(int pChangeRequestId);
        List<ProjectDetailCommentsList> GetChangeRequestCommentsById(int pChangeRequestID);
        List<ProjectListView> GetAllActiveProjects();
        ChangeRequest GetByID(int pChangeRequestId);
    }
    public class ChangeRequestDetailRepository : BaseRepository<ChangeRequest>, IChangeRequestDetailRepository
    {
        private readonly PMDBContext _dbContext;
        public ChangeRequestDetailRepository(PMDBContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
        public ChangeRequest GetByID(int pChangeRequestId)
        {
            return _dbContext.ChangeRequest.Where(x => x.ChangeRequestId == pChangeRequestId).FirstOrDefault();
        }
        public List<ChangeRequestView> GetChangeRequestDetailByProjectId(int pProjectId)
        {
            return GetChangeRequestList(pProjectId: pProjectId);
        }
        public List<ChangeRequestType> GetAllChangeRequestType()
        {
            return _dbContext.ChangeRequestType.ToList();
        }
        public ChangeRequestView GetChangeRequestDetailById(int pChangeRequestId)
        {
            var CRDetails = GetChangeRequestList(pChangeRequestId: pChangeRequestId).AsQueryable().FirstOrDefault();
            return CRDetails == null ? new ChangeRequestView() : CRDetails;
        }
        private List<ChangeRequestView> GetChangeRequestList(int pProjectId = 0, int pChangeRequestId = 0)
        {
            List<ChangeRequest> changeRequests = new List<ChangeRequest>();
            if (pProjectId > 0) changeRequests = _dbContext.ChangeRequest.Where(x => x.ProjectId == pProjectId).ToList();
            if (pChangeRequestId > 0) changeRequests = _dbContext.ChangeRequest.Where(x => x.ChangeRequestId == pChangeRequestId).ToList();
            List<ChangeRequestView> changeRequestViews = new List<ChangeRequestView>();
            if (changeRequests != null && changeRequests.Count > 0)
            {
                foreach (ChangeRequest changeRequest in changeRequests)
                {
                    ChangeRequestView changeRequestView = new ChangeRequestView
                    {
                        //FormattedChangeRequestId = changeRequest.FormattedChangeRequestId,
                        ChangeRequestDescription = changeRequest.ChangeRequestDescription,
                        ChangeRequestDuration = changeRequest.ChangeRequestDuration,
                        ChangeRequestEndDate = changeRequest.ChangeRequestEndDate,
                        ChangeRequestId = changeRequest.ChangeRequestId,
                        ChangeRequestName = changeRequest.ChangeRequestName,
                        ChangeRequestStartDate = changeRequest.ChangeRequestStartDate,
                        ChangeRequestStatus = changeRequest.ChangeRequestStatus,
                        ChangeRequestType = _dbContext.ChangeRequestType.Where(x => x.ChangeRequestTypeId == changeRequest.ChangeRequestTypeId).Select(x => x.ChangeRequestTypeDescription).FirstOrDefault(),
                        ChangeRequestTypeId = changeRequest.ChangeRequestTypeId,
                        CreatedBy = changeRequest.CreatedBy,
                        CreatedByName = "",
                        CreatedOn = changeRequest.CreatedOn,
                        CurrencyId = changeRequest.CurrencyId,
                        CurrencyType = _dbContext.CurrencyType.Where(x => x.CurrencyTypeId == changeRequest.CurrencyId).Select(x => x.CurrencyCode).FirstOrDefault(),
                        ModifiedBy = changeRequest.ModifiedBy,
                        ModifiedOn = changeRequest.ModifiedOn,
                        ProjectId = changeRequest.ProjectId,
                        SOWAmount = changeRequest.SOWAmount,
                        CRStatusCode = changeRequest.CRStatusCode,
                        CRStatusName = "",
                        CRChanges = string.IsNullOrEmpty(changeRequest.CRChanges) ? string.Empty : SharedLibraries.CommonLib.RemoveDuplicateFromJsonString(changeRequest.CRChanges),
                        FormattedProjectId = "PRJ1" + changeRequest.ProjectId.ToString().PadLeft(4, '0'),
                        FormattedChangeRequestId = "RFC1" + changeRequest.ChangeRequestId.ToString().PadLeft(4, '0'),
                    };
                    List<ResourceAllocation> resourceAllocations = _dbContext.ResourceAllocation.Where(x => x.ChangeRequestId == changeRequest.ChangeRequestId).ToList();
                    List<ResourceAllocationList> resourceAllocationLists = new List<ResourceAllocationList>();
                    foreach (ResourceAllocation resourceAllocation in resourceAllocations)
                    {
                        ResourceAllocationList resourceAllocationList = new ResourceAllocationList
                        {
                            Allocation = _dbContext.Allocation.Where(x => x.AllocationId == resourceAllocation.AllocationId).Select(x => x.AllocationDescription).FirstOrDefault(),
                            AllocationId = resourceAllocation.AllocationId,
                            CreatedBy = resourceAllocation.CreatedBy,
                            CreatedOn = resourceAllocation.CreatedOn,
                            EmployeeId = resourceAllocation.EmployeeId,
                            EmployeeName = "",
                            EndDate = resourceAllocation.EndDate,
                            ModifiedBy = resourceAllocation.ModifiedBy,
                            ModifiedOn = resourceAllocation.ModifiedOn,
                            ProjectId = resourceAllocation.ProjectId,
                            RateFrequency = _dbContext.RateFrequency.Where(x => x.RateFrequencyId == resourceAllocation.RateFrequencyId).Select(x => x.RateFrequencyDescription).FirstOrDefault(),
                            RateFrequencyId = resourceAllocation.RateFrequencyId,
                            RequiredSkillSet = "",
                            RequiredSkillSetId = resourceAllocation.RequiredSkillSetId,
                            ResourceAllocationId = resourceAllocation.ResourceAllocationId,
                            SkillRate = resourceAllocation.SkillRate,
                            StartDate = resourceAllocation.StartDate,
                            IsBillable = resourceAllocation.IsBillable,
                            Experience = resourceAllocation.Experience,
                        };
                        resourceAllocationLists.Add(resourceAllocationList);
                    }
                    changeRequestView.ResourceAllocation = resourceAllocationLists;
                    changeRequestViews.Add(changeRequestView);
                }
            }
            return changeRequestViews;
        }
        public List<ProjectDetailCommentsList> GetChangeRequestCommentsById(int pChangeRequestId)
        {
            List<ProjectDetailCommentsList> ProjectDetailCommentsLists = new List<ProjectDetailCommentsList>();
            List<ProjectDetailComments> ProjectDetailComments = _dbContext.ProjectDetailComments.Where(x => x.ChangeRequestId == pChangeRequestId).ToList();
            foreach (ProjectDetailComments detailComments in ProjectDetailComments.OrderByDescending(x => x.ProjectDetailCommentId))
            {
                ProjectDetailCommentsList ProjectDetailCommentsList = new ProjectDetailCommentsList
                {
                    ProjectDetailCommentId = detailComments.ProjectDetailCommentId,
                    ChangeRequestId = detailComments.ChangeRequestId,
                    CreatedByName = "",
                    Comments = detailComments.Comments,
                    CreatedBy = detailComments.CreatedBy,
                    CreatedOn = detailComments.CreatedOn,
                    ModifiedBy = detailComments.ModifiedBy,
                    ModifiedOn = detailComments.ModifiedOn
                };
                ProjectDetailCommentsLists.Add(ProjectDetailCommentsList);
            }
            return ProjectDetailCommentsLists;
        }
        public List<ProjectListView> GetAllActiveProjects()
        {
            return _dbContext.ProjectDetails.Select(project =>
                new ProjectListView
                {
                    ProjectId = project.ProjectId,
                    ProjectStatus = project.ProjectStatus,
                    AccountId = project.AccountId,
                    AccountName = "",
                    ProjectName = project.ProjectName,
                    SPOCId = project.ProjectSPOC == null ? 0 : (int)project.ProjectSPOC,
                    SPOC = "",
                    ProjectStartDate = project.ProjectStartDate,
                    ProjectEndDate = project.ProjectEndDate,
                    ProjectStatusCode = project.ProjectStatusCode,
                    ProjectStatusName = "",
                    Logo = project.Logo,
                    LogoBase64 = project.Logo != null ? Convert.ToBase64String(System.IO.File.ReadAllBytes(project.Logo)) : null
                }
                ).ToList();            
        }
    }
}