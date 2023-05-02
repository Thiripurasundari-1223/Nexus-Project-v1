using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
     public interface IEmployeeApplicableLeaveRepository : IBaseRepository<EmployeeApplicableLeave>
    {
        List<EmployeeApplicableLeave> GetEmployeeApplicableLeaveDetailsByLeaveTypeId(int leaveTypeId);
        List<int?> GetEmployeeApplicableLeaveByLeaveId(int leaveId);
        List<int?> GetLeaveExceptionEmployeeIdByLeaveId(int leaveId);
        List<EmployeeApplicableLeave> GetEmployeeExceptionLeaveDetailsByLeaveTypeId(int leaveTypeId);
    }
    public class EmployeeApplicableLeaveRepository : BaseRepository<EmployeeApplicableLeave>, IEmployeeApplicableLeaveRepository
    {
        private readonly LeaveDBContext dbContext;
        public EmployeeApplicableLeaveRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<EmployeeApplicableLeave> GetEmployeeApplicableLeaveDetailsByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.EmployeeApplicableLeave.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
        }
        public List<int?> GetEmployeeApplicableLeaveByLeaveId(int leaveId)
        {
            return dbContext.EmployeeApplicableLeave.Where(x => x.LeaveTypeId == leaveId && x.EmployeeId > 0).Select(x => x.EmployeeId).ToList();
        }
        public List<int?> GetLeaveExceptionEmployeeIdByLeaveId(int leaveId)
        {
            return dbContext.EmployeeApplicableLeave.Where(x => x.LeaveTypeId == leaveId && x.LeaveExceptionEmployeeId > 0).Select(x => x.LeaveExceptionEmployeeId).ToList();
        }
        public List<EmployeeApplicableLeave> GetEmployeeExceptionLeaveDetailsByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.EmployeeApplicableLeave.Where(x => x.LeaveTypeId == leaveTypeId && x.LeaveExceptionEmployeeId > 0).ToList();
        }

    }
}
