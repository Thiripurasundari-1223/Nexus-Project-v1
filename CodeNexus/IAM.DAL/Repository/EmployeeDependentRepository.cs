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
    public interface IEmployeeDependentRepository : IBaseRepository<EmployeeDependent>
    {
        List<EmployeeDependent> GetEmployeeDependentByEmployeeId(int EmployeeId);
        EmployeeDependent GetEmployeeDependentDetailsByEmployeeId(int employeeId);
        Task<EmployeeDependent> GetEmployeeDependentByDependentId(int DependentId);
        EmployeeDependentView GetEmployeeDependentDetailById(int DependedtId);
    }
    public class EmployeeDependentRepository : BaseRepository<EmployeeDependent>, IEmployeeDependentRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeDependentRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<EmployeeDependent> GetEmployeeDependentByEmployeeId(int EmployeeId)
        {
            return dbContext.EmployeeDependent.Where(x => x.EmployeeID == EmployeeId).ToList();
        }
        public EmployeeDependent GetEmployeeDependentDetailsByEmployeeId(int employeeId)
        {
            EmployeeDependent employeeDependent = new EmployeeDependent();
            employeeDependent = dbContext.EmployeeDependent.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
            if (employeeDependent == null)
            {
                EmployeeDependent employeeDependentDetails = new EmployeeDependent()
                {
                    EmployeeID = 0
                };
                return employeeDependentDetails;
            }
            return employeeDependent;

        }
        public async Task<EmployeeDependent> GetEmployeeDependentByDependentId(int DependentId)
        {
            return dbContext.EmployeeDependent.Where(x => x.EmployeeDependentId == DependentId).FirstOrDefault();
        }

        public EmployeeDependentView GetEmployeeDependentDetailById(int DependedtId)
        {
            return dbContext.EmployeeDependent.Where(x => x.EmployeeDependentId == DependedtId).Select(
                x=> new EmployeeDependentView { 
                EmployeeDependentId = x.EmployeeDependentId,
                EmployeeRelationName =x.EmployeeRelationName,
                EmployeeID = x.EmployeeID,
                EmployeeRelationshipId = x.EmployeeRelationshipId,
                EmployeeRelationDateOfBirth = x.EmployeeRelationDateOfBirth
                } ).FirstOrDefault();
        }
    }
}
