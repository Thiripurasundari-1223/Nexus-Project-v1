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
    public interface ILeaveApplicableRepository : IBaseRepository<LeaveApplicable>
    {
        LeaveApplicable GetByID(int pLeaveTypeId);
        public LeaveApplicable GetleaveApplicableByLeaveId(int leaveTypeId);
        LeaveApplicable GetleaveExceptionByLeaveId(int leaveTypeId);
    }
    public class LeaveApplicableRepository : BaseRepository<LeaveApplicable>, ILeaveApplicableRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveApplicableRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public LeaveApplicable GetByID(int pLeaveTypeId)
        {
            return dbContext.LeaveApplicable.Where(x => x.LeaveTypeId == pLeaveTypeId).FirstOrDefault();
        }
        public LeaveApplicable GetleaveApplicableByLeaveId(int leaveTypeId)
        {
            LeaveApplicable Applicable = new LeaveApplicable();
            Applicable = dbContext.LeaveApplicable.Where(x => x.LeaveTypeId == leaveTypeId && x.Type.ToLower() == "leaveapplicable").FirstOrDefault();
            if (Applicable == null)
            {
                LeaveApplicable AddApplicable = new LeaveApplicable()
                {
                    LeaveApplicableId = 0
                };
                return AddApplicable;
            }
            return Applicable;
        }
        public LeaveApplicable GetleaveExceptionByLeaveId(int leaveTypeId)
        {
            LeaveApplicable Exception = new LeaveApplicable();
            Exception = dbContext.LeaveApplicable.Where(x => x.LeaveTypeId == leaveTypeId && x.Type.ToLower() == "leaveexception").FirstOrDefault();
            if (Exception == null)
            {
                LeaveApplicable addException = new LeaveApplicable()
                {
                    LeaveApplicableId = 0
                };
                return addException;
            }
            return Exception;
        }
    }
}