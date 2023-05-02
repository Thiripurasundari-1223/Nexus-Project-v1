using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface ILeaveEmployeeTypeRepository : IBaseRepository<LeaveEmployeeType>
    {
        List<LeaveEmployeeType> GetLeaveEmployeeTypeDetailsByLeaveTypeId(int leaveTypeId);
        List<int?> GetLeaveApplicableEmployeeTypeByLeaveId(int leaveId);
        List<int?> GetLeaveExceptionEmployeeTypeByLeaveId(int leaveId);
    }
    public class LeaveEmployeeTypeRepository : BaseRepository<LeaveEmployeeType>, ILeaveEmployeeTypeRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveEmployeeTypeRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<LeaveEmployeeType> GetLeaveEmployeeTypeDetailsByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.LeaveEmployeeType.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
        }
        public List<int?> GetLeaveApplicableEmployeeTypeByLeaveId(int leaveId)
        {
            return dbContext.LeaveEmployeeType.Where(x => x.LeaveTypeId == leaveId && x.LeaveApplicableEmployeeTypeId > 0).Select(x => x.LeaveApplicableEmployeeTypeId).ToList();
        }
        public List<int?> GetLeaveExceptionEmployeeTypeByLeaveId(int leaveId)
        {
            return dbContext.LeaveEmployeeType.Where(x => x.LeaveTypeId == leaveId && x.LeaveExceptionEmployeeTypeId > 0).Select(x => x.LeaveExceptionEmployeeTypeId).ToList();
        }
    }
}
