using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaves.DAL.Repository
{
    public interface ILeaveCarryForwardRepository:IBaseRepository<LeaveCarryForward>
    {
        LeaveCarryForward GetLeaveCarryForwardByID(int? appConstantID);
        LeaveCarryForward GetLeaveCarryForwardByLeaveTypeId(int employeeId, int leaveTypeId, DateTime resetDate);
        LeaveCarryForward GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(int employeeId, int leaveTypeId);
    }
    public class LeaveCarryForwardRepository : BaseRepository<LeaveCarryForward>, ILeaveCarryForwardRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveCarryForwardRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public LeaveCarryForward GetLeaveCarryForwardByID(int? leaveCarryForwardID)
        {
            return dbContext.LeaveCarryForward.Where(x => x.LeaveCarryForwardID == leaveCarryForwardID).FirstOrDefault();
        }
        public LeaveCarryForward GetLeaveCarryForwardByLeaveTypeId(int employeeId, int leaveTypeId, DateTime resetDate)
        {
            return dbContext.LeaveCarryForward.Where(x => x.EmployeeID == employeeId &&
            x.LeaveTypeID==leaveTypeId && (x.ResetDate==null? DateTime.MinValue.Date : x.ResetDate.Value.Date) == resetDate.Date).FirstOrDefault();
        }
        public LeaveCarryForward GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(int employeeId, int leaveTypeId)
        {
            return dbContext.LeaveCarryForward.Where(x => x.EmployeeID == employeeId &&
            x.LeaveTypeID == leaveTypeId ).OrderByDescending(x=>x.LeaveCarryForwardID).FirstOrDefault();
        }
    }

   
}
