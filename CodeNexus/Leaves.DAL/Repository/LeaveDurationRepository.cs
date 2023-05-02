using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaves.DAL.Repository
{
    public interface ILeaveDurationRepository : IBaseRepository<LeaveDuration>
    {
        List<LeaveDuration> GetByID(int leaveTypeId);
    }
    public class LeaveDurationRepository : BaseRepository<LeaveDuration>, ILeaveDurationRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveDurationRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<LeaveDuration> GetByID(int leaveTypeId)
        {
            if (leaveTypeId > 0)
            {
                return dbContext.LeaveDuration.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
            }
            return null;
        }
    }
}
