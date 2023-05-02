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
    public interface IGrantLeaveApprovalRepository : IBaseRepository<GrantLeaveApproval>
    {
        GrantLeaveApproval GetGrantLeaveApprovalById(int grantLeaveApprovalId);
        List<GrantLeaveApproval> GetGrantLeaveApprovalByLeaveTypeId(int leaveTypeId);
    }
    public class GrantLeaveApprovalRepository : BaseRepository<GrantLeaveApproval>, IGrantLeaveApprovalRepository
    {
        private readonly LeaveDBContext dbContext;
        public GrantLeaveApprovalRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public GrantLeaveApproval GetGrantLeaveApprovalById(int grantLeaveApprovalId)
        {
            return dbContext.GrantLeaveApproval.Where(x => x.GrantLeaveApprovalId == grantLeaveApprovalId).FirstOrDefault();
        }
        public List<GrantLeaveApproval> GetGrantLeaveApprovalByLeaveTypeId(int leaveTypeId)
        {
            return dbContext.GrantLeaveApproval.Where(x => x.LeaveTypeId == leaveTypeId).OrderBy(x=>x.LevelId).ToList();
        }

    }
}
