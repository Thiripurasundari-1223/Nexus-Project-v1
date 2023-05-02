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
    public interface ISpecificEmployeeDetailLeaveRepository : IBaseRepository<SpecificEmployeeDetailLeave>
    {
        List<SpecificEmployeeDetailLeave> GetByID(int leaveTypeId);
        List<SpecificEmployeeDetailLeaveView> GetByLeaveTypeID(int leaveTypeId);
    }
    public class SpecificEmployeeDetailLeaveRepository : BaseRepository<SpecificEmployeeDetailLeave>, ISpecificEmployeeDetailLeaveRepository
    {
        private readonly LeaveDBContext dbContext;
        public SpecificEmployeeDetailLeaveRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<SpecificEmployeeDetailLeaveView> GetByLeaveTypeID(int leaveTypeId)
        {
            if (leaveTypeId > 0)
            {
                return dbContext.SpecificEmployeeDetailLeave.Join(dbContext.AppConstants, sedl => sedl.EmployeeDetailLeaveId, ac => ac.AppConstantId, (sedl, ac) => new { sedl, ac }).Where(x => x.sedl.LeaveTypeId == leaveTypeId).Select(x => new SpecificEmployeeDetailLeaveView
                {
                    LeaveTypeId = x.sedl.LeaveTypeId,
                    EmployeeDetailLeaveId = x.sedl.EmployeeDetailLeaveId,
                    EmployeeDetailLeaveText = x.ac.DisplayName
                }).ToList();
            }
            return null;
        }
        public List<SpecificEmployeeDetailLeave> GetByID(int leaveTypeId)
        {
            if (leaveTypeId > 0)
            {
                return dbContext.SpecificEmployeeDetailLeave.Where(x => x.LeaveTypeId == leaveTypeId).ToList();
            }
            return null;
        }
    }
}
