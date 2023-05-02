using System.Collections.Generic;
using System.Data;
using System.Linq;
using Appraisal.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;

namespace Appraisal.DAL.Repository
{
    public interface IWorkDayDetailRepository : IBaseRepository<WorkDayDetail>
    {
        WorkDayDetail GetByID(int pWorkDayDetailId);
        List<WorkDayDetail> GetByIds(List<int> pWorkDayDetailIds);
        //List<WorkDayDetail> GetWorkDayDetailByProjectIds(List<int> projectIds);
        //List<WorkDayDetail> GetWorkDayDetailByEmployeeId(int employeeId = 0);
        List<WorkDayDetail> GetWorkDayDetailByFilter(AppraisalWorkDayFilterView appraisalWorkDayFilterView);
        List<EmployeeRequestCount> WorkdayDetailCount(List<int> employeeIdList);

    }
    public class WorkDayDetailRepository : BaseRepository<WorkDayDetail>, IWorkDayDetailRepository
    {
        private readonly AppraisalDBContext _dbContext;
        public WorkDayDetailRepository(AppraisalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public WorkDayDetail GetByID(int workDayDetailId)
        {
            return _dbContext.WorkDayDetail.Where(x => x.WorkDayDetailId == workDayDetailId).FirstOrDefault();
        }
        public List<WorkDayDetail> GetByIds(List<int> pWorkDayDetailIds)
        {
            if (pWorkDayDetailIds?.Count > 0)
                return _dbContext.WorkDayDetail.Where(x => pWorkDayDetailIds.Contains(x.WorkDayDetailId)).ToList();
            return new List<WorkDayDetail>();
        }
        //public List<WorkDayDetail> GetWorkDayDetailByProjectIds(List<int> projectIds)
        //{
        //    if(projectIds?.Count>0)
        //        return _dbContext.WorkDayDetail.Where(x => projectIds.Contains(x.ProjectId)).ToList();
        //    return new List<WorkDayDetail>();
        //}
        //public List<WorkDayDetail> GetWorkDayDetailByEmployeeId(int employeeId = 0)
        //{
        //    if (employeeId > 0)
        //    {
        //        return _dbContext.WorkDayDetail.Where(x => x.EmployeeId == employeeId).ToList();
        //    }
        //    return new List<WorkDayDetail>(); 
        //}

        public List<WorkDayDetail> GetWorkDayDetailByFilter(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
        {
            if (appraisalWorkDayFilterView.EmployeeId > 0)
            {
                return _dbContext.WorkDayDetail.ToList();
            }
            return new List<WorkDayDetail>();
        }
        public List<EmployeeRequestCount> WorkdayDetailCount(List<int> employeeIdList)
        {
            List<EmployeeRequestCount> employeeRequests = _dbContext.WorkDay.Join(_dbContext.WorkDayDetail, wd => wd.WorkDayId, cd => cd.WorkDayId, (wd, cd) => new { wd, cd })
                      .Where(rs => employeeIdList.Contains(rs.wd.EmployeeId) && rs.cd.Status == "Pending").ToList().GroupBy(x => x.wd.EmployeeId).Select(x => new EmployeeRequestCount
                      {
                          EmployeeId = x.Key,
                          RequestCount = x.Count()
                      }).ToList();
            return employeeRequests;

        }
    }
    public interface IWorkdayObjectiveRepository : IBaseRepository<WorkdayObjective>
    {
    }
    public class WorkdayObjectiveRepository : BaseRepository<WorkdayObjective>, IWorkdayObjectiveRepository
    {
        private readonly AppraisalDBContext _dbContext;
        public WorkdayObjectiveRepository(AppraisalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
    public interface IWorkdayKRARepository : IBaseRepository<WorkdayKRA>
    {
    }
    public class WorkdayKRARepository : BaseRepository<WorkdayKRA>, IWorkdayKRARepository
    {
        private readonly AppraisalDBContext _dbContext;
        public WorkdayKRARepository(AppraisalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}