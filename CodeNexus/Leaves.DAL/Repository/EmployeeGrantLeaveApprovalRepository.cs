using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaves.DAL.Repository
{    
    public interface IEmployeeGrantLeaveApprovalRepository : IBaseRepository<EmployeeGrantLeaveApproval>
    {
        EmployeeGrantLeaveApproval GetEmployeeGrantLeaveApprovalById(int employeeGrantLeaveApprovalId);
        List<EmployeeGrantLeaveApproval> GetEmployeeGrantLeaveApprovalByLeaveGrantDetailId(int leaveGrantDetailId);
        EmployeeGrantLeaveApproval GetEmployeeGrantLeaveApprovalByApprover(int? leaveGrantDetailId, int levelId);
        EmployeeGrantLeaveApproval GetEmployeeGrantLeaveApprovalByLevelId(int? leaveGrantDetailId, int levelId);
    }
    public class EmployeeGrantLeaveApprovalRepository : BaseRepository<EmployeeGrantLeaveApproval>, IEmployeeGrantLeaveApprovalRepository
    {
        private readonly LeaveDBContext dbContext;
        public EmployeeGrantLeaveApprovalRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public EmployeeGrantLeaveApproval GetEmployeeGrantLeaveApprovalById(int employeeGrantLeaveApprovalId)
        {
            return dbContext.EmployeeGrantLeaveApproval.Where(x => x.EmployeeGrantLeaveApprovalId == employeeGrantLeaveApprovalId).FirstOrDefault();
        }
        public List<EmployeeGrantLeaveApproval> GetEmployeeGrantLeaveApprovalByLeaveGrantDetailId(int leaveGrantDetailId)
        {
            return dbContext.EmployeeGrantLeaveApproval.Where(x => x.LeaveGrantDetailId == leaveGrantDetailId).ToList();
        }
        public EmployeeGrantLeaveApproval GetEmployeeGrantLeaveApprovalByApprover(int? leaveGrantDetailId, int levelId)
        {
            return dbContext.EmployeeGrantLeaveApproval.Where(x => x.LeaveGrantDetailId == leaveGrantDetailId && x.LevelId==levelId).FirstOrDefault();
        }
        public EmployeeGrantLeaveApproval GetEmployeeGrantLeaveApprovalByLevelId(int? leaveGrantDetailId,int levelId)
        {
            return dbContext.EmployeeGrantLeaveApproval.Where(x => x.LeaveGrantDetailId == leaveGrantDetailId &&
             x.LevelId == levelId).FirstOrDefault();
        }

    }
}
