using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Leaves;



namespace Leaves.DAL.Repository
{

    public interface ILeaveTypeRepository : IBaseRepository<LeaveTypes>
    {
        LeaveTypes GetByID(int LeaveTypeID);
        List<LeaveTypes> GetAllLeaveType(DateTime todayDate);
        bool CheckIsGrantLeaveByLeaveId(int leaveId);
        bool CheckIsGrantLeaveByLeaveTypeId(int leaveTypeId);
        List<LeaveTypes> GetAllLeaveType();
        List<LeaveTypes> GetAllLeaveType(DateTime fromDate, DateTime toDate);
    }
    //public class HolidayRepository : BaseRepository<Holiday>, IHolidayRepository
    public class LeaveTypeRepository : BaseRepository<LeaveTypes>, ILeaveTypeRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveTypeRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveTypes GetByID(int LeaveTypeID)
        {
            return dbContext.LeaveTypes.Where(x => x.LeaveTypeId == LeaveTypeID).FirstOrDefault();
        }
        public List<LeaveTypes> GetAllLeaveType()
        {
            return dbContext.LeaveTypes.ToList();
        }
        public List<LeaveTypes> GetAllLeaveType(DateTime fromDate, DateTime toDate)
        {
            return dbContext.LeaveTypes.Where(x => x.EffectiveFromDate <= fromDate && (x.EffectiveToDate == null || x.EffectiveToDate >= toDate) ).ToList();
        }
        public List<LeaveTypes> GetAllLeaveType(DateTime todayDate)
        {
            //DateTime todate = DateTime.UtcNow;
            return dbContext.LeaveTypes.Where(x => x.EffectiveToDate == null || x.EffectiveToDate >= todayDate).ToList();
           
        }
        public bool CheckIsGrantLeaveByLeaveId(int leaveId)
        {
            return (from leave in dbContext.ApplyLeave
                    join ly in dbContext.LeaveTypes on leave.LeaveTypeId equals ly.LeaveTypeId
                    join ac in dbContext.AppConstants on ly.BalanceBasedOn equals ac.AppConstantId
                    where leave.LeaveId == leaveId && ac.AppConstantValue == "LeaveGrant"
                    select ac).Any();
        }
        public bool CheckIsGrantLeaveByLeaveTypeId(int leaveTypeId)
        {
            return (from leave in dbContext.LeaveTypes
                    join ac in dbContext.AppConstants on leave.BalanceBasedOn equals ac.AppConstantId
                    where leave.LeaveTypeId == leaveTypeId && ac.AppConstantValue == "LeaveGrant"
                    select ac).Any();
        }
    }
}
