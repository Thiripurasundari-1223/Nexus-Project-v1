using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeRequestDetailsRepository : IBaseRepository<EmployeeRequestDetail>
    {
        List<EmployeeRequestDetail> GetEmployeeRequestDetailByCRId(Guid ChangeRequestId);
    }
    public class EmployeeRequestDetailsRepository : BaseRepository<EmployeeRequestDetail>, IEmployeeRequestDetailsRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeRequestDetailsRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<EmployeeRequestDetail> GetEmployeeRequestDetailByCRId(Guid ChangeRequestId)
        {
            return dbContext.EmployeeRequestDetails.Where(x => x.ChangeRequestId == ChangeRequestId).ToList();
        }
    }
}
