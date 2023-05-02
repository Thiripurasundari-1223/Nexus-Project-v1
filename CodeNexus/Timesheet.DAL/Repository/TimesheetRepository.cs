using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Reports;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Timesheet.DAL.DBContext;
using Timesheet.DAL.Models;

namespace Timesheet.DAL.Repository
{
    public interface ITimesheetRepository : IBaseRepository<SharedLibraries.Models.Timesheet.Timesheet>
    {
        SharedLibraries.Models.Timesheet.Timesheet GetByName(string pProjectName, int pTimesheetId = 0);
        SharedLibraries.Models.Timesheet.Timesheet GetByID(int pTimesheetId);
        List<SharedLibraries.Models.Timesheet.Timesheet> GetByName(string[] pProjectName);
        List<SharedLibraries.Models.Timesheet.Timesheet> GetEmployeeTimesheet(int resourceId = 0);
       
        List<RejectionReason> GetRejectionReasonList();
        
        List<SharedLibraries.Models.Timesheet.Timesheet> GetAllTimesheet();

    }
    public class TimesheetRepository : BaseRepository<SharedLibraries.Models.Timesheet.Timesheet>, ITimesheetRepository
    {
        private readonly TSDBContext _dbContext;
        public TimesheetRepository(TSDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public SharedLibraries.Models.Timesheet.Timesheet GetByID(int pTimesheetId)
        {
            return _dbContext.Timesheet.Where(x => x.TimesheetId == pTimesheetId).FirstOrDefault();
        }
        public SharedLibraries.Models.Timesheet.Timesheet GetByName(string pProjectName, int pTimesheetId = 0)
        {
            if (pTimesheetId > 0)
            {
                return _dbContext.Timesheet.Where(x => x.TimesheetId == pTimesheetId).FirstOrDefault();
            }
            return null;
        }
        public List<SharedLibraries.Models.Timesheet.Timesheet> GetByName(string[] pProjectName)
        {
            //return _dbContext.Timesheets.Where(x => pProjectName.Contains(x.ProjectName)).ToList();
            return null;
        }
        public List<SharedLibraries.Models.Timesheet.Timesheet> GetEmployeeTimesheet(int TimesheetId = 0)
        {
            if (TimesheetId > 0)
            {
                return _dbContext.Timesheet.Where(x => x.TimesheetId == TimesheetId).ToList();
            }
            return null;
        }
              
        public List<RejectionReason> GetRejectionReasonList()
        {
            return _dbContext.RejectionReason.ToList();
        }
        

        

        public List<SharedLibraries.Models.Timesheet.Timesheet> GetAllTimesheet()
        {
            return _dbContext.Timesheet.ToList();
        }

    }
}