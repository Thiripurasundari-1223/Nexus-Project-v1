using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaves.DAL.Repository
{
    public interface ILeaveAdjustmentDetailsRepository : IBaseRepository<LeaveAdjustmentDetails>
    {
        LeaveAdjustmentDetails GetByDateAndEmployeeId(int employeeId,DateTime? effectiveFromDate, int leaveTypeId);
        //List<LeaveAdjustmentDetails> GetLeaveAdjustmentDetailsByEmployeeId(int employeeId, int leaveTypeId);
        LeaveAdjustmentDetails GetPreviousLeaveAdjustmentDetailsByEmployeeId(int employeeId, int leaveTypeId,DateTime? adjustmentEffectiveFromDate);
        List<LeaveAdjustmentDetails> GetLeaveAdjustmentDetailsByDate(int employeeId, int leaveTypeId, DateTime fromDate, DateTime toDate);
    }
    public class LeaveAdjustmentDetailsRepository : BaseRepository<LeaveAdjustmentDetails>, ILeaveAdjustmentDetailsRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveAdjustmentDetailsRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveAdjustmentDetails GetByDateAndEmployeeId(int employeeId, DateTime? effectiveFromDate,  int leaveTypeId)
        {
            return dbContext.LeaveAdjustmentDetails.Where(x => x.EmployeeId == employeeId && x.EffectiveFromDate.Value.Date == effectiveFromDate.Value.Date
            && x.LeavetypeId== leaveTypeId).OrderByDescending(x=>x.CreatedOn).FirstOrDefault();
        }
        //public List<LeaveAdjustmentDetails> GetLeaveAdjustmentDetailsByEmployeeId(int employeeId,int leaveTypeId)
        //{
        //    return dbContext.LeaveAdjustmentDetails.Where(x => x.EmployeeId == employeeId && x.LeavetypeId== leaveTypeId).ToList();
        //}
        public LeaveAdjustmentDetails GetPreviousLeaveAdjustmentDetailsByEmployeeId(int employeeId, int leaveTypeId, DateTime? adjustmentEffectiveFromDate)
        {
            DateTime? fromDate = default;
            LeaveCarryForward leaveCarryForward= dbContext.LeaveCarryForward.Where(x => x.LeaveTypeID==leaveTypeId &&
            x.EmployeeID==employeeId && adjustmentEffectiveFromDate <= x.ResetDate).OrderByDescending(x=>x.ResetDate).FirstOrDefault();

            if (leaveCarryForward != null)
            {
                fromDate = leaveCarryForward.ResetDate;
            }
            else
            {
                fromDate = dbContext.LeaveTypes.Where(x=>x.LeaveTypeId==leaveTypeId).Select(x=>x.EffectiveFromDate).FirstOrDefault();
            }
            return dbContext.LeaveAdjustmentDetails.Where(x => x.EmployeeId == employeeId && x.LeavetypeId == leaveTypeId && x.EffectiveFromDate> adjustmentEffectiveFromDate
            && fromDate<= x.EffectiveFromDate).OrderByDescending(x=>x.CreatedOn).FirstOrDefault();
        }
        public List<LeaveAdjustmentDetails> GetLeaveAdjustmentDetailsByDate(int employeeId, int leaveTypeId, DateTime fromDate, DateTime toDate)
        {
            return dbContext.LeaveAdjustmentDetails.Where(x => x.EmployeeId == employeeId && x.LeavetypeId == leaveTypeId && x.EffectiveFromDate >= fromDate
            && x.EffectiveFromDate<=toDate).ToList();
        }

    }

}
