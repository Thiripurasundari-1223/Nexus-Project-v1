using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface ILeaveDesignationRepository : IBaseRepository<LeaveDesignation>
    {
        List<LeaveDesignation> GetLeaveApplicableDesignationDetailsByLeaveTypeId(int leaveTypeId);
        LeaveDesignation GetLeaveApplicableDesignationByID(int leaveApplicableDesignationId);
        List<int> GetLeaveApplicableDesignationByLeaveId(int leaveId);
        List<int> GetLeaveExceptionDesignationByLeaveId(int LeaveId);
       

    }
    public class LeaveDesignationRepository : BaseRepository<LeaveDesignation>, ILeaveDesignationRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveDesignationRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveDesignation GetLeaveApplicableDesignationByID(int leaveApplicableDesignationId)
        {
            return dbContext.LeaveDesignation.Where(x => x.LeaveApplicableDesignationId == leaveApplicableDesignationId).FirstOrDefault();
        }
        public List<LeaveDesignation> GetLeaveApplicableDesignationDetailsByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.LeaveDesignation.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
        }
        public List<int> GetLeaveApplicableDesignationByLeaveId(int leaveId)
        {
            return dbContext.LeaveDesignation.Where(x => x.LeaveTypeId == leaveId && x.LeaveApplicableDesignationId > 0).Select(x => x.LeaveApplicableDesignationId == null ? 0 : (int)x.LeaveApplicableDesignationId).ToList();
        }
        public List<int> GetLeaveExceptionDesignationByLeaveId(int leaveId)
        {
            return dbContext.LeaveDesignation.Where(x => x.LeaveTypeId == leaveId && x.LeaveExceptionDesignationId > 0).Select(x => x.LeaveExceptionDesignationId == null ? 0 : (int)x.LeaveExceptionDesignationId).ToList();
        }


    }
}
