using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface ILeaveLocationRepository : IBaseRepository<LeaveLocation>
    {
        List<LeaveLocation> GetLeaveApplicableLocationDetailsByLeaveTypeId(int leaveTypeId);
        LeaveLocation GetLeaveApplicableLocationByID(int leaveApplicableDesignationId);
        List<int> GetLeaveApplicableLocationByleaveId(int leaveId);
        List<int> GetLeaveExceptionLocationByleaveId(int leaveId);
    }
    public class LeaveLocationRepository : BaseRepository<LeaveLocation>, ILeaveLocationRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveLocationRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveLocation GetLeaveApplicableLocationByID(int leaveApplicableDesignationId)
        {
            return dbContext.LeaveLocation.Where(x => x.LeaveApplicableLocationId == leaveApplicableDesignationId).FirstOrDefault();
        }
        public List<LeaveLocation> GetLeaveApplicableLocationDetailsByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.LeaveLocation.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
        }
        public List<int> GetLeaveApplicableLocationByleaveId(int leaveId)
        {
            return dbContext.LeaveLocation.Where(x => x.LeaveTypeId == leaveId && x.LeaveApplicableLocationId > 0).Select(x => x.LeaveApplicableLocationId == null ? 0 : (int)x.LeaveApplicableLocationId).ToList();
        }
        public List<int> GetLeaveExceptionLocationByleaveId(int leaveId)
        {
            return dbContext.LeaveLocation.Where(x => x.LeaveTypeId == leaveId && x.LeaveExceptionLocationId > 0).Select(x => x.LeaveExceptionLocationId == null ? 0 : (int)x.LeaveExceptionLocationId).ToList();
        }
    }
}
