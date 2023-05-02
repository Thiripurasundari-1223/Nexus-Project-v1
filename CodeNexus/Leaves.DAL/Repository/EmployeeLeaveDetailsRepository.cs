using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{

    public interface IEmployeeLeaveDetailsRepository : IBaseRepository<EmployeeLeaveDetails>
    {
        EmployeeLeaveDetails GetEmployeeLeaveDetailByID(int EmployeeLeaveDetailsID);
        EmployeeLeaveDetails GetEmployeeLeaveDetailByEmployeeIDandLeaveID(int EmployeeID, int LeaveTypeID);
        List<EmployeeLeaveDetails> GetEmployeeLeaveDetailByLeaveID(int LeaveTypeID);
        EmployeeLeaveDetails GetEmployeeLeaveDetailByLeaveId(int leaveId);
    }
    public class EmployeeLeaveDetailsRepository : BaseRepository<EmployeeLeaveDetails>, IEmployeeLeaveDetailsRepository
    {
        private readonly LeaveDBContext dbContext;
        public EmployeeLeaveDetailsRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public EmployeeLeaveDetails GetEmployeeLeaveDetailByID(int EmployeeLeaveDetailsID)
        {
            return dbContext.EmployeeLeaveDetails.Where(x => x.EmployeeLeaveDetailsID == EmployeeLeaveDetailsID).FirstOrDefault();
        }
        public EmployeeLeaveDetails GetEmployeeLeaveDetailByLeaveId(int leaveId)
        {
            return (from emp in dbContext.EmployeeLeaveDetails
                    join leave in dbContext.ApplyLeave
on emp.LeaveTypeID equals leave.LeaveTypeId
                    where emp.EmployeeID == leave.EmployeeId && leave.LeaveId == leaveId
                    select emp).FirstOrDefault();
        }
        public EmployeeLeaveDetails GetEmployeeLeaveDetailByEmployeeIDandLeaveID(int EmployeeID,int LeaveTypeID)
        {
            return dbContext.EmployeeLeaveDetails.Where(x => x.EmployeeID == EmployeeID && x.LeaveTypeID== LeaveTypeID).FirstOrDefault();
        }
        public List<EmployeeLeaveDetails> GetEmployeeLeaveDetailByLeaveID(int LeaveTypeID)
        {
            return dbContext.EmployeeLeaveDetails.Where(x => x.LeaveTypeID == LeaveTypeID).ToList();
        }

    }
}
