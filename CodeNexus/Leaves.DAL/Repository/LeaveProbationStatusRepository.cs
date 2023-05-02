using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface ILeaveProbationStatuRepository : IBaseRepository<LeaveProbationStatus>
    {
        List<LeaveProbationStatus> GetLeaveProbationStatusDetailsByLeaveTypeId(int leaveTypeId);
        List<int?> GetLeaveApplicableProbationStatusByLeaveId(int leaveId);
        List<int?> GetLeaveExceptionProbationStatusByLeaveId(int leaveId);
    }
    public class LeaveProbationStatuRepository : BaseRepository<LeaveProbationStatus>, ILeaveProbationStatuRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveProbationStatuRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<LeaveProbationStatus> GetLeaveProbationStatusDetailsByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.LeaveProbationStatus.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
        }
        public List<int?> GetLeaveApplicableProbationStatusByLeaveId(int leaveId)
        {
            return dbContext.LeaveProbationStatus.Where(x => x.LeaveTypeId == leaveId && x.LeaveApplicableProbationStatus > 0).Select(x => x.LeaveApplicableProbationStatus).ToList();
        }
        public List<int?> GetLeaveExceptionProbationStatusByLeaveId(int leaveId)
        {
            return dbContext.LeaveProbationStatus.Where(x => x.LeaveTypeId == leaveId && x.LeaveExceptionProbationStatus > 0).Select(x => x.LeaveExceptionProbationStatus).ToList();
        }
    }
}
