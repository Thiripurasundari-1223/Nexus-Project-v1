using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface ILeaveRejectionReasonRepository : IBaseRepository<LeaveRejectionReason>
    {
        LeaveRejectionReason GetByID(int pLeaveRejectionReasonId);
        List<LeaveRejectionReason> GetLeaveRejectionReason();
    }
    public class LeaveRejectionReasonRepository : BaseRepository<LeaveRejectionReason>, ILeaveRejectionReasonRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveRejectionReasonRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveRejectionReason GetByID(int pLeaveRejectionReasonId)
        {
            return dbContext.LeaveRejectionReason.Where(x => x.LeaveRejectionReasonId == pLeaveRejectionReasonId).FirstOrDefault();
        }
        public List<LeaveRejectionReason> GetLeaveRejectionReason()
        {
            return dbContext.LeaveRejectionReason.ToList();
        }
    }
}
