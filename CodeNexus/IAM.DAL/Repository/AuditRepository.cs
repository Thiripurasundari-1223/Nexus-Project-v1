using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IAuditRepository : IBaseRepository<EmployeeAudit>
    {
        List<AuditDetailView> GetAuditListByEmployeeId(int employeeId);
    }
    public class AuditRepository : BaseRepository<EmployeeAudit>, IAuditRepository
    {
        private readonly IAMDBContext dbContext;
        public AuditRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<AuditDetailView> GetAuditListByEmployeeId(int employeeId)
        {
            return dbContext.EmployeeAudit.Where(x => x.EmployeeId == employeeId).OrderByDescending(x=>x.CreatedOn).ToList().GroupBy(x =>  x.CreatedOn.Value.Date).Select(x=> new AuditDetailView
            {
                //ChangeRequestId = x.Key,
                CreatedOnDate = x.Key,
                EmployeeAuditList=x.ToList()
            }
            ).ToList();
        }
    }

}
