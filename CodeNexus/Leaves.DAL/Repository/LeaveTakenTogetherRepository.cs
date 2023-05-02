using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaves.DAL.Repository
{
    public interface ILeaveTakenTogetherRepository : IBaseRepository<LeaveTakenTogether>
    {
        List<LeaveTakenTogether> GetByID(int leaveTypeId);
        List<LeaveTakenTogether> GetByLeaveType(int leaveTypeId);
    }
    public class LeaveTakenTogetherRepository : BaseRepository<LeaveTakenTogether>, ILeaveTakenTogetherRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveTakenTogetherRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<LeaveTakenTogether> GetByID(int leaveTypeId)
        {
            if (leaveTypeId > 0)
            {
                return dbContext.LeaveTakenTogether.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
            }
            return null;
        }
        public List<LeaveTakenTogether> GetByLeaveType(int leaveTypeId)
        {
            if (leaveTypeId > 0)
            {
                return dbContext.LeaveTakenTogether.Where(x => x.LeaveOrHolidayId == leaveTypeId && x.LeaveTakenType.ToLower() == "leave").ToList();
            }
            return null;
        }
    }
}
