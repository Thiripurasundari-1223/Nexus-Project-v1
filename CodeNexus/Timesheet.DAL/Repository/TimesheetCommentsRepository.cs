using SharedLibraries.ViewModels;
using System.Linq;
using Timesheet.DAL.DBContext;
using Timesheet.DAL.Models;

namespace Timesheet.DAL.Repository
{
    public interface ITimesheetCommentsRepository : IBaseRepository<TimesheetComments>
    {
        TimesheetComments GetByID(int pTimesheetId);
    }
    public class TimesheetCommentsRepository : BaseRepository<TimesheetComments>, ITimesheetCommentsRepository
    {
        private readonly TSDBContext _dbContext;
        public TimesheetCommentsRepository(TSDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public TimesheetComments GetByID(int pTimesheetCommentsId)
        {
            return _dbContext.TimesheetComments.Where(x => x.TimesheetCommentsId == pTimesheetCommentsId).FirstOrDefault();
        }
    }
}