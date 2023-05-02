using ProjectManagement.DAL.DBContext;
using SharedLibraries;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.DAL.Repository
{
    public interface IResouceAllocationRepository : IBaseRepository<ResourceAllocation>
    {
        ResourceAllocation GetResourceByEmployeeID(int pEmployeeID);
        List<ResourceAllocation> GetResourceByProjectID(int pProjectID);
        ResourceAllocation GetByID(int resourceAllocationId);
        string ResourceAllocationDuplication(UpdateResourceAllocation pUpdateResourceAllocation);
        List<int> GetProjectAllocatedResourceList();
        List<ResourceAllocation> GetAllResourceAllocation();
        int GetContributionHomeReport(int employeeId);
        HomeReportData GetResourceBillabilityHomeReport();
        HomeReportData GetResourceAvailabilityHomeReport();
        ResourceAllocation GetResourceByEmployeeId(int projectId, int employeeId);
        ResourceAllocation GetResourceAllocationById(int resourceAllocationId, int iterationID, int projectId);
       
        VersionResourceAllocation GetVersionByID(int? resourceAllocationId);
      
    }
    public class ResourceAllocationRepository : BaseRepository<ResourceAllocation>, IResouceAllocationRepository
    {
        private readonly PMDBContext _dbContext;
        public ResourceAllocationRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public ResourceAllocation GetResourceByEmployeeID(int pEmployeeID)
        {
            if (pEmployeeID > 0)
            {
                return _dbContext.ResourceAllocation.Where(r => r.EmployeeId == pEmployeeID).FirstOrDefault();
            }
            return null;
        }
        public ResourceAllocation GetByID(int resourceAllocationId)
        {
            if (resourceAllocationId > 0)
            {
                return _dbContext.ResourceAllocation.Where(r => r.ResourceAllocationId == resourceAllocationId).FirstOrDefault();
            }
            return null;
        }
        public List<ResourceAllocation> GetResourceByProjectID(int pProjectID)
        {
            if (pProjectID > 0)
            {
                return _dbContext.ResourceAllocation.Where(r => r.ProjectId == pProjectID).ToList();
            }
            return null;
        }
   /*     public string ResourceAllocationDuplication(UpdateResourceAllocation pUpdateResourceAllocation)
        {
            List<ResourceAllocation> resourceAllocations = _dbContext.ResourceAllocation.Where(x => x.ProjectId == pUpdateResourceAllocation.ProjectId &&
                                                    x.ResourceAllocationId != pUpdateResourceAllocation.ResourceAllocationId && x.EmployeeId == pUpdateResourceAllocation.EmployeeId).ToList();
            List<ResourceAllocationList> allocations = new List<ResourceAllocationList>();
            if (resourceAllocations?.Count > 0)
            {
                return "The selected resource is already assigned. Please change resource and try again.";
            }
            if (pUpdateResourceAllocation?.EndDate == null)
            {
                allocations = (from x in _dbContext.ResourceAllocation
                               join a in _dbContext.Allocation on x.AllocationId equals a.AllocationId

                               where x.EmployeeId == pUpdateResourceAllocation.EmployeeId && x.ResourceAllocationId != pUpdateResourceAllocation.ResourceAllocationId &&
                               (pUpdateResourceAllocation.StartDate <= x.StartDate  || x.EndDate == null) 
                               select new ResourceAllocationList { ProjectId = x.ProjectId, Allocation = a.AllocationDescription }
            ).ToList();



            }
            else
            {
                //           allocations = (from x in _dbContext.ResourceAllocation
                //                          join a in _dbContext.Allocation on x.AllocationId equals a.AllocationId
                //                          where
                //(pUpdateResourceAllocation.StartDate >= x.StartDate && (pUpdateResourceAllocation.EndDate >= x.EndDate || pUpdateResourceAllocation.EndDate == null)) ||
                //   (pUpdateResourceAllocation.StartDate <= x.StartDate && (pUpdateResourceAllocation.EndDate <= x.EndDate || pUpdateResourceAllocation.EndDate == null)) ||
                //   (pUpdateResourceAllocation.StartDate >= x.StartDate && (pUpdateResourceAllocation.EndDate <= x.EndDate || pUpdateResourceAllocation.EndDate == null)) ||
                //   (pUpdateResourceAllocation.StartDate <= x.StartDate && (pUpdateResourceAllocation.EndDate >= x.EndDate || pUpdateResourceAllocation.EndDate == null))
                //                          select new ResourceAllocationList { ProjectId = x.ProjectId, Allocation = a.AllocationDescription }
                //       ).ToList();
                allocations = (from x in _dbContext.ResourceAllocation
                               join a in _dbContext.Allocation on x.AllocationId equals a.AllocationId
                               where x.EmployeeId == pUpdateResourceAllocation.EmployeeId && x.ResourceAllocationId != pUpdateResourceAllocation.ResourceAllocationId &&
           ((pUpdateResourceAllocation.StartDate <= x.StartDate  && (x.StartDate <=pUpdateResourceAllocation.EndDate )) || (x.EndDate == null && x.StartDate <= pUpdateResourceAllocation.EndDate)) 
                               select new ResourceAllocationList { ProjectId = x.ProjectId, Allocation = a.AllocationDescription }
                       ).ToList();
            }

            decimal totalAllocation = allocations.Sum(x => string.IsNullOrEmpty(x.Allocation) ? 0 : Convert.ToInt32(x.Allocation.Substring(0, x.Allocation.Length - 1)));

            decimal currentAllocation = 0;
            if (pUpdateResourceAllocation?.AllocationId > 0)
            {
                string allocation = _dbContext.Allocation.Where(x => x.AllocationId == pUpdateResourceAllocation.AllocationId).Select(x => x.AllocationDescription).FirstOrDefault();
                currentAllocation = string.IsNullOrEmpty(allocation) ? 0 : Convert.ToDecimal(allocation.Substring(0, allocation.Length - 1));
            }
            if ((totalAllocation + currentAllocation) > 100)
            {
                List<int> projectIdList = allocations.Select(x => x.ProjectId == null ? 0 : (int)x.ProjectId).ToList();
                List<string> projectList = _dbContext.ProjectDetails.Where(x => projectIdList.Contains(x.ProjectId)).Select(x => x.ProjectName).ToList();
                return "The system won't allow resource allocate more then 100% on same date, The selected resource already allocated other project (" + string.Join(",", projectList) + "). Please check it.";
            }
            return "";
        }
   */     public List<int> GetProjectAllocatedResourceList()
        {
            DateTime lastWeekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek).AddDays(-7);
            return _dbContext.ResourceAllocation.Where(x => x.EndDate >= lastWeekStartDate.Date).Select(x => x.EmployeeId == null ? 0 : (int)x.EmployeeId).Distinct().ToList();
        }
        public List<ResourceAllocation> GetAllResourceAllocation()
        {
            return _dbContext.ResourceAllocation.ToList();
        }
        public int GetContributionHomeReport(int employeeId)
        {

            var contribution = (from RA in _dbContext.ResourceAllocation
                                join A in _dbContext.Allocation on RA.AllocationId equals A.AllocationId
                                where RA.EmployeeId.HasValue && RA.ProjectId > 0
                                           && (RA.StartDate.HasValue && RA.StartDate.Value.Date <= CommonLib.GetTodayStartTime().Date)
                                           && (RA.EndDate == null || (RA.EndDate.HasValue && RA.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date))
                                           && RA.EmployeeId == employeeId
                                group new { RA, A } by new { RA.EmployeeId } into report
                                select new
                                {
                                    data = report.Sum(x => string.IsNullOrEmpty(x.A.AllocationDescription) ? 0 : Convert.ToInt32(x.A.AllocationDescription.Substring(0, x.A.AllocationDescription.Length - 1)))

                                }
                                                   ).FirstOrDefault();
            return contribution?.data == null ? 0 : (int)contribution?.data;
        }
        public HomeReportData GetResourceBillabilityHomeReport()
        {
            List<HomeReportData> resourceList = new List<HomeReportData>();
            HomeReportData billableResource = new HomeReportData();
            billableResource.ReportTitle = "Billable";
            billableResource.ReportData = _dbContext.ResourceAllocation.Where(x => x.IsBillable == true
            && (x.StartDate.HasValue && x.StartDate.Value.Date <= CommonLib.GetTodayStartTime().Date)
            && (x.EndDate == null || (x.EndDate.HasValue && x.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date))).Select(x => x.EmployeeId).Distinct().ToList().Count().ToString();
            return billableResource;
        }
        public HomeReportData GetResourceAvailabilityHomeReport()
        {
            HomeReportData billableResource = new HomeReportData();
            billableResource.ReportTitle = "On Boarding";
            billableResource.ReportData = _dbContext.ResourceAllocation.Where(x =>
             (x.StartDate.HasValue && x.StartDate.Value.Date <= CommonLib.GetTodayStartTime().Date)
            && (x.EndDate == null || (x.EndDate.HasValue && x.EndDate.Value.Date >= CommonLib.GetTodayEndTime().Date))).Select(x => x.EmployeeId).Distinct().ToList().Count().ToString();
            return billableResource;
        }
        public ResourceAllocation GetResourceByEmployeeId(int projectId, int employeeId)
        {
            return _dbContext.ResourceAllocation.Where(x => x.EmployeeId == employeeId && x.ProjectId == projectId).FirstOrDefault();
        }

        ResourceAllocation IResouceAllocationRepository.GetResourceByEmployeeID(int pEmployeeID)
        {
            throw new NotImplementedException();
        }

        List<ResourceAllocation> IResouceAllocationRepository.GetResourceByProjectID(int pProjectID)
        {
            throw new NotImplementedException();
        }

        ResourceAllocation IResouceAllocationRepository.GetByID(int resourceAllocationId)
        {
            throw new NotImplementedException();
        }

        string IResouceAllocationRepository.ResourceAllocationDuplication(UpdateResourceAllocation pUpdateResourceAllocation)
        {
            throw new NotImplementedException();
        }

        List<int> IResouceAllocationRepository.GetProjectAllocatedResourceList()
        {
            throw new NotImplementedException();
        }

        List<ResourceAllocation> IResouceAllocationRepository.GetAllResourceAllocation()
        {
            throw new NotImplementedException();
        }

        int IResouceAllocationRepository.GetContributionHomeReport(int employeeId)
        {
            throw new NotImplementedException();
        }

        HomeReportData IResouceAllocationRepository.GetResourceBillabilityHomeReport()
        {
            throw new NotImplementedException();
        }

        HomeReportData IResouceAllocationRepository.GetResourceAvailabilityHomeReport()
        {
            throw new NotImplementedException();
        }

        ResourceAllocation IResouceAllocationRepository.GetResourceByEmployeeId(int projectId, int employeeId)
        {
            throw new NotImplementedException();
        }

        public ResourceAllocation GetResourceAllocationById(int resourceAllocationId, int iterationID, int projectId)
        {
            if (resourceAllocationId > 0 && resourceAllocationId > 0 && iterationID > 0)
            {
                return _dbContext.ResourceAllocation.Where(r => r.IterationID == iterationID && r.ProjectId == projectId && r.ResourceAllocationId == resourceAllocationId).FirstOrDefault();
            }
            return null;
        }


        VersionResourceAllocation IResouceAllocationRepository.GetVersionByID(int? resourceAllocationId)
        {
            return _dbContext.VersionResourceAllocation.Where(r => r.ResourceAllocationId == resourceAllocationId).FirstOrDefault();
        }

        
    }
}