using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
     public interface ILeaveRoleRepository : IBaseRepository<LeaveRole>
    {
        List<LeaveRole> GetLeaveApplicableRoleDetailsByLeaveTypeId(int leaveTypeId);
        LeaveRole GetLeaveApplicableRoleByID(int leaveApplicableRoleId);
        List<int> GetLeaveApplicableRoleByLeaveId(int leaveId);
        List<int> GetLeaveExceptionRoleByLeaveId(int leaveId);
    }
    public class LeaveRoleRepository : BaseRepository<LeaveRole>, ILeaveRoleRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveRoleRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveRole GetLeaveApplicableRoleByID(int leaveApplicableRoleId)
        {
            return dbContext.LeaveRole.Where(x => x.LeaveApplicableRoleId == leaveApplicableRoleId).FirstOrDefault();
        }
        public List<LeaveRole> GetLeaveApplicableRoleDetailsByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.LeaveRole.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
        }
        public List<int> GetLeaveApplicableRoleByLeaveId(int leaveId)
        {
            return dbContext.LeaveRole.Where(x => x.LeaveTypeId == leaveId && x.LeaveApplicableRoleId > 0).Select(x => x.LeaveApplicableRoleId == null ? 0 : (int)x.LeaveApplicableRoleId).ToList();
        }
        public List<int> GetLeaveExceptionRoleByLeaveId(int leaveId)
        {
            return dbContext.LeaveRole.Where(x => x.LeaveTypeId == leaveId && x.LeaveExceptionRoleId > 0).Select(x => x.LeaveExceptionRoleId == null ? 0 : (int)x.LeaveExceptionRoleId).ToList();
        }

    }
}
