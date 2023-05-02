using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface ILeaveDepartmentRepository : IBaseRepository<LeaveDepartment>
    {
        List<LeaveDepartment> GetLeaveApplicableDepartmentDetailsByLeaveTypeId(int leaveTypeId);
        LeaveDepartment GetLeaveApplicableDepartmentByID(int leaveApplicableDepartmentId);
        List<int> GetLeaveApplicableDepartmentByLeaveId(int leaveId);
        List<int> GetLeaveExceptionDepartmentByLeaveId(int leaveId);
        
    }
    public class LeaveDepartmentRepository : BaseRepository<LeaveDepartment>, ILeaveDepartmentRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveDepartmentRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveDepartment GetLeaveApplicableDepartmentByID(int leaveApplicableDepartmentId)
        {
            return dbContext.LeaveDepartment.Where(x => x.LeaveApplicableDepartmentId == leaveApplicableDepartmentId).FirstOrDefault();
        }
        public List<LeaveDepartment> GetLeaveApplicableDepartmentDetailsByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.LeaveDepartment.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
        }
        public List<int> GetLeaveApplicableDepartmentByLeaveId(int leaveId)
        {
            return dbContext.LeaveDepartment.Where(x => x.LeaveTypeId == leaveId && x.LeaveApplicableDepartmentId > 0).Select(x => x.LeaveApplicableDepartmentId == null ? 0 : (int)x.LeaveApplicableDepartmentId).ToList();
        }
        public List<int> GetLeaveExceptionDepartmentByLeaveId(int leaveId)
        {
            return dbContext.LeaveDepartment.Where(x => x.LeaveTypeId == leaveId && x.LeaveExceptionDepartmentId > 0).Select(x => x.LeaveExceptionDepartmentId == null ? 0 : (int)x.LeaveExceptionDepartmentId).ToList();
        }
       
    }
}
