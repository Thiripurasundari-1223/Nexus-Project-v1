using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Appraisal.DAL.Repository
{
    public interface IWorkDayRepository : IBaseRepository<WorkDay>
    {
        List<WorkdayListView> GetEmployeeWorkDayDetailByEmployeeId(WorkdayFilterView workdayFilter);
        List<WorkDayView> GetWorkdayListByEmployeeId(int employeeId, DateTime fromdate, DateTime todate);
    }
    public class WorkDayRepository : BaseRepository<WorkDay>, IWorkDayRepository
    {
        private readonly AppraisalDBContext _dbContext;
        public WorkDayRepository(AppraisalDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public List<WorkDayView> GetWorkdayListByEmployeeId(int employeeId, DateTime fromdate, DateTime todate)
        {
            return _dbContext.WorkDay.Where(x => x.EmployeeId == employeeId && x.WorkDate >= fromdate && x.WorkDate <= todate).
                Select(x => new WorkDayView
                {
                    WorkDayId = x.WorkDayId,
                    WorkDate = x.WorkDate,
                    EmployeeId = x.EmployeeId,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn
                }).ToList();
        }

        public List<WorkdayListView> GetEmployeeWorkDayDetailByEmployeeId(WorkdayFilterView workdayFilter)
        {
            List<WorkdayListView> workdayList = new();
            List<WorkDay> workday = new();
            if (workdayFilter.NOOfRecords == 0) workdayFilter.NOOfRecords = 100;
            if(workdayFilter.isExport)
            {
                    int count  = _dbContext.WorkDay.Where(x => x.EmployeeId == workdayFilter.EmployeeId).Count();
                    workdayFilter.PageNumber = 0;
                    workdayFilter.NOOfRecords = count;
            }
           
                if ((workdayFilter.FromDate != null && workdayFilter.ToDate != null) || workdayFilter.isFiltered)
                {
                    if (workdayFilter.isFiltered)
                    {
                        workday = _dbContext.WorkDay.Where(x => x.EmployeeId == workdayFilter.EmployeeId &&
                        ((workdayFilter.FromDate == null) || (workdayFilter.dateCondition == "<" ? (x.WorkDate <= workdayFilter.FromDate) :
                        workdayFilter.dateCondition == ">" ? (x.WorkDate >= workdayFilter.FromDate) :
                        (x.WorkDate >= workdayFilter.FromDate && x.WorkDate <= workdayFilter.ToDate)))).
                        OrderByDescending(x => x.CreatedOn).Skip((workdayFilter.PageNumber) * (workdayFilter.NOOfRecords)).
                        Take(workdayFilter.NOOfRecords).ToList();
                    }
                    else
                    {
                        workday = _dbContext.WorkDay.Where(x => x.EmployeeId == workdayFilter.EmployeeId && x.WorkDate >= workdayFilter.FromDate
                        && x.WorkDate <= workdayFilter.ToDate).OrderByDescending(x => x.CreatedOn).
                        Skip((workdayFilter.PageNumber) * (workdayFilter.NOOfRecords)).Take(workdayFilter.NOOfRecords).ToList();
                    }
                }
                else
                {
                    workday = _dbContext.WorkDay.Where(x => x.EmployeeId == workdayFilter.EmployeeId).OrderByDescending(x => x.CreatedOn).
                        Skip((workdayFilter.PageNumber) * (workdayFilter.NOOfRecords)).Take(workdayFilter.NOOfRecords).ToList();
                }
          
            workdayList = workday.Select(x => new WorkdayListView
            {
                WorkDayId = x.WorkDayId,
                WorkDate = x.WorkDate,
                EmployeeId = x.EmployeeId,
                ObjectiveDetail = _dbContext.WorkdayObjective.Where(y => y.WorkDayId == x.WorkDayId).
                Select(y => new WorkdayObjectiveView
                {
                    ObjectiveId = y.WorkdayObjectiveId,
                    ObjectiveName = y.ObjectiveName,
                    KRADetail = _dbContext.WorkdayKRA.Where(z => z.WorkdayObjectiveId == y.WorkdayObjectiveId &&
                    z.WorkDayId == x.WorkDayId).
                    Select(z => new WorkdayKRAView
                    {
                        KRAId = z.WorkdayKRAId,
                        KRAName = z.KRAName,
                        ContributionDetail = _dbContext.WorkDayDetail.
                        Where(a => a.WorkdayKRAId == z.WorkdayKRAId &&
                        a.WorkDayId == x.WorkDayId).
                        Select(a => new WorkdayKRADetailView
                        {
                            WorkDayDetailId = a.WorkDayDetailId,
                            WorkDayId = a.WorkDayId,
                            ProjectId = a.ProjectId,
                            ProjectName = a.ProjectName,
                            WorkHours = a.WorkHours.ToString(),
                            EmployeeRemark = a.EmployeeRemark,
                            Status = a.Status
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList();
            return workdayList;
        }
    }
}